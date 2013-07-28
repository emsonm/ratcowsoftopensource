/*
 * SharpAllegro: a C# wrapper around Allegro game library.
 * Copyright (C) 2007  Eugenio Favalli
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation;
 * version 2.1 of the License.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 * 
 * $Id: Test.cs 117 2011-01-10 13:23:17Z eugeniofavalli $
 * 
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using sharpallegro;

namespace sharpallegro_test
{
    class Test : Allegro
    {
        public static void close_button_callback()
        {
            allegro_message("close_button_callback");
        }

        public static void Allegro()
        {
            set_window_title("set_window_title");
            set_close_button_callback(new CloseButtonCallback(close_button_callback));
            check_cpu();

            string message = string.Empty;
            message += "allegro_id: " + allegro_id + "\n";
            message += "allegro_error: " + allegro_error + "\n";
            message += "ALLEGRO_VERSION: " + ALLEGRO_VERSION + "\n";
            message += "ALLEGRO_SUB_VERSION: " + ALLEGRO_SUB_VERSION + "\n";
            message += "ALLEGRO_WIP_VERSION: " + ALLEGRO_WIP_VERSION + "\n";
            message += "ALLEGRO_VERSION_STR: " + ALLEGRO_VERSION_STR + "\n";
            message += "ALLEGRO_DATE_STR: " + ALLEGRO_DATE_STR + "\n";
            message += "ALLEGRO_DATE: " + ALLEGRO_DATE + "\n";
            string os = string.Empty;
            if (os_type == OSTYPE_WIN3) os = "OSTYPE_WIN3";
            else if (os_type == OSTYPE_WIN95) os = "OSTYPE_WIN95";
            else if (os_type == OSTYPE_WIN98) os = "OSTYPE_WIN98";
            else if (os_type == OSTYPE_WINME) os = "OSTYPE_WINME";
            else if (os_type == OSTYPE_WINNT) os = "OSTYPE_WINNT";
            else if (os_type == OSTYPE_WIN2000) os = "OSTYPE_WIN2000";
            else if (os_type == OSTYPE_WINXP) os = "OSTYPE_WINXP";
            else if (os_type == OSTYPE_WIN2003) os = "OSTYPE_WIN2003";
            else if (os_type == OSTYPE_WINVISTA) os = "OSTYPE_WINVISTA";
            else os = "OSTYPE_UNKNOWN";
            message += "os_type: " + os + "\n";
            message += "os_version: " + os_version + "\n";
            message += "os_revision: " + os_revision + "\n";
            message += "os_multitasking: " + os_multitasking + "\n";
            message += "desktop_color_depth: " + desktop_color_depth() + "\n";
            int width, height;
            get_desktop_resolution(out width, out height);
            message += "get_desktop_resolution: " + width + "x" + height + "\n";
            message += "cpu_vendor: " + cpu_vendor + "\n";
            message += "cpu_family: " + cpu_family + "\n";
            message += "cpu_model: " + cpu_model + "\n";
            message += "cpu_capabilities: " + cpu_capabilities + "\n";

            allegro_message(message);
        }

        static int test_proc(int msg, IntPtr d, int c)
        {
            return D_O_K;
        }

        static DIALOG_PROC d_test_proc = new DIALOG_PROC(test_proc);

        //static GuiMouseCallback m_x = new GuiMouseCallback(get_m_x);
        //static int get_m_x()
        //{
        //    return mouse_x;
        //}

        static int update_color_value(IntPtr dp3, int val)
        {
            return D_O_K;
        }
        //static UpdateColorValue d_update_color_value = new UpdateColorValue(update_color_value);

        static string foobar(int index, IntPtr list_size)
        {
            if (index < 0)
            {
                Marshal.WriteInt32(list_size, 1);
                return null;
            }
            else
            {
                return "test";
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string ListCallback(int index, IntPtr list_size);
        static ListCallback d_foobar = new ListCallback(foobar);

        public static void Gui()
        {
            BITMAP buffer = create_bitmap(SCREEN_W, SCREEN_H);
            //gui_set_screen(buffer);

            //alert("CONFIG_FILE", "not found.", "Using defaults.", "&Continue", null, (int)'c', 0);

            BITMAP bmp1 = create_bitmap(180, 80);
            clear_to_color(bmp1, makecol(10, 10, 10));
            line(bmp1, 0, 0, 180, 80, makecol(128, 128, 128));
            line(bmp1, 180, 0, 0, 80, makecol(128, 128, 128));

            BITMAP bmp2 = create_bitmap(20, 20);
            clear_to_color(bmp2, makecol(0, 0, 0));
            circle(bmp2, 10, 10, 10, makecol(255, 0, 0));

            DIALOG test_dialog = new DIALOG(d_test_proc, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);
            //test_dialog.flags = D_HIDDEN;
            //object_message(test_dialog, MSG_START, 0);

            //return;

            //GCHandle handle = GCHandle.Alloc(d_test_proc, GCHandleType.Pinned);

            int ret = 0;

            //DIALOG d = new DIALOG(d_slider_proc, 0, 0, 0, 0, 0, 0, 0, 0, 100, 20, NULL, NULL, NULL);
            //DIALOG d = new DIALOG(d_slider_proc, 32, 16, 256, 16, 0, 255, 0, 0, 255, 0, NULL, NULL, NULL);
            //int ret = d_slider_proc(4, d, 1);

            //return;


            DIALOGS the_dialog = new DIALOGS(2);
            //the_dialog[0] = new DIALOG("d_clear_proc", 0, 0, 0, 0, 0, makecol(255, 255, 255), 0, 0, 0, 0, NULL, NULL, NULL);
            //the_dialog[1] = new DIALOG("d_box_proc", 10, 10, 180, 80, makecol(0, 255, 0), makecol(255, 0, 255), 0, 0, 0, 0, NULL, NULL, NULL);
            //the_dialog[2] = new DIALOG("d_shadow_box_proc", 20, 20, 180, 80, makecol(0, 0, 255), makecol(0, 255, 255), 0, 0, 0, 0, NULL, NULL, NULL);
            //the_dialog[3] = new DIALOG("d_bitmap_proc", 30, 30, 180, 80, 0, 0, 0, 0, 0, 0, bmp1, NULL, NULL);
            //the_dialog[4] = new DIALOG("d_text_proc", 10, 120, 180, 20, 0, makecol(255, 255, 255), 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("I guess this is text"), NULL, NULL);
            //the_dialog[5] = new DIALOG("d_ctext_proc", 10, 140, 180, 20, 0, makecol(255, 255, 255), 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("Centered"), NULL, NULL);
            //the_dialog[6] = new DIALOG("d_rtext_proc", 10, 160, 180, 20, 0, makecol(255, 255, 255), 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("Right"), NULL, NULL);
            //the_dialog[7] = new DIALOG("d_button_proc", 10, 180, 100, 20, makecol(255, 0, 0), makecol(0, 0, 255), 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("Push me"), NULL, NULL);
            //the_dialog[8] = new DIALOG("d_check_proc", 10, 200, 100, 20, 0, makecol(255, 255, 255), 0, D_SELECTED, 0, 0, Marshal.StringToCoTaskMemAnsi("&Check me"), NULL, NULL);
            //the_dialog[9] = new DIALOG("d_radio_proc", 10, 220, 100, 20, 0, makecol(255, 255, 255), 0, D_SELECTED, 1, 0, Marshal.StringToCoTaskMemAnsi("Radio1"), NULL, NULL);
            //the_dialog[10] = new DIALOG("d_radio_proc", 110, 220, 100, 20, 0, makecol(255, 255, 255), 0, 0, 1, 1, Marshal.StringToCoTaskMemAnsi("Radio2"), NULL, NULL);
            //the_dialog[11] = new DIALOG("d_icon_proc", 10, 240, 20, 20, 0, makecol(255, 255, 255), 0, 0, 1, 1, bmp2, NULL, NULL);
            //the_dialog[12] = new DIALOG("d_keyboard_proc", 0, 0, 0, 0, 0, 0, (int)'a', 0, 0, 0, NULL, NULL, NULL);
            //the_dialog[13] = new DIALOG("d_edit_proc", 10, 280, 100, 20, 0, makecol(255, 255, 255), 0, 0, 20, "Edit me".Length, Marshal.StringToCoTaskMemAnsi("Edit me"), NULL, NULL);
            the_dialog[0] = new DIALOG("d_list_proc", 0, 0, 200, 20, 0, makecol(255, 255, 255), 1, 1, 1, 1, Marshal.GetFunctionPointerForDelegate(d_foobar), NULL, NULL);
            //the_dialog[0].proc = Marshal.GetFunctionPointerForDelegate(d_test_proc);
            the_dialog[the_dialog.Length - 1] = new DIALOG(NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);



            DIALOG_PLAYER player = NULL;
            player = init_dialog(the_dialog, -1);
            while (update_dialog(player) > 0) ;
            int shutodown = shutdown_dialog(player);

            ret = do_dialog(the_dialog, -1);
            //init_dialog(the_dialog, -1);

            //d_test_proc(MSG_DRAW, d2, 0);
            //handle.Free();

            readkey();
            return;
        }

        static int check_callback()
        {
            alert("Menu item has been toggled!", null, null, "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_check_callback = new MenuCallback(check_callback);

        public static void Menu()
        {
            MENUS helpmenu = new MENUS(5);
            helpmenu[0] = new MENU("Test &1 \t1", NULL, NULL, D_DISABLED, NULL);
            helpmenu[1] = new MENU("Test &2 \t2", d_check_callback, NULL, D_SELECTED, NULL);
            helpmenu[2] = new MENU("", NULL, NULL, 0, NULL);
            helpmenu[3] = new MENU("&Quit \tq/Esc", NULL, NULL, 0, NULL);
            helpmenu[4] = new MENU(null, NULL, NULL, 0, NULL);

            int ret = do_menu(helpmenu, 10, 10);

        }

        static V3D_f[] quad = {
            new V3D_f(  10.0f,  10.0f, -10.0f, 0.0f, 0.0f, makecol(255, 255, 255) ),
            new V3D_f( -10.0f,  10.0f, -10.0f, 0.0f, 0.0f, makecol(255, 255, 255) ),
            new V3D_f( -10.0f, -10.0f, -10.0f, 0.0f, 0.0f, makecol(255, 255, 255) ),
            new V3D_f(  10.0f, -10.0f, -10.0f, 0.0f, 0.0f, makecol(255, 255, 255) )
        };

        static void draw_cube(ref MATRIX_f matrix)
        {
            for (int j = 0; j < 4; j++)
            {
                apply_matrix_f(ref matrix, quad[j].x, quad[j].y, quad[j].z, ref quad[j].x, ref quad[j].y, ref quad[j].z);
            }

            for (int j = 0; j < 4; j++)
            {
                persp_project_f(quad[j].x, quad[j].y, quad[j].z, ref quad[j].x, ref quad[j].y);
            }

            ManagedPointer vtx = new ManagedPointer(4 * sizeof(Int32));
            for (int j = 0; j < 4; j++)
            {
                ManagedPointer v = new ManagedPointer(6 * sizeof(Int32));
                v.WriteInt(0, (int)quad[j].x);
                v.WriteInt(sizeof(Int32), (int)quad[j].y);
                v.WriteInt(2 * sizeof(Int32), (int)quad[j].z);
                v.WriteInt(3 * sizeof(Int32), (int)quad[j].u);
                v.WriteInt(4 * sizeof(Int32), (int)quad[j].v);
                v.WriteInt(5 * sizeof(Int32), (int)quad[j].c);
                vtx.WritePointer(j * sizeof(Int32), v);
            }

            if (scene_polygon3d_f(POLYTYPE_GCOL, NULL, 4, vtx) != 0) allegro_message("Lack of rendering routine");

        }

        public static void _3D()
        {
            BITMAP buffer = create_bitmap(SCREEN_W, SCREEN_H);
            create_scene(24, 6);
            set_projection_viewport(0, 0, SCREEN_W, SCREEN_H);
            MATRIX_f matrix = new MATRIX_f(), matrix_r = new MATRIX_f(), matrix_t = new MATRIX_f();
            int rx = 0, ry = 0, tz = 40;
            int rot = 0, inc = 1;
            int k = 1;

            while (!key[KEY_ESC])
            {
                clear_bitmap(buffer);
                clear_scene(buffer);

                get_rotation_matrix_f(ref matrix_r, rx, ry, 0);

                get_translation_matrix_f(ref matrix_t, 0, 0, tz + k * 40);

                matrix_mul_f(ref matrix_r, ref matrix_t, out matrix);

                matrix = matrix_r;

                draw_cube(ref matrix);

                render_scene();

                tz -= 2;
                if (tz == 0) tz = 40;
                rx += 4;
                ry += 4;
                rot += inc;
                if ((rot >= 25) || (rot <= -25)) inc = -inc;
            }
            destroy_bitmap(buffer);
            destroy_scene();
        }

        static void Direct(int bpp)
        {
            //COLOR_MAP _color_map = new COLOR_MAP();

            //for (int x = 0; x < 256; x++)
            //{
            //    for (int y = 0; y < 256; y++)
            //    {
            //        int xc = x & 31;
            //        int yc = y & 31;

            //        int xl = x >> 5;
            //        int yl = y >> 5;

            //        int c = 0;
            //        if (xc > 0)
            //            c = (xc + yc) / 2;
            //        else
            //            c = yc;

            //        int l = xl + yl;
            //        if (l > 7)
            //            l = 7;

            //        _color_map.data[x, y] = (byte)(c | (l << 5));
            //    }
            //}
            //color_map = _color_map;

            //drawing_mode(DRAW_MODE_TRANS, NULL, 0, 0);

            //BITMAP buffer = create_bitmap(256, 256);
            //int step = bpp / 8;
            //for (int y = 0; y < buffer.h; y++)
            //{
            //    for (int x = 0; x < buffer.w; x++)
            //    {
            //        ManagedPointer line = buffer.line.Offset(256 * y * step);
            //        ManagedPointer dot = new ManagedPointer(line.Offset(x * step));
            //        if (bpp == 8)
            //        {
            //            dot.WriteByte(0, (byte)(x % 256));
            //        }
            //        else
            //        {
            //            dot.WriteInt(0, makecol(x % 256, y % 256, 0));
            //        }
            //    }
            //}
            //blit(buffer, screen, 0, 0, 0, 0, 256, 256);

            int AMBIENT_LIGHT = 0x20;
            int color = makecol(255, 0, 0);
            BITMAP bmp = create_bitmap(90, 30);
            clear_to_color(bmp, 0);
            for (int y = 0; y < bmp.h; y++)
            {
                for (int x = 0; x < bmp.w / 3; x++)
                {
                    int c = getpixel(bmp, x, y);

                    int r = getr_depth(bpp, c);
                    int g = getg_depth(bpp, c);
                    int b = getb_depth(bpp, c);

                    r = (r * 31 / 255) | AMBIENT_LIGHT;
                    g = (g * 31 / 255) | AMBIENT_LIGHT;
                    b = (b * 31 / 255) | AMBIENT_LIGHT;

                    //ManagedPointer line = new ManagedPointer(bmp.line).Offset(y * bmp.w * sizeof(byte));
                    //line.WriteByte(x * 3 * sizeof(byte), (byte)color);
                    //line.WriteByte((x * 3 + 1) * sizeof(byte), (byte)color);
                    //line.WriteByte((x * 3 + 2) * sizeof(byte), (byte)0);

                    ManagedBytePointerArray line = new ManagedBytePointerArray(bmp.line).Offset(y * bmp.w * sizeof(byte));
                    line[x * 3] = (byte)r;
                    line[x * 3 + 1] = (byte)g;
                    line[x * 3 + 2] = (byte)b;

                    //putpixel(bmp, x * 3, y, color);
                    //putpixel(bmp, x * 3 + 1, y, color);
                    //putpixel(bmp, x * 3 + 2, y, color);
                }
            }
            blit(bmp, screen, 0, 0, 0, 0, bmp.w, bmp.h);

            readkey();
        }

        static int ftofix2(float x)
        {
            return (int)(x * 65536.0 + (x < 0 ? -0.5 : 0.5));
        }

        static void Fixed()
        {
            double v = 63.5;
            int f = ftofix(v);
            int t = fixtan(f);
            double r = fixtof(t);


            /* declare three 32 bit (16.16) fixed point variables */
            int x, y, z;

            /* convert integers to fixed point like this */
            x = itofix(10);

            /* convert floating point to fixed point like this */
            y = ftofix(3.14);
            int y2 = ftofix2(3.14f);

            /* fixed point variables can be assigned, added, subtracted, negated,
             * and compared just like integers, eg: 
             */
            z = x + y;
            //allegro_message(string.Format("{0} + {1} = {2}\n", fixtof(x), fixtof(y), fixtof(z)));
            //allegro_message(string.Format("{0} + {1} = {2}\n", fixtof(x), fixtof(y2), fixtof(z)));

            /* you can't add integers or floating point to fixed point, though:
             *    z = x + 3;
             * would give the wrong result.
             */

            /* fixed point variables can be multiplied or divided by integers or
             * floating point numbers, eg:
             */
            z = y * 2;
            //allegro_message(string.Format("{0} * 2 = {1}\n", fixtof(y), fixtof(z)));

            /* you can't multiply or divide two fixed point numbers, though:
             *    z = x * y;
             * would give the wrong result. Use fixmul() and fixdiv() instead, eg:
             */
            z = fixmul(x, y);
            //allegro_message(string.Format("{0} * {1} = {2}\n", fixtof(x), fixtof(y), fixtof(z)));

            /* fixed point trig and square root are also available, eg: */
            z = fixsqrt(x);
            //allegro_message(string.Format("fixsqrt({0}) = {1}\n", fixtof(x), fixtof(z)));
        }

        const int RODENTS = 4;
        static Random rand = new Random();


        /* We define dumb memory management operators in order
         * not to have to link against libstdc++ with GCC 3.x.
         * Don't do that in real-life C++ code unless you know
         * what you're doing.
         */
        //void *operator new(size_t size)
        //{
        //   return malloc(size);
        //}

        //void operator delete(void *ptr)
        //{
        //  if (ptr)
        //    free (ptr);
        //}


        /* Our favorite pet. */
        class rodent
        {
            public rodent()
            {
            }
            public rodent(BITMAP bmp)
            {
                //x = rand.Next() % (SCREEN_W - bmp.w);
                //y = rand.Next() % (SCREEN_H - bmp.h);

                do
                {
                    delta_x = (rand.Next() % 11) - 5;
                } while (delta_x == 0);

                do
                {
                    delta_y = (rand.Next() % 11) - 5;
                } while (delta_y == 0);

                //sprite = bmp;
            }
            public void move()
            {
                if ((x + sprite.w + delta_x >= SCREEN_W) || (x + delta_x < 0)) delta_x = -delta_x;
                if ((y + sprite.h + delta_y >= SCREEN_H) || (y + delta_y < 0)) delta_y = -delta_y;

                x += delta_x;
                y += delta_y;
            }
            public void draw(BITMAP bmp)
            {
                draw_sprite(bmp, sprite, x, y);
            }

            private int x, y;
            private int delta_x, delta_y;
            private BITMAP sprite;
        }

        /* Yup, you read correctly, we're creating the World here. */
        class world
        {
            /* Genesis */
            public world()
            {
                mouse = new rodent[RODENTS];
                PALETTE pal = new PALETTE();
                active = TRUE;

                dbuffer = create_bitmap(SCREEN_W, SCREEN_H);
                mouse_sprite = load_bitmap("./examples/mysha.pcx", NULL);

                if (!mouse_sprite)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Error loading bitmap\n{0}\n", allegro_error));
                    //exit(1);
                }

                set_palette(pal);

                for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                    mouse[what_mouse] = new rodent(mouse_sprite);
            }
            /* Apocalypse */
            ~world()
            {
                //destroy_bitmap(dbuffer);
                //destroy_bitmap(mouse_sprite);

                //for(int what_mouse=0; what_mouse < RODENTS; what_mouse++)
                //   delete mouse[what_mouse];
            }
            public void draw()
            {
                clear_bitmap(dbuffer);

                //for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                //    mouse[what_mouse].draw(dbuffer);

                blit(dbuffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
            }
            public void logic()
            {
                if (key[KEY_ESC]) active = FALSE;

                //for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                //    mouse[what_mouse].move();
            }
            public void loop()
            {
                //install_int_ex(t_my_timer_handler, BPS_TO_TIMER(10));

                while (active == TRUE)
                {
                    //while (counter > 0)
                    //{
                    //    counter--;
                    //    logic();
                    //}
                    draw();
                }

                //remove_int(t_my_timer_handler);
            }

            private static BITMAP dbuffer;
            private static BITMAP mouse_sprite;
            private static int active;
            private static rodent[] mouse = new rodent[RODENTS];
        }

        static void Main()
        {
            allegro_init();
            if (install_keyboard() != 0)
            {
                return;
            }
            install_mouse();
            install_timer();

            int bpp = 32;
            set_color_depth(bpp);
            if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 640, 480, 0, 0) != 0)
            {
                allegro_message("Error: " + allegro_error);
            }

            //Allegro();
            //Gui();
            //Menu();
            //_3D();
            //Direct(bpp);
            //Fixed();

            BITMAP source = create_bitmap(24, 48);
            clear_to_color(source, makecol(0, 0, 255));
            RLE_SPRITE sprite = get_rle_sprite(source);
            COMPILED_SPRITE compiled = get_compiled_sprite(source, FALSE);

            draw_sprite(screen, source, 64, 64);
            draw_rle_sprite(screen, sprite, 32, 32);
            rectfill(screen, 96, 96, 128, 128, makecol(255, 0, 0));
            draw_compiled_sprite(screen, compiled, 160, 160);
            
            readkey();

            //world game = new world();
            //game.Dispose();

            allegro_exit();
        }
    }
}

