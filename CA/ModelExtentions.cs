using System;
using System.Linq;
using System.Text;

namespace CA
{
    public static class ModelExtentions
    {
        private static readonly string _format =
            "{0, -8}|{1,-15}|{2,-15}|{3,-15}|{4,-15}|{5,-15}|{6,-15}";
        private static readonly string _breaker =
            new string(Enumerable.Repeat<char>('=', 21 + 15 * 5).ToArray());

        public static void AddSymbol(this Model model, char symbol, double pSymbol)
        {
            model.Code.Add(new SymbolWrapper(symbol, pSymbol));
            model.Code = model.Code
                .OrderBy(x => x.Probability)
                .Reverse()
                .ToList();
        }

        public static void CalculateValues(this Model model)
        {
            model.CalcEntropy();
            model.CalcRedundancy();

            model.BlockCode.CalculateValues(model);
            model.ShannonWeaver.CalculateValues(model);
            model.ShannonFano.CalculateValues(model);
            model.Huffman.CalculateValues(model);
        }



        public static string Print(this Model model)
        {
            var sb = new StringBuilder();
            var header = string.Format(_format,
                "Symbol", "P(Symbol)", "Auto Info", "Block code", "Shannon-Weaver", "Shannon-Fano", "Huffman");
            sb.Append(header);
            sb.Append("\n");
            sb.Append(_breaker);
            sb.Append("\n");

            model.Print(sb);

            return sb.ToString();
        }

        private static void Print(this Model model, StringBuilder sb)
        {
            model.Code
                .OrderBy(_ => _.Symbol)
                .ToList()
                .ForEach(x => sb.Append(x.ToPrint(model) + "\n"));

            sb.Append(_breaker);
            sb.Append("\n");

            var entropy = $"Entropy={model.Entropy}";
            var redundancy = $"Redundancy={model.Redundancy}";

            var kraft = model.PrintKraft();
            var length = model.PrintLength();
            var effeciency = model.PrintEffeciency();

            sb.Append(kraft);
            sb.Append("\n");
            sb.Append(length);
            sb.Append("\n");
            sb.Append(effeciency);
            sb.Append("\n");
            sb.Append(_breaker);
        }

        private static string PrintKraft(this Model model)
        {
            var block = model.BlockCode.Kraft;
            var sw = model.ShannonWeaver.Kraft;
            var sf = model.ShannonFano.Kraft;
            var huf = model.Huffman.Kraft;

            return string.Format(_format,
               "", "", "Kraft =", block, sw, sf, huf);
        }

        private static string PrintLength(this Model model)
        {
            var block = string.Format("{0:N5}", model.BlockCode.AverageLength);
            var sw = string.Format("{0:N5}", model.ShannonWeaver.AverageLength);
            var sf = string.Format("{0:N5}", model.ShannonFano.AverageLength);
            var huf = string.Format("{0:N5}", model.Huffman.AverageLength);

            return string.Format(_format,
               "", "", "E(n) =", block, sw, sf, huf);
        }

        private static string PrintEffeciency(this Model model)
        {
            var block = string.Format("{0:N5}", model.BlockCode.Effeciency);
            var sw = string.Format("{0:N5}", model.ShannonWeaver.Effeciency);
            var sf = string.Format("{0:N5}", model.ShannonFano.Effeciency);
            var huf = string.Format("{0:N5}", model.Huffman.Effeciency);

            return string.Format(_format,
                "", "", "Effeciency = ", block, sw, sf, huf);
        }

        private static string ToPrint(this SymbolWrapper wrapper, Model model)
        {
            var symbol = wrapper.Symbol;
            var p = string.Format("{0:N8}", wrapper.Probability);
            var ai = string.Format("{0:N8}", wrapper.AutoInformation);
            var block = model.BlockCode.Find(symbol);
            var sw = model.ShannonWeaver.Find(symbol);
            var sf = model.ShannonFano.Find(symbol);
            var huf = model.Huffman.Find(symbol);

            return string.Format(_format,
                symbol, p, ai, block, sw, sf, huf);
        }

        private static string Find(this CodeWrapper wrapper, char symbol)
        {

            wrapper.Code.TryGetValue(symbol, out string result);

            if (string.IsNullOrWhiteSpace(result))
                throw new Exception($"No result for key [{symbol}]");

            return result;
        }

        internal static void AddBlockCode(this Model model, char symbol, string code)
        {
            model.BlockCode.Add(symbol, code);
        }

        internal static void AddSWCode(this Model model, char symbol, string code)
        {
            model.ShannonWeaver.Add(symbol, code);
        }

        internal static void AddSFCode(this Model model, char symbol, string code)
        {
            model.ShannonFano.Add(symbol, code);
        }

        internal static void AddHuffmanCode(this Model model, char symbol, string code)
        {
            model.Huffman.Add(symbol, code);
        }

        private static void Add(this CodeWrapper wrapper, char symbol, string code)
        {
            wrapper.Code.Add(symbol, code);
        }

        private static void CalculateValues(this CodeWrapper wrapper, Model model)
        {
            wrapper.CalcAverageCodeLength(model.Code);
            wrapper.CalcKraftNr();
            wrapper.CalcEffeciency(model.Entropy);
        }
    }
}
