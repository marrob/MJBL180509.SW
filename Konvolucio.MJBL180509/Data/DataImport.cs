

namespace Konvolucio.MJBL180509.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data;
    using System.Diagnostics;


    class DataImporter
    {

        const int MaxTableRowsCount = 16384;
        const int MaxTableCoulmsCount = 16384;

        //const int MaxTableRowsCount = 500;
        //const int MaxTableCoulmsCount = 500;

        public string[,] Table { get { return _table; } }
        public int RowsCount { get; private set; }
        public int ColumsCount { get; private set; }

        private readonly string[,] _table = new string[MaxTableRowsCount, MaxTableCoulmsCount];

        enum LineParseStates
        {
            FIND_DATAFIELD_START_ESCAPE,
            IN_DATAFIELD,
            FIND_SEPARATOR,
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public DataImporter()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public DataTable CsvImport(string path)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
#endif
            var lines = Read(path);
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
                                    _table[rowIndex, coulmnIndex] = datafield; 
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
#if DEBUG
            stopwatch.Stop();
            Debug.WriteLine("CsvImport-> Elapsed Time:" + stopwatch.ElapsedMilliseconds.ToString() + "ms");
#endif
            RowsCount = lines.Count;
            ColumsCount = coulmnsCount;
            return (ConvertToDataTable(_table, lines.Count, coulmnsCount));
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rows"></param>
        /// <param name="coulmns"></param>
        /// <returns></returns>
        DataTable ConvertToDataTable(string[,] source, int rows, int coulmns)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
