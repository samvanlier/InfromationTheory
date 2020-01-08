using System;
namespace CA
{
    public class HuffmanNode : IComparable
    {
        public HuffmanNode(HuffmanNode left, HuffmanNode right)
        {
            Left = left;
            Right = right;

            Probability = left.Probability + right.Probability;
            IsLeaf = false;
        }

        public HuffmanNode(char symbol, double propability)
        {
            Symbol = symbol;
            Probability = propability;
            IsLeaf = true;
        }

        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
        public double Probability { get; set; }

        public bool IsLeaf { get; set; }
        public char Symbol { get; set; }

        public int CompareTo(object obj)
        {
            var temp = (HuffmanNode)obj;

            return Probability.CompareTo(temp.Probability);
        }

        public void Print(string code = "")
        {
            if (IsLeaf)
                Console.WriteLine("leaf - " + Symbol + " - " + code);
            else
            {
                Console.WriteLine("internal node - " + code);
                this.Left.Print(code + "0");
                this.Right.Print(code + "1");
            }
        }

    }
}
