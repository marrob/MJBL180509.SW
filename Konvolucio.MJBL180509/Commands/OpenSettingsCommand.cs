
namespace Konvolucio.MJBL180509.Commands
{
    using System;
    using System.Windows.Forms;
    using View;

    class OpenSettingsCommand : ToolStripButton
    {
        public OpenSettingsCommand()
        {
            //    Image = Resources.Delete32x32;
            DisplayStyle = ToolStripItemDisplayStyle.Text;
            //    Size = new System.Drawing.Size(50, 50);
            Text = "Settings";

            //   _diagnosticsView = diagnosticsView;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var sf = new SettingsForm();
            sf.ShowDialog();
        }
    }
}
