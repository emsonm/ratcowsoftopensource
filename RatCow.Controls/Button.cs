using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class Button : FocusControl
    {
        public Button()
            : base()
        {
            PressedColor = System.Drawing.Color.LightGray;
        }

        public bool Pressed { get; set; }
        public System.Drawing.Color PressedColor { get; set; }

        public override void Paint()
        {
            if (Visible)
            {
                var g = GraphicContext.Instance;

                System.Drawing.Color color;
                if (Enabled)
                {
                    if (Pressed)
                    {
                        color = PressedColor;
                    }
                    else
                    {
                        color = EnabledColor;
                    }
                }
                else
                {
                    color = DisabledColor;
                }

                g.Rectangle(
                    Left,
                    Top,
                    Width,
                    Height,
                    color,
                    true);

                g.Rectangle(
                    Left,
                    Top,
                    Width,
                    Height,
                    (Focused ? FocusedColor : UnfocusedColor));

                g.Text(Left + 10, Top + (Height / 2 - 5), 9, TextColor, Text);
            }
        }

        public override bool HitTest(int x, int y, bool? mouseIsDown)
        {
            var result = base.HitTest(x, y, mouseIsDown);

            if (mouseIsDown.HasValue)
            {
                Pressed = (result & mouseIsDown.Value);
            }

            return result;
        }

        protected override void DoAction()
        {
            if (Click != null)
                Click(this);
        }


        public event ControlAction Click;
    }
}
