using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fibs
{
    class Program
    {
        static void Main()
        {
            var cur = 0;
            var next = 1;
            var last = 0;

            //1 1 2 3 5 8 13 21 34

            for (int i = 0; i < 30; i++)
            {
                Console.Write(cur + " ");
                cur = next;                                  
                next = last + cur;
                last = cur;                
            }
        }
    }
}
