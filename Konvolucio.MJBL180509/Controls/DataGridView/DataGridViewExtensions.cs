

namespace Konvolucio.MJBL180509.Controls
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public static class DataGridViewExtensions
    {
        /// <summary>
        /// Double Buffering vezérlése.
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="setting"></param>
        public static void KnvDoubleBuffered(this DataGridView dataGridView, bool setting)
        {
            Type dgvType = dataGridView.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView, setting, null);
        }

    }
}
