using System;
using System.Text;

using sharpallegro;

namespace exbitmap
{
  class exbitmap : Allegro
  {
    static int Main(string[] argv)
    {
      BITMAP the_image;
      PALETTE the_palette = new PALETTE();

      if (allegro_init() != 0)
        return 1;

      if (argv.Length != 1)
      {
        allegro_message("Usage: 'exbitmap filename.[bmp|lbm|pcx|tga]'\n");
        return 1;
      }

      install_keyboard();

      if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message("Unable to set any graphic mode\n" + allegro_error);
          return 1;
        }
      }

      /* read in the bitmap file */
      the_image = load_bitmap(argv[0], the_palette);
      if (!the_image)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading bitmap file '{0}'\n", argv[0]));
        return 1;
      }

      /* select the bitmap palette */
      set_palette(the_palette);

      /* blit the image onto the screen */
      blit(the_image, screen, 0, 0, (SCREEN_W - the_image.w) / 2,
     (SCREEN_H - the_image.h) / 2, the_image.w, the_image.h);

      /* destroy the bitmap */
      destroy_bitmap(the_image);

      readkey();
      return 0;
    }
  }
}
