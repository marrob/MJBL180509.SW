
namespace Konvolucio.MJBL180509
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;


    public interface IMainForm
    {
        //event FormClosedEventHandler FormClosed;
        //event FormClosingEventHandler FormClosing;
        //event EventHandler Disposed;

        event EventHandler Shown;
        //event KeyEventHandler KeyUp;
        //event HelpEventHandler HelpRequested; /*????*/
        event EventHandler<string[]> FileOpen;

        string Text { get; set; }
        //string Status { get; set; }

        //void ProgressBarUpdate(ProgressChangedEventArgs arg);
        //void Close();
        string Version { get; set; }

        IMainViewControl MainView { get; }

        //void CursorWait();
        //void CursorDefault();
    }



    public partial class MainForm : Form, IMainForm
    {
        public event EventHandler<string[]> FileOpen;
        public IMainViewControl MainView { get { return mainViewControl1; } }

        public string Version
        {
            get { return toolStripStatusLabelVersion.Text; }
            set { toolStripStatusLabelVersion.Text = value; }
        }

        /// <summary>
        /// Konstructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (FileOpen != null)
                FileOpen(this, files);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Open from menu
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Filter = AppConstants.FileFilter;

            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FileOpen != null)
                    FileOpen(this, ofd.FileNames);
            }

        }
    }
}
