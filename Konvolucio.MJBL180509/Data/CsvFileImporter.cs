

namespace Konvolucio.MJBL180509.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Data;
    using System.Diagnostics;

    class CsvFileImporter : IFileImporter
    {
        enum LineParseStates
        {
            FIND_DATAFIELD_START_ESCAPE,
            IN_DATAFIELD,
            FIND_SEPARATOR,
        };

        public CsvFileImporter(){  }

        private int CalcRowCount(List<string> lines)
        {
            return lines.Count;
        }

        private int CalcColumnCount(List<string> lines)
        {
            var coulmnsCount = 0;
            for (int rowIndex = 0; rowIndex < lines.Count; rowIndex++)
            {
                int coulmnIndex = 0;
                var row = lines[rowIndex];
                LineParseStates state = LineParseStates.FIND_DATAFIELD_START_ESCAPE;

                for (int chIndex = 0; chIndex < row.Length; chIndex++)
                {
                    char ch = row[chIndex];
                    switch (state)
                    {
                        case LineParseStates.FIND_DATAFIELD_START_ESCAPE:
                            {
                                if (ch == '\"')
                                    state = LineParseStates.IN_DATAFIELD;
                                break;
                            };

                        case LineParseStates.IN_DATAFIELD:
                            {
                                if (ch == '\"')
                                {
                                    coulmnIndex++;
                                    state = LineParseStates.FIND_SEPARATOR;

                                    if (coulmnIndex > coulmnsCount)
                                        coulmnsCount = coulmnIndex;
                                }
                                break;
                            }
                        case LineParseStates.FIND_SEPARATOR:
                            {
                                if (ch == ',')
                                    state = LineParseStates.FIND_DATAFIELD_START_ESCAPE;
                                break;
                            }
                    }
                }
            }
            return coulmnsCount;
        }

        public void Parser(List<string> lines, ref string[,] table)
        {
            var coulmnsCount = 0;

            for (int rowIndex = 0; rowIndex < lines.Count; rowIndex++)
            {
                int coulmnIndex = 0;
                var row = lines[rowIndex];
                string datafield = string.Empty;
                LineParseStates state = LineParseStates.FIND_DATAFIELD_START_ESCAPE;

                for (int chIndex = 0; chIndex < row.Length; chIndex++)
                {
                    char ch = row[chIndex];
                    switch (state)
                    {
                        case LineParseStates.FIND_DATAFIELD_START_ESCAPE:
                            {
                                if (ch == '\"')
                                    state = LineParseStates.IN_DATAFIELD;
                                break;
                            };

                        case LineParseStates.IN_DATAFIELD:
                            {
                                if (ch != '\"')
                                    datafield += row[chIndex];
                                if (ch == '\"')
                                {
                                    table[rowIndex, coulmnIndex] = datafield;
                                    coulmnIndex++;
                                    datafield = string.Empty;
                                    state = LineParseStates.FIND_SEPARATOR;

                                    if (coulmnIndex > coulmnsCount)
                                        coulmnsCount = coulmnIndex;
                                }
                                break;
                            }
                        case LineParseStates.FIND_SEPARATOR:
                            {
                                if (ch == ',')
                                {
                                    state = LineParseStates.FIND_DATAFIELD_START_ESCAPE;
                                }
                                break;
                            }
                    }
                }
            }
        }
        public ImportResult FileImport(string path)
        {

            /*--- Stopper Start ---*/
            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            /*--- All read in  ---*/
            var lines = Read(path);

            /*--- Get current row and coulmn counts ---*/
            var rows = CalcRowCount(lines);
            var columns = CalcColumnCount(lines);

            /*--- array allocation ---*/
            string[,] table = new string[rows, columns];

            /*--- parsing... ---*/
            Parser(lines, ref table);

            /*--- Stopper stop ---*/
            stopwatch.Stop();
#if DEBUG
            Debug.WriteLine("CsvImport-> Elapsed Time:" + stopwatch.ElapsedMilliseconds.ToString() + "ms");
#endif
            /* --- Create Result ---*/
           return new ImportResult(rows, columns, stopwatch.ElapsedMilliseconds, table);

        }

        public DataTable ConvertToDataTable(string[,] source, int rows, int coulmns)
        {

#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
#endif
            DataTable dt = new DataTable();

            for (int coulmn = 0; coulmn < coulmns; coulmn++)
                dt.Columns.Add(coulmn.ToString());

            for (int row = 0; row < rows; row++)
            {
                dt.Rows.Add();
                DataRow dr = dt.Rows[row];
                for (int coulmn = 0; coulmn < coulmns; coulmn++)
                    dr[coulmn] = source[row, coulmn];
            }

            for (int coulmn = 0; coulmn < coulmns; coulmn++)
                dt.Columns[coulmn].ReadOnly = true;

#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine("ConvertToDataTable-> Elapsed Time:" + stopwatch.ElapsedMilliseconds.ToString() + "ms");
#endif
            return dt;
        }

        List<string> Read(string path)
        {
            List<string> lines = new List<string>();
            string line = null;
            StreamReader sr;
            //http://stackoverflow.com/questions/897796/how-do-i-open-an-already-opened-file-with-a-net-streamreader
            Stream s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr = new StreamReader(s, Encoding.ASCII);
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
            sr.Close();
            return lines;
        }
    }
}
