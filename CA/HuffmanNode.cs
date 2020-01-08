using System;
namespace CA
{
    internal class HuffmanNode : IComparable
    {
        internal HuffmanNode(HuffmanNode left, HuffmanNode right)
        {
            Left = left;
            Right = right;

            Probability = left.Probability + right.Probability;
            IsLeaf = false;
        }

        internal HuffmanNode(char symbol, double propability)
        {
            Symbol = symbol;
            Probability = propability;
            IsLeaf = true;
        }

        internal HuffmanNode Left { get; set; }
        internal HuffmanNode Right { get; set; }
        internal double Probability { get; set; }

        internal bool IsLeaf { get; set; }
        internal char Symbol { get; set; }

        int IComparable.CompareTo(object obj)
        {
            var temp = (HuffmanNode)obj;

            return Probability.CompareTo(temp.Probability);
        }

        internal void Print(string code = "")
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
