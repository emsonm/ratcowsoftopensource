using System;
using System.Text;

using sharpallegro;

namespace exshade
{
  class exshade : Allegro
  {
    /* RGB -> color mapping table. Not needed, but speeds things up */
    static RGB_MAP rgb_table = new RGB_MAP();

    static COLOR_MAP light_table = new COLOR_MAP();



    /* Considered a line between (x1, y1) and (x2, y2), the longer the line,
     * the smaller the return value will be. If the line length is zero, the
     * function returns the maximum value of 255.
     */
    static int distance(int x1, int y1, int x2, int y2)
    {
      int dx = x2 - x1;
      int dy = y2 - y1;
      int temp = (int)Math.Sqrt((dx * dx) + (dy * dy));

      temp *= 2;
      if (temp > 255)
        temp = 255;

      return (255 - temp);
    }



    static int Main(string[] argv)
    {
      PALETTE pal = new PALETTE();
      BITMAP buffer;
      BITMAP planet;
      byte[] buf = new byte[256];

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_mouse();

      if (set_gfx_mode(GFX_AUTODETECT, 320, 240, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 240, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
              allegro_error));
          return 1;
        }
      }

      replace_filename(buf, "./", "planet.pcx", buf.Length);

      planet = load_bitmap(Encoding.ASCII.GetString(buf), pal);
      if (!planet)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading {0}\n", Encoding.ASCII.GetString(buf)));
        return 1;
      }

      buffer = create_bitmap(SCREEN_W, SCREEN_H);
      clear_bitmap(buffer);

      set_palette(pal);

      create_rgb_table(rgb_table, pal, NULL);
      rgb_map = rgb_table;

      create_light_table(light_table, pal, 0, 0, 0, NULL);
      color_map = light_table;

      set_trans_blender(0, 0, 0, 128);

      do
      {
        poll_mouse();

        draw_gouraud_sprite(buffer, planet, SCREEN_W / 2, SCREEN_H / 2,
          distance(SCREEN_W / 2, SCREEN_H / 2, mouse_x, mouse_y),
          distance(SCREEN_W / 2 + planet.w, SCREEN_H / 2,
             mouse_x, mouse_y),
          distance(SCREEN_W / 2 + planet.w,
             SCREEN_H / 2 + planet.h, mouse_x, mouse_y),
          distance(SCREEN_W / 2, SCREEN_H / 2 + planet.h,
             mouse_x, mouse_y));

        textout_ex(buffer, font, "Gouraud Shaded Sprite Demo", 0, 0,
          palette_color[10], -1);

        show_mouse(buffer);
        blit(buffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
        show_mouse(NULL);

      } while (!keypressed());

      destroy_bitmap(planet);
      destroy_bitmap(buffer);

      return 0;
    }
  }
}
