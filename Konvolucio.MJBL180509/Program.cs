
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

            public App()
            {
                _mainForm.FileOpen += MainForm_FileOpen;
                _mainForm.Shown += MainForm_Shown;
                _mainForm.Version = Application.ProductVersion;
                _mainForm.Text = AppConstants.SoftwareTitle;

                Application.Run( (MainForm) _mainForm);
            }

            private void MainForm_Shown(object sender, EventArgs e)
            {
                SyncContext = SynchronizationContext.Current;
                var arg  = Environment.GetCommandLineArgs();
                if (arg.Length > 1)
                    FileOpen(arg[1]);
            }

            private void MainForm_FileOpen(object sender, string[] e)
            {
                if (e.Length != 0)
                    FileOpen(e[0]);
            }

            private void FileOpen(string path)
            {

                string ext = Path.GetExtension(path);
                string name = Path.GetFileName(path);
                string dir = Path.GetDirectoryName(path);
                if (ext == ".mes" ||  ext == ".typ" || ext == ".csv")
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Restart();

                    if (_fileWatcher != null)
                    {
                        _fileWatcher.Changed -= FileWatcher_Changed;
                        _fileWatcher.Dispose();
                        _fileWatcher = null;
                    }

                    _mainForm.StatusClear();
                    _fileWatcher = new FileSystemWatcher();
                    _fileWatcher.Path = dir;
                    _fileWatcher.Filter = name;
                    _mainForm.Text = name +" - " + AppConstants.SoftwareTitle;
                    _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    _fileWatcher.Changed += FileWatcher_Changed;
                    _fileWatcher.EnableRaisingEvents = true;
                  
                    var table = _importer.CsvImport(path);
                    var row = _importer.RowCount;
                    var column = _importer.ColumCount;
                    _mainForm.MainView.Update(table, row, column);
                    stopwatch.Stop();

                    _mainForm.LoadTime = _importer.LoadedTimeMs.ToString() + "ms/" + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                    _mainForm.LastModified = File.GetLastWriteTime(path).ToString(AppConstants.GenericTimestampFormat);
                    _mainForm.RowCoulmn = row.ToString() + "/" + column.ToString();
                }
            }

            void UpdateOpenedFile(string path)
            {
                Action doMehtod = () =>
                {
                    var table = _importer.CsvImport(path);
                    var row = _importer.RowCount;
                    var column = _importer.ColumCount;
                    _mainForm.MainView.Update(table, row, column);
                    _mainForm.LastModified = File.GetLastWriteTime(path).ToString(AppConstants.GenericTimestampFormat);
                    _mainForm.RowCoulmn = row.ToString() + "/" + column.ToString();
                };

                if (SyncContext != null)
                    SyncContext.Post((e1) => { doMehtod(); }, null);
                else
                    doMehtod();
            }

            private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
            {      
                UpdateOpenedFile(e.FullPath);
            }
        }
    }
}
