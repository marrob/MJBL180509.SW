
namespace Konvolucio.MJBL180509
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Permissions;
    using System.Threading;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new App();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        class App
        {
            IMainForm _mainForm = new MainForm();
            FileSystemWatcher _fileWatcher;
            Data.DataImporter _importer = new Data.DataImporter();
            SynchronizationContext SyncContext;

            /// <summary>
            /// Constructor
            /// </summary>
            public App()
            {
                _mainForm.FileOpen += MainForm_FileOpen;
                _mainForm.Shown += MainForm_Shown;
                _mainForm.Version = Application.ProductVersion;

                Application.Run( (MainForm) _mainForm);
            }

            /// <summary>
            /// Main Window is Shown
            /// </summary>
            private void MainForm_Shown(object sender, EventArgs e)
            {
                SyncContext = SynchronizationContext.Current;
                var arg  = Environment.GetCommandLineArgs();
                if (arg.Length > 1)
                    FileOpen(arg[1]);
            }

            /// <summary>
            /// File Opened form menu
            /// </summary>
            private void MainForm_FileOpen(object sender, string[] e)
            {
                if (e.Length != 0)
                    FileOpen(e[0]);
            }

            /// <summary>
            /// File Opne
            /// </summary>
            /// <param name="path"></param>
            private void FileOpen(string path)
            {

                string ext = Path.GetExtension(path);
                string name = Path.GetFileName(path);
                string dir = Path.GetDirectoryName(path);
                if (ext == ".mes" ||  ext == ".typ" || ext == ".csv")
                {
#if DEBUG
                    var stopwatch = new Stopwatch();
                    stopwatch.Restart();
#endif
                    if (_fileWatcher != null)
                    {
                        _fileWatcher.Changed -= FileWatcher_Changed;
                        _fileWatcher.Dispose();
                        _fileWatcher = null;
                    }

                    _fileWatcher = new FileSystemWatcher();
                    _fileWatcher.Path = dir;
                    _fileWatcher.Filter = name;
                    _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    _fileWatcher.Changed += FileWatcher_Changed;
                    _fileWatcher.EnableRaisingEvents = true;
                    _mainForm.MainView.Table = _importer.CsvImport(path);

#if DEBUG
                    stopwatch.Stop();
                    Debug.WriteLine("View-> Elapsed Time:" + stopwatch.ElapsedMilliseconds.ToString() + "ms");
#endif
                    _mainForm.Text = name;
                }
            }


            /// <summary>
            /// UpdateOpened File
            /// </summary>
            /// <param name="path"></param>
            void UpdateOpenedFile(string path)
            {
                Action doMehtod = () =>
                {
                    _mainForm.MainView.Table = _importer.CsvImport(path);
                };

                if (SyncContext != null)
                    SyncContext.Post((e1) => { doMehtod(); }, null);
                else
                    doMehtod();

            }

            /// <summary>
            /// Opened file content changed
            /// </summary>
            private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
            {
#if DEBUG
                Debug.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
#endif             
                UpdateOpenedFile(e.FullPath);
            }
        }
    }
}
