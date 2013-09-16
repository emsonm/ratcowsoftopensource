using System;
using System.Collections;
using System.Text;

using sharpallegro;

namespace ex3d
{
  class ex3d : Allegro
  {
    const int NUM_SHAPES = 8;     /* number of bouncing cubes */

    const int NUM_VERTICES = 8;     /* a cube has eight corners */
    const int NUM_FACES = 6;     /* a cube has six faces */


    public struct VTX
    {
      public VTX(int x, int y, int z)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }

      public int x, y, z;
    }


    public class QUAD : IComparable             /* four vertices makes a quad */
    {
      public QUAD(VTX[] vtxlist, int v1, int v2, int v3, int v4)
      {
        this.vtxlist = vtxlist;
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
      }

      public QUAD Clone()
      {
        return new QUAD(this.vtxlist, this.v1, this.v2, this.v3, this.v4);
      }

      public int CompareTo(object obj)
      {
        return quad_cmp((QUAD)this, (QUAD)obj);
      }

      public VTX[] vtxlist = new VTX[NUM_VERTICES];
      public int v1, v2, v3, v4;
    }


    public struct SHAPE             /* store position of a shape */
    {
      public int x, y, z;                /* x, y, z position */
      public int rx, ry, rz;             /* rotations */
      public int dz;                     /* speed of movement */
      public int drx, dry, drz;          /* speed of rotation */
    }


    static VTX[] points =                   /* a cube, centered on the origin */
    {
      /* vertices of the cube */
      new VTX( -32 << 16, -32 << 16, -32 << 16 ),
      new VTX( -32 << 16,  32 << 16, -32 << 16 ),
      new VTX(  32 << 16,  32 << 16, -32 << 16 ),
      new VTX(  32 << 16, -32 << 16, -32 << 16 ),
      new VTX( -32 << 16, -32 << 16,  32 << 16 ),
      new VTX( -32 << 16,  32 << 16,  32 << 16 ),
      new VTX(  32 << 16,  32 << 16,  32 << 16 ),
      new VTX(  32 << 16, -32 << 16,  32 << 16 ),
    };


    static QUAD[] faces =                   /* group the vertices into polygons */
    {
      new QUAD( points, 0, 3, 2, 1 ),
      new QUAD( points, 4, 5, 6, 7 ),
      new QUAD( points, 0, 1, 5, 4 ),
      new QUAD( points, 2, 3, 7, 6 ),
      new QUAD( points, 0, 4, 7, 3 ),
      new QUAD( points, 1, 2, 6, 5 )
    };


    static SHAPE[] shapes = new SHAPE[NUM_SHAPES];        /* a list of shapes */


    /* somewhere to put translated vertices */
    static VTX[] output_points = new VTX[NUM_VERTICES * NUM_SHAPES];
    static QUAD[] output_faces = new QUAD[NUM_FACES * NUM_SHAPES];


    enum render_mode
    {
      wireframe,
      flat,
      gcol,
      grgb,
      atex,
      ptex,
      atex_mask,
      ptex_mask,
      atex_lit,
      ptex_lit,
      atex_mask_lit,
      ptex_mask_lit,
      atex_trans,
      ptex_trans,
      atex_mask_trans,
      ptex_mask_trans,
      last_mode
    }

    static render_mode _render_mode = render_mode.wireframe;


    static int[] render_type =
    {
      0,
      POLYTYPE_FLAT,
      POLYTYPE_GCOL,
      POLYTYPE_GRGB,
      POLYTYPE_ATEX,
      POLYTYPE_PTEX,
      POLYTYPE_ATEX_MASK,
      POLYTYPE_PTEX_MASK,
      POLYTYPE_ATEX_LIT,
      POLYTYPE_PTEX_LIT,
      POLYTYPE_ATEX_MASK_LIT,
      POLYTYPE_PTEX_MASK_LIT,
      POLYTYPE_ATEX_TRANS,
      POLYTYPE_PTEX_TRANS,
      POLYTYPE_ATEX_MASK_TRANS,
      POLYTYPE_PTEX_MASK_TRANS
    };


    static string[] mode_desc =
    {
      "Wireframe",
      "Flat shaded",
      "Single color Gouraud shaded",
      "Gouraud shaded",
      "Texture mapped",
      "Perspective correct texture mapped",
      "Masked texture mapped",
      "Masked persp. correct texture mapped",
      "Lit texture map",
      "Lit persp. correct texture map",
      "Masked lit texture map",
      "Masked lit persp. correct texture map",
      "Transparent texture mapped",
      "Transparent perspective correct texture mapped",
      "Transparent masked texture mapped",
      "Transparent masked persp. correct texture mapped",
    };


    static BITMAP texture;



    /* initialise shape positions */
    static void init_shapes()
    {
      int c;

      for (c = 0; c < NUM_SHAPES; c++)
      {
        shapes[c].x = ((AL_RAND() & 255) - 128) << 16;
        shapes[c].y = ((AL_RAND() & 255) - 128) << 16;
        shapes[c].z = 768 << 16;
        shapes[c].rx = 0;
        shapes[c].ry = 0;
        shapes[c].rz = 0;
        shapes[c].dz = ((AL_RAND() & 255) - 8) << 12;
        shapes[c].drx = ((AL_RAND() & 31) - 16) << 12;
        shapes[c].dry = ((AL_RAND() & 31) - 16) << 12;
        shapes[c].drz = ((AL_RAND() & 31) - 16) << 12;
      }
    }



    /* update shape positions */
    static void animate_shapes()
    {
      int c;

      for (c = 0; c < NUM_SHAPES; c++)
      {
        shapes[c].z += shapes[c].dz;

        if ((shapes[c].z > itofix(1024)) ||
          (shapes[c].z < itofix(192)))
          shapes[c].dz = -shapes[c].dz;

        shapes[c].rx += shapes[c].drx;
        shapes[c].ry += shapes[c].dry;
        shapes[c].rz += shapes[c].drz;
      }
    }



    /* translate shapes from 3d world space to 2d screen space */
    static void translate_shapes()
    {
      int c, d;
      MATRIX matrix = new MATRIX();
      int outpoint = 0;
      int outface = 0;

      for (c = 0; c < NUM_SHAPES; c++)
      {
        /* build a transformation matrix */
        get_transformation_matrix(ref matrix, itofix(1),
          shapes[c].rx, shapes[c].ry, shapes[c].rz,
          shapes[c].x, shapes[c].y, shapes[c].z);

        /* output the vertices */
        for (d = 0; d < NUM_VERTICES; d++)
        {
          // TODO: salvare i valori in variabili temporanee e poi salvarle in output_points
          apply_matrix(ref matrix, points[d].x, points[d].y, points[d].z,
                 ref output_points[d + outpoint].x, ref output_points[d + outpoint].y, ref output_points[d + outpoint].z);
          persp_project(output_points[d + outpoint].x, output_points[d + outpoint].y, output_points[d + outpoint].z,
                  ref output_points[d + outpoint].x, ref output_points[d + outpoint].y);
        }

        /* output the faces */
        for (d = 0; d < NUM_FACES; d++)
        {
          output_faces[d + outface] = faces[d].Clone();
          VTX[] temp = new VTX[NUM_VERTICES];
          Array.Copy(output_points, c * NUM_VERTICES, temp, 0, NUM_VERTICES);
          output_faces[d + outface].vtxlist = temp;
        }

        outpoint += NUM_VERTICES;
        outface += NUM_FACES;
      }
    }




    /* draw a line (for wireframe display) */
    static void wire(BITMAP b, VTX v1, VTX v2)
    {
      int col = MID(128, 255 - fixtoi(v1.z + v2.z) / 16, 255);
      line(b, fixtoi(v1.x), fixtoi(v1.y), fixtoi(v2.x), fixtoi(v2.y),
        palette_color[col]);
    }



    /* draw a quad */
    static void draw_quad(BITMAP b, VTX v1, VTX v2, VTX v3, VTX v4, int mode)
    {
      int col;

      /* four vertices */
      V3D vtx1 = new V3D(0, 0, 0, 0, 0, 0);
      V3D vtx2 = new V3D(0, 0, 0, 32 << 16, 0, 0);
      V3D vtx3 = new V3D(0, 0, 0, 32 << 16, 32 << 16, 0);
      V3D vtx4 = new V3D(0, 0, 0, 0, 32 << 16, 0);

      vtx1.x = v1.x; vtx1.y = v1.y; vtx1.z = v1.z;
      vtx2.x = v2.x; vtx2.y = v2.y; vtx2.z = v2.z;
      vtx3.x = v3.x; vtx3.y = v3.y; vtx3.z = v3.z;
      vtx4.x = v4.x; vtx4.y = v4.y; vtx4.z = v4.z;

      /* cull backfaces */
      if ((mode != POLYTYPE_ATEX_MASK) && (mode != POLYTYPE_PTEX_MASK) &&
          (mode != POLYTYPE_ATEX_MASK_LIT) && (mode != POLYTYPE_PTEX_MASK_LIT) &&
          (polygon_z_normal(ref vtx1, ref vtx2, ref vtx3) < 0))
        return;

      /* set up the vertex color, differently for each rendering mode */
      switch (mode)
      {

        case POLYTYPE_FLAT:
          col = MID(128, 255 - fixtoi(v1.z + v2.z) / 16, 255);
          vtx1.c = vtx2.c = vtx3.c = vtx4.c = palette_color[col];
          break;

        case POLYTYPE_GCOL:
          vtx1.c = palette_color[0xD0];
          vtx2.c = palette_color[0x80];
          vtx3.c = palette_color[0xB0];
          vtx4.c = palette_color[0xFF];
          break;

        case POLYTYPE_GRGB:
          vtx1.c = 0x000000;
          vtx2.c = 0x7F0000;
          vtx3.c = 0xFF0000;
          vtx4.c = 0x7F0000;
          break;

        case POLYTYPE_ATEX_LIT:
        case POLYTYPE_PTEX_LIT:
        case POLYTYPE_ATEX_MASK_LIT:
        case POLYTYPE_PTEX_MASK_LIT:
          vtx1.c = MID(0, 255 - fixtoi(v1.z) / 4, 255);
          vtx2.c = MID(0, 255 - fixtoi(v2.z) / 4, 255);
          vtx3.c = MID(0, 255 - fixtoi(v3.z) / 4, 255);
          vtx4.c = MID(0, 255 - fixtoi(v4.z) / 4, 255);
          break;
      }

      /* draw the quad */
      quad3d(b, mode, texture, ref vtx1, ref vtx2, ref vtx3, ref vtx4);
    }



    /* callback for qsort() */
    static int quad_cmp(QUAD e1, QUAD e2)
    {
      QUAD q1 = (QUAD)e1;
      QUAD q2 = (QUAD)e2;

      int d1 = q1.vtxlist[q1.v1].z + q1.vtxlist[q1.v2].z +
           q1.vtxlist[q1.v3].z + q1.vtxlist[q1.v4].z;

      int d2 = q2.vtxlist[q2.v1].z + q2.vtxlist[q2.v2].z +
           q2.vtxlist[q2.v3].z + q2.vtxlist[q2.v4].z;

      return d2 - d1;
    }



    /* draw the shapes calculated by translate_shapes() */
    static void draw_shapes(BITMAP b)
    {
      int c;
      QUAD[] face = output_faces;
      VTX v1, v2, v3, v4;

      /* depth sort */
      Array.Sort(output_faces);

      for (c = 0; c < NUM_FACES * NUM_SHAPES; c++)
      {
        /* find the vertices used by the face */
        v1 = face[c].vtxlist[face[c].v1];
        v2 = face[c].vtxlist[face[c].v2];
        v3 = face[c].vtxlist[face[c].v3];
        v4 = face[c].vtxlist[face[c].v4];

        /* draw the face */
        if (_render_mode == render_mode.wireframe)
        {
          wire(b, v1, v2);
          wire(b, v2, v3);
          wire(b, v3, v4);
          wire(b, v4, v1);
        }
        else
        {
          draw_quad(b, v1, v2, v3, v4, render_type[(int)_render_mode]);
        }
      }
    }



    /* RGB -> color mapping table. Not needed, but speeds things up */
    static RGB_MAP rgb_table = new RGB_MAP();


    /* lighting color mapping table */
    static COLOR_MAP light_table = new COLOR_MAP();

    /* transparency color mapping table */
    static COLOR_MAP trans_table = new COLOR_MAP();



    static int Main()
    {
      BITMAP buffer;
      PALETTE pal = new PALETTE();
      int c, w, h, bpp;
      int last_retrace_count;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_mouse();
      install_timer();

      /* color 0 = black */
      pal[0].r = pal[0].g = pal[0].b = 0;

      /* copy the desktop palette */
      for (c = 1; c < 64; c++)
        pal[c] = desktop_palette[c];

      /* make a red gradient */
      for (c = 64; c < 96; c++)
      {
        pal[c].r = (byte)((c - 64) * 2);
        pal[c].g = pal[c].b = 0;
      }

      /* make a green gradient */
      for (c = 96; c < 128; c++)
      {
        pal[c].g = (byte)((c - 96) * 2);
        pal[c].r = pal[c].b = 0;
      }

      /* set up a greyscale in the top half of the palette */
      for (c = 128; c < 256; c++)
        pal[c].r = pal[c].g = pal[c].b = (byte)((c - 128) / 2);

      /* build rgb_map table */
      create_rgb_table(rgb_table, pal, NULL);
      rgb_map = rgb_table;

      /* build a lighting table */
      create_light_table(light_table, pal, 0, 0, 0, NULL);
      color_map = light_table;

      /* build a transparency table */
      /* textures are 25% transparent (75% opaque) */
      create_trans_table(trans_table, pal, 192, 192, 192, NULL);

      /* set up the truecolor blending functions */
      /* textures are 25% transparent (75% opaque) */
      set_trans_blender(0, 0, 0, 192);

      /* set the graphics mode */
      if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
        return 1;
      }
      set_palette(desktop_palette);

      c = GFX_AUTODETECT;
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

      set_palette(pal);

      /* make a bitmap for use as a texture map */
      texture = create_bitmap(32, 32);
      clear_to_color(texture, bitmap_mask_color(texture));
      line(texture, 0, 0, 31, 31, palette_color[1]);
      line(texture, 0, 31, 31, 0, palette_color[1]);
      rect(texture, 0, 0, 31, 31, palette_color[1]);
      textout_ex(texture, font, "dead", 0, 0, palette_color[2], -1);
      textout_ex(texture, font, "pigs", 0, 8, palette_color[2], -1);
      textout_ex(texture, font, "cant", 0, 16, palette_color[2], -1);
      textout_ex(texture, font, "fly.", 0, 24, palette_color[2], -1);

      /* double buffer the animation */
      buffer = create_bitmap(SCREEN_W, SCREEN_H);

      /* set up the viewport for the perspective projection */
      set_projection_viewport(0, 0, SCREEN_W, SCREEN_H);

      /* initialise the bouncing shapes */
      init_shapes();

      last_retrace_count = retrace_count;

      for (; ; )
      {
        clear_bitmap(buffer);

        while (last_retrace_count < retrace_count)
        {
          animate_shapes();
          last_retrace_count++;
        }

        translate_shapes();
        draw_shapes(buffer);

        textprintf_ex(buffer, font, 0, 0, palette_color[192], -1, string.Format("{0}, {1} bpp",
          mode_desc[(int)_render_mode], bitmap_color_depth(screen)));
        textout_ex(buffer, font, "Press a key to change", 0, 12,
          palette_color[192], -1);

        vsync();
        blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);

        if (keypressed())
        {
          if ((readkey() & 0xFF) == 27)
            break;
          else
          {
            _render_mode++;
            if (_render_mode >= render_mode.last_mode)
            {
              _render_mode = render_mode.wireframe;
              color_map = light_table;
            }
            if (render_type[(int)_render_mode] >= POLYTYPE_ATEX_TRANS)
              color_map = trans_table;
          }
        }
      }

      destroy_bitmap(buffer);
      destroy_bitmap(texture);

      return 0;
    }
  }
}
