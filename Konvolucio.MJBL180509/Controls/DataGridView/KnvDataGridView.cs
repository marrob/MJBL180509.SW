
namespace Konvolucio.MJBL180509.Controls
{
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;


    public class KnvDataGridView : DataGridView
    {

        #region Events Overrides
        protected override void OnPaint(PaintEventArgs e)
        {
            PaintHandlerForBackgoroundText(e);
        }
        #endregion

        #region Background Text
        /// <summary>
        /// Ha nincs tartalom ez a szöveg látszik.
        /// </summary>
        [Category("KNV")]
        [Description("Ha nincs tartalom ez a szöveg látszik.")]
        public string BackgroundText
        {
            get { return _backgroundText; }
            set
            {
                _backgroundText = value;
                Refresh();
            }
        }
        private string _backgroundText = "Backgorund Text";

        private void PaintHandlerForBackgoroundText(PaintEventArgs e)
        {
            var dgv = this;
            float width = dgv.Bounds.Width;
            float height = dgv.Bounds.Height;

            int backgroundTextSize = 25;

            base.OnPaint(e);

            if (dgv.Rows.Count == 0)
            {
                Color clear = dgv.BackgroundColor;
                if (backgroundTextSize == 0) backgroundTextSize = 10;
                Font f = new Font("Seqoe", 20, FontStyle.Bold);
                Brush b = new SolidBrush(Color.Orange);
                SizeF strSize = e.Graphics.MeasureString(_backgroundText, f);
                e.Graphics.DrawString(_backgroundText, f, b, (width / 2) - (strSize.Width / 2), (height / 2) - strSize.Height / 2);
            }
        }
        #endregion


        private void KOmarker(DataGridViewRowPrePaintEventArgs e)
        {

            //if()
            //        Rows[rowIndex].DefaultCellStyle.BackColor = _firstZebraColor;
            //    else
            //        Rows[rowIndex].DefaultCellStyle.BackColor ;
        }



        #region Data Error Handler
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            /*base.OnDataError(displayErrorDialogIfNoHandler, e);*/
            MessageBox.Show(e.Exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        #endregion
    }
}
