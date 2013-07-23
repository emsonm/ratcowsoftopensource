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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RatCow.Controls.GDIPlusGraphicsContext
{
    public class GraphicContext : IGraphicContext, IGraphicContext<Bitmap>
    {
        public static void Init()
        {
            RatCow.Controls.GraphicContext.Create(new GraphicContext());
        }

        public GraphicContext()
        {
            GraphicsObjects = new List<Control>();
        }

        private Bitmap _nativeTarget = null;
        private bool _valid = false;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public List<Control> GraphicsObjects { get; internal set; }

        public void AddGraphicsObject(Control control)
        {
            GraphicsObjects.Add(control);
        }

        public void RenderText(char key)
        {
            Clear();

            foreach (var control in GraphicsObjects)
            {
                if (control is TextControl)
                {
                    var tc = (control as TextControl);
                    if (tc.Selected)
                    {
                        tc.TextInput(key);
                    }
                }
                control.Paint();
            }
        }

        public void RenderKey(Key key, bool shiftDown, bool controlDown, bool altDown)
        {
            Clear();

            foreach (var control in GraphicsObjects)
            {
                if (control is TextControl)
                {
                    var tc = (control as TextControl);
                    if (tc.Selected)
                    {
                        tc.KeyInput(key, shiftDown, controlDown, altDown);
                    }
                }
                control.Paint();
            }
        }

        public void Render(int x, int y, bool? mouseIsDown)
        {
            Clear();

            foreach (var control in GraphicsObjects)
            {
                bool hittestPassed = false;
                if (control is FocusControl)
                {
                    hittestPassed = (control as FocusControl).HitTest(x, y, mouseIsDown);
                }

                if (control is TextControl)
                {
                    if (!hittestPassed)
                    {
                        //we have moved the mouse, but did we click?
                        if (mouseIsDown.HasValue && mouseIsDown.Value)
                        {
                            (control as TextControl).Selected = false;
                        }
                    }
                    else
                    {
                        //we have moved the mouse, but did we click?
                        if (mouseIsDown.HasValue && mouseIsDown.Value)
                        {
                            (control as TextControl).Selected = true;
                        }
                    }
                }
                else if (control is SelectionControl)
                {
                    (control as SelectionControl).Selected = hittestPassed;
                }
                else
                {
                    if (control is Container)
                    {
                        var container = (control as Container);
                        foreach (var subcontrol in container.Controls)
                        {
                            if (subcontrol is FocusControl)
                            {
                                hittestPassed = (subcontrol as FocusControl).HitTest(x, y, mouseIsDown);
                            }

                            if (subcontrol is SelectionControl)
                            {
                                (subcontrol as SelectionControl).Selected = hittestPassed;
                            }
                        }
                    }
                }

                control.Paint();
            }
        }

        private void Clear()
        {
            if (_valid)
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public Bitmap NativeTarget
        {
            get
            {
                return _nativeTarget;
            }
            set
            {
                _nativeTarget = value;
                _valid = (_nativeTarget != null);

                if (_valid)
                {
                    Width = _nativeTarget.Width;
                    Height = _nativeTarget.Height;
                }
            }
        }

        public void Plot(int x, int y, Color pixelColor)
        {
            if (_valid)
            {
                _nativeTarget.SetPixel(x, y, pixelColor);
            }
        }

        public void Line(int x, int y, int tx, int ty, Color pixelColor)
        {
            if (_valid)
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    g.DrawLine(new Pen(pixelColor), x, y, tx, ty);
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public void Rectangle(int x, int y, int width, int height, Color pixelColor, bool fill = false)
        {
            if (_valid)
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    if (fill)
                    {
                        g.FillRectangle(new SolidBrush(pixelColor), x, y, width, height);
                    }
                    else
                    {
                        g.DrawRectangle(new Pen(pixelColor), x, y, width, height);
                    }
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public void RoundRectangle(int x, int y, int width, int height, Color pixelColor, bool fill = false)
        {
            if (_valid)
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    if (fill)
                    {
                        g.FillPath(new SolidBrush(pixelColor), GetRoundRectPath(x, y, width, height, 7));
                    }
                    else
                    {
                        g.DrawPath(new Pen(pixelColor), GetRoundRectPath(x, y, width, height, 7));
                    }
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        private GraphicsPath GetRoundRectPath(int x, int y, int width, int height, int rad)
        {
            int dia = rad * 2;

            int tx = x + width;
            int ty = y + height;

            var path = new GraphicsPath();

            // Create the path by connecting arcs.
            path.AddArc(x, y, dia, dia, 180, 90);
            path.AddArc(tx - dia, y, dia, dia, 270, 90);
            path.AddArc(tx - dia, ty - dia, dia, dia, 0, 90);
            path.AddArc(x, ty - dia, dia, dia, 90, 90);
            path.CloseFigure();

            return path;
        }

        public void Text(int x, int y, int size, Color pixelColor, string text)
        {
            Text(x, y, -1, -1, size, pixelColor, text);
        }

        public void Text(int x, int y, int width, int height, int size, Color pixelColor, string text)
        {
            if (_valid)
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    if (width == -1)
                    {
                        g.DrawString(text, new Font("Arial", size), new SolidBrush(pixelColor), x, y);
                    }
                    else
                    {
                        g.DrawString(text, new Font("Arial", size), new SolidBrush(pixelColor), new RectangleF(x, y, width, height));
                    }
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public string MeasureText(string text, int size, int width)
        {
            var result = String.Empty;
            if (_valid & !String.IsNullOrEmpty(text))
            {
                var g = Graphics.FromImage(_nativeTarget);
                try
                {
                    int l = text.Length - 1;
                    int w = 0;
                    var f = new Font("Arial", size);
                    while (w < width)
                    {
                        result = text[l] + result;

                        w = (int)g.MeasureString(result, f, width).Width;

                        l--;

                        if (l < 0) break;
                    }
                }
                finally
                {
                    g.Dispose();
                }
            }
            return result;
        }

        public object NativeTargetObject
        {
            get
            {
                return (object)NativeTarget;
            }
            set
            {
                if (value is Bitmap)
                {
                    NativeTarget = (Bitmap)value;
                }
            }
        }
    }
}