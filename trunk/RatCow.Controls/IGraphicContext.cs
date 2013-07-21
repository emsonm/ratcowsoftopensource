using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public interface IGraphicContext
    {

        void AddGraphicsObject(Control control);
        System.Collections.Generic.List<Control> GraphicsObjects { get; }
        int Height { get; }
        void Line(int x, int y, int tx, int ty, System.Drawing.Color pixelColor);
        string MeasureText(string text, int size, int width);
        void Plot(int x, int y, System.Drawing.Color pixelColor);
        void Rectangle(int x, int y, int width, int height, System.Drawing.Color pixelColor, bool fill = false);
        void Render(int x, int y, bool? mouseIsDown);
        void RenderKey(Key key, bool shiftDown, bool controlDown, bool altDown);
        void RenderText(char key);
        void Text(int x, int y, int size, System.Drawing.Color pixelColor, string text);
        void Text(int x, int y, int width, int height, int size, System.Drawing.Color pixelColor, string text);
        int Width { get; }

        object NativeTargetObject { get; set; }
    }

    public interface IGraphicContext<T> : IGraphicContext
    {
        T NativeTarget { get; set; }
    }
}
