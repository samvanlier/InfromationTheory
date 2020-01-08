using System.Linq;
using CA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestEncoder
    {
        private Model model;

        [TestInitialize]
        public void Initialize()
        {
            model = new Model();

            char[] sym = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            double dev = 10881.0;
            double[] p = { 3320 / dev, 1458 / dev, 1067 / dev, 1749 / dev, 547 / dev, 2474 / dev, 266 / dev };

            //adding symbols to model
            for (int i = 0; i < sym.Length; i++)
            {
                model.AddSymbol(sym[i], p[i]);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            model = null;
        }

        [TestMethod]
        public void TestBlockCode()
        {
            Encoder.BlockCode(model);
            var block = model.BlockCode;

            Assert.IsNotNull(block);

            var codes = block.Code;

            codes.TryGetValue('A', out string A);
            Assert.AreEqual("000", A);
            codes.TryGetValue('B', out string B);
            Assert.AreEqual("001", B);
            codes.TryGetValue('C', out string C);
            Assert.AreEqual("010", C);
            codes.TryGetValue('D', out string D);
            Assert.AreEqual("011", D);
            codes.TryGetValue('E', out string E);
            Assert.AreEqual("100", E);
            codes.TryGetValue('F', out string F);
            Assert.AreEqual("101", F);
            codes.TryGetValue('G', out string G);
            Assert.AreEqual("110", G);
        }

        [TestMethod]
        public void TestShannonWeaver()
        {
            Encoder.SWCode(model);

            var sw = model.ShannonWeaver;

            Assert.IsNotNull(sw);

            var codes = sw.Code;

            codes.TryGetValue('A', out string A);
            Assert.AreEqual("0", A);
            codes.TryGetValue('F', out string F);
            Assert.AreEqual("10", F);
            codes.TryGetValue('D', out string D);
            Assert.AreEqual("110", D);
            codes.TryGetValue('B', out string B);
            Assert.AreEqual("1110", B);
            codes.TryGetValue('C', out string C);
            Assert.AreEqual("11110", C);
            codes.TryGetValue('E', out string E);
            Assert.AreEqual("111110", E);
            codes.TryGetValue('G', out string G);
            Assert.AreEqual("111111", G);
        }

        [TestMethod]
        public void TestShannonFano()
        {
            Encoder.SFCode(model);

            var sf = model.ShannonFano;

            Assert.IsNotNull(sf);

            var codes = sf.Code;

            codes.TryGetValue('A', out string A);
            Assert.AreEqual("00", A);
            codes.TryGetValue('F', out string F);
            Assert.AreEqual("01", F);
            codes.TryGetValue('D', out string D);
            Assert.AreEqual("100", D);
            codes.TryGetValue('B', out string B);
            Assert.AreEqual("101", B);
            codes.TryGetValue('C', out string C);
            Assert.AreEqual("110", C);
            codes.TryGetValue('E', out string E);
            Assert.AreEqual("1110", E);
            codes.TryGetValue('G', out string G);
            Assert.AreEqual("1111", G);
        }

        [TestMethod]
        public void TestHuffman()
        {
            Encoder.Huffman(model);

            var huffman = model.Huffman;


            Assert.IsNotNull(huffman);

            var codes = huffman.Code;

            codes.TryGetValue('A', out string A);
            Assert.AreEqual("11", A);
            codes.TryGetValue('F', out string F);
            Assert.AreEqual("01", F);
            codes.TryGetValue('D', out string D);
            Assert.AreEqual("101", D);
            codes.TryGetValue('B', out string B);
            Assert.AreEqual("100", B);
            codes.TryGetValue('C', out string C);
            Assert.AreEqual("001", C);
            codes.TryGetValue('E', out string E);
            Assert.AreEqual("0001", E);
            codes.TryGetValue('G', out string G);
            Assert.AreEqual("0000", G);

        }

        [TestMethod]
        public void TestFull()
        {
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
