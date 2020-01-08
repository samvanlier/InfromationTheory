using System.Linq;
using CA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestEncoder
    {
        //    [TestMethod]
        //    public void TestShannonFano()
        //    {
        //        var model = new Model();

        //        char[] sym = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
        //        var dev = 10881.0;
        //        double[] p = { 3320 / dev, 1458 / dev, 1067 / dev, 1749 / dev, 547 / dev, 2474 / dev, 266 / dev };

        //        //adding symbols to model
        //        for (int i = 0; i < sym.Length; i++)
        //        {
        //            model.AddSymbol(sym[i], p[i]);
        //        }

        //        Encoder.SFCode(model);

        //        //TODO: deftige testen schrijven
        //        Assert.IsNotNull(model);
        //    }

        //    [TestMethod]
        //    public void TestHufffman()
        //    {
        //        var model = new Model();

        //        char[] sym = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
        //        var dev = 10881.0;
        //        double[] p = { 3320 / dev, 1458 / dev, 1067 / dev, 1749 / dev, 547 / dev, 2474 / dev, 266 / dev };

        //        //adding symbols to model
        //        for (int i = 0; i < sym.Length; i++)
        //        {
        //            model.AddSymbol(sym[i], p[i]);
        //        }

        //        Encoder.Huffman(model);

        //        Assert.IsNotNull(model);
        //    }

        [TestMethod]
        public void TestFull()
        {
            var model = new Model();

            char[] sym = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            var dev = 10881.0;
            double[] p = { 3320 / dev, 1458 / dev, 1067 / dev, 1749 / dev, 547 / dev, 2474 / dev, 266 / dev };

            //adding symbols to model
            for (int i = 0; i < sym.Length; i++)
            {
                model.AddSymbol(sym[i], p[i]);
            }

            Encoder.BlockCode(model);
            Encoder.SWCode(model);
            Encoder.SFCode(model);
            Encoder.Huffman(model);

            model.CalculateValues();

            //test Kraft
            Assert.AreEqual(1, model.ShannonWeaver.Kraft);
            Assert.AreEqual(1, model.ShannonFano.Kraft);
            Assert.AreEqual(1, model.Huffman.Kraft);

            Assert.AreEqual(3, model.BlockCode.AverageLength);
        }

    }
}
