using System;
using System.Text;

using sharpallegro;

namespace exzbuf
{
  class exzbuf : Allegro
  {
    public struct FACE
    {
      public FACE(int v1, int v2, int v3, int v4)
      {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
      }

      public int v1, v2, v3, v4;
    }


    static V3D_f[] cube1 =
    {
       new V3D_f(-32.0F, -32.0F, -32.0F, 0.0F, 0.0F, 72),
       new V3D_f(-32.0F,  32.0F, -32.0F, 0.0F, 0.0F, 80),
       new V3D_f( 32.0F,  32.0F, -32.0F, 0.0F, 0.0F, 95),
       new V3D_f( 32.0F, -32.0F, -32.0F, 0.0F, 0.0F, 88),
       new V3D_f(-32.0F, -32.0F,  32.0F, 0.0F, 0.0F, 72),
       new V3D_f(-32.0F,  32.0F,  32.0F, 0.0F, 0.0F, 80),
       new V3D_f( 32.0F,  32.0F,  32.0F, 0.0F, 0.0F, 95),
       new V3D_f( 32.0F, -32.0F,  32.0F, 0.0F, 0.0F, 88)
    };


    static V3D_f[] cube2 =
    {
       new V3D_f(-32.0F, -32.0F, -32.0F, 0.0F, 0.0F, 104),
       new V3D_f(-32.0F,  32.0F, -32.0F, 0.0F, 0.0F, 112),
       new V3D_f( 32.0F,  32.0F, -32.0F, 0.0F, 0.0F, 127),
       new V3D_f( 32.0F, -32.0F, -32.0F, 0.0F, 0.0F, 120),
       new V3D_f(-32.0F, -32.0F,  32.0F, 0.0F, 0.0F, 104),
       new V3D_f(-32.0F,  32.0F,  32.0F, 0.0F, 0.0F, 112),
       new V3D_f( 32.0F,  32.0F,  32.0F, 0.0F, 0.0F, 127),
       new V3D_f( 32.0F, -32.0F,  32.0F, 0.0F, 0.0F, 120)
    };


    static FACE[] faces =
    {
       new FACE( 2, 1, 0, 3 ),
       new FACE( 4, 5, 6, 7 ),
       new FACE( 0, 1, 5, 4 ),
       new FACE( 2, 3, 7, 6 ),
       new FACE( 4, 7, 3, 0 ),
       new FACE( 1, 2, 6, 5 )
    };



    volatile static int t;


    /* timer interrupt handler */
    static void tick()
    {
      t++;
    }

    public static TimerHandler t_tick = new TimerHandler(tick);





    /* update cube positions */
    static void anim_cube(MATRIX_f matrix1, MATRIX_f matrix2, V3D_f[] x1, V3D_f[] x2)
    {
      int i;

      for (i = 0; i < 8; i++)
      {
        apply_matrix_f(ref matrix1, cube1[i].x, cube1[i].y, cube1[i].z,
           ref (x1[i].x), ref (x1[i].y), ref (x1[i].z));
        apply_matrix_f(ref matrix2, cube2[i].x, cube2[i].y, cube2[i].z,
           ref (x2[i].x), ref (x2[i].y), ref (x2[i].z));
        persp_project_f(x1[i].x, x1[i].y, x1[i].z, ref (x1[i].x), ref (x1[i].y));
        persp_project_f(x2[i].x, x2[i].y, x2[i].z, ref (x2[i].x), ref (x2[i].y));
      }
    }



    /* cull backfaces and draw cubes */
    static void draw_cube(BITMAP buffer, V3D_f[] x1, V3D_f[] x2)
    {
      int i;

      for (i = 0; i < 6; i++)
      {
        V3D_f vtx1, vtx2, vtx3, vtx4;

        vtx1 = x1[faces[i].v1];
        vtx2 = x1[faces[i].v2];
        vtx3 = x1[faces[i].v3];
        vtx4 = x1[faces[i].v4];
        if (polygon_z_normal_f(ref vtx1, ref vtx2, ref vtx3) > 0)
          quad3d_f(buffer, POLYTYPE_GCOL | POLYTYPE_ZBUF, NULL,
        ref vtx1, ref vtx2, ref vtx3, ref vtx4);

        vtx1 = x2[faces[i].v1];
        vtx2 = x2[faces[i].v2];
        vtx3 = x2[faces[i].v3];
        vtx4 = x2[faces[i].v4];
        if (polygon_z_normal_f(ref vtx1, ref vtx2, ref vtx3) > 0)
          quad3d_f(buffer, POLYTYPE_GCOL | POLYTYPE_ZBUF, NULL,
        ref vtx1, ref vtx2, ref vtx3, ref vtx4);
      }
    }



    static int Main()
    {
      ZBUFFER zbuf;
      BITMAP buffer;
      PALETTE pal = new PALETTE();
      MATRIX_f matrix1 = new MATRIX_f(), matrix2 = new MATRIX_f();
      V3D_f[] x1 = new V3D_f[8], x2 = new V3D_f[8];

      int i;
      int c = GFX_AUTODETECT;
      int w, h, bpp;

      int frame = 0;
      float fps = 0.0F;

      float rx1, ry1, rz1;		/* cube #1 rotations */
      float drx1, dry1, drz1;	/* cube #1 rotation speed */
      float rx2, ry2, rz2;		/* cube #2 rotations */
      float drx2, dry2, drz2;	/* cube #1 rotation speed */
      float tx = 16.0F;		/* x shift between cubes */
      float tz1 = 100.0F;		/* cube #1 z coordinate */
      float tz2 = 105.0F;		/* cube #2 z coordinate */

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_mouse();
      install_timer();

      LOCK_VARIABLE(t);
      LOCK_FUNCTION(t_tick);

      install_int(tick, 10);

      /* color 0 = black */
      pal[0].r = pal[0].g = pal[0].b = 0;
      /* color 1 = red */
      pal[1].r = 255;
      pal[1].g = pal[1].b = 0;

      /* copy the desktop palette */
      for (i = 1; i < 64; i++)
        pal[i] = desktop_palette[i];

      /* make a blue gradient */
      for (i = 64; i < 96; i++)
      {
        pal[i].b = (byte)((i - 64) * 2);
        pal[i].g = pal[i].r = 0;
      }

      /* make a green gradient */
      for (i = 96; i < 128; i++)
      {
        pal[i].g = (byte)((i - 96) * 2);
        pal[i].r = pal[i].b = 0;
      }

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
      if (gfx_mode_select_ex(ref c, ref w, ref h, ref bpp) == 0)
      {
        allegro_exit();
        return 1;
      }

      set_color_depth(bpp);

      if (set_gfx_mode(c, w, h, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
        return 1;
      }

      set_palette_range(pal, 0, 127, FALSE);

      /* double buffer the animation and create the Z-buffer */
      buffer = create_bitmap(SCREEN_W, SCREEN_H);
      zbuf = create_zbuffer(buffer);
      set_zbuffer(zbuf);

      /* set up the viewport for the perspective projection */
      set_projection_viewport(0, 0, SCREEN_W, SCREEN_H);

      /* compute rotations and speed rotation */
      rx1 = ry1 = rz1 = 0.0F;
      rx2 = ry2 = rz2 = 0.0F;

      drx1 = (float)(((AL_RAND() & 31) - 16) / 4.0);
      dry1 = (float)(((AL_RAND() & 31) - 16) / 4.0);
      drz1 = (float)(((AL_RAND() & 31) - 16) / 4.0);

      drx2 = (float)(((AL_RAND() & 31) - 16) / 4.0);
      dry2 = (float)(((AL_RAND() & 31) - 16) / 4.0);
      drz2 = (float)(((AL_RAND() & 31) - 16) / 4.0);

      /* set the transformation matrices */
      get_transformation_matrix_f(ref matrix1, 1.0F, rx1, ry1, rz1, tx, 0.0F, tz1);
      get_transformation_matrix_f(ref matrix2, 1.0F, rx2, ry2, rz2, -tx, 0.0F, tz2);

      /* set colors */
      for (i = 0; i < 8; i++)
      {
        x1[i].c = palette_color[cube1[i].c];
        x2[i].c = palette_color[cube2[i].c];
      }

      /* main loop */
      while (true)
      {
        clear_bitmap(buffer);
        clear_zbuffer(zbuf, 0.0F);

        anim_cube(matrix1, matrix2, x1, x2);
        draw_cube(buffer, x1, x2);

        /* update transformation matrices */
        rx1 += drx1;
        ry1 += dry1;
        rz1 += drz1;
        rx2 += drx2;
        ry2 += dry2;
        rz2 += drz2;
        get_transformation_matrix_f(ref matrix1, 1.0F, rx1, ry1, rz1, tx, 0.0F, tz1);
        get_transformation_matrix_f(ref matrix2, 1.0F, rx2, ry2, rz2, -tx, 0.0F, tz2);

        //textprintf_ex(buffer, font, 10, 1, palette_color[1], 0,
        //  string.Format("Z-buffered polygons ({0:.0} fps)", fps));
        textprintf_ex(buffer, font, 10, 1, makecol(255, 0, 0), 0,
          string.Format("Z-buffered polygons ({0:.0} fps)", fps));

        vsync();
        blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
        frame++;

        if (t > 100)
        {
          fps = (float)((100.0 * frame) / t);
          t = 0;
          frame = 0;
        }

        if (keypressed())
        {
          if ((readkey() & 0xFF) == 27)
            break;
        }
      }

      destroy_bitmap(buffer);
      destroy_zbuffer(zbuf);

      return 0;
    }
  }
}
