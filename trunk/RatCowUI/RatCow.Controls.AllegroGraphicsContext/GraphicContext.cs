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
using sharpallegro;

namespace RatCow.Controls.AllegroGraphicsContext
{
    public class GraphicContext : Allegro, IGraphicContext
    {
        public static void Init()
        {
            if (allegro_init() != 0)
                throw new Exception("Allegro did not initialise");

            /* set up the keyboard handler */
            install_keyboard();

            install_timer();

            /* set a graphics mode sized 320x200 */
            if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 320, 200, 0, 0) != 0)
            {
                if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
                    throw new Exception(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
                }
            }

            /* set the color palette */
            set_palette(desktop_palette);
            clear_to_color(screen, makecol(255, 255, 255));

            //if (!InitFont())
            //    throw new Exception("Font did not initialise");

            /* Detect mouse presence */
            if (install_mouse() < 0)
            {
                textout_centre_ex(screen, font, "No mouse detected, but you need one!",
                SCREEN_W / 2, SCREEN_H / 2, makecol(0, 0, 0),
                makecol(255, 255, 255));
                readkey();
                throw new Exception("No mouse was found!");
            }
            show_mouse(screen);

            RatCow.Controls.GraphicContext.Create(new GraphicContext());
        }

        public GraphicContext()
        {
            GraphicsObjects = new List<Control>();

            _valid = true;

        }

        private bool _valid = false;

        public int Width { get { return SCREEN_W; } }

        public int Height { get { return SCREEN_H; } }

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
            acquire_screen();
            try
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
            finally
            {
                release_screen();
            }
        }

        private void Clear()
        {
            if (_valid)
            {
                /* clear the screen to white */
                clear_to_color(screen, makecol(255, 255, 255));
            }
        }

        public Allegro NativeTarget
        {
            get
            {
                return this;
            }
            set
            {

            }
        }

        //load fonts
        private static bool InitFont()
        {
            /* We will use the lower case letters from Allegro's normal font and the
       * uppercase letters from the font in unifont.dat
       */
            FONT f1 = load_font("unifont.dat", NULL, NULL);
            if (!f1)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message("Cannot find unifont.dat in current directory.\n");
                return false;
            }

            /* Extract character ranges */
            FONT f2 = extract_font_range(font, ' ', 'A' - 1);
            FONT f3 = extract_font_range(f1, 'A', 'Z');
            FONT f4 = extract_font_range(font, 'Z' + 1, 'z');
            FONT f5;

            /* Merge fonts */
            font = merge_fonts(f4, f5 = merge_fonts(f2, f3));

            /* Destroy temporary fonts */
            destroy_font(f1);
            destroy_font(f2);
            destroy_font(f3);
            destroy_font(f4);
            destroy_font(f5);

            return true;
        }

        private int SDColToACol(System.Drawing.Color sdcol)
        {
            return makeacol(sdcol.R, sdcol.G, sdcol.B, sdcol.A);
        }

        public void Plot(int x, int y, System.Drawing.Color pixelColor)
        {
            if (_valid)
            {


                putpixel(screen, x, y, SDColToACol(pixelColor));


            }
        }

        public void Line(int x, int y, int tx, int ty, System.Drawing.Color pixelColor)
        {
            if (_valid)
            {
                line(screen, x, y, tx, ty, SDColToACol(pixelColor));
            }
        }

        public void Rectangle(int x, int y, int width, int height, System.Drawing.Color pixelColor, bool fill = false)
        {
            if (_valid)
            {


                if (fill)
                    rectfill(screen, x, y, x + width, y + height, SDColToACol(pixelColor));
                else
                    rect(screen, x, y, x + width, y + height, SDColToACol(pixelColor));


            }
        }

        public void RoundRectangle(int x, int y, int width, int height, System.Drawing.Color pixelColor, bool fill = false)
        {
            if (_valid)
            {

                //temp
                Rectangle(x, y, width, height, pixelColor, fill);

            }
        }

        //private GraphicsPath GetRoundRectPath(int x, int y, int width, int height, int rad)
        //{
        //    int dia = rad * 2;

        //    int tx = x + width;
        //    int ty = y + height;

        //    var path = new GraphicsPath();

        //    // Create the path by connecting arcs.
        //    path.AddArc(x, y, dia, dia, 180, 90);
        //    path.AddArc(tx - dia, y, dia, dia, 270, 90);
        //    path.AddArc(tx - dia, ty - dia, dia, dia, 0, 90);
        //    path.AddArc(x, ty - dia, dia, dia, 90, 90);
        //    path.CloseFigure();

        //    return path;
        //}

        public void Text(int x, int y, int size, System.Drawing.Color pixelColor, string text)
        {
            Text(x, y, -1, -1, size, pixelColor, text);
        }

        public void Text(int x, int y, int width, int height, int size, System.Drawing.Color pixelColor, string text)
        {
            if (_valid)
            {

                textout_ex(screen, font, text, x + 2, y + 5, SDColToACol(pixelColor), -1);

            }
        }

        public string MeasureText(string text, int size, int width)
        {
            var result = String.Empty;
            if (_valid & !String.IsNullOrEmpty(text))
            {
                result = text;
                //var g = Graphics.FromImage(_nativeTarget);
                //try
                //{
                //    int l = text.Length - 1;
                //    int w = 0;
                //    var f = new Font("Arial", size);
                //    while (w < width)
                //    {
                //        result = text[l] + result;

                //        w = (int)g.MeasureString(result, f, width).Width;

                //        l--;

                //        if (l < 0) break;
                //    }
                //}
                //finally
                //{
                //    g.Dispose();
                //}
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
                if (value is Allegro)
                {
                    NativeTarget = (Allegro)value;
                }
            }
        }

    }
}
