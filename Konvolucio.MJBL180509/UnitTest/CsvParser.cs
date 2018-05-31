namespace Konvolucio.MJBL180509.UnitTest
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Konvolucio.MJBL180509.Data;
    using NUnit.Framework;
    using System.Data;
    using System.IO;

    [TestFixture]
    class CsvParser
    {
        const string TesFilesDirectory = @"D:\@@@!ProjectS\MJBL180509\GoldMesFiles\";

        [SetUp]
        public void SetUpV2()
        {

        }

        [Test, Order(1)]
        public void ImportEmptyFile()
        {
            var path = TesFilesDirectory + "0000_Empty.mes";
            var importer = new DataImporter();
            var imported =  importer.CsvFileImport(path);

            Assert.AreEqual(0, imported.RowCount);
            Assert.AreEqual(0, imported.ColumCount);
        }

        [Test, Order(1)]
        public void BigTable()
        {
            int maxcolumns = 600;
            int maxrows = 10000;

            string path = TesFilesDirectory + @"\BigTable.mes";

            string[] lines = new string[maxrows + 4];


            Random rnd = new Random();

            /* Name */
            lines[0] = "\"Status\",\"DateTime\",\"ProductSerial\",\"TestTime\",\"ReplaceTime\"";
            /*Min value*/
            lines[1] += "\"\",\"\",\"\",\"\",\"\"";
            /*Max value*/
            lines[2] += "\"\",\"\",\"\",\"\",\"\"";
            /*Unit*/
            lines[3] += "\"\",\"\",\"\",\"\",\"\"";

            for (int col = 0; col < maxcolumns; col++)
                lines[0] += ",\"" + "Step_" + col.ToString() +  "\"";

            for (int col = 0; col < maxcolumns; col++)
                lines[1] += ",\"" + "1" + "\"";

            for (int col = 0; col < maxcolumns; col++)
                lines[2] += ",\"" + "100" + "\"";

            for (int col = 0; col < maxcolumns; col++)
                lines[3] += ",\"" + "V" + "\"";

            for (int row = 4; row < maxrows+4; row++)
            {
                var datarow = string.Empty;
                datarow += "\"OK";                        /*Status*/
                datarow += "\",\"16/1/2018 17:28:14";     /*DateTime*/
                datarow += "\",\"AA90148181180120026F0J"; /*ProductSerial*/
                datarow += "\",\"1";                      /*TestTime*/
                datarow += "\",\"2";                      /*ReplaceTime*/
                datarow += "\"";

                for (int col = 0; col < maxcolumns; col++)
                    datarow += ",\"" + (rnd.NextDouble() * 100 + 1).ToString() + "\"";

                lines[row] = datarow; 
               
            }
    
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllLines(path, lines);
        }

        [TearDown]
        public void Clean()
        {

        }
    }
}
