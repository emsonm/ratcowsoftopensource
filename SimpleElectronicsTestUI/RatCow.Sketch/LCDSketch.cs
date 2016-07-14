using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatCow.Sketch
{
    /// <summary>
    /// a sketch that already has the LCS enabled
    /// </summary>
    public class LCDSketch: Sketch
    {
        protected ILcd lcd;

        public override void InitInterface<T>(T inf)
        {
            if (typeof(T) == typeof(ILcd))
            {
                lcd = (ILcd)inf;
            }
            else
                base.InitInterface(inf);
        }
    }
}
