using System;
using System.Text;
using System.Runtime.InteropServices;

using sharpallegro;

namespace exscn3d
{
    class exscn3d : Allegro
    {
        const int MAX_CUBES = 4;

        public class V3D_t : ManagedPointer                /* a 3d point (floating point version) */
        {
            public V3D_t(float x, float y, float z, float u, float v, int c)
                : base(Alloc(6 * sizeof(Int32)))
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.u = u;
                this.v = v;
                this.c = c;
            }

            public V3D_t(IntPtr pointer)
                : base(pointer)
            {
            }

            /* position */
            public float x
            {
                get
                {
                    return (float)ReadInt(0);
                }
                set
                {
                    WriteInt(0, (int)value);
                }
            }

            public float y
            {
                get
                {
                    return (float)ReadInt(sizeof(Int32));
                }
                set
                {
                    WriteInt(sizeof(Int32), (int)value);
                }
            }

            public float z
            {
                get
                {
                    return (float)ReadInt(2 * sizeof(Int32));
                }
                set
                {
                    WriteInt(2 * sizeof(Int32), (int)value);
                }
            }

            /* texture map coordinates */
            public float u
            {
                get
                {
                    return (float)ReadInt(3 * sizeof(Int32));
                }
                set
                {
                    WriteInt(3 * sizeof(Int32), (int)value);
                }
            }

            public float v
            {
                get
                {
                    return (float)ReadInt(4 * sizeof(Int32));
                }
                set
                {
                    WriteInt(4 * sizeof(Int32), (int)value);
                }
            }

            /* color */
            public int c
            {
                get
                {
                    return ReadInt(5 * sizeof(Int32));
                }
                set
                {
                    WriteInt(5 * sizeof(Int32), value);
                }
            }
        }

        public class QUAD
        {
            public QUAD(int v1, int v2, int v3, int v4)
            {
                v = new int[4];
                this.v[0] = v1;
                this.v[1] = v2;
                this.v[2] = v3;
                this.v[3] = v4;
            }

            //int v[4];
            public int[] v;
        }


        public static V3D_t[] vertex =
        {
           new V3D_t( -10.0f, -10.0f, -10.0f, 0.0f, 0.0f, 72 ),
           new V3D_t( -10.0f,  10.0f, -10.0f, 0.0f, 0.0f, 80 ),
           new V3D_t(  10.0f,  10.0f, -10.0f, 0.0f, 0.0f, 95 ),
           new V3D_t(  10.0f, -10.0f, -10.0f, 0.0f, 0.0f, 88 ),
           new V3D_t( -10.0f, -10.0f,  10.0f, 0.0f, 0.0f, 72 ),
           new V3D_t( -10.0f,  10.0f,  10.0f, 0.0f, 0.0f, 80 ),
           new V3D_t(  10.0f,  10.0f,  10.0f, 0.0f, 0.0f, 95 ),
           new V3D_t(  10.0f, -10.0f,  10.0f, 0.0f, 0.0f, 88 )
        };


        public static QUAD[] cube =
        {
           new QUAD( 2, 1, 0, 3 ),
           new QUAD( 4, 5, 6, 7 ),
           new QUAD( 0, 1, 5, 4 ),
           new QUAD( 2, 3, 7, 6 ),
           new QUAD( 4, 7, 3, 0 ),
           new QUAD( 1, 2, 6, 5 )
        };


        static IntPtr[] v = new IntPtr[4], vout = new IntPtr[12], vtmp = new IntPtr[12];
        //V3D_t *pv[4], *pvout[12], *pvtmp[12];
        static IntPtr[] pv = new IntPtr[4], pvout = new IntPtr[12], pvtmp = new IntPtr[12];



        volatile static int t;

        /* timer interrupt handler */
        static void tick()
        {
            t++;
        }
        static TimerHandler t_tick = new TimerHandler(tick);



        /* init_gfx:
         *  Set up graphics mode.
         */
        static int init_gfx(PALETTE pal)
        {
            int i;
            int gfx_mode = GFX_AUTODETECT;
            int w, h, bpp;

            /* color 0 = black */
            pal[0].r = pal[0].g = pal[0].b = 0;
            /* color 1 = red */
            pal[1].r = 63;
            pal[1].g = pal[1].b = 0;

            /* copy the desktop palette */
            for (i = 2; i < 64; i++)
                pal[i] = desktop_palette[i];

            ///* make a blue gradient */
            for (i = 64; i < 96; i++)
            {
                pal[i].b = (byte)((i - 64) * 2);
                pal[i].g = pal[i].r = 0;
            }

            for (i = 96; i < 256; i++)
                pal[i].r = pal[i].g = pal[i].b = 0;

            /* set the graphics mode */
            if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
                return 1;
            }
            set_palette(desktop_palette);

            w = SCREEN_W;
            h = SCREEN_H;
            bpp = bitmap_color_depth(screen);
            if (gfx_mode_select_ex(ref gfx_mode, ref w, ref h, ref bpp) == 0)
            {
                allegro_exit();
                return 1;
            }

            set_color_depth(bpp);

            if (set_gfx_mode(gfx_mode, w, h, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
                return 1;
            }

            return 0;
        }



        /* draw_cube:
         *  Translates, rotates, clips, projects, culls backfaces and draws a cube.
         */
        static void draw_cube(ref MATRIX_f matrix, int num_poly)
        {
            int i, j, nv;
            int[] _out = new int[12];
            //IntPtr _out = Marshal.AllocHGlobal(12 * sizeof(Int32));

            for (i = 0; i < num_poly; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    v[j] = vertex[cube[i].v[j]];
                    V3D_t _v = new V3D_t(v[j]);
                    float x = 0, y = 0, z = 0;
                    apply_matrix_f(ref matrix, _v.x, _v.y, _v.z,
                       ref x, ref y, ref z);
                    _v.x = x;
                    _v.y = y;
                    _v.z = z;
                }

                for (int t = 0; t < pv.Length; t++)
                {
                    pv[t] = v[t];
                }
                for (int t = 0; t < pvout.Length; t++)
                {
                    pvout[t] = new V3D_t(0, 0, 0, 0, 0, makecol(255, 255, 0));
                    pvtmp[t] = new V3D_t(0, 0, 0, 0, 0, makecol(255, 255, 0));
                }


                //   /* nv: number of vertices after clipping is done */
                //nv = clip3d_f(POLYTYPE_GCOL, 0.1f, 1000.0f, 4, pv, pvout,
                //      pvtmp, _out);
                //if (nv > 0)
                //{
                //    for (j = 0; j < nv; j++)
                //    {
                //        float x = 0f, y = 0f;
                //        V3D_t _vout = new V3D_t(vout[j]);
                //        persp_project_f(_vout.x, _vout.y, _vout.z, ref x,
                //                ref y);
                //        _vout.x = x;
                //        _vout.y = y;
                //    }

                //    V3D_t vt0 = new V3D_t(vout[0]);
                //    V3D_t vt1 = new V3D_t(vout[0]);
                //    V3D_t vt2 = new V3D_t(vout[0]);

                //    V3D_f[] _vout2 = new V3D_f[] {
                //           new V3D_f(vt0.x, vt0.y, vt0.z, vt0.u, vt0.v, vt0.c),
                //           new V3D_f(vt1.x, vt1.y, vt1.z, vt1.u, vt1.v, vt1.c),
                //           new V3D_f(vt2.x, vt2.y, vt2.z, vt2.u, vt2.v, vt2.c)
                //       };

                //    //if (polygon_z_normal_f(ref _vout2[0], ref _vout2[1], ref _vout2[2]) > 0)
                //    //    scene_polygon3d_f(POLYTYPE_GCOL, NULL, nv, pvout);
                //}
            }
        }



        static int Main()
        {
            BITMAP buffer;
            PALETTE pal = new PALETTE();
            MATRIX_f matrix = new MATRIX_f(), matrix1 = new MATRIX_f(), matrix2 = new MATRIX_f(), matrix3 = new MATRIX_f();
            int rx = 0, ry = 0, tz = 40;
            int rot = 0, inc = 1;
            int i, j, k;

            int frame = 0;
            float fps = 0.0f;

            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            install_mouse();
            install_timer();

            LOCK_VARIABLE(t);
            LOCK_FUNCTION(t_tick);

            install_int(t_tick, 10);

            /* set up graphics mode */
            if (init_gfx(pal) > 0)
                return 1;

            set_palette(pal);

            /* initialize buffers and viewport */
            buffer = create_bitmap(SCREEN_W, SCREEN_H);
            create_scene(24 * MAX_CUBES * MAX_CUBES * MAX_CUBES,
                 6 * MAX_CUBES * MAX_CUBES * MAX_CUBES);
            set_projection_viewport(0, 0, SCREEN_W, SCREEN_H);

            /* initialize pointers */
            for (j = 0; j < 4; j++)
                //pv[j] = &v[j];
                pv[j] = v[j];

            for (j = 0; j < 12; j++)
            {
                //pvtmp[j] = &vtmp[j];
                //pvout[j] = &vout[j];
                pvtmp[j] = vtmp[j];
                pvout[j] = vout[j];
            }


            while (!key[KEY_ESC])
            {
                clear_bitmap(buffer);
                clear_scene(buffer);

                /* matrix2: rotates cube */
                get_rotation_matrix_f(ref matrix2, rx, ry, 0);
                /* matrix3: turns head right/left */
                get_rotation_matrix_f(ref matrix3, 0, rot, 0);

                for (k = MAX_CUBES - 1; k >= 0; k--)
                    for (j = 0; j < MAX_CUBES; j++)
                        for (i = 0; i < MAX_CUBES; i++)
                        {
                            /* matrix1: locates cubes */
                            get_translation_matrix_f(ref matrix1, j * 40 - MAX_CUBES * 20 + 20,
                                         i * 40 - MAX_CUBES * 20 + 20, tz + k * 40);

                            /* matrix: rotates cube THEN locates cube THEN turns
                             * head right/left */
                            matrix_mul_f(ref matrix2, ref matrix1, out matrix);
                            matrix_mul_f(ref matrix, ref matrix3, out matrix);

                            /* cubes are just added to the scene.
                             * No sorting is done at this stage.
                             */
                            draw_cube(ref matrix, 6);
                        }

                /* sorts and renders polys */
                render_scene();
                textprintf_ex(buffer, font, 2, 2, palette_color[1], -1,
                      string.Format("({0:f1} fps)", fps));
                blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
                frame++;

                /* manage cubes movement */
                tz -= 2;
                if (tz == 0) tz = 40;
                rx += 4;
                ry += 4;
                rot += inc;
                if ((rot >= 25) || (rot <= -25)) inc = -inc;

                /* computes fps */
                if (t > 100)
                {
                    fps = (100.0f * frame) / t;
                    t = 0;
                    frame = 0;
                }
            }

            destroy_bitmap(buffer);
            destroy_scene();

            return 0;
        }
    }
}
