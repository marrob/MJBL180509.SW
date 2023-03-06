

namespace Konvolucio.MJBL180509.View
{
    using System;
    using System.Windows.Forms;
    using Properties;

    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            comboBoxDelimiter.Items.Clear();
            comboBoxDelimiter.Items.AddRange(new string[] { "Comma", "Tab" });

            if (!comboBoxDelimiter.Items.Contains(Settings.Default.Delimiter))
                comboBoxDelimiter.Items.Add(Settings.Default.Delimiter);

            comboBoxDelimiter.SelectedItem = Settings.Default.Delimiter;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Settings.Default.Delimiter = comboBoxDelimiter.SelectedItem.ToString();
            DialogResult= DialogResult.OK;
        }
    }
}
