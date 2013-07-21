using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class Container : Control
    {
        public Container()
        {
            Controls = new List<Control>();
        }

        public List<Control> Controls { get; set; }
    }
}
