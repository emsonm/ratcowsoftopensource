using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RatCow.Controls
{
    public class GraphicContext
    {
        //this needs to be set
        private IGraphicContext _nativeInstance = null;
        public static void Create(IGraphicContext context)
        {
            Instance = new GraphicContext();
            Instance._nativeInstance = context;
        }

        //this supports the code already written
        public static GraphicContext Instance { get; internal set; }

        public int Width { get { return _nativeInstance.Width; } }
        public int Height { get { return _nativeInstance.Height; } }

        public List<Control> GraphicsObjects { get { return _nativeInstance.GraphicsObjects; } }
        public void AddGraphicsObject(Control control)
        {
            _nativeInstance.GraphicsObjects.Add(control);
        }

        public void RenderText(char key)
        {
            _nativeInstance.RenderText(key);
        }

        public void RenderKey(Key key, bool shiftDown, bool controlDown, bool altDown)
        {
            _nativeInstance.RenderKey(key, shiftDown, controlDown, altDown);
        }

        public void Render(int x, int y, bool? mouseIsDown)
        {
            _nativeInstance.Render(x, y, mouseIsDown);
        }

     
        public void Plot(int x, int y, Color pixelColor)
        {
            _nativeInstance.Plot(x, y, pixelColor); 
        }

        public void Line(int x, int y, int tx, int ty, Color pixelColor)
        {
            _nativeInstance.Line(x, y, tx, ty, pixelColor);
        }

        public void Rectangle(int x, int y, int width, int height, Color pixelColor, bool fill = false)
        {
            _nativeInstance.Rectangle(x, y, width, height, pixelColor, fill);
        }

        public void Text(int x, int y, int size, Color pixelColor, string text)
        {
            _nativeInstance.Text(x, y, size, pixelColor, text);
        }

        public void Text(int x, int y, int width, int height, int size, Color pixelColor, string text)
        {
            _nativeInstance.Text(x, y, width, height, size, pixelColor, text);
        }

        public string MeasureText(string text, int size, int width)
        {
            return _nativeInstance.MeasureText(text, size, width);
        }

        public object NativeTargetObject
        {
            get { return _nativeInstance.NativeTargetObject; }
            set { _nativeInstance.NativeTargetObject = value; }
        }
    }
}
