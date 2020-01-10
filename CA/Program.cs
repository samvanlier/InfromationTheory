using System;
using System.IO;

namespace CA
{
    class Program
    {
        static void Main(string[] args)
        {

            var model = new Model();

            char[] sym = { 'A', 'B', 'C', 'D', 'E' };
            double[] p = { 24.0 / 62, 12.0 / 62, 10.0 / 62, 8.0 / 62, 8.0 / 62 };

            if (sym.Length != p.Length)
                throw new Exception("Lenght has to be equal");

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

            Console.WriteLine();

            var result = model.Print();

            //assume path
            // run --path path
            if (args.Length > 1)
            {
                var path = args[1];
                WriteToFile(path, result);
                Console.WriteLine($"DONE - file is @{path}");
            }
            else
                Console.WriteLine(result);

            //Console.ReadKey();
        }

        private static void WriteToFile(string path, string result)
        {
            File.WriteAllText(path, result);
        }
    }
}
