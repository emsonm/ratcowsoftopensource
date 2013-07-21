using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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

                if (control is SelectionControl)
                {
                    (control as SelectionControl).Selected = hittestPassed;
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
