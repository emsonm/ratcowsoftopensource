using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCow.Sketch
{
    public class Sketch
    {
        public const byte OUTPUT = 0;
        public const byte INPUT = 1;

        public const int LOW = 0;
        public const int HIGH = 1;

        protected IGeneral api;

        public virtual void InitInterface<T>( T inf)
        {
            if (inf is IGeneral)
            {
                api = (IGeneral)inf;
            }

        }

        public virtual void setup()
        {

        }

        public virtual void loop()
        {
        }
    }
}
