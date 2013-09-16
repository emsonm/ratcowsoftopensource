using System;

using sharpallegro;

namespace exmouse
{
  class exmouse : Allegro
  {
    static void print_all_buttons()
{
   int i;
   int fc = makecol(0, 0, 0);
   int bc = makecol(255, 255, 255);
   textprintf_right_ex(screen, font, 320, 50, fc, bc, "buttons");
   for (i = 0; i < 8; i++) {
      int x = 320;
      int y = 60 + i * 10;
      if ((mouse_b & (1 << i)) > 0)
         textprintf_right_ex(screen, font, x, y, fc, bc, string.Format("{0,2}", 1 + i));
      else
         textprintf_right_ex(screen, font, x, y, fc, bc, "  ");
   }
}
    static unsafe int Main()
    {
      int mickeyx = 0;
      int mickeyy = 0;
      BITMAP custom_cursor;
      int c = 0;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();

      if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message("Unable to set any graphic mode\n" + allegro_error);
          return 1;
        }
      }

      set_palette(desktop_palette);
      clear_to_color(screen, makecol(255, 255, 255));

      /* Detect mouse presence */
      if (install_mouse() < 0)
      {
        textout_centre_ex(screen, font, "No mouse detected, but you need one!",
        SCREEN_W / 2, SCREEN_H / 2, makecol(0, 0, 0),
        makecol(255, 255, 255));
        readkey();
        return 0;
      }

      textprintf_centre_ex(screen, font, SCREEN_W / 2, 8, makecol(0, 0, 0),
         makecol(255, 255, 255),
         "Driver: " + mouse_driver.name);

      do
      {
        /* On most platforms (eg. DOS) things will still work correctly
         * without this call, but it is a good idea to include it in any
         * programs that you want to be portable, because on some platforms
         * you may not be able to get any mouse input without it.
         */
        poll_mouse();

        acquire_screen();

        /* the mouse position is stored in the variables mouse_x and mouse_y */
        textprintf_ex(screen, font, 16, 48, makecol(0, 0, 0),
          makecol(255, 255, 255), string.Format("mouse_x = {0,-5}", mouse_x));
        textprintf_ex(screen, font, 16, 64, makecol(0, 0, 0),
          makecol(255, 255, 255), string.Format("mouse_y = {0,-5}", mouse_y));

        /* or you can use this function to measure the speed of movement.
         * Note that we only call it every fourth time round the loop:
         * there's no need for that other than to slow the numbers down
         * a bit so that you will have time to read them...
         */
        c++;
        if ((c & 3) == 0)
          get_mouse_mickeys(ref mickeyx, ref mickeyy);

        textprintf_ex(screen, font, 16, 88, makecol(0, 0, 0),
          makecol(255, 255, 255), string.Format("mickey_x = {0,-7}", mickeyx));
        textprintf_ex(screen, font, 16, 104, makecol(0, 0, 0),
          makecol(255, 255, 255), string.Format("mickey_y = {0,-7}", mickeyy));

        /* the mouse button state is stored in the variable mouse_b */
        if ((mouse_b & 1) > 0)
          textout_ex(screen, font, "left button is pressed ", 16, 128,
               makecol(0, 0, 0), makecol(255, 255, 255));
        else
          textout_ex(screen, font, "left button not pressed", 16, 128,
               makecol(0, 0, 0), makecol(255, 255, 255));

        if ((mouse_b & 2) > 0)
          textout_ex(screen, font, "right button is pressed ", 16, 144,
               makecol(0, 0, 0), makecol(255, 255, 255));
        else
          textout_ex(screen, font, "right button not pressed", 16, 144,
               makecol(0, 0, 0), makecol(255, 255, 255));

        if ((mouse_b & 4) > 0)
          textout_ex(screen, font, "middle button is pressed ", 16, 160,
               makecol(0, 0, 0), makecol(255, 255, 255));
        else
          textout_ex(screen, font, "middle button not pressed", 16, 160,
               makecol(0, 0, 0), makecol(255, 255, 255));

        /* the wheel position is stored in the variable mouse_z */
        textprintf_ex(screen, font, 16, 184, makecol(0, 0, 0),
          makecol(255, 255, 255), string.Format("mouse_z = {0,-5} mouse_w = {1,-5}", mouse_z, mouse_w));

        print_all_buttons();

        release_screen();

        vsync();

      } while (!keypressed());

      clear_keybuf();

      /*  To display a mouse pointer, call show_mouse(). There are several
       *  things you should be aware of before you do this, though. For one,
       *  it won't work unless you call install_timer() first. For another,
       *  you must never draw anything onto the screen while the mouse
       *  pointer is visible. So before you draw anything, be sure to turn 
       *  the mouse off with show_mouse(NULL), and turn it back on again when
       *  you are done.
       */
      clear_to_color(screen, makecol(255, 255, 255));
      textout_centre_ex(screen, font, "Press a key to change cursor",
            SCREEN_W / 2, SCREEN_H / 2, makecol(0, 0, 0),
            makecol(255, 255, 255));
      show_mouse(screen);
      readkey();
      show_mouse(NULL);

      /* create a custom mouse cursor bitmap... */
      custom_cursor = create_bitmap(32, 32);
      clear_to_color(custom_cursor, bitmap_mask_color(screen));
      for (c = 0; c < 8; c++)
        circle(custom_cursor, 16, 16, c * 2, palette_color[c]);

      /* select the custom cursor and set the focus point to the middle of it */
      set_mouse_sprite(custom_cursor);
      set_mouse_sprite_focus(16, 16);

      clear_to_color(screen, makecol(255, 255, 255));
      textout_centre_ex(screen, font, "Press a key to quit", SCREEN_W / 2,
            SCREEN_H / 2, makecol(0, 0, 0), makecol(255, 255, 255));
      show_mouse(screen);
      readkey();
      show_mouse(NULL);

      destroy_bitmap(custom_cursor);

      return 0;
    }
  }
}
