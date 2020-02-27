using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using LiveSplit.UI.Components.AutoSplit;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace EHBSplitter
{
    class EHBSplitter : IAutoSplitter
    {
        private Process game;
        private Timer timer;
        private readonly static SigScanTarget target =
            new SigScanTarget(8, "55 8B EC 83 EC 08 8B 05 ?? ?? ?? ?? 85 C0 75 20");
        private bool ready;
        private IntPtr worldOwner;
        private IntPtr totalWorldTime;
        private IntPtr beaten;
        private IntPtr storyEventLog;
        private readonly EHBSettings settings;

        internal EHBSplitter(EHBSettings settings)
        {
            this.settings = settings;
        }
        public bool ShouldStart(LiveSplitState state)
        {
            if (game is null)
            {
                game = Process.GetProcessesByName("ElseHeartbreak").FirstOrDefault(
                    // after a reset, the closed process is still visible for a moment
                    p => !p.HasExited
                    );
                if (!(game is null))
                {
                    // We need to wait until the required code is JITed.
                    // 2 seconds is the theoretical minimum time from the code becoming available
                    // to actually needing to start the timer.
                    // In practice 4 seconds would probably do the job, due to long loading times,
                    // but let's not take chances.
                    timer = new Timer(CheckMemory, null, 0, 2000);
                }
                return false;
            }
            if (!ready)
            {
                if (worldOwner == IntPtr.Zero)
                {
                    return false;
                }
                var status = game.ReadPointer(worldOwner + 0x10);
                // yes, the capital I is correct
                if (game.ReadString(status + 0x0c, 24) == "World Is set")
                {
                    var worldSettings = game.ReadPointer(game.ReadPointer(worldOwner + 0x08) + 0x20);
                    totalWorldTime = game.ReadPointer(worldSettings + 0x24);
                    beaten = game.ReadPointer(worldSettings + 0x50);
                    storyEventLog = game.ReadPointer(worldSettings + 0x58);
                    ready = true;
                    return false;
                }
            }
            return game.ReadValue<float>(totalWorldTime + 0x0c) > 2;
        }
        private void CheckMemory(object o)
        {
            foreach(var page in game.MemoryPages())
            {
                var scanner = new SignatureScanner(game, page.BaseAddress, (int)page.RegionSize);
                IntPtr ptr = scanner.Scan(target);
                if (ptr != IntPtr.Zero)
                {
                    worldOwner = game.ReadPointer(game.ReadPointer(ptr));
                    timer.Dispose();
                    break;
                }
            }
        }
        public bool ShouldSplit(LiveSplitState state)
        {
            if (settings.splitOnArrival && state.CurrentSplitIndex == 0)
            {
                // We check the length of the log, to see how many story events have occurred.
                // If the answer is more than one, we are in Dorisburg - this is true even in
                // press H%, where one of the events doesn't trigger.
                // Although in that case this split wouldn't be needed anyway.
                return game.ReadValue<int>(game.ReadPointer(storyEventLog + 0x08) + 0x0c) > 1;
            }
            else
            {
                return game.ReadValue<bool>(beaten + 0x0c);
            }
        }
        public bool ShouldReset(LiveSplitState state)
        {
            if (game.HasExited)
            {
                timer.Dispose();
                game = null;
                ready = false;
                worldOwner = IntPtr.Zero;
                return true;
            }
            else
            {
                return false;
            }
        }
        public TimeSpan? GetGameTime(LiveSplitState state) { return null; }
        public bool IsGameTimePaused(LiveSplitState state) { return false; }
    }
}
