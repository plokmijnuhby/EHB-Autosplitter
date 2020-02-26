using EHBSplitter;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;

[assembly: ComponentFactory(typeof(EHBFactory))]

namespace EHBSplitter
{
    class EHBFactory : IComponentFactory
    {
        public IComponent Create(LiveSplitState state)
        {
            return new EHBComponent(state);
        }
        public string ComponentName => "else Heart.Break() Autosplitter";
        public string Description => "Automates splits for else Heart.Break()";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => "else Heart.Break()";
        public string XMLURL => null;
        public string UpdateURL => null;
        public Version Version => new Version("1.0");
    }
}
