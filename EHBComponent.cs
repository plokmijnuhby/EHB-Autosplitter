using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components.AutoSplit;
using System.Windows.Forms;
using System.Xml;

namespace EHBSplitter
{
    public sealed class EHBComponent : AutoSplitComponent
    {
        public override string ComponentName => "else Heart.Break() Autosplitter";

        private readonly static EHBSettings settings = new EHBSettings();
        public override void SetSettings(XmlNode node)
        {
            if (bool.TryParse(node["splitOnArrival"]?.InnerText, out bool value))
            {
                settings.splitOnArrival = value;
            }
        }
        public override XmlNode GetSettings(XmlDocument document)
        {
            XmlElement settings_Node = document.CreateElement("Settings");
            XmlElement splitOnArrival_Node = document.CreateElement("splitOnArrival");
            bool splitOnArrival = settings.splitOnArrival;
            splitOnArrival_Node.InnerText = splitOnArrival.ToString();
            settings_Node.AppendChild(splitOnArrival_Node);
            return settings_Node;
        }
        public override Control GetSettingsControl(LayoutMode m)
        {
            return settings;
        }
        public override void Dispose() { }

        internal EHBComponent(LiveSplitState state) : base(new EHBSplitter(settings), state) { }
    }
}
