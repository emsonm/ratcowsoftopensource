/*
 * Copyright 2013 Rat Cow Software and Matt Emson. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used
 *    to endorse or promote products derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 *
 */

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
