using System;
using System.Text;

using sharpallegro;

namespace exxfade
{
  class exxfade : Allegro
  {
    static int show(string name)
    {
      BITMAP bmp, buffer;
      PALETTE pal = new PALETTE();
      int alpha;

      /* load the file */
      bmp = load_bitmap(name, pal);
      if (!bmp)
        return -1;

      buffer = create_bitmap(SCREEN_W, SCREEN_H);
      blit(screen, buffer, 0, 0, 0, 0, SCREEN_W, SCREEN_H);

      set_palette(pal);

      /* fade it in on top of the previous picture */
      for (alpha = 0; alpha < 256; alpha += 8)
      {
        set_trans_blender(0, 0, 0, alpha);
        draw_trans_sprite(buffer, bmp,
        (SCREEN_W - bmp.w) / 2, (SCREEN_H - bmp.h) / 2);
        vsync();
        blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
        if (keypressed())
        {
          destroy_bitmap(bmp);
          destroy_bitmap(buffer);
          if ((readkey() & 0xFF) == 27)
            return 1;
          else
            return 0;
        }
      }

      blit(bmp, screen, 0, 0, (SCREEN_W - bmp.w) / 2, (SCREEN_H - bmp.h) / 2,
     bmp.w, bmp.h);

      destroy_bitmap(bmp);
      destroy_bitmap(buffer);

      if ((readkey() & 0xFF) == 27)
        return 1;
      else
        return 0;
    }

    static int Main(string[] argv)
    {
      int i;

      if (allegro_init() != 0)
        return 1;

      if (argv.Length < 1)
      {
        allegro_message("Usage: 'exxfade files.[bmp|lbm|pcx|tga]'\n");
        return 1;
      }

      install_keyboard();

      /* set the best color depth that we can find */
      set_color_depth(16);
      if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
      {
        set_color_depth(15);
        if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
        {
          set_color_depth(32);
          if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
          {
            set_color_depth(24);
            if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
            {
              set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
              allegro_message(string.Format("Error setting graphics mode\n{0}\n",
                  allegro_error));
              return 1;
            }
          }
        }
      }

      /* load all images in the same color depth as the display */
      set_color_conversion(COLORCONV_TOTAL);

      /* process all the files on our command line */
      i = 0;
      for (; ; )
      {
        switch (show(argv[i]))
        {

          case -1:
            /* error */
            set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
            allegro_message(string.Format("Error loading image file '{0}'\n", argv[i]));
            return 1;

          case 0:
            /* ok! */
            break;

          case 1:
            /* quit */
            allegro_exit();
            return 0;
        }

        if (++i >= argv.Length)
          i = 0;
      }

      return 0;
    }
  }
}
