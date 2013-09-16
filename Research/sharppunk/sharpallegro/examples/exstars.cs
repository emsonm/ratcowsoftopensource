using System;
using System.Text;
using System.Runtime.InteropServices;

using sharpallegro;

namespace exstars
{
    class exstars : Allegro
    {
        /* star field system */
        [StructLayout(LayoutKind.Sequential)]
        struct VECTOR
        {
            public int x, y, z;
        }


        const int NUM_STARS = 512;

        const int Z_NEAR = 24;
        const int Z_FAR = 1024;
        const int XY_CUBE = 2048;

        const int SPEED_LIMIT = 18;

        static VECTOR[] stars = new VECTOR[NUM_STARS];

        static int[] star_x = new int[NUM_STARS];
        static int[] star_y = new int[NUM_STARS];

        static VECTOR delta;


        /* polygonal models */
        const int NUM_VERTS = 4;
        const int NUM_FACES = 4;

        const int ENGINE = 3;     /* which face is the engine */
        const int ENGINE_ON = 64;    /* colour index */
        const int ENGINE_OFF = 32;

        [StructLayout(LayoutKind.Sequential)]
        struct FACE              /* for triangular models */
        {
            public int v1, v2, v3;
            public int colour, range;
            public VECTOR normal, rnormal;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MODEL
        {
            public MODEL(int x, int y, int z, int rx, int ry, int rz, int minx, int miny, int maxx, int maxy, int velocity)
            {
                points = new VECTOR[NUM_VERTS];
                faces = new FACE[NUM_FACES];
                this.x = x;
                this.y = y;
                this.z = z;
                this.rx = rx;
                this.ry = ry;
                this.rz = rz;
                this.minx = minx;
                this.miny = miny;
                this.maxx = maxx;
                this.maxy = maxy;
                aim = new VECTOR();
                this.velocity = velocity;
            }

            public VECTOR[] points;
            public FACE[] faces;
            public int x, y, z;
            public int rx, ry, rz;
            public int minx, miny, maxx, maxy;
            public VECTOR aim;
            public int velocity;
        }


        static MODEL ship = new MODEL(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        static VECTOR direction;


        static BITMAP buffer;



        /* initialises the star field system */
        static void init_stars()
        {
            int i;

            for (i = 0; i < NUM_STARS; i++)
            {
                stars[i].x = itofix((AL_RAND() % XY_CUBE) - (XY_CUBE >> 1));
                stars[i].y = itofix((AL_RAND() % XY_CUBE) - (XY_CUBE >> 1));
                stars[i].z = itofix((AL_RAND() % (Z_FAR - Z_NEAR)) + Z_NEAR);
            }

            delta.x = 0;
            delta.y = 0;
            delta.z = 0;
        }

        /* draws the star field */
        static void draw_stars()
        {
            int i, c, last_i = -1;
            MATRIX m = new MATRIX();
            VECTOR[] outs = new VECTOR[NUM_STARS];

            for (i = 0; i < NUM_STARS; i++)
            {
                get_translation_matrix(ref m, delta.x, delta.y, delta.z);
                apply_matrix(ref m, stars[i].x, stars[i].y, stars[i].z,
                     ref outs[i].x, ref outs[i].y, ref outs[i].z);
                // TODO: check why it corrupts memory
                int tmpx = 0;
                int tmpy = 0;
                int x = 0;
                int y = 0;
                int z = 0;
                persp_project(outs[i].x, outs[i].y, outs[i].z, ref star_x[i], ref star_y[i]);
                c = (fixtoi(outs[i].z) >> 8) + 16;
                putpixel(buffer, fixtoi(star_x[i]), fixtoi(star_y[i]),
                     palette_color[c]);
                if (i <= last_i)
                {
                    allegro_message("Loop!");
                }
                last_i = i;

            }
            int a = 0;
        }



        /* deletes the stars from the screen */
        static void erase_stars()
        {
            int i;

            for (i = 0; i < NUM_STARS; i++)
                putpixel(buffer, fixtoi(star_x[i]), fixtoi(star_y[i]),
                     palette_color[0]);
        }



        /* moves the stars */
        static void move_stars()
        {
            int i;

            for (i = 0; i < NUM_STARS; i++)
            {
                stars[i].x += delta.x;
                stars[i].y += delta.y;
                stars[i].z += delta.z;

                if (stars[i].x > itofix(XY_CUBE >> 1))
                    stars[i].x = itofix(-(XY_CUBE >> 1));
                else if (stars[i].x < itofix(-(XY_CUBE >> 1)))
                    stars[i].x = itofix(XY_CUBE >> 1);

                if (stars[i].y > itofix(XY_CUBE >> 1))
                    stars[i].y = itofix(-(XY_CUBE >> 1));
                else if (stars[i].y < itofix(-(XY_CUBE >> 1)))
                    stars[i].y = itofix(XY_CUBE >> 1);

                if (stars[i].z > itofix(Z_FAR))
                    stars[i].z = itofix(Z_NEAR);
                else if (stars[i].z < itofix(Z_NEAR))
                    stars[i].z = itofix(Z_FAR);
            }
        }



        /* initialises the ship model */
        static void init_ship()
        {
            //VECTOR v1, v2, *pts;
            VECTOR v1, v2;
            VECTOR[] pts;
            //FACE *face;
            FACE face;
            int i;

            ship.points[0].x = itofix(0);
            ship.points[0].y = itofix(0);
            ship.points[0].z = itofix(32);

            ship.points[1].x = itofix(16);
            ship.points[1].y = itofix(-16);
            ship.points[1].z = itofix(-32);

            ship.points[2].x = itofix(-16);
            ship.points[2].y = itofix(-16);
            ship.points[2].z = itofix(-32);

            ship.points[3].x = itofix(0);
            ship.points[3].y = itofix(16);
            ship.points[3].z = itofix(-32);

            ship.faces[0].v1 = 3;
            ship.faces[0].v2 = 0;
            ship.faces[0].v3 = 1;
            //pts = &ship.points[0];
            pts = ship.points;
            //face = &ship.faces[0];
            face = ship.faces[0];
            v1.x = (pts[face.v2].x - pts[face.v1].x);
            v1.y = (pts[face.v2].y - pts[face.v1].y);
            v1.z = (pts[face.v2].z - pts[face.v1].z);
            v2.x = (pts[face.v3].x - pts[face.v1].x);
            v2.y = (pts[face.v3].y - pts[face.v1].y);
            v2.z = (pts[face.v3].z - pts[face.v1].z);
            cross_product(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z,
                  ref (face.normal.x), ref (face.normal.y), ref (face.normal.z));

            ship.faces[1].v1 = 2;
            ship.faces[1].v2 = 0;
            ship.faces[1].v3 = 3;
            //face = &ship.faces[1];
            face = ship.faces[1];
            v1.x = (pts[face.v2].x - pts[face.v1].x);
            v1.y = (pts[face.v2].y - pts[face.v1].y);
            v1.z = (pts[face.v2].z - pts[face.v1].z);
            v2.x = (pts[face.v3].x - pts[face.v1].x);
            v2.y = (pts[face.v3].y - pts[face.v1].y);
            v2.z = (pts[face.v3].z - pts[face.v1].z);
            cross_product(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z, ref (face.normal.x),
                  ref (face.normal.y), ref (face.normal.z));

            ship.faces[2].v1 = 1;
            ship.faces[2].v2 = 0;
            ship.faces[2].v3 = 2;
            //face = &ship.faces[2];
            face = ship.faces[2];
            v1.x = (pts[face.v2].x - pts[face.v1].x);
            v1.y = (pts[face.v2].y - pts[face.v1].y);
            v1.z = (pts[face.v2].z - pts[face.v1].z);
            v2.x = (pts[face.v3].x - pts[face.v1].x);
            v2.y = (pts[face.v3].y - pts[face.v1].y);
            v2.z = (pts[face.v3].z - pts[face.v1].z);
            cross_product(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z,
                  ref (face.normal.x), ref (face.normal.y), ref (face.normal.z));

            ship.faces[3].v1 = 2;
            ship.faces[3].v2 = 3;
            ship.faces[3].v3 = 1;
            //face = &ship.faces[3];
            face = ship.faces[3];
            v1.x = (pts[face.v2].x - pts[face.v1].x);
            v1.y = (pts[face.v2].y - pts[face.v1].y);
            v1.z = (pts[face.v2].z - pts[face.v1].z);
            v2.x = (pts[face.v3].x - pts[face.v1].x);
            v2.y = (pts[face.v3].y - pts[face.v1].y);
            v2.z = (pts[face.v3].z - pts[face.v1].z);
            cross_product(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z,
                  ref (face.normal.x), ref (face.normal.y), ref (face.normal.z));

            for (i = 0; i < NUM_FACES; i++)
            {
                ship.faces[i].colour = 32;
                ship.faces[i].range = 15;
                normalize_vector(ref ship.faces[i].normal.x, ref ship.faces[i].normal.y,
                         ref ship.faces[i].normal.z);
                ship.faces[i].rnormal.x = ship.faces[i].normal.x;
                ship.faces[i].rnormal.y = ship.faces[i].normal.y;
                ship.faces[i].rnormal.z = ship.faces[i].normal.z;
            }

            ship.x = ship.y = 0;
            ship.z = itofix(192);
            ship.rx = ship.ry = ship.rz = 0;

            ship.aim.x = direction.x = 0;
            ship.aim.y = direction.y = 0;
            ship.aim.z = direction.z = itofix(-1);
            ship.velocity = 0;
        }



        /* draws the ship model */
        static void draw_ship()
        {
            VECTOR[] outs = new VECTOR[NUM_VERTS];
            MATRIX m = new MATRIX();
            int i, col;

            ship.minx = SCREEN_W;
            ship.miny = SCREEN_H;
            ship.maxx = ship.maxy = 0;

            get_rotation_matrix(ref m, ship.rx, ship.ry, ship.rz);
            apply_matrix(ref m, ship.aim.x, ship.aim.y, ship.aim.z,
                 ref outs[0].x, ref outs[0].y, ref outs[0].z);
            direction.x = outs[0].x;
            direction.y = outs[0].y;
            direction.z = outs[0].z;

            for (i = 0; i < NUM_FACES; i++)
                apply_matrix(ref m, ship.faces[i].normal.x, ship.faces[i].normal.y,
                     ship.faces[i].normal.z, ref ship.faces[i].rnormal.x,
                     ref ship.faces[i].rnormal.y, ref ship.faces[i].rnormal.z);

            get_transformation_matrix(ref m, itofix(1), ship.rx, ship.ry, ship.rz,
                          ship.x, ship.y, ship.z);

            //for (i = 0; i < NUM_VERTS; i++)
            //{
            //    apply_matrix(ref m, ship.points[i].x, ship.points[i].y, ship.points[i].z,
            //         ref outs[i].x, ref outs[i].y, ref outs[i].z);
            //    persp_project(outs[i].x, outs[i].y, outs[i].z, ref outs[i].x, ref outs[i].y);
            //    if (fixtoi(outs[i].x) < ship.minx)
            //        ship.minx = fixtoi(outs[i].x);
            //    if (fixtoi(outs[i].x) > ship.maxx)
            //        ship.maxx = fixtoi(outs[i].x);
            //    if (fixtoi(outs[i].y) < ship.miny)
            //        ship.miny = fixtoi(outs[i].y);
            //    if (fixtoi(outs[i].y) > ship.maxy)
            //        ship.maxy = fixtoi(outs[i].y);
            //}

            for (i = 0; i < NUM_FACES; i++)
            {
                if (fixtof(ship.faces[i].rnormal.z) < 0.0)
                {
                    col = fixtoi(fixmul(dot_product(ship.faces[i].rnormal.x,
                                    ship.faces[i].rnormal.y,
                                    ship.faces[i].rnormal.z, 0, 0,
                                    itofix(1)),
                                itofix(ship.faces[i].range)));
                    if (col < 0)
                        col = -col + ship.faces[i].colour;
                    else
                        col = col + ship.faces[i].colour;

                    //triangle(buffer, fixtoi(outs[ship.faces[i].v1].x),
                    //     fixtoi(outs[ship.faces[i].v1].y),
                    //     fixtoi(outs[ship.faces[i].v2].x),
                    //     fixtoi(outs[ship.faces[i].v2].y),
                    //     fixtoi(outs[ship.faces[i].v3].x),
                    //     fixtoi(outs[ship.faces[i].v3].y), palette_color[col]);
                }
            }
        }



        /* removes the ship model from the screen */
        static void erase_ship()
        {
            rectfill(buffer, ship.minx, ship.miny, ship.maxx, ship.maxy,
                 palette_color[0]);
        }



        static int Main(string[] argv)
        {
            PALETTE pal = new PALETTE();
            int i;

            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            install_timer();

            if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
            {
                if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
                            allegro_error));
                    return 1;
                }
            }

            for (i = 0; i < 16; i++)
                pal[i].r = pal[i].g = pal[i].b = 0;

            /* greyscale */
            pal[16].r = pal[16].g = pal[16].b = 63;
            pal[17].r = pal[17].g = pal[17].b = 48;
            pal[18].r = pal[18].g = pal[18].b = 32;
            pal[19].r = pal[19].g = pal[19].b = 16;
            pal[20].r = pal[20].g = pal[20].b = 8;

            /* red range */
            for (i = 0; i < 16; i++)
            {
                pal[i + 32].r = (byte)(31 + i * 2);
                pal[i + 32].g = 15;
                pal[i + 32].b = 7;
            }

            /* a nice fire orange */
            for (i = 64; i < 68; i++)
            {
                pal[i].r = 63;
                pal[i].g = (byte)(17 + (i - 64) * 3);
                pal[i].b = 0;
            }

            set_palette(pal);

            buffer = create_bitmap(SCREEN_W, SCREEN_H);
            clear_bitmap(buffer);

            set_projection_viewport(0, 0, SCREEN_W, SCREEN_H);

            init_stars();
            draw_stars();
            init_ship();
            draw_ship();

            int a = 0;

            for (; ; )
            {
                erase_stars();
                erase_ship();

                move_stars();
                draw_stars();

                textprintf_centre_ex(buffer, font, SCREEN_W / 2, SCREEN_H - 10,
                         palette_color[17], 0,
                         string.Format("     direction: [{0:f6}] [{0:f6}] [{0:f6}]     ",
                         fixtof(direction.x), fixtof(direction.y),
                         fixtof(direction.z)));
                textprintf_centre_ex(buffer, font, SCREEN_W / 2, SCREEN_H - 20,
                         palette_color[17], 0,
                         string.Format("   delta: [{0:f6}] [{0:f6}] [{0:f6}]   ", fixtof(delta.x),
                         fixtof(delta.y), fixtof(delta.z)));
                textprintf_centre_ex(buffer, font, SCREEN_W / 2, SCREEN_H - 30,
                         palette_color[17], 0, string.Format("   velocity: {0}   ",
                         ship.velocity));

                textout_centre_ex(buffer, font, "Press ESC to exit", SCREEN_W / 2, 16,
                      palette_color[18], 0);
                textout_centre_ex(buffer, font, "Press CTRL to fire engine",
                      SCREEN_W / 2, 32, palette_color[18], 0);

                draw_ship();

                vsync();
                blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);

                poll_keyboard();

                /* exit program */
                if (key[KEY_ESC])
                    break;

                /* rotates */
                if (key[KEY_UP])
                    ship.rx -= itofix(5);
                else if (key[KEY_DOWN])
                    ship.rx += itofix(5);

                if (key[KEY_LEFT])
                    ship.ry -= itofix(5);
                else if (key[KEY_RIGHT])
                    ship.ry += itofix(5);

                if (key[KEY_PGUP])
                    ship.rz -= itofix(5);
                else if (key[KEY_PGDN])
                    ship.rz += itofix(5);

                /* thrust */
                if ((key[KEY_LCONTROL]) || (key[KEY_RCONTROL]))
                {
                    ship.faces[ENGINE].colour = ENGINE_ON;
                    ship.faces[ENGINE].range = 3;
                    if (ship.velocity < SPEED_LIMIT)
                        ship.velocity += 2;
                }
                else
                {
                    ship.faces[ENGINE].colour = ENGINE_OFF;
                    ship.faces[ENGINE].range = 15;
                    if (ship.velocity > 0)
                        ship.velocity -= 2;
                }

                ship.rx &= itofix(255);
                ship.ry &= itofix(255);
                ship.rz &= itofix(255);

                delta.x = fixmul(direction.x, itofix(ship.velocity));
                delta.y = fixmul(direction.y, itofix(ship.velocity));
                delta.z = fixmul(direction.z, itofix(ship.velocity));
            }

            destroy_bitmap(buffer);
            return 0;
        }
    }
}
