using System;
using System.Text;

using sharpallegro;

namespace extrans
{
  class extrans : Allegro
  {
    /* RGB -> color mapping table. Not needed, but speeds things up */
    static RGB_MAP rgb_table = new RGB_MAP();

    /* lighting color mapping table */
    static COLOR_MAP light_table = new COLOR_MAP();

    /* translucency color mapping table */
    static COLOR_MAP trans_table = new COLOR_MAP();



    static int Main(string[] argv)
    {
      PALETTE pal = new PALETTE();
      BITMAP s;
      BITMAP spotlight;
      BITMAP truecolor_spotlight;
      BITMAP background;
      int i, x, y;
      byte[] buf = new byte[256];
      string filename;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();
      install_mouse();

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

      /* load the main screen image */
      if (argv.Length > 0)
        filename = argv[1];
      else
      {
        replace_filename(buf, "./", "allegro.pcx", buf.Length);
        filename = Encoding.ASCII.GetString(buf);
      }

      background = load_bitmap(filename, pal);
      if (!background)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading {0}!\n", filename));
        return 1;
      }

      /* this isn't needed, but it speeds up the color table calculations */
      create_rgb_table(rgb_table, pal, NULL);
      rgb_map = rgb_table;

      /* build a color lookup table for lighting effects */
      create_light_table(light_table, pal, 0, 0, 0, NULL);

      /* build a color lookup table for translucent drawing */
      create_trans_table(trans_table, pal, 128, 128, 128, NULL);

      set_palette(pal);

      s = create_bitmap(320, 200);
      spotlight = create_bitmap_ex(8, 128, 128);
      truecolor_spotlight = create_bitmap_ex(32, 128, 128);

      /* generate an 8bpp spotlight image */
      clear_bitmap(spotlight);
      for (i = 0; i < 256; i++)
        circlefill(spotlight, 64, 64, 64 - i / 4, i);

      /* select the lighting table */
      color_map = light_table;

      /* display a spotlight effect */
      do
      {
        poll_mouse();

        x = mouse_x - SCREEN_W / 2 - 64 + 160;
        y = mouse_y - SCREEN_H / 2 - 64 + 100;

        clear_bitmap(s);

        /* unluckily we have to do something 'weird' for truecolor modes */
        if (bitmap_color_depth(screen) != 8)
        {
          /* copy background on the truecolor spotlight */
          blit(background, truecolor_spotlight, x, y, 0, 0, 127, 127);
          /* set special write alpha blender */
          set_write_alpha_blender();
          drawing_mode(DRAW_MODE_TRANS, NULL, 0, 0);
          /* draw the alpha channel */
          rectfill(truecolor_spotlight, 0, 0, 128, 128, 0);
          draw_trans_sprite(truecolor_spotlight, spotlight, 0, 0);
          /* set alpha blender and draw on the 's' bitmap */
          drawing_mode(DRAW_MODE_SOLID, NULL, 0, 0);
          set_alpha_blender();
          draw_trans_sprite(s, truecolor_spotlight, x, y);
          /* restore a not-alpha blender */
          set_trans_blender(0, 0, 0, 128);
        }
        else
        {
          blit(background, s, x, y, x, y, 128, 128);
          draw_trans_sprite(s, spotlight, x, y);
        }

        blit(s, screen, 0, 0, SCREEN_W / 2 - 160, SCREEN_H / 2 - 100, 320, 200);

        /* reduce CPU usage */
        rest(20);

      } while (!keypressed());

      clear_keybuf();

      /* for the next part we want spotlight and s to share color depth */
      if (bitmap_color_depth(spotlight) != bitmap_color_depth(s))
      {
        destroy_bitmap(spotlight);
        spotlight = create_bitmap(128, 128);
      }

      /* generate an overlay image (just shrink the main image) */
      stretch_blit(background, spotlight, 0, 0, 320, 200, 0, 0, 128, 128);

      /* select the translucency table */
      color_map = trans_table;

      /* select translucency blender */
      set_trans_blender(0, 0, 0, 128);

      /* display a translucent overlay */
      do
      {
        poll_mouse();

        x = mouse_x - SCREEN_W / 2 - 64 + 160;
        y = mouse_y - SCREEN_H / 2 - 64 + 100;

        blit(background, s, 0, 0, 0, 0, 320, 200);
        draw_trans_sprite(s, spotlight, x, y);

        blit(s, screen, 0, 0, SCREEN_W / 2 - 160, SCREEN_H / 2 - 100, 320, 200);

        /* reduce CPU usage */
        rest(20);

      } while (!keypressed());

      clear_keybuf();

      destroy_bitmap(s);
      destroy_bitmap(spotlight);
      destroy_bitmap(truecolor_spotlight);
      destroy_bitmap(background);

      return 0;
    }
  }
}
