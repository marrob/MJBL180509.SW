

namespace Konvolucio.MJBL180509
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImportResult
    {
        public int RowCount { get; private set; }
        public int ColumCount { get; private set; }
        public long LoadedTimeMs { get; private set; }
        public string[,] Table { get; private set; }

        public ImportResult(int row, int column, long loadedTime, string [,] table)
        {
            RowCount = row;
            ColumCount = column;
            LoadedTimeMs = loadedTime;
            Table = table;
        }
    }
}
