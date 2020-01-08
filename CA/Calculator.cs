using System;
using System.Collections.Generic;
using System.Linq;

namespace CA
{
    internal static class Calculator
    {
        internal static void CalcEntropy(this Model model)
        {
            model.Entropy = model.Code
                 .Select(_ => -1 * Math.Log(_.Probability, 2) * _.Probability)
                 .Sum();
        }

        internal static void CalcAverageCodeLength(this CodeWrapper wrapper, ICollection<SymbolWrapper> code)
        {
            wrapper.AverageLength = wrapper.Code
                .Select(x =>
                {
                    var p = code.Single(el => el.Symbol == x.Key).Probability;
                    return x.Value.Length * p;
                })
                .Sum();
        }

        internal static void CalcKraftNr(this CodeWrapper wrapper)
        {
            wrapper.Kraft = wrapper.Code
                .Select(_ => Math.Pow(2, -1 * _.Value.Length))
                .Sum();
        }

        internal static void CalcEffeciency(this CodeWrapper wrapper, double entropy)
        {
            wrapper.Effeciency = entropy / wrapper.AverageLength;
        }

        internal static void CalcRedundancy(this Model model)
        {
            model.Redundancy = Math.Log(model.Code.Count, 2) - model.Entropy;
        }
    }
}
