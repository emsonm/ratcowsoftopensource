using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class TextBox : TextControl
    {
        public TextBox()
            : base()
        {

        }

        public bool MultiLine { get; set; }

        public override void Paint()
        {
            if (Visible)
            {
                var g = GraphicContext.Instance;

                g.Rectangle(
                    Left,
                    Top,
                    Width,
                    Height,
                    (Focused ? FocusedColor : UnfocusedColor));

                var displayText = g.MeasureText(Text, 9, Width - 2);

                g.Text(Left + 2, Top + (Height / 2 - 10), Width - 2, Height - 5, 9, TextColor, displayText + "|");
            }

        }
    }
}
