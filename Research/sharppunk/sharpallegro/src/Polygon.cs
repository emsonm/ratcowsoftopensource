using sharpallegro;
using System;
using System.Runtime.InteropServices;

public class Polygon : Allegro
{
    const float M_PI = 3.14159f;
    const int GRID_SIZE = 8;
    const int FPS_INT = 2;

    public static float DEG(float n)
    {
        return ((n) * 180.0f / M_PI);
    }

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

    static void draw_square(BITMAP bmp, MATRIX_f camera, int x, int z)
    {
        V3D_f[] _v = new V3D_f[4], _vout = new V3D_f[8], _vtmp = new V3D_f[8];
        //V3D_f *v[4], *vout[8], *vtmp[8];
        int[] flags = new int[4], _out = new int[8];
        int c, vc;

        //for (c=0; c<4; c++)
        //   v[c] = &_v[c];

        //for (c=0; c<8; c++) {
        //   vout[c] = &_vout[c];
        //   vtmp[c] = &_vtmp[c];
        //}

        /* set up four vertices with the world-space position of the tile */
        _v[0].x = x - GRID_SIZE / 2;
        _v[0].y = 0;
        _v[0].z = z - GRID_SIZE / 2;

        _v[1].x = x - GRID_SIZE / 2 + 1;
        _v[1].y = 0;
        _v[1].z = z - GRID_SIZE / 2;

        _v[2].x = x - GRID_SIZE / 2 + 1;
        _v[2].y = 0;
        _v[2].z = z - GRID_SIZE / 2 + 1;

        _v[3].x = x - GRID_SIZE / 2;
        _v[3].y = 0;
        _v[3].z = z - GRID_SIZE / 2 + 1;

        /* apply the camera matrix, translating world space -> view space */
        for (c = 0; c < 4; c++)
        {
            apply_matrix_f(ref camera, _v[c].x, _v[c].y, _v[c].z,
                   ref _v[c].x, ref _v[c].y, ref _v[c].z);

            flags[c] = 0;

            //   /* set flags if this vertex is off the edge of the screen */
            if (_v[c].x < -_v[c].z)
                flags[c] |= 1;
            else if (_v[c].x > _v[c].z)
                flags[c] |= 2;

            if (_v[c].y < -_v[c].z)
                flags[c] |= 4;
            else if (_v[c].y > _v[c].z)
                flags[c] |= 8;

            if (_v[c].z < 0.1)
                flags[c] |= 16;
        }

        /* quit if all vertices are off the same edge of the screen */
        if ((flags[0] & flags[1] & flags[2] & flags[3]) > 0)
            return;

        if ((flags[0] | flags[1] | flags[2] | flags[3]) > 0)
        {
            /* clip if any vertices are off the edge of the screen */
            //vc = clip3d_f(POLYTYPE_FLAT, 0.1, 0.1, 4, (AL_CONST V3D_f **)v,
            //      vout, vtmp, out);

            //if (vc <= 0)
            return;
        }
        else
        {
            /* no need to bother clipping this one */
            _vout[0] = _v[0];
            _vout[1] = _v[1];
            _vout[2] = _v[2];
            _vout[3] = _v[3];

            vc = 4;
        }

        /* project view space -> screen space */
        //for (c=0; c<vc; c++)
        //   persp_project_f(vout[c].x, vout[c].y, vout[c].z,
        //           &vout[c].x, &vout[c].y);

        /* set the color */
        _vout[0].c = ((x + z) & 1) > 0 ? makecol(0, 255, 0) : makecol(255, 255, 0);

        vc = 4;

        /* render the polygon */
        //IntPtr p = new IntPtr();
        //Marshal.
        
        //polygon3d_f(bmp, POLYTYPE_FLAT, NULL, vc, ref _vout[0]);
        //polygon3d_f(bmp, POLYTYPE_FLAT, NULL, vc, p);
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
                     roll * 128.0f / M_PI);
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
                draw_square(bmp, camera, x, y);

        /* overlay some text */
        set_clip_rect(bmp, 0, 0, bmp.w, bmp.h);
        textprintf_ex(bmp, font, 0, 0, makecol(0, 0, 0), -1,
              string.Format("Viewport width: {0} (w/W changes)", viewport_w));
        textprintf_ex(bmp, font, 0, 8, makecol(0, 0, 0), -1,
              string.Format("Viewport height: {0} (h/H changes)", viewport_h));
        textprintf_ex(bmp, font, 0, 16, makecol(0, 0, 0), -1,
              string.Format("Field of view: {0} (f/F changes)", fov));
        textprintf_ex(bmp, font, 0, 24, makecol(0, 0, 0), -1,
              string.Format("Aspect ratio: {0:f2} (a/A changes)", aspect));
        textprintf_ex(bmp, font, 0, 32, makecol(0, 0, 0), -1,
              string.Format("X position: {0:f2} (x/X changes)", xpos));
        textprintf_ex(bmp, font, 0, 40, makecol(0, 0, 0), -1,
              string.Format("Y position: {0:f2} (y/Y changes)", ypos));
        textprintf_ex(bmp, font, 0, 48, makecol(0, 0, 0), -1,
              string.Format("Z position: {0:f2} (z/Z changes)", zpos));
        textprintf_ex(bmp, font, 0, 56, makecol(0, 0, 0), -1,
              string.Format("Heading: {0:f2} deg (left/right changes)", DEG(heading)));
        textprintf_ex(bmp, font, 0, 64, makecol(0, 0, 0), -1,
              string.Format("Pitch: {0:f2} deg (pgup/pgdn changes)", DEG(pitch)));
        textprintf_ex(bmp, font, 0, 72, makecol(0, 0, 0), -1,
              string.Format("Roll: {0:f2} deg (r/R changes)", DEG(roll)));
        textprintf_ex(bmp, font, 0, 80, makecol(0, 0, 0), -1,
              string.Format("Front vector: {0:f2}, {1:f2}, {2:f2}", xfront, yfront, zfront));
        textprintf_ex(bmp, font, 0, 88, makecol(0, 0, 0), -1,
              string.Format("Up vector: {0:f2}, {1:f2}, {2:f2}", xup, yup, zup));
    }



    /* deal with user input */
    //static void process_input()
    //{
    //   double frac, iptr;

    //   poll_keyboard();

    //   if (key[KEY_W]) {
    //      if (key_shifts & KB_SHIFT_FLAG) {
    //     if (viewport_w < SCREEN_W)
    //        viewport_w += 8;
    //      }
    //      else { 
    //     if (viewport_w > 16)
    //        viewport_w -= 8;
    //      }
    //   }

    //   if (key[KEY_H]) {
    //      if (key_shifts & KB_SHIFT_FLAG) {
    //     if (viewport_h < SCREEN_H)
    //        viewport_h += 8;
    //      }
    //      else {
    //     if (viewport_h > 16)
    //        viewport_h -= 8;
    //      }
    //   }

    //   if (key[KEY_F]) {
    //      if (key_shifts & KB_SHIFT_FLAG) {
    //     if (fov < 96)
    //        fov++;
    //      }
    //      else {
    //     if (fov > 16)
    //        fov--;
    //      }
    //   }

    //   if (key[KEY_A]) {
    //      frac = modf(aspect*10.0, &iptr);

    //      if (key_shifts & KB_SHIFT_FLAG) {
    //     if ((frac>0.59) && (frac<0.61))
    //        aspect += 0.04f;
    //     else
    //        aspect += 0.03f;
    //     if (aspect > 2)
    //        aspect = 2;
    //      }
    //      else {
    //     if ((frac>0.99) || (frac<0.01))
    //        aspect -= 0.04f;
    //     else
    //        aspect -= 0.03f;
    //     if (aspect < .1)
    //        aspect = .1;
    //      }
    //   }

    //   if (key[KEY_X]) {
    //      if (key_shifts & KB_SHIFT_FLAG)
    //     xpos += 0.05;
    //      else
    //     xpos -= 0.05;
    //   }

    //   if (key[KEY_Y]) {
    //      if (key_shifts & KB_SHIFT_FLAG)
    //     ypos += 0.05;
    //      else
    //     ypos -= 0.05;
    //   }

    //   if (key[KEY_Z]) {
    //      if (key_shifts & KB_SHIFT_FLAG)
    //     zpos += 0.05;
    //      else
    //     zpos -= 0.05;
    //   }

    //   if (key[KEY_LEFT])
    //      heading -= 0.05;

    //   if (key[KEY_RIGHT])
    //      heading += 0.05;

    //   if (key[KEY_PGUP])
    //      if (pitch > -M_PI/4)
    //     pitch -= 0.05;

    //   if (key[KEY_PGDN])
    //      if (pitch < M_PI/4)
    //     pitch += 0.05;

    //   if (key[KEY_R]) {
    //      if (key_shifts & KB_SHIFT_FLAG) {
    //     if (roll < M_PI/4)
    //        roll += 0.05;
    //      }
    //      else {
    //     if (roll > -M_PI/4)
    //        roll -= 0.05;
    //      }
    //   }

    //   if (key[KEY_UP]) {
    //      xpos += sin(heading) / 4;
    //      zpos += cos(heading) / 4;
    //   }

    //   if (key[KEY_DOWN]) {
    //      xpos -= sin(heading) / 4;
    //      zpos -= cos(heading) / 4;
    //   }
    //}



    public static void Start()
    {
        BITMAP buffer;

        allegro_init();
        install_keyboard();
        install_timer();
        set_gfx_mode(GFX_AUTODETECT_WINDOWED, 640, 480, 0, 0);
        set_palette(desktop_palette);
        buffer = create_bitmap(SCREEN_W, SCREEN_H);

        while (!key[KEY_ESC])
        {
            render(buffer);
            blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
            //process_input();
        }
    }
}