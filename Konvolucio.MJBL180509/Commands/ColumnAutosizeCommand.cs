
namespace Konvolucio.MJBL180509.Commands
{
    using System;
    using System.Windows.Forms;

    class ColumnAutosizeCommand : ToolStripButton
    {
        readonly IMainViewControl _mainView;

        public ColumnAutosizeCommand(IMainViewControl mainForm)
        {
            //    Image = Resources.Delete32x32;
            DisplayStyle = ToolStripItemDisplayStyle.Text;
            //    Size = new System.Drawing.Size(50, 50);
            Text = "Columns make Autosize";

            _mainView = mainForm;
            //   _diagnosticsView = diagnosticsView;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _mainView.CoulmnAutosize();
        }
    }
}
