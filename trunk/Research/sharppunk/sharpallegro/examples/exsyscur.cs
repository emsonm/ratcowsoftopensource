using System;
using System.Text;

using sharpallegro;

namespace exsyscur
{
  class exsyscur : Allegro
  {
    static int black, white;


    static void do_select_cursor(int cursor)
    {
      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3 + 2 * text_height(font), black, white,
         string.Format("Before: {0}, {1}",
         (gfx_capabilities & GFX_HW_CURSOR) > 0 ? "HW_CURSOR   " : "no HW_CURSOR",
         (gfx_capabilities & GFX_SYSTEM_CURSOR) > 0 ? "SYSTEM_CURSOR   " : "no SYSTEM_CURSOR"));

      select_mouse_cursor(cursor);
      show_mouse(screen);

      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3 + 3 * text_height(font), black, white,
         string.Format("After:  {0}, {1}",
         (gfx_capabilities & GFX_HW_CURSOR) > 0 ? "HW_CURSOR   " : "no HW_CURSOR",
         (gfx_capabilities & GFX_SYSTEM_CURSOR) > 0 ? "SYSTEM_CURSOR   " : "no SYSTEM_CURSOR"));
    }


    static bool handle_key()
    {
      int k;

      k = readkey() & 0xff;

      switch (k)
      {
        case 27:
        case 'q':
        case 'Q':
          return true;
        case '1':
          do_select_cursor(MOUSE_CURSOR_ALLEGRO);
          break;
        case '2':
          do_select_cursor(MOUSE_CURSOR_ARROW);
          break;
        case '3':
          do_select_cursor(MOUSE_CURSOR_BUSY);
          break;
        case '4':
          do_select_cursor(MOUSE_CURSOR_QUESTION);
          break;
        case '5':
          do_select_cursor(MOUSE_CURSOR_EDIT);
          break;
        default:
          break;
      }

      return false;
    }

    static int Main()
    {
      int height;

      /* Initialize Allegro */
      if (allegro_init() != 0)
      {
        allegro_message("Error initializing Allegro: " + allegro_error + "\n");
        return 1;
      }

      /* Initialize mouse and keyboard */
      install_timer();
      install_mouse();
      install_keyboard();

      if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Error setting video mode: " + allegro_error + "\n");
        return 1;
      }
      black = makecol(0, 0, 0);
      white = makecol(255, 255, 255);
      clear_to_color(screen, white);

      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3, black, white,
                           "Graphics driver: " + gfx_driver.name);
      enable_hardware_cursor();

      height = SCREEN_H / 3 + 5 * text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "1) MOUSE_CURSOR_ALLEGRO  ");
      height += text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "2) MOUSE_CURSOR_ARROW    ");
      height += text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "3) MOUSE_CURSOR_BUSY     ");
      height += text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "4) MOUSE_CURSOR_QUESTION ");
      height += text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "5) MOUSE_CURSOR_EDIT     ");
      height += text_height(font);
      textprintf_centre_ex(screen, font, SCREEN_W / 2, height, black, white,
                           "Escape) Quit             ");

      /* first cursor shown */
      do_select_cursor(MOUSE_CURSOR_ALLEGRO);

      do
      {
      } while (!handle_key());

      return 0;
    }
  }
}
