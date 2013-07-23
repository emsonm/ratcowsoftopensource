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

        public void RoundRectangle(int x, int y, int width, int height, Color pixelColor, bool fill = false)
        {
            _nativeInstance.RoundRectangle(x, y, width, height, pixelColor, fill);
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
