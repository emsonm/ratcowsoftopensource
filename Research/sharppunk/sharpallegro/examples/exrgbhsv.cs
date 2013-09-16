using System;
using System.Runtime.InteropServices;

using sharpallegro;

namespace exrgbhsv
{
    class exrgbhsv : Allegro
    {
        const int DIALOG_NUM_SLIDERS = 6;
        const int DIALOG_FIRST_COLOR_BAR = 12;
        const int DIALOG_COLOR_BOX = 18;

        /* slider types (R, G, B, and H, S, V) */
        const int S_R = 0;
        const int S_G = 1;
        const int S_B = 2;
        const int S_H = 3;
        const int S_S = 4;
        const int S_V = 5;



        /* the current color values */
        static int[] colors = 
        {
           255,     /* red */
           255,     /* green */
           255,     /* blue */
           0,       /* hue */
           0,       /* saturation */
           255,     /* value */
        };
        static ManagedPointer _colors = new ManagedPointer(6 * sizeof(Int32));


        /* the bitmaps containing the color-bars */
        static BITMAP[] color_bar_bitmap = new BITMAP[DIALOG_NUM_SLIDERS];

#if ZERO
/* RGB -> color mapping table. Not needed, but speeds things up in 8-bit mode */
RGB_MAP rgb_table;
#endif

        //int my_slider_proc(int msg, IntPtr d, int c);
        //int update_color_value(IntPtr dp3, int val);



        //DIALOG the_dlg[] =
        //{
        //   /* (dialog proc)     (x)   (y)   (w)   (h)   (fg)  (bg)  (key) (flags)  (d1)  (d2)  (dp)     (dp2)          (dp3) */
        //   { my_slider_proc,    32,   16,   256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_R]  },
        //   { my_slider_proc,    32,   64,   256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_G]  },
        //   { my_slider_proc,    32,   112,  256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_B]  },
        //   { my_slider_proc,    352,  336,  256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_H]  },
        //   { my_slider_proc,    352,  384,  256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_S]  },
        //   { my_slider_proc,    352,  432,  256,  16,   0,    255,  0,    0,       255,  0,    NULL,    (void *)update_color_value,  &colors[S_V]  },
        //   { d_text_proc,       308,  22,   0,    0,    0,    255,  0,    0,       0,    0,    "R",     NULL,          NULL          },
        //   { d_text_proc,       308,  70,   0,    0,    0,    255,  0,    0,       0,    0,    "G",     NULL,          NULL          },
        //   { d_text_proc,       308,  118,  0,    0,    0,    255,  0,    0,       0,    0,    "B",     NULL,          NULL          },
        //   { d_text_proc,       326,  342,  0,    0,    0,    255,  0,    0,       0,    0,    "H",     NULL,          NULL          },
        //   { d_text_proc,       326,  390,  0,    0,    0,    255,  0,    0,       0,    0,    "S",     NULL,          NULL          },
        //   { d_text_proc,       326,  438,  0,    0,    0,    255,  0,    0,       0,    0,    "V",     NULL,          NULL          },
        //   { d_bitmap_proc,     32,   32,   256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_bitmap_proc,     32,   80,   256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_bitmap_proc,     32,   128,  256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_bitmap_proc,     352,  352,  256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_bitmap_proc,     352,  400,  256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_bitmap_proc,     352,  448,  256,  16,   0,    255,  0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { d_box_proc,        170,  170,  300,  140,  0,    0,    0,    0,       0,    0,    NULL,    NULL,          NULL          },
        //   { NULL,              0,    0,    0,    0,    0,    0,    0,    0,       0,    0,    NULL,    NULL,          NULL          }
        //};

        static DIALOGS the_dlg = new DIALOGS(20);



        /* helper that updates the color box */
        static void update_color_rectangle(int r, int g, int b)
        {
            the_dlg[DIALOG_COLOR_BOX].bg = makecol(r, g, b);
        }



        /* helper that updates all six color-bar bitmaps according to the
         * given RGB and HSV values */
        static void update_color_bars(int r, int g, int b, float h, float s, float v)
        {
            int i;
            int hr, hg, hb;   /* Temp RGB values for drawing the HSV sliders */


            for (i = 0; i < 256; i++)
            {
                /* Red color-bar */
                vline(color_bar_bitmap[S_R], i, 0, 15, makecol32(i, g, b));

                /* Green color-bar */
                vline(color_bar_bitmap[S_G], i, 0, 15, makecol32(r, i, b));

                /* Blue color-bar */
                vline(color_bar_bitmap[S_B], i, 0, 15, makecol32(r, g, i));

                /* Hue color-bar */
                hsv_to_rgb(i * 360.0f / 255.0f, s, v, out hr, out hg, out hb);
                vline(color_bar_bitmap[S_H], i, 0, 15, makecol32(hr, hg, hb));

                /* Saturation color-bar */
                hsv_to_rgb(h, i / 255.0f, v, out hr, out hg, out hb);
                vline(color_bar_bitmap[S_S], i, 0, 15, makecol32(hr, hg, hb));

                /* Value color-bar */
                hsv_to_rgb(h, s, i / 255.0f, out hr, out hg, out hb);
                vline(color_bar_bitmap[S_V], i, 0, 15, makecol32(hr, hg, hb));
            }
        }

        /* helper for reacting to the changing one of the sliders */
        static int update_color_value(IntPtr dp3, int val)
        {
            /* 'val' is the value of the slider's position (0-255),
               'type' is which slider was changed */
            //int type = ((uintptr_t)dp3 - (uintptr_t)colors) / sizeof(colors[0]);
            int type = (dp3.ToInt32() - _colors.pointer.ToInt32()) / sizeof(Int32);
            int r, g, b;
            float h = 0f, s = 0f, v = 0f;

            if (colors[type] != val)
            {
                colors[type] = val;

                if ((type == S_R) || (type == S_G) || (type == S_B))
                {
                    /* The slider that's changed is either R, G, or B.
                       Convert RGB color to HSV. */
                    r = colors[S_R];
                    g = colors[S_G];
                    b = colors[S_B];

                    rgb_to_hsv(r, g, b, ref h, ref s, ref v);

                    colors[S_H] = (int)(h * 255.0f / 360.0f + 0.5f);
                    colors[S_S] = (int)(s * 255.0f + 0.5f);
                    colors[S_V] = (int)(v * 255.0f + 0.5f);
                }
                else
                {
                    /* The slider that's changed is either H, S, or V.
                       Convert HSV color to RGB. */
                    h = colors[S_H] * 360.0f / 255.0f;
                    s = colors[S_S] / 255.0f;
                    v = colors[S_V] / 255.0f;

                    hsv_to_rgb(h, s, v, out r, out g, out b);

                    colors[S_R] = r;
                    colors[S_G] = g;
                    colors[S_B] = b;
                }

                update_color_bars(r, g, b, h, s, v);

                vsync();

                if (get_color_depth() == 8)
                {
                    /* set the screen background to the new color if in paletted mode */
                    RGB rgb = new RGB();
                    rgb.r = (byte)(colors[S_R] >> 2);
                    rgb.g = (byte)(colors[S_G] >> 2);
                    rgb.b = (byte)(colors[S_B] >> 2);

                    set_color(255, rgb);   /* in 8-bit color modes, we're changing the
                    'white' background-color */
                }

                /* Update the rectangle in the middle to the new color */
                update_color_rectangle(r, g, b);
                object_message(the_dlg[DIALOG_COLOR_BOX], MSG_DRAW, 0);

                /* Display the updated color-bar bitmaps */
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR], MSG_DRAW, 0);
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR + 1], MSG_DRAW, 0);
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR + 2], MSG_DRAW, 0);
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR + 3], MSG_DRAW, 0);
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR + 4], MSG_DRAW, 0);
                object_message(the_dlg[DIALOG_FIRST_COLOR_BAR + 5], MSG_DRAW, 0);
            }

            return D_O_K;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int UpdateColorValue(IntPtr dp3, int val);
        static UpdateColorValue d_update_color_value = new UpdateColorValue(update_color_value);



        /* gui object procedure for the color selection sliders */
        static int my_slider_proc(int msg, IntPtr _d, int c)
        {
            DIALOG d = (DIALOG)_d;

            //int* color = (int*)d->dp3;
            int color = Marshal.ReadInt32(d.dp3);

            switch (msg)
            {

                case MSG_START:
                    /* initialise the slider position */
                    //d->d2 = *color;
                    d.d2 = color;
                    break;

                // TODO: check why this one seem to reset the positiondk
                //case MSG_IDLE:
                //    /* has the slider position changed? */
                //    //if (d->d2 != *color)
                //    if (d.d2 != color)
                //    {
                //        //d->d2 = *color;
                //        d.d2 = color;
                //        object_message(d, MSG_DRAW, 0);
                //    }
                //    break;
            }

            return d_slider_proc(msg, d, c);
        }

        static DIALOG_PROC d_my_slider_proc = new DIALOG_PROC(my_slider_proc);

        static int Main()
        {
            //IntPtr _colors = new Marshal.AllocCoTaskMem(6 * sizeof(Int32));
            _colors[0].WriteInt(0, 255);
            _colors[1].WriteInt(0, 255);
            _colors[2].WriteInt(0, 255);
            _colors[3].WriteInt(0, 0);
            _colors[4].WriteInt(0, 0);
            _colors[5].WriteInt(0, 255);
            the_dlg[0] = new DIALOG(d_my_slider_proc, 32, 16, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_R]);
            the_dlg[1] = new DIALOG(d_my_slider_proc, 32, 64, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_G]);
            the_dlg[2] = new DIALOG(d_my_slider_proc, 32, 112, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_B]);
            the_dlg[3] = new DIALOG(d_my_slider_proc, 352, 336, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_H]);
            the_dlg[4] = new DIALOG(d_my_slider_proc, 352, 384, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_S]);
            the_dlg[5] = new DIALOG(d_my_slider_proc, 352, 432, 256, 16, 0, 255, 0, 0, 255, 0, NULL, Marshal.GetFunctionPointerForDelegate(d_update_color_value), _colors[S_V]);
            the_dlg[6] = new DIALOG("d_text_proc", 308, 22, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("R"), NULL, NULL);
            the_dlg[7] = new DIALOG("d_text_proc", 308, 70, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("G"), NULL, NULL);
            the_dlg[8] = new DIALOG("d_text_proc", 308, 118, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("B"), NULL, NULL);
            the_dlg[9] = new DIALOG("d_text_proc", 326, 342, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("H"), NULL, NULL);
            the_dlg[10] = new DIALOG("d_text_proc", 326, 390, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("S"), NULL, NULL);
            the_dlg[11] = new DIALOG("d_text_proc", 326, 438, 0, 0, 0, 255, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("V"), NULL, NULL);
            the_dlg[12] = new DIALOG("d_bitmap_proc", 32, 32, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[13] = new DIALOG("d_bitmap_proc", 32, 80, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[14] = new DIALOG("d_bitmap_proc", 32, 128, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[15] = new DIALOG("d_bitmap_proc", 352, 352, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[16] = new DIALOG("d_bitmap_proc", 352, 400, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[17] = new DIALOG("d_bitmap_proc", 352, 448, 256, 16, 0, 255, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[18] = new DIALOG("d_box_proc", 170, 170, 300, 140, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dlg[19] = new DIALOG(NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);

            int i;

            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            install_mouse();
            install_timer();

            /* Set the deepest color depth we can set */
            set_color_depth(32);
            if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
            {
                set_color_depth(24);
                if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
                {
                    set_color_depth(16);
                    if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
                    {
                        set_color_depth(15);
                        if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
                        {
                            set_color_depth(8);
                            if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
                            {
                                allegro_message(string.Format("Error setting a graphics mode\n{0}\n",
                                        allegro_error));
                                return 1;
                            }
                        }
                    }
                }
            }

            /* In the case we're using an 8-bit color screen, we must set up the palette */
            if (get_color_depth() == 8)
            {
                PALETTE pal332 = new PALETTE();
                generate_332_palette(pal332);

                /* Set the palette to the best approximation of a truecolor palette
                   we can get with 8-bit color */
                set_palette(pal332);

                /* In 8-bit color mode, if there's an RGB table, the sliders
               will move a lot more smoothly and the updating will be
               a lot quicker. But if there's no RGB table, this has the
               advantage that the color conversion routines will take into
               account any changes in the background color. Instead of
               changing background color, we could also rely on the colored
               rectangle like in the other color modes, but using the 3-3-2
               palette, this doesn't display the color as accurately as
               changing the background color. */
#if ZERO
      /* Create an RGB table to speedup makecol8() */
      create_rgb_table(&rgb_table, pal332, NULL);
      rgb_map = &rgb_table;
#endif
            }

            clear_to_color(screen, makecol(255, 255, 255));

            /* color the dialog controls appropriately */
            /* R -> Red */
            the_dlg[S_R].fg = the_dlg[DIALOG_NUM_SLIDERS + S_R].fg = makecol(255, 0, 0);
            /* G -> Green */
            the_dlg[S_G].fg = the_dlg[DIALOG_NUM_SLIDERS + S_G].fg = makecol(0, 255, 0);
            /* B -> Blue */
            the_dlg[S_B].fg = the_dlg[DIALOG_NUM_SLIDERS + S_B].fg = makecol(0, 0, 255);

            /* H -> Grey */
            the_dlg[S_H].fg = the_dlg[DIALOG_NUM_SLIDERS + S_H].fg = makecol(192, 192, 192);
            /* S -> Dark Grey */
            the_dlg[S_S].fg = the_dlg[DIALOG_NUM_SLIDERS + S_S].fg = makecol(128, 128, 128);
            /* V -> Black */
            the_dlg[S_V].fg = the_dlg[DIALOG_NUM_SLIDERS + S_V].fg = makecol(0, 0, 0);

            /* Create the bitmaps for the color-bars */
            for (i = 0; i < DIALOG_NUM_SLIDERS; i++)
            {
                if (!(color_bar_bitmap[i] = create_bitmap_ex(32, 256, 16)))
                {
                    allegro_message("Error creating a color-bar bitmap\n");
                    return 1;
                }

                the_dlg[DIALOG_FIRST_COLOR_BAR + i].dp = color_bar_bitmap[i];
            }

            for (i = 0; i < DIALOG_NUM_SLIDERS * 3; i++)
                the_dlg[i].bg = makecol(255, 255, 255);

            the_dlg[DIALOG_COLOR_BOX].fg = makecol(0, 0, 0);

            textout_ex(screen, font, "RGB<->HSV color spaces example.", 344, 4,
                   makecol(0, 0, 0), -1);
            textout_ex(screen, font, "Drag sliders to change color values.", 344, 12,
                   makecol(0, 0, 0), -1);

            textout_ex(screen, font, "The color-bars beneath the sliders", 24, 384,
                   makecol(128, 128, 128), -1);
            textout_ex(screen, font, "show what the resulting color will", 24, 392,
                   makecol(128, 128, 128), -1);
            textout_ex(screen, font, "look like when the slider is", 24, 400,
                   makecol(128, 128, 128), -1);
            textout_ex(screen, font, "dragged to that position.", 24, 408,
                   makecol(128, 128, 128), -1);

            switch (get_color_depth())
            {

                case 32:
                    textout_ex(screen, font, "Running in truecolor (32-bit 888)", 352, 24,
                           makecol(128, 128, 128), -1);
                    textout_ex(screen, font, "16777216 colors", 352, 32,
                           makecol(128, 128, 128), -1);
                    break;

                case 24:
                    textout_ex(screen, font, "Running in truecolor (24-bit 888)", 352, 24,
                           makecol(128, 128, 128), -1);
                    textout_ex(screen, font, "16777216 colors", 352, 32,
                           makecol(128, 128, 128), -1);
                    break;

                case 16:
                    textout_ex(screen, font, "Running in hicolor (16-bit 565)", 352, 24,
                           makecol(128, 128, 128), -1);
                    textout_ex(screen, font, "65536 colors", 352, 32,
                           makecol(128, 128, 128), -1);
                    break;

                case 15:
                    textout_ex(screen, font, "Running in hicolor (15-bit 555)", 352, 24,
                           makecol(128, 128, 128), -1);
                    textout_ex(screen, font, "32768 colors", 352, 32,
                           makecol(128, 128, 128), -1);
                    break;

                case 8:
                    textout_ex(screen, font, "Running in paletted mode (8-bit 332)", 352, 24,
                           makecol(128, 128, 128), -1);
                    textout_ex(screen, font, "256 colors", 352, 32,
                           makecol(128, 128, 128), -1);
                    break;

                default:
                    textout_ex(screen, font, "Unknown color depth", 400, 16, 0, -1);
                    break;
            }

            update_color_rectangle(colors[S_R], colors[S_G], colors[S_B]);
            update_color_bars(colors[S_R], colors[S_G], colors[S_B],
                      colors[S_H] * 360.0f / 255.0f,
                      colors[S_S] / 255.0f, colors[S_V] / 255.0f);

            do_dialog(the_dlg, -1);

            for (i = 0; i < DIALOG_NUM_SLIDERS; i++)
                destroy_bitmap(color_bar_bitmap[i]);

            return 0;
        }
    }
}
