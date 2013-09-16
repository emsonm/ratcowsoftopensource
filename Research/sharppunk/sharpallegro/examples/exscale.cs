using System;
using System.Text;

using sharpallegro;

namespace exscale
{
  unsafe class exscale : Allegro
  {
    static int Main(string[] argv)
    {
      PALETTE my_palette = new PALETTE();
      BITMAP scr_buffer;
      byte[] pcx_name = new byte[256];

      if (allegro_init() != 0)
        return 1;
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

      replace_filename(pcx_name, "./", "mysha.pcx", 256);
      scr_buffer = load_pcx(Encoding.ASCII.GetString(pcx_name), my_palette);
      if (!scr_buffer)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Error loading " + pcx_name + "!\n");
        return 1;
      }

      set_palette(my_palette);
      blit(scr_buffer, screen, 0, 0, 0, 0, scr_buffer.w, scr_buffer.h);

      while (!keypressed())
      {
        stretch_blit(scr_buffer, screen, 0, 0, AL_RAND() % scr_buffer.w,
         AL_RAND() % scr_buffer.h, AL_RAND() % SCREEN_W, AL_RAND() % SCREEN_H,
         AL_RAND() % SCREEN_W, AL_RAND() % SCREEN_H);
        vsync();
      }

      destroy_bitmap(scr_buffer);
      return 0;
    }
  }
}
