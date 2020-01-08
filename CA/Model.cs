using System;
using System.Collections.Generic;

namespace CA
{
    public class Model
    {
        public Model()
        {
            Code = new List<SymbolWrapper>();
            BlockCode = new CodeWrapper();
            ShannonWeaver = new CodeWrapper();
            ShannonFano = new CodeWrapper();
            Huffman = new CodeWrapper();
        }

        public double Entropy { get; set; }

        public ICollection<SymbolWrapper> Code { get; set; }

        public CodeWrapper BlockCode { get; set; }

        public CodeWrapper ShannonWeaver { get; set; }
        public CodeWrapper ShannonFano { get; set; }

        public CodeWrapper Huffman { get; set; }
        public double Redundancy { get; set; }
    }

    public class CodeWrapper
    {
        public CodeWrapper()
        {
            Code = new Dictionary<char, string>();
        }

        public Dictionary<char, string> Code { get; set; }
        public double AverageLength { get; set; }
        public double Kraft { get; set; }
        public double Effeciency { get; set; }
    }

    public class SymbolWrapper
    {
        public SymbolWrapper(char symbol, double probability)
        {
            Symbol = symbol;
            Probability = probability;
            AutoInformation = Math.Log(probability, 2) * -1;
        }

        public double AutoInformation { get; }
        public char Symbol { get; set; }
        public double Probability { get; set; }
    }
}
