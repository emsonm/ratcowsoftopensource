using System;
using System.Text;

using sharpallegro;

namespace extimer
{
  class extimer : Allegro
  {
    /* these must be declared volatile so the optimiser doesn't mess up */
    volatile static int x = 0;
    volatile static int y = 0;
    volatile static int z = 0;

    /* timer interrupt handler */
    static void inc_x()
    {
      x++;
    }

    /* timer interrupt handler */
    static void inc_y()
    {
      y++;
    }

    /* timer interrupt handler */
    static void inc_z()
    {
      z++;
    }

    static TimerHandler t_x = new TimerHandler(inc_x);
    static TimerHandler t_y = new TimerHandler(inc_y);
    static TimerHandler t_z = new TimerHandler(inc_z);

    static int Main()
    {
      int c;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();

      if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
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

      textprintf_centre_ex(screen, font, SCREEN_W / 2, 8, makecol(0, 0, 0),
         makecol(255, 255, 255), "Driver: " +
         timer_driver.name);

      /* use rest() to delay for a specified number of milliseconds */
      textprintf_centre_ex(screen, font, SCREEN_W / 2, 48, makecol(0, 0, 0),
         makecol(255, 255, 255), "Timing five seconds:");

      for (c = 1; c <= 5; c++)
      {
        textprintf_centre_ex(screen, font, SCREEN_W / 2, 62 + c * 10,
           makecol(0, 0, 0), makecol(255, 255, 255), c.ToString());
        rest(1000);
      }

      textprintf_centre_ex(screen, font, SCREEN_W / 2, 142, makecol(0, 0, 0),
         makecol(255, 255, 255),
         "Press a key to set up interrupts");
      readkey();

      /* all variables and code used inside interrupt handlers must be locked */
      LOCK_VARIABLE(x);
      LOCK_VARIABLE(y);
      LOCK_VARIABLE(z);
      LOCK_FUNCTION(t_x);
      LOCK_FUNCTION(t_y);
      LOCK_FUNCTION(t_z);

      /* the speed can be specified in milliseconds (this is once a second) */
      install_int(t_x, 1000);

      /* or in beats per second (this is 10 ticks a second) */
      install_int_ex(t_y, BPS_TO_TIMER(10));

      /* or in seconds (this is 10 seconds a tick) */
      install_int_ex(t_z, SECS_TO_TIMER(10));

      /* the interrupts are now active... */
      while (!keypressed())
        textprintf_centre_ex(screen, font, SCREEN_W / 2, 176, makecol(0, 0, 0),
           makecol(255, 255, 255), string.Format("x={0}, y={1}, z={2}", x, y, z));

      return 0;
    }
  }
}
