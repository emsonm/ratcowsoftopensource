using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class CheckBox : FocusControl
    {
        public CheckBox()
            : base()
        {
            PressedColor = System.Drawing.Color.LightGray;
            UnfocusedColor = System.Drawing.Color.Transparent;
        }

        public bool Pressed { get; set; }
        public System.Drawing.Color PressedColor { get; set; }

        public bool Checked { get; set; }

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
                    Left + 5,
                    Top + 10,
                    20,
                    20,
                    color,
                    true);

                if (Checked)
                {
                    //g.Rectangle(
                    //   Left + 7,
                    //   Top + 12,
                    //   16,
                    //   16,
                    //   System.Drawing.Color.Black,
                    //   true);
                    var tx = Left + 8;
                    var ty = Top + 14;
                    var bx = Left + 20;
                    var by = Top + 26;
                    g.Line(tx, ty, bx, by, System.Drawing.Color.Black);
                    g.Line(tx, by, bx, ty, System.Drawing.Color.Black);
                }

                g.Rectangle(
                    Left + 4,
                    Top + 9,
                    21,
                    21,
                    System.Drawing.Color.Black);


                g.Rectangle(
                    Left,
                    Top,
                    Width,
                    Height,
                    (Focused ? FocusedColor : UnfocusedColor));

                g.Text(Left + 27, Top + (Height / 2 - 5), 9, TextColor, Text);
            }
        }

        public override bool HitTest(int x, int y, bool? mouseIsDown)
        {
            var result = base.HitTest(x, y, mouseIsDown);

            if (result & mouseIsDown.HasValue)
            {
                Pressed = (result & mouseIsDown.Value);
                if (mouseIsDown.Value)
                    Checked = !Checked;
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
