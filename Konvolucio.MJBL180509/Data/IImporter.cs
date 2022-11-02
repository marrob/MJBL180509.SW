
namespace Konvolucio.MJBL180509.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data;

    public interface IFileImporter
    {
        void Parser(List<string> lines, ref string[,] table);
        ImportResult FileImport(string path);
        DataTable ConvertToDataTable(string[,] source, int rows, int coulmns);
    }
}
