using System;
using System.Text;

using sharpallegro;
using System.Runtime.InteropServices;

namespace excamera
{
    class excamera : Allegro
    {
#if! M_PI
        const float M_PI = 3.14159F;
#endif


        /* display a nice 8x8 chessboard grid */
        const int GRID_SIZE = 8;


        /* convert radians to degrees */
        static float DEG(float n)
        {
            return ((n) * 180.0F / M_PI);
        }


        /* how many times per second the fps will be checked */
        const int FPS_INT = 2;


        /* uncomment to disable waiting for vsync */
        /* #define DISABLE_VSYNC */


        /* parameters controlling the camera and projection state */
        static int viewport_w = 320;
        static int viewport_h = 240;
        static int fov = 48;
        static float aspect = 1.33f;
        static float xpos = 0;
        static float ypos = -2;
        static float zpos = -4;
        static float heading = 0;
        static float pitch = 0;
        static float roll = 0;

        volatile static int fps;
        volatile static int framecount;

        public class V3D_p : ManagedPointer
        {
            public V3D_p() 
                : base(Alloc(6 * sizeof(Int32)))
            {
            }

            public V3D_p(float x, float y, float z, float u, float v, int c)
                : this()
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.u = u;
                this.v = v;
                this.c = c;
            }            

            public float x
            {
                get { return (float)ReadInt(0); }
                set { WriteInt(0, (int)value); }
            }

            public float y
            {
                get { return (float)ReadInt(sizeof(Int32)); }
                set { WriteInt(sizeof(Int32), (int)value); }
            }

            public float z
            {
                get { return (float)ReadInt(2 * sizeof(Int32)); }
                set { WriteInt(2 * sizeof(Int32), (int)value); }
            }

            public float u
            {
                get { return (float)ReadInt(3 * sizeof(Int32)); }
                set { WriteInt(3 * sizeof(Int32), (int)value); }
            }

            public float v
            {
                get { return (float)ReadInt(4 * sizeof(Int32)); }
                set { WriteInt(4 * sizeof(Int32), (int)value); }
            }

            public int c
            {
                get { return ReadInt(5 * sizeof(Int32)); }
                set { WriteInt(5 * sizeof(Int32), value); }
            }            
        }



        /* render a tile of the grid */
        static void draw_square(BITMAP bmp, ref MATRIX_f camera, int x, int z)
        {
            V3D_f[] _v = new V3D_f[4], _vout = new V3D_f[8], _vtmp = new V3D_f[8];
            V3D_f[] v = new V3D_f[4], vout = new V3D_f[8], vtmp = new V3D_f[8];
            int[] flags = new int[4], _out = new int[8];
            int c, vc = 0;

            for (c = 0; c < 4; c++)
                v[c] = _v[c];

            for (c = 0; c < 8; c++)
            {
                vout[c] = _vout[c];
                vtmp[c] = _vtmp[c];
            }

            /* set up four vertices with the world-space position of the tile */
            v[0].x = x - GRID_SIZE / 2;
            v[0].y = 0;
            v[0].z = z - GRID_SIZE / 2;

            v[1].x = x - GRID_SIZE / 2 + 1;
            v[1].y = 0;
            v[1].z = z - GRID_SIZE / 2;

            v[2].x = x - GRID_SIZE / 2 + 1;
            v[2].y = 0;
            v[2].z = z - GRID_SIZE / 2 + 1;

            v[3].x = x - GRID_SIZE / 2;
            v[3].y = 0;
            v[3].z = z - GRID_SIZE / 2 + 1;

            /* apply the camera matrix, translating world space . view space */
            for (c = 0; c < 4; c++)
            {
                apply_matrix_f(ref camera, v[c].x, v[c].y, v[c].z,
                   ref v[c].x, ref v[c].y, ref v[c].z);

                flags[c] = 0;

                /* set flags if this vertex is off the edge of the screen */
                if (v[c].x < -v[c].z)
                    flags[c] |= 1;
                else if (v[c].x > v[c].z)
                    flags[c] |= 2;

                if (v[c].y < -v[c].z)
                    flags[c] |= 4;
                else if (v[c].y > v[c].z)
                    flags[c] |= 8;

                if (v[c].z < 0.1)
                    flags[c] |= 16;
            }

            /* quit if all vertices are off the same edge of the screen */
            if ((flags[0] & flags[1] & flags[2] & flags[3]) != 0)
                return;

            //if ((flags[0] | flags[1] | flags[2] | flags[3]) != 0)
            //{
            //    /* clip if any vertices are off the edge of the screen */
            //    //vc = clip3d_f(POLYTYPE_FLAT, 0.1f, 0.1f, 4, v,
            //    //  vout, vtmp, _out);
            //    IntPtr w = Marshal.AllocHGlobal(24 * 4);
            //    IntPtr wout = Marshal.AllocHGlobal(24 * 8);
            //    IntPtr wtmp = Marshal.AllocHGlobal(24 * 8);
            //    IntPtr iout = Marshal.AllocHGlobal(sizeof(Int32) * 8);
            //    vc = clip3d_f(POLYTYPE_FLAT, 0.1f, 0.1f, 4, w, wout, wtmp, iout);

            //    if (vc <= 0)
            //        return;
            //}
            //else
            //{
            /* no need to bother clipping this one */
            vout[0] = v[0];
            vout[1] = v[1];
            vout[2] = v[2];
            vout[3] = v[3];

            vc = 4;
            //}

            /* project view space -> screen space */
            for (c = 0; c < vc; c++)
                persp_project_f(vout[c].x, vout[c].y, vout[c].z,
                        ref vout[c].x, ref vout[c].y);

            /* set the color */
            vout[0].c = ((x + z) & 1) > 0 ? makecol(0, 255, 0) : makecol(255, 255, 0);

            /* render the polygon */
            //ManagedPointer wout = Marshal.AllocHGlobal(4 * sizeof(Int32));
            //IntPtr[] wout = new IntPtr[4];
            V3D_f[] wout = new V3D_f[4];
            V3D_p v0 = new V3D_p(vout[0].x, vout[0].y, vout[0].z, vout[0].u, vout[0].v, vout[0].c);
            V3D_p v1 = new V3D_p(vout[1].x, vout[1].y, vout[1].z, vout[1].u, vout[1].v, vout[1].c);
            V3D_p v2 = new V3D_p(vout[2].x, vout[2].y, vout[2].z, vout[2].u, vout[2].v, vout[2].c);
            V3D_p v3 = new V3D_p(vout[3].x, vout[3].y, vout[3].z, vout[3].u, vout[3].v, vout[3].c);
            //ManagedPointer v0 = Marshal.AllocHGlobal(6 * sizeof(Int32));
            //ManagedPointer v1 = Marshal.AllocHGlobal(6 * sizeof(Int32));
            //ManagedPointer v2 = Marshal.AllocHGlobal(6 * sizeof(Int32));
            //ManagedPointer v3 = Marshal.AllocHGlobal(6 * sizeof(Int32));            
            //copyVertex(vout[0], v0);
            //copyVertex(vout[1], v1);
            //copyVertex(vout[2], v2);
            //copyVertex(vout[3], v3);
            //wout.WritePointer(0, v0);
            //wout.WritePointer(sizeof(Int32), v1);
            //wout.WritePointer(2 * sizeof(Int32), v2);
            //wout.WritePointer(3 * sizeof(Int32), v3);
            //wout[0] = v0;
            //wout[1] = v1;
            //wout[2] = v2;
            //wout[3] = v3;
            //wout[0] = vout[0];
            //wout[1] = vout[1];
            //wout[2] = vout[2];
            //wout[3] = vout[3];
            //polygon3d_f(bmp, POLYTYPE_FLAT, NULL, vc, vout);
            //polygon3d_f(bmp, POLYTYPE_FLAT, NULL, vc, ref wout);
            //IntPtr[] wwout = new IntPtr[4];
            //wwout[0] = v0;
            //wwout[1] = v1;
            //wwout[2] = v2;
            //wwout[3] = v3;
            ManagedPointer wwout = new ManagedPointer(4 * sizeof(Int32));
            wwout.WritePointer(0, v0);
            wwout.WritePointer(sizeof(Int32), v1);
            wwout.WritePointer(2 * sizeof(Int32), v2);
            wwout.WritePointer(3 * sizeof(Int32), v3);
            polygon3d_f(bmp, POLYTYPE_FLAT, NULL, 4, wwout.pointer);
            //quad3d_f(bmp, POLYTYPE_FLAT, NULL, ref vout[0], ref vout[1], ref vout[2], ref vout[3]);
        }

        public static void copyVertex(V3D_f _in, ManagedPointer _out)
        {
            _out.WriteInt(0, (int)_in.x);
            _out.WriteInt(sizeof(Int32), (int)_in.y);
            _out.WriteInt(2 * sizeof(Int32), (int)_in.z);
            _out.WriteInt(5 * sizeof(Int32), (int)_in.c);
        }

        /* draw everything */
        static void render(BITMAP bmp)
        {
            MATRIX_f roller = new MATRIX_f(), camera = new MATRIX_f();
            int x, y, w, h;
            float xfront, yfront, zfront;
            float xup = 0, yup = 0, zup = 0;

            /* clear the background */
            clear_to_color(bmp, makecol(255, 255, 255));

            /* set up the viewport region */
            x = (SCREEN_W - viewport_w) / 2;
            y = (SCREEN_H - viewport_h) / 2;
            w = viewport_w;
            h = viewport_h;

            set_projection_viewport(x, y, w, h);
            rect(bmp, x - 1, y - 1, x + w, y + h, makecol(255, 0, 0));
            set_clip_rect(bmp, x, y, x + w - 1, y + h - 1);

            /* calculate the in-front vector */
            xfront = (float)(Math.Sin(heading) * Math.Cos(pitch));
            yfront = (float)Math.Sin(pitch);
            zfront = (float)(Math.Cos(heading) * Math.Cos(pitch));

            /* rotate the up vector around the in-front vector by the roll angle */
            get_vector_rotation_matrix_f(ref roller, xfront, yfront, zfront,
                 (float)(roll * 128.0 / M_PI));
            apply_matrix_f(ref roller, 0, -1, 0, ref xup, ref yup, ref zup);

            /* build the camera matrix */
            get_camera_matrix_f(ref camera,
                    xpos, ypos, zpos,        /* camera position */
                    xfront, yfront, zfront,  /* in-front vector */
                    xup, yup, zup,           /* up vector */
                    fov,                     /* field of view */
                    aspect);                 /* aspect ratio */

            /* draw the grid of squares */
            for (x = 0; x < GRID_SIZE; x++)
                for (y = 0; y < GRID_SIZE; y++)
                    draw_square(bmp, ref camera, x, y);

            /* overlay some text */
            set_clip_rect(bmp, 0, 0, bmp.w, bmp.h);
            textprintf_ex(bmp, font, 0, 0, makecol(0, 0, 0), -1,
              string.Format("Viewport width: {0} (w/W changes)", viewport_w));
            textprintf_ex(bmp, font, 0, 8, makecol(0, 0, 0), -1,
              string.Format("Viewport height: {0} (h/H changes)", viewport_h));
            textprintf_ex(bmp, font, 0, 16, makecol(0, 0, 0), -1,
              string.Format("Field of view: {0} (f/F changes)", fov));
            textprintf_ex(bmp, font, 0, 24, makecol(0, 0, 0), -1,
              string.Format("Aspect ratio: {0:0.00} (a/A changes)", aspect));
            textprintf_ex(bmp, font, 0, 32, makecol(0, 0, 0), -1,
              string.Format("X position: {0:0.00} (x/X changes)", xpos));
            textprintf_ex(bmp, font, 0, 40, makecol(0, 0, 0), -1,
              string.Format("Y position: {0:0.00} (y/Y changes)", ypos));
            textprintf_ex(bmp, font, 0, 48, makecol(0, 0, 0), -1,
              string.Format("Z position: {0:0.00} (z/Z changes)", zpos));
            textprintf_ex(bmp, font, 0, 56, makecol(0, 0, 0), -1,
              string.Format("Heading: {0:0.00} deg (left/right changes)", DEG(heading)));
            textprintf_ex(bmp, font, 0, 64, makecol(0, 0, 0), -1,
              string.Format("Pitch: {0:0.00} deg (pgup/pgdn changes)", DEG(pitch)));
            textprintf_ex(bmp, font, 0, 72, makecol(0, 0, 0), -1,
              string.Format("Roll: {0:0.00} deg (r/R changes)", DEG(roll)));
            textprintf_ex(bmp, font, 0, 80, makecol(0, 0, 0), -1,
              string.Format("Front vector: {0:0.00}, {1:0.00}, {2:0.00}", xfront, yfront, zfront));
            textprintf_ex(bmp, font, 0, 88, makecol(0, 0, 0), -1,
              string.Format("Up vector: {0:0.00}, {1:0.00}, {2:0.00}", xup, yup, zup));
            textprintf_ex(bmp, font, 0, 96, makecol(0, 0, 0), -1,
              string.Format("Frames per second: {0}", fps));
        }



        /* deal with user input */
        static void process_input()
        {
            double frac, iptr;

            poll_keyboard();

            if (key[KEY_W])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                {
                    if (viewport_w < SCREEN_W)
                        viewport_w += 8;
                }
                else
                {
                    if (viewport_w > 16)
                        viewport_w -= 8;
                }
            }

            if (key[KEY_H])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                {
                    if (viewport_h < SCREEN_H)
                        viewport_h += 8;
                }
                else
                {
                    if (viewport_h > 16)
                        viewport_h -= 8;
                }
            }

            if (key[KEY_F])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                {
                    if (fov < 96)
                        fov++;
                }
                else
                {
                    if (fov > 16)
                        fov--;
                }
            }

            if (key[KEY_A])
            {
                frac = (aspect * 10) - (iptr = Math.Floor(aspect * 10));

                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                {
                    if ((frac > 0.59) && (frac < 0.61))
                        aspect += 0.04f;
                    else
                        aspect += 0.03f;
                    if (aspect > 2)
                        aspect = 2;
                }
                else
                {
                    if ((frac > 0.99) || (frac < 0.01))
                        aspect -= 0.04f;
                    else
                        aspect -= 0.03f;
                    if (aspect < .1)
                        aspect = .1f;
                }
            }

            if (key[KEY_X])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                    xpos += 0.05f;
                else
                    xpos -= 0.05f;
            }

            if (key[KEY_Y])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                    ypos += 0.05f;
                else
                    ypos -= 0.05f;
            }

            if (key[KEY_Z])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                    zpos += 0.05f;
                else
                    zpos -= 0.05f;
            }

            if (key[KEY_LEFT])
                heading -= 0.05f;

            if (key[KEY_RIGHT])
                heading += 0.05f;

            if (key[KEY_PGUP])
                if (pitch > -M_PI / 4)
                    pitch -= 0.05f;

            if (key[KEY_PGDN])
                if (pitch < M_PI / 4)
                    pitch += 0.05f;

            if (key[KEY_R])
            {
                if ((key_shifts & KB_SHIFT_FLAG) > 0)
                {
                    if (roll < M_PI / 4)
                        roll += 0.05f;
                }
                else
                {
                    if (roll > -M_PI / 4)
                        roll -= 0.05f;
                }
            }

            if (key[KEY_UP])
            {
                xpos += (float)(Math.Sin(heading) / 4);
                zpos += (float)(Math.Cos(heading) / 4);
            }

            if (key[KEY_DOWN])
            {
                xpos -= (float)(Math.Sin(heading) / 4);
                zpos -= (float)(Math.Cos(heading) / 4);
            }
        }



        static void fps_check()
        {
            fps = framecount * FPS_INT;
            framecount = 0;
        }
        public static TimerHandler t_fps_check = new TimerHandler(fps_check);



        static int Main()
        {
            BITMAP buffer;

            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            install_timer();

            set_color_depth(32);
            if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 640, 480, 0, 0) != 0)
            {
                if (set_gfx_mode(GFX_SAFE, 640, 480, 0, 0) != 0)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
                        allegro_error));
                    return 1;
                }
            }

            //set_palette(desktop_palette);
            buffer = create_bitmap(SCREEN_W, SCREEN_H);
            install_int_ex(t_fps_check, BPS_TO_TIMER(FPS_INT));

            while (!key[KEY_ESC])
            {
                render(buffer);

#if !DISABLE_VSYNC
                vsync();
#endif

                blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
                framecount++;

                process_input();
            }

            destroy_bitmap(buffer);
            return 0;
        }
    }
}
