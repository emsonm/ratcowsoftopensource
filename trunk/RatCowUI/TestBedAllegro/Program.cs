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
using RatCow.Controls;
using RatCow.Controls.AllegroGraphicsContext;

namespace TestBedAllegro
{
    class Program
    {
        static int Main(string[] args)
        {
            new Program().Run();

            return 0;
        }

        private void Run()
        {
            RatCow.Controls.AllegroGraphicsContext.GraphicContext.Init();

            var button1 = new Button() { Top = 30, Left = 30, Height = 35, Width = 50, Text = "test", Enabled = true, Visible = true, Focused = false, Name = "Button1" };
            button1.Click += new ControlAction(button1_Click);

            var textBox1 = new TextBox() { Top = 100, Left = 100, Height = 35, Width = 100, Enabled = true, Visible = true, Focused = false, Selected = false };

            var cbx1 = new CheckBox() { Top = 100, Left = 30, Height = 35, Width = 50, Text = "test", Enabled = true, Visible = true, Focused = false, Name = "Checkbox1" };


            RatCow.Controls.GraphicContext.Instance.GraphicsObjects.Add(button1);
            RatCow.Controls.GraphicContext.Instance.GraphicsObjects.Add(textBox1);
            RatCow.Controls.GraphicContext.Instance.GraphicsObjects.Add(cbx1);

            MainLoop();
        }

        private void MainLoop()
        {
            bool exit = false;
            int oldmouse_x = -1;
            int oldmouse_y = -1;
            bool? lastmousepoll = null;
            bool? thismousepoll = null;
            int lastkey = -1;

            do
            {
                AllegroAPI.poll_mouse(); //get mouse data

                if ((AllegroAPI.mouse_b & 1) > 0)
                    thismousepoll = true;
                else if (lastmousepoll.HasValue && lastmousepoll.Value == true)
                    thismousepoll = false;
                else
                    thismousepoll = null;




                if (AllegroAPI.mouse_x != oldmouse_x | Allegro.mouse_y != oldmouse_y)
                {
                    RatCow.Controls.GraphicContext.Instance.Render(AllegroAPI.mouse_x, AllegroAPI.mouse_y, thismousepoll);

                    oldmouse_x = AllegroAPI.mouse_x;
                    oldmouse_y = AllegroAPI.mouse_y;

                }
                else if (thismousepoll.HasValue)
                {
                    RatCow.Controls.GraphicContext.Instance.Render(AllegroAPI.mouse_x, AllegroAPI.mouse_y, thismousepoll);
                }

                lastmousepoll = thismousepoll;

                AllegroAPI.poll_keyboard();

                //read keyboard
                if (AllegroAPI.keypressed() != 0)
                {
                    var mapping = TranslateKey();

                    var k = AllegroAPI.readkey();
                    if (k != -1)
                    {
                        char c = (char)(k & 0xFF);

                        if (mapping == Key.Unmapped)
                            RatCow.Controls.GraphicContext.Instance.RenderText(c);
                        else
                            RatCow.Controls.GraphicContext.Instance.RenderKey(mapping, false, false, false);

                    }

                    lastkey = k;
                }

                AllegroAPI.vsync();
            }
            while (!exit);
        }

        private Key TranslateKey()
        {
            if (AllegroAPI.key[AllegroAPI.KEY_DEL])
                return Key.Delete;
            else if (AllegroAPI.key[AllegroAPI.KEY_BACKSPACE])
                return Key.BackSpace;
            else if (AllegroAPI.key[AllegroAPI.KEY_LEFT])
                return Key.Left;
            else if (AllegroAPI.key[AllegroAPI.KEY_RIGHT])
                return Key.Right;
            else
                return Key.Unmapped;

        }

        void button1_Click(object sender)
        {
            (sender as Button).Text = "Blip!";
        }
    }
}
