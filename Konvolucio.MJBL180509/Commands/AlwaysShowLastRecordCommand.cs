
namespace Konvolucio.MJBL180509.Commands
{
    using System;
    using System.Windows.Forms;

    class AlwaysShowLastRecordCommand : ToolStripButton
    {
        readonly IMainViewControl _mainView;

        public AlwaysShowLastRecordCommand(IMainViewControl mainView)
        {
            //    Image = Resources.Delete32x32;
            DisplayStyle = ToolStripItemDisplayStyle.Text;
            //    Size = new System.Drawing.Size(50, 50);
            Text = "Always Show Last Record";

            _mainView = mainView;
            //   _diagnosticsView = diagnosticsView;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _mainView.AlwaysShowLastRecord = !_mainView.AlwaysShowLastRecord;
            Checked = _mainView.AlwaysShowLastRecord;
        }
    }
}
