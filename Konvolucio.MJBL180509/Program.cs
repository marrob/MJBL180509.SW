
/*
 * ToDo: 
 * 
 *  - Settings menüt csinálni 
 *  - Beállíható legyen default csv vagy tabbed file
 *  - Fixálhandó sorok száma a fejlécben állítható legyen 
 *      Conti esetén is jó a fixált sorok száma...
 *  - A helpert frissíteni kell.
 *   
 */

namespace Konvolucio.MJBL180509
{
    using System;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Permissions;
    using System.Threading;
    using Data;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new App();
        }

    }

    public interface IApp
    {
        void FileOpen(string path);
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    class App : IApp
    {
        IMainForm _mainForm = new MainForm();
        FileSystemWatcher _fileWatcher;
        //IFileImporter _importer = new Data.CsvFileImporter();
        IFileImporter _importer = new TabDelimitedFileImporter();
        SynchronizationContext SyncContext;

        public App()
        {
            _mainForm.FileOpen += MainForm_FileOpen;
            _mainForm.Shown += MainForm_Shown;
            _mainForm.Version = Application.ProductVersion;
            _mainForm.Text = AppConstants.SoftwareTitle + " - " + Application.ProductVersion;

            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += FileWatcher_Changed;

            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDown.Items.AddRange(
                new ToolStripItem[] 
                {
                    new Commands.FileOpenCommand(this)
                });

            var viewMenu = new ToolStripMenuItem("View");
            viewMenu.DropDown.Items.AddRange( 
                 new ToolStripItem[]
                 {
                    new Commands.AlwaysOnTopCommand(_mainForm),
                    new Commands.AlwaysShowLastRecordCommand(_mainForm.MainView),
                    new Commands.ColumnAutosizeCommand(_mainForm.MainView)
                 });

            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDown.Items.AddRange(
                 new ToolStripItem[]
                 {
                     new Commands.HowIsWorkingCommand(),
                     new Commands.UpdatesCommands(),
                 });

            _mainForm.MenuBar = new ToolStripItem[]
                {
                    fileMenu,
                    viewMenu,
                    helpMenu,
                };

            Application.Run((MainForm)_mainForm);
        }

        /// <summary>
        /// MainForm is show
        /// </summary>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            SyncContext = SynchronizationContext.Current;
            var arg = Environment.GetCommandLineArgs();
            if (arg.Length > 1)
                FileOpen(arg[1]);
        }

        /// <summary>
        /// File Open from argument
        /// </summary>
        private void MainForm_FileOpen(object sender, string[] e)
        {
            if (e.Length != 0)
                FileOpen(e[0]);
        }

        /// <summary>
        /// File Open 
        /// </summary>
        public void FileOpen(string path)
        {

            string ext = Path.GetExtension(path);
            string name = Path.GetFileName(path);
            string dir = Path.GetDirectoryName(path);
            if (ext == ".mes" || ext == ".typ" || ext == ".csv")
            {
                var stopwatch = new Stopwatch();
                stopwatch.Restart();

                _mainForm.StatusClear();
                _fileWatcher.Path = dir;
                _fileWatcher.Filter = name;
                _mainForm.Text = name + " - " + AppConstants.SoftwareTitle + " - " + Application.ProductVersion;
                _fileWatcher.EnableRaisingEvents = true;

                var imported = _importer.FileImport(path);
                _mainForm.MainView.FillContent(imported.Table, imported.RowCount, imported.ColumCount);
                stopwatch.Stop();

                _mainForm.LoadTime = "Load : " + imported.LoadedTimeMs.ToString() + "ms/" + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                _mainForm.LastModified = "Last write : " + File.GetLastWriteTime(path).ToString(AppConstants.GenericTimestampFormat);
                _mainForm.RowCoulmn = "Row : " +  imported.RowCount.ToString() + "  " + "Col : " + imported.ColumCount.ToString();
            }
        }

        /// <summary>
        /// File Content changed handler
        /// </summary>
        void UpdateOpenedFile(string path)
        {
            Action doMehtod = () =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Restart();

                var imported = _importer.FileImport(path);
                _mainForm.MainView.UpdateContent(imported.Table, imported.RowCount, imported.ColumCount);

                stopwatch.Stop();

                _mainForm.LoadTime = "Load : " + imported.LoadedTimeMs.ToString() + "ms/" + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                _mainForm.LastModified = "Last write : " + File.GetLastWriteTime(path).ToString(AppConstants.GenericTimestampFormat);
                _mainForm.RowCoulmn = "Row : " + imported.RowCount.ToString() + "  " + "Col : " + imported.ColumCount.ToString();
            };

            if (SyncContext != null)
                SyncContext.Post((e1) => { doMehtod(); }, null);
            else
                doMehtod();
        }

        /// <summary>
        /// FileWatcher Changed
        /// </summary>
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            UpdateOpenedFile(e.FullPath);
            Debug.WriteLine("Changed:" + DateTime.Now.ToLongTimeString());
        }
    }
}

