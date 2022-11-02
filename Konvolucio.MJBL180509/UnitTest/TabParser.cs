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
    class TabParser
    {
        const string TesFilesDirectory = @"d:\@@@!ProjectS\KonvolucioApp\MJBL180509.SW\GoldMesFiles\";

        [SetUp]
        public void SetUpV2()
        {

        }

        [Test]
        public void ImportEmptyFile()
        {
            var path = TesFilesDirectory + "tab_0000_Empty.mes";
            var importer = new TabDelimitedFileImporter();
            var imported =  importer.FileImport(path);

            Assert.AreEqual(0, imported.RowCount);
            Assert.AreEqual(0, imported.ColumCount);
        }

        [Test, Order(1)]
        public void ImportColumn()
        {
            var path = TesFilesDirectory + "tab_Columns_TabParser.mes";
            var importer = new TabDelimitedFileImporter();
            var imported = importer.FileImport(path);

            Assert.AreEqual(3, imported.RowCount);
            Assert.AreEqual(6, imported.ColumCount);
        }


        [Test, Order(1)]
        public void MaxCoulmnsTable()
        {
            CreateVectorTable(10, 650, TesFilesDirectory + @"\MaxCoulmnsTable.mes");
        }

        [Test, Order(1)]
        public void MaxTooMuchCoulmnsTable()
        {
            CreateVectorTable(10, 700, TesFilesDirectory + @"\TooMuchCoulmnsTable.mes");
        }

        [Test, Order(1)]
        public void MaxRowsTable()
        {
            CreateVectorTable(30000, 1, TesFilesDirectory + @"\MaxRowsTable.mes");
        }

        [Test, Order(1)]
        public void RowsTooMuch()
        {
            CreateVectorTable( 5000000, 1, TesFilesDirectory + @"\TooMuchRowsTable.mes");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="path"></param>
        private void CreateVectorTable(int rows, int columns, string path)
        {
            string[] lines = new string[rows + 4];

            Random rnd = new Random();

            /* Name */
            lines[0] = "Status\tDateTime\tProductSerial\tTestTime\tReplaceTime";
            /*Min value*/
            lines[1] += "\"\",\"\",\"\",\"\",\"\"";
            /*Max value*/
            lines[2] += "\"\",\"\",\"\",\"\",\"\"";
            /*Unit*/
            lines[3] += "\"\",\"\",\"\",\"\",\"\"";

            for (int c = 0; c < columns; c++)
                lines[0] += ",\"" + "Step_" + c.ToString() + "\"";

            for (int c = 0; c < columns; c++)
                lines[1] += ",\"" + "1" + "\"";

            for (int c = 0; c < columns; c++)
                lines[2] += ",\"" + "100" + "\"";

            for (int c = 0; c < columns; c++)
                lines[3] += ",\"" + "V" + "\"";

            for (int row = 4; row < rows + 4; row++)
            {
                var datarow = string.Empty;
                datarow += "\"OK";                        /*Status*/
                datarow += "\",\"16/1/2018 17:28:14";     /*DateTime*/
                datarow += "\",\"AA90148181180120026F0J"; /*ProductSerial*/
                datarow += "\",\"1";                      /*TestTime*/
                datarow += "\",\"2";                      /*ReplaceTime*/
                datarow += "\"";

                for (int c = 0; c < columns; c++)
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
