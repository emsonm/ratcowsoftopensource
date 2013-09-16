using System;
using System.Text;

using sharpallegro;

namespace exscale
{
  class exscale : Allegro
  {
    static void test(int colordepth)
    {
      PALETTE pal = new PALETTE();
      int x;

      /* set the screen mode */
      set_color_depth(colordepth);

      if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
        return;

      /* in case this is a 256 color mode, we'd better make sure that the
       * palette is set to something sensible. This function generates a
       * standard palette with a nice range of different colors...
       */
      generate_332_palette(pal);
      set_palette(pal);

      acquire_screen();

      clear_to_color(screen, makecol(0, 0, 0));

      textprintf_ex(screen, font, 0, 0, makecol(255, 255, 255), -1,
        colordepth + " bit color...");

      /* use the makecol() function to specify RGB values... */
      textout_ex(screen, font, "Red", 32, 80, makecol(255, 0, 0), -1);
      textout_ex(screen, font, "Green", 32, 100, makecol(0, 255, 0), -1);
      textout_ex(screen, font, "Blue", 32, 120, makecol(0, 0, 255), -1);
      textout_ex(screen, font, "Yellow", 32, 140, makecol(255, 255, 0), -1);
      textout_ex(screen, font, "Cyan", 32, 160, makecol(0, 255, 255), -1);
      textout_ex(screen, font, "Magenta", 32, 180, makecol(255, 0, 255), -1);
      textout_ex(screen, font, "Grey", 32, 200, makecol(128, 128, 128), -1);

      /* or we could draw some nice smooth color gradients... */
      for (x = 0; x < 256; x++)
      {
        vline(screen, 192 + x, 112, 176, makecol(x, 0, 0));
        vline(screen, 192 + x, 208, 272, makecol(0, x, 0));
        vline(screen, 192 + x, 304, 368, makecol(0, 0, x));
      }

      textout_centre_ex(screen, font, "<press a key>", SCREEN_W / 2, SCREEN_H - 16,
            makecol(255, 255, 255), -1);

      release_screen();

      readkey();
    }

    static int Main()
    {
      if (allegro_init() != 0)
        return 1;
      install_keyboard();

      /* try each of the possible possible color depths... */
      test(8);
      test(15);
      test(16);
      test(24);
      test(32);

      return 0;
    }
  }
}
