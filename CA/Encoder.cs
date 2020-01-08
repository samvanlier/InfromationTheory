using System;
using System.Collections.Generic;
using System.Linq;

namespace CA
{
    public static class Encoder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void BlockCode(Model model)
        {
            Console.WriteLine("Making block code");

            var toCode = model.Code
                .OrderBy(x => x.Symbol)
                .ToList();

            var codeLength = (int)Math.Ceiling(Math.Log(toCode.Count, 2));

            var ctr = 0;
            foreach (var x in toCode)
            {
                var code = Convert.ToString(ctr, 2);
                while (code.Length < codeLength)
                    code = "0" + code;

                model.AddBlockCode(x.Symbol, code);
                ctr++;
            }
            Console.WriteLine("Finished block code");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="node"></param>
        /// <param name="code"></param>
        private static void ConstructHuffmanCode(Model model, HuffmanNode node, string code = "")
        {
            var left = "0";
            var right = "1";

            if (node.IsLeaf)
                model.AddHuffmanCode(node.Symbol, code);
            else
            {
                ConstructHuffmanCode(model, node.Left, code + left);
                ConstructHuffmanCode(model, node.Right, code + right);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private static HuffmanNode ConstructHuffmanTree(ICollection<HuffmanNode> nodes)
        {
            if (nodes.Count < 2)
                return nodes.First(); //finished

            var left = nodes.Pop();
            var right = nodes.Pop();

            var node = new HuffmanNode(left, right);


            return ConstructHuffmanTree(nodes.AddSorted(node));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void SFCode(Model model)
        {
            Console.WriteLine("Making Shannon-Fano code");

            var toCode = model.Code.ToList();

            CalcCode(model, toCode);

            Console.WriteLine("Finished Shannon-Fano code");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="toCode"></param>
        /// <param name="code"></param>
        private static void CalcCode(Model model, List<SymbolWrapper> toCode, string code = "")
        {
            var carCode = code + "0";
            var cdrCode = code + "1";

            (var car, var cdr) = toCode.Split();

            if (car.Count == 1)
                model.AddSFCode(car.First().Symbol, carCode);
            else
                CalcCode(model, car, carCode);

            if (cdr.Count == 1)
                model.AddSFCode(cdr.First().Symbol, cdrCode);
            else
                CalcCode(model, cdr, cdrCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void SWCode(Model model)
        {
            Console.WriteLine("Making Shannon-Weaver code");

            char prefix = '1';
            char other = '0';

            var toCode = model.Code.ToList();
            var first = toCode.First();
            var last = toCode.Last();

            model.AddSWCode(first.Symbol, "0");
            string code = "" + other;
            for (int i = 1; i < toCode.Count; i++)
            {
                var item = toCode[i];
                if (item.Symbol == last.Symbol)
                {
                    code = code.Remove(code.Length - 1, 1) + "1";
                }
                else
                {
                    code = prefix + code;
                }

                model.AddSWCode(item.Symbol, code);
            }
            Console.WriteLine("Finished Shannon-Weaver code");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void Huffman(Model model)
        {
            HuffmanNode huffmanTree = ConstructHuffmanTree(model.GetHuffmanLeafs());

            ConstructHuffmanCode(model, huffmanTree);
        }
    }
}
