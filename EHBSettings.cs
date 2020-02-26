using System.Windows.Forms;

namespace EHBSplitter
{
    class EHBSettings : UserControl
    {
        private CheckBox checkBox;

        internal bool splitOnArrival
        {
            get => checkBox.Checked;
            set => checkBox.Checked = value;
        }

        private void InitializeComponent()
        {
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox.Location = new System.Drawing.Point(3, 3);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(141, 17);
            this.checkBox.TabIndex = 0;
            this.checkBox.Text = "Split after boat cutscene";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // EHBSettings
            // 
            this.AutoSize = true;
            this.Controls.Add(this.checkBox);
            this.Name = "EHBSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal EHBSettings()
        {
            InitializeComponent();
        }
    }
}
