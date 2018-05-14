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

    [TestFixture]
    class CsvParser
    {

        [SetUp]
        public void SetUpV2()
        {

        }

        [Test, Order(1)]
        public void First()
        {
            var path = @"D:\@@@!ProjectS\MJBL180509\SampleMesFiles\0001.mes";

            var import = new DataImporter() ;

            DataTable dt = import.CsvImport(path);
        }

        [Test, Order(1)]
        public void TowDimArray()
        {
            string[,] arr = new string[0,0];


        }



        [TearDown]
        public void Clean()
        {

        }
    }
}
