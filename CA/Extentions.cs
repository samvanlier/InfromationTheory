using System;
using System.Collections.Generic;
using System.Linq;

namespace CA
{
    public static class Extentions
    {
        internal static double TotValue(this ICollection<SymbolWrapper> collection)
        {
            var result = .0;
            collection.ToList()
                .ForEach(x => result += x.Probability);
            return result;
        }

        private static void Add(this ICollection<SymbolWrapper> collection, ICollection<SymbolWrapper> list)
        {
            list.ToList()
                .ForEach(x => collection.Add(x));
        }

        internal static (List<SymbolWrapper> car, List<SymbolWrapper> cdr) Split(this List<SymbolWrapper> list)
        {

            var car = new HashSet<SymbolWrapper>();
            var cdr = new HashSet<SymbolWrapper>
            {
                list
            };

            double margin = Double.MaxValue;
            foreach (var item in list)
            {
                car.Add(item);
                cdr.Remove(item);

                var x = Math.Abs(car.TotValue() - cdr.TotValue());
                if (x <= margin)
                {
                    margin = x;
                }
                else
                {
                    cdr.Add(item);
                    car.Remove(item);
                    return (car.ToList(), cdr.ToList());
                }
            }


            return (car.ToList(), cdr.ToList());
        }

        internal static ICollection<HuffmanNode> GetHuffmanLeafs(this Model model)
        {
            return model.Code
                .Select(x => new HuffmanNode(x.Symbol, x.Probability))
                .Reverse()
                .ToList();
        }

        internal static HuffmanNode Pop(this ICollection<HuffmanNode> collection)
        {
            var popped = collection.First();
            collection.Remove(popped);

            return popped;
        }

        internal static ICollection<HuffmanNode> AddSorted(this ICollection<HuffmanNode> collection, HuffmanNode element)
        {
            collection.Add(element);
            return collection
                .OrderBy(x => x.Probability)
                .ToList();
        }
    }
}
