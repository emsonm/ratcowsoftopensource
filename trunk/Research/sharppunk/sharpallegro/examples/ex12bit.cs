using sharpallegro;

namespace ex12bit
{
    class ex12bit : Allegro
    {
        /* declare the screen size and mask colour we will use */
        const int GFXW = 320;
        const int GFXH = 480;
        const int MASK_COLOR_12 = 0xFFE0;


        /* these are specific to this example. They say how big the balls are */
        const int BALLW = 12;
        const int BALLH = 24;
        const int BIGBALLW = 20;
        const int BIGBALLH = 40;
        const string MESSAGE_STR = "Allegro";


        public struct POINT_T
        {
            public int x, y;
            public int c;
        }


        /* these functions can be used in any 12-bit program */
        //int makecol12(int r, int g, int b);
        //void set_12bit_palette();
        //BITMAP create_bitmap_12(int w, int h);


        /* these functions are just for this example */
        //void blur_12(BITMAP bmp, BITMAP back);
        //void rgb_scales_12(BITMAP bmp, int ox, int oy, int w, int h);
        //POINT_T make_points(ref int numpoints, string msg);
        //BITMAP make_ball(int w, int h, int br, int bg, int bb);



        /* construct the magic palette that makes it all work */
        static void set_12bit_palette()
        {
            int r, g, b;
            PALETTE pal = new PALETTE();

            for (b = 0; b < 15; b++)
            {
                for (g = 0; g < 16; g++)
                {
                    pal[b * 16 + g].r = 0;
                    pal[b * 16 + g].g = (byte)(g * 63 / 15);
                    pal[b * 16 + g].b = (byte)(b * 63 / 15);
                }
            }

            for (r = 0; r < 16; r++)
            {
                pal[r + 240].r = (byte)(r * 63 / 15);
                pal[r + 240].g = 0;
                pal[r + 240].b = 0;
            }

            set_palette(pal);
        }



        /* the other magic routine - use this to make colours instead of makecol */
        static int makecol12(int r, int g, int b)
        {
            /* returns a 16-bit integer - here's the format:
             *
             *    0xARBG - where A=0xf (reserved, if you like),
             *                   R=red (0-15)
             *                   B=blue (0-14)
             *                   G=green (0-15)
             */

            r = r * 16 / 256;
            g = g * 16 / 256;
            b = b * 16 / 256 - 1;

            if (b < 0)
                b = 0;

            return (r << 8) | (b << 4) | g | 0xF000;
        }



        /* extract red component from color */
        static int getr12(int color)
        {
            return (color >> 4) & 0xF0;
        }



        /* extract green component from color */
        static int getg12(int color)
        {
            return (color << 4) & 0xF0;
        }



        /* extract blue component from color */
        static int getb12(int color)
        {
            return (color & 0xF0);
        }



        /* use this instead of create_bitmap, because the vtable needs changing
         * so that the drawing functions will use the 16-bit functions.
         */
        static BITMAP create_bitmap_12(int w, int h)
        {
            BITMAP bmp;

            bmp = create_bitmap_ex(16, w, h);

            if (bmp)
            {
                ((GFX_VTABLE)bmp.vtable).color_depth = 12;
                ((GFX_VTABLE)bmp.vtable).mask_color = MASK_COLOR_12;
            }

            return bmp;
        }



        /* this merges 'bmp' into 'back'. This is how the trails work */
        static void blur_12(BITMAP bmp, BITMAP back)
        {
            int x, y, r1, g1, b1, r2, g2, b2, c1, c2;

            // TODO: check why I had to add "2 *"
            for (y = 0; y < 2 * bmp.h; y++)
            {
                ManagedPointerArray backline = back.line[y];
                ManagedPointerArray bmpline = bmp.line[y];

                for (x = 0; x < bmp.w / 2; x++)
                {
                    /* first get the pixel from each bitmap, then move the first
                     * colour value slightly towards the second.
                     */
                    c1 = bmpline[x];
                    c2 = backline[x];
                    r1 = c1 & 0xF00;
                    r2 = c2 & 0xF00;
                    if (r1 < r2)
                        c1 += 0x100;
                    else if (r1 > r2)
                        c1 -= 0x100;

                    b1 = c1 & 0xF0;
                    b2 = c2 & 0xF0;
                    if (b1 < b2)
                        c1 += 0x10;
                    else if (b1 > b2)
                        c1 -= 0x10;

                    g1 = c1 & 0x0F;
                    g2 = c2 & 0x0F;
                    if (g1 < g2)
                        c1 += 0x01;
                    else if (g1 > g2)
                        c1 -= 0x01;

                    /* then put it back in the bitmap */
                    bmpline[x] = (ushort)c1;
                }
            }
        }



        /* generates some nice RGB scales onto the specified bitmap */
        static void rgb_scales_12(BITMAP bmp, int ox, int oy, int w, int h)
        {
            int x, y;

            for (y = 0; y < h; y++)
                for (x = 0; x < w; x++)
                    putpixel(bmp, ox + x, oy + y, makecol12(x * 256 / w, y * 256 / h, 0));

            for (y = 0; y < h; y++)
                for (x = 0; x < w; x++)
                    putpixel(bmp, ox + x + w, oy + y, makecol12(x * 256 / w, 0, y * 256 / h));

            for (y = 0; y < h; y++)
                for (x = 0; x < w; x++)
                    putpixel(bmp, ox + x, oy + y + h, makecol12(0, x * 256 / w, y * 256 / h));

            for (y = 0; y < h; y++)
                for (x = 0; x < w; x++)
                    putpixel(bmp, ox + x + w, oy + y + h, makecol12(x * 128 / w + y * 128 / h,
                              x * 128 / w + y * 128 / h,
                              x * 128 / w + y * 128 / h));
        }



        /* turns the string in 'msg' into a series of 2D points. These can then
         * be drawn with the vector balls.
         */
        static POINT_T[] make_points(ref int numpoints, string msg)
        {
            BITMAP bmp;
            POINT_T[] points;
            int n, x, y;

            bmp = create_bitmap_ex(8, text_length(font, msg), text_height(font));
            clear_bitmap(bmp);
            textout_ex(bmp, font, msg, 0, 0, 1, -1);

            /* first, count how much memory we will need to reserve */
            n = 0;

            for (y = 0; y < bmp.h; y++)
                for (x = 0; x < bmp.w; x++)
                    if (getpixel(bmp, x, y) != 0)
                        n++;

            points = new POINT_T[n];

            /* then redo it all, but actually store the points this time */
            n = 0;

            for (y = 0; y < bmp.h; y++)
            {
                for (x = 0; x < bmp.w; x++)
                {
                    if (getpixel(bmp, x, y) != 0)
                    {
                        points[n].x = itofix(x - bmp.w / 2) * 6;
                        points[n].y = itofix(y - bmp.h / 2) * 12;
                        points[n].c = AL_RAND() % 4;
                        n++;
                    }
                }
            }

            numpoints = n;

            destroy_bitmap(bmp);

            return points;
        }



        /* this draws a vector ball. br/bg/bg is the colour of the brightest spot */
        static BITMAP make_ball(int w, int h, int br, int bg, int bb)
        {
            BITMAP bmp;
            int r, rx, ry;
            bmp = create_bitmap_12(w, h);

            clear_to_color(bmp, MASK_COLOR_12);

            for (r = 0; r < 16; r++)
            {
                rx = w * (15 - r) / 32;
                ry = h * (15 - r) / 32;
                ellipsefill(bmp, w / 2, h / 2, rx, ry, makecol12(br * r / 15, bg * r / 15, bb * r / 15));
            }

            return bmp;
        }



        static int Main()
        {
            BITMAP rgbpic, buffer, bigball;
            BITMAP[] ball = new BITMAP[4];
            int x, r = 0, g = 0, b = 0, numpoints = 0, thispoint;
            int xangle, yangle, zangle, newx = 0, newy = 0, newz = 0;
            GFX_VTABLE orig_vtable;
            POINT_T[] points;
            MATRIX m = new MATRIX();

            if (allegro_init() != 0)
                return 1;
            install_keyboard();

            /* first set your graphics mode as normal, except twice as wide because
             * we are using 2-bytes per pixel, but the graphics card doesn't know this.
             */
            if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, GFXW * 2, GFXH, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error setting {0}x{1}x12 (really {2}x{3}x8, but we fake " +
                    "it):\n{4}\n", GFXW, GFXH, GFXW * 2, GFXH,
                    allegro_error));
                return 1;
            }

            /* then set your magic palette. From now on you can't use the set_color
             * or set_palette functions or they will mess up this palette. You can
             * still use the fade routines, if you make sure you fade back into this
             * palette.
             */
            set_12bit_palette();

            /* then hack the vtable so it uses the 16-bit functions */
            orig_vtable = (GFX_VTABLE)((BITMAP)screen).vtable;

#if ALLEGRO_COLOR16
      screen->vtable = &__linear_vtable16;
#endif

            ((GFX_VTABLE)((BITMAP)screen).vtable).color_depth = 12;
            ((GFX_VTABLE)((BITMAP)screen).vtable).mask_color = MASK_COLOR_12;
            ((GFX_VTABLE)((BITMAP)screen).vtable).unwrite_bank = orig_vtable.unwrite_bank;
            ((BITMAP)screen).w /= sizeof(short);

            /* reset the clip window to it's new parameters */
            set_clip_rect(screen, 0, 0, ((BITMAP)screen).w - 1, ((BITMAP)screen).h - 1);

            /* then generate 4 vector balls of different colours */
            for (x = 0; x < 4; x++)
            {
                switch (x)
                {
                    case 0: r = 255; g = 0; b = 0; break;
                    case 1: r = 0; g = 255; b = 0; break;
                    case 2: r = 0; g = 0; b = 255; break;
                    case 3: r = 255; g = 255; b = 0; break;
                }

                ball[x] = make_ball(BALLW, BALLH, r, g, b);
            }

            /* also make one big red vector ball */
            bigball = make_ball(BIGBALLW, BIGBALLH, 255, 0, 0);

            /* make the off-screen buffer that everything will be drawn onto */
            buffer = create_bitmap_12(GFXW, GFXH);

            /* convert the text message into the coordinates of the vector balls */
            points = make_points(ref numpoints, MESSAGE_STR);

            /* create the background picture */
            rgbpic = create_bitmap_12(GFXW, GFXH);

            rgb_scales_12(rgbpic, 0, 0, GFXW / 2, GFXH / 2);

            /* copy the background into the buffer */
            blit(rgbpic, buffer, 0, 0, 0, 0, GFXW, GFXH);

            xangle = yangle = zangle = 0;

            /* put a message in the top-left corner */
            textprintf_ex(rgbpic, font, 3, 3, makecol12(255, 255, 255), -1,
              string.Format("{0}x{1} 12-bit colour on an 8-bit card", GFXW, GFXH));
            textprintf_ex(rgbpic, font, 3, 13, makecol12(255, 255, 255), -1,
              "(3840 colours at once!)");

            while (!keypressed())
            {
                /* first, draw some vector balls moving in a circle round the edge */
                for (x = 0; x < itofix(256); x += itofix(32))
                {
                    masked_blit(bigball, buffer, 0, 0,
                          fixtoi(150 * fixcos(xangle + x)) + GFXW / 2 - BALLW / 2,
                          fixtoi(200 * fixsin(xangle + x)) + GFXH / 2 - BALLH / 2,
                          BIGBALLW, BIGBALLH);
                }

                /* rotate the vector balls */

                get_rotation_matrix(ref m, xangle, yangle, zangle);

                for (thispoint = 0; thispoint < numpoints; thispoint++)
                {
                    apply_matrix(ref m, points[thispoint].x,
                         points[thispoint].y,
                         0,
                         ref newx, ref newy, ref newz);

                    masked_blit(ball[points[thispoint].c], buffer, 0, 0,
                          fixtoi(newx) + GFXW / 2,
                          fixtoi(newy) + GFXH / 2, BALLW, BALLH);
                }

                /* then blur the buffer so it fades into the background picture */
                blur_12(buffer, rgbpic);

                /* finally copy everything to the screen */
                blit(buffer, screen, 0, 0, 0, 0, GFXW, GFXH);

                /* rotate it a bit more */
                xangle += itofix(1);
                yangle += itofix(1);
                zangle += itofix(1);
            }

            clear_keybuf();

            /* clean it all up */
            for (x = 0; x < 4; x++)
                destroy_bitmap(ball[x]);

            destroy_bitmap(bigball);
            //free(points);

            //destroy_bitmap(rgbpic);
            //destroy_bitmap(buffer);

            ((BITMAP)screen).vtable = orig_vtable;
            fade_out(4);

            return 0;
        }
    }
}
