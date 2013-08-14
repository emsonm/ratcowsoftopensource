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
            string[] errors;
            if (!XmlValidator.Validate(args[0], args[1], out errors))
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
