using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.XmlValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isHelp = args.Select(x => (x.StartsWith("/") || x.StartsWith("-")) && (x.EndsWith("?") || x.ToUpper().EndsWith("H"))).Contains(true);
            if (args.Length < 2 || args.Contains("/?") || isHelp)
            {
                Console.WriteLine("Useage: xmlvalidation xmlfileOrPath xsdFile [options] ");
                Console.WriteLine("/? or /H - show this help");
                Console.WriteLine("/F:????  - to specify a different file extension");
                return;
            }

            string extension = "xml";
            bool useExtension = args.Select(x => (x.StartsWith("/") || x.StartsWith("-")) && (x.Length > 4 && x.ToUpper().Substring(1).StartsWith("F:"))).Contains(true);
            if (useExtension)
            {
                extension = args.Where(x => (x.StartsWith("/") || x.StartsWith("-")) && (x.Length > 4 && x.ToUpper().Substring(1).StartsWith("F:"))).FirstOrDefault().Substring(3);
            }

            string[] errors;
            if (!XmlValidator.Validate(args[0], args[1], extension, out errors))
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
