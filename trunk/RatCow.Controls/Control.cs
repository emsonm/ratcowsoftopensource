using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class Control
    {
        public Control()
        {
            EnabledColor = System.Drawing.Color.Silver;
            DisabledColor = System.Drawing.Color.Coral;
            TextColor = System.Drawing.Color.Black;
        }

        public bool Enabled { get; set; }
        public System.Drawing.Color EnabledColor { get; set; }
        public System.Drawing.Color DisabledColor { get; set; }

        public bool Visible { get; set; }
        public string Name { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Text { get; set; }
        public System.Drawing.Color TextColor { get; set; }

        public virtual void Paint()
        {

        }
    }

    public delegate void ControlAction(object sender);
}
