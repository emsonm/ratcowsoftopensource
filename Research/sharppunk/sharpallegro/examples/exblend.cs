using System;
using System.Text;

using sharpallegro;

namespace exblend
{
  class exblend : Allegro
  {
    static int[] color_depths = { 16, 15, 32, 24, 0 };

    static int Main(string[] argv)
    {
      byte[] buf = new byte[256];
      PALETTE pal = new PALETTE();
      BITMAP image1;
      BITMAP image2;
      BITMAP buffer;
      int r, g, b, a;
      int x, y, w, h;
      int x1, y1, x2, y2;
      int prevx1, prevy1, prevx2, prevy2;
      int timer;
      int bpp = -1;
      int ret = -1;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();

      /* what color depth should we use? */
      if (argv.Length > 0)
      {
        if ((argv[0][0] == '-') || (argv[0][0] == '/'))
          argv[0] = argv[0].Substring(1);
        bpp = int.Parse(argv[0]);
        if ((bpp != 15) && (bpp != 16) && (bpp != 24) && (bpp != 32))
        {
          allegro_message(string.Format("Invalid color depth '{0}'\n", argv[0]));
          return 1;
        }
      }

      if (bpp > 0)
      {
        /* set a user-requested color depth */
        set_color_depth(bpp);
        ret = set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0);
      }
      else
      {
        /* autodetect what color depths are available */
        for (a = 0; color_depths[a] > 0; a++)
        {
          bpp = color_depths[a];
          set_color_depth(bpp);
          ret = set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0);
          if (ret == 0)
            break;
        }
      }

      /* did the video mode set properly? */
      if (ret != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error setting {0} bit graphics mode\n{1}\n", bpp,
            allegro_error));
        return 1;
      }

      /* specify that images should be loaded in a truecolor pixel format */
      set_color_conversion(COLORCONV_TOTAL);

      /* load the first picture */
      replace_filename(buf, "./", "allegro.pcx", 256);
      image1 = load_bitmap(Encoding.ASCII.GetString(buf), pal);
      if (!image1)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading {0}!\n", buf));
        return 1;
      }

      /* load the second picture */
      replace_filename(buf, "./", "mysha.pcx", 256);
      image2 = load_bitmap(Encoding.ASCII.GetString(buf), pal);
      if (!image2)
      {
        destroy_bitmap(image1);
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading {0}!\n", buf));
        return 1;
      }

      /* create a double buffer bitmap */
      buffer = create_bitmap(SCREEN_W, SCREEN_H);

      /* Note that because we loaded the images as truecolor bitmaps, we don't
       * need to bother setting the palette, and we can display both on screen
       * at the same time even though the source files use two different 256
       * color palettes...
       */

      prevx1 = prevy1 = prevx2 = prevy2 = 0;

      textprintf_ex(screen, font, 0, SCREEN_H - 8, makecol(255, 255, 255), 0,
        string.Format("{0} bpp", bpp));

      while (!keypressed())
      {
        timer = retrace_count;
        clear_bitmap(buffer);

        /* the first image moves in a slow circle while being tinted to 
         * different colors...
         */
        x1 = 160 + fixtoi(fixsin(itofix(timer) / 16) * 160);
        y1 = 140 - fixtoi(fixcos(itofix(timer) / 16) * 140);
        r = 127 - fixtoi(fixcos(itofix(timer) / 6) * 127);
        g = 127 - fixtoi(fixcos(itofix(timer) / 7) * 127);
        b = 127 - fixtoi(fixcos(itofix(timer) / 8) * 127);
        a = 127 - fixtoi(fixcos(itofix(timer) / 9) * 127);
        set_trans_blender(r, g, b, 0);
        draw_lit_sprite(buffer, image1, x1, y1, a);
        textprintf_ex(screen, font, 0, 0, makecol(r, g, b), 0, string.Format("light: {0} ", a));

        /* the second image moves in a faster circle while the alpha value
         * fades in and out...
         */
        x2 = 160 + fixtoi(fixsin(itofix(timer) / 10) * 160);
        y2 = 140 - fixtoi(fixcos(itofix(timer) / 10) * 140);
        a = 127 - fixtoi(fixcos(itofix(timer) / 4) * 127);
        set_trans_blender(0, 0, 0, a);
        draw_trans_sprite(buffer, image2, x2, y2);
        textprintf_ex(screen, font, 0, 8, makecol(a, a, a), 0, string.Format("alpha: {0} ", a));

        /* copy the double buffer across to the screen */
        vsync();

        x = MIN(x1, prevx1);
        y = MIN(y1, prevy1);
        w = MAX(x1, prevx1) + 320 - x;
        h = MAX(y1, prevy1) + 200 - y;
        blit(buffer, screen, x, y, x, y, w, h);

        x = MIN(x2, prevx2);
        y = MIN(y2, prevy2);
        w = MAX(x2, prevx2) + 320 - x;
        h = MAX(y2, prevy2) + 200 - y;
        blit(buffer, screen, x, y, x, y, w, h);

        prevx1 = x1;
        prevy1 = y1;
        prevx2 = x2;
        prevy2 = y2;
      }

      clear_keybuf();

      destroy_bitmap(image1);
      destroy_bitmap(image2);
      destroy_bitmap(buffer);

      return 0;
    }
  }
}
