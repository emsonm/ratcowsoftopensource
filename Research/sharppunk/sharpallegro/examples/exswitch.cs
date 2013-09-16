using System;
using System.Text;

using sharpallegro;

namespace exswitch
{
  class exswitch : Allegro
  {
    /* there is no particular reason to use sub-bitmaps here: I just do it as a
     * stress-test, to make sure the switching code will handle them correctly.
     */
    static BITMAP text_area;
    static BITMAP graphics_area;

    static int in_callback = 0;
    static int out_callback = 0;



    /* timer callbacks should go on running when we are in background mode */
    volatile static int counter = 0;


    static void increment_counter()
    {
      counter++;
    }

    static TimerHandler t_counter = new TimerHandler(increment_counter);



    /* displays a text message in the scrolling part of the screen */
    static void show_msg(string msg)
    {
      acquire_bitmap(text_area);

      blit(text_area, text_area, 0, 8, 0, 0, text_area.w, text_area.h - 8);
      rectfill(text_area, 0, text_area.h - 8, text_area.w, text_area.h,
        palette_color[0]);

      if (msg != null)
        textout_centre_ex(text_area, font, msg, text_area.w / 2, text_area.h - 8,
          palette_color[255], palette_color[0]);

      release_bitmap(text_area);
    }



    /* displays the current switch mode setting */
    static void show_switch_mode()
    {
      switch (get_display_switch_mode())
      {

        case SWITCH_NONE:
          show_msg("Current mode is SWITCH_NONE");
          break;

        case SWITCH_PAUSE:
          show_msg("Current mode is SWITCH_PAUSE");
          break;

        case SWITCH_AMNESIA:
          show_msg("Current mode is SWITCH_AMNESIA");
          break;

        case SWITCH_BACKGROUND:
          show_msg("Current mode is SWITCH_BACKGROUND");
          break;

        case SWITCH_BACKAMNESIA:
          show_msg("Current mode is SWITCH_BACKAMNESIA");
          break;

        default:
          show_msg("Eeek! Unknown switch mode...");
          break;
      }
    }



    /* callback for switching back to our program */
    static void switch_in_callback()
    {
      in_callback++;
    }



    /* callback for switching away from our program */
    static void switch_out_callback()
    {
      out_callback++;
    }

    static TimerHandler t_in = new TimerHandler(switch_in_callback);
    static TimerHandler t_out = new TimerHandler(switch_out_callback);



    /* changes the display switch mode */
    static void set_sw_mode(int mode)
    {
      if (set_display_switch_mode(mode) != 0)
      {
        show_msg("Error changing switch mode");
        show_msg(null);
        return;
      }

      show_switch_mode();

      if (set_display_switch_callback(SWITCH_IN, switch_in_callback) == 0)
        show_msg("SWITCH_IN callback activated");
      else
        show_msg("SWITCH_IN callback not available");

      if (set_display_switch_callback(SWITCH_OUT, switch_out_callback) == 0)
        show_msg("SWITCH_OUT callback activated");
      else
        show_msg("SWITCH_OUT callback not available");

      show_msg(null);
    }

    static int x = 0, y = 0;

    /* draws some graphics, for no particular reason at all */
    static void draw_pointless_graphics()
    {
      
      float zr, zi, cr, ci, tr, ti;
      int c;

      if ((x == 0) && (y == 0))
        clear_to_color(graphics_area, palette_color[255]);

      cr = (float)(((float)x / (float)graphics_area.w - 0.75) * 2.0);
      ci = (float)(((float)y / (float)graphics_area.h - 0.5) * 1.8);

      zr = 0;
      zi = 0;

      for (c = 0; c < 100; c++)
      {
        tr = zr * zr - zi * zi;
        ti = zr * zi * 2;

        zr = tr + cr;
        zi = ti + ci;
        if ((zr < -10) || (zr > 10) || (zi < -10) || (zi > 10))
          break;
      }

      if ((zi != zi) || (zr != zr))
        c = 0;
      else if ((zi <= -1) || (zi >= 1) || (zr <= -1) || (zr >= 1))
        c = 255;
      else
      {
        c = (int)(Math.Sqrt(zi * zi + zr * zr) * 256);
        if (c > 255)
          c = 255;
      }

      putpixel(graphics_area, x, y, makecol(c, c, c));

      x++;
      if (x >= graphics_area.w)
      {
        x = 0;
        y++;
        if (y >= graphics_area.h)
          y = 0;
      }
    }


    static int Main()
    {
      PALETTE pal = new PALETTE();
      bool finished = false;
      int last_counter = 0;
      int c = GFX_AUTODETECT;
      int w, h, bpp, i;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_mouse();
      install_timer();

      if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Unable to set any graphic mode\n" + allegro_error);
        return 1;
      }
      set_palette(desktop_palette);

      w = SCREEN_W;
      h = SCREEN_H;
      bpp = bitmap_color_depth(screen);
      if (gfx_mode_select_ex(ref c, ref w, ref h, ref bpp) == 0)
      {
        allegro_exit();
        return 1;
      }

      set_color_depth(bpp);

      if (set_gfx_mode(c, w, h, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Error setting graphics mode\n" + allegro_error);
        return 1;
      }

      for (i = 0; i < 256; i++)
        pal[i].r = pal[i].g = pal[i].b = (byte)(i / 4);

      int c0 = makecol(pal[0].r, pal[0].g, pal[0].b);
      int c255 = makecol(pal[255].r, pal[255].g, pal[255].b);
      

      set_palette(pal);

      int c2 = palette_color[255];

      text_area = create_sub_bitmap(screen, 0, 0, SCREEN_W, SCREEN_H / 2);
      graphics_area = create_sub_bitmap(screen, 0, SCREEN_H / 2, SCREEN_W / 2,
                SCREEN_H / 2);
      if ((!text_area) || (!graphics_area))
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Out of memory!\n");
        return 1;
      }

      LOCK_VARIABLE(counter);
      LOCK_FUNCTION(t_counter);

      install_int(t_counter, 10);

      show_msg("Console switching test");
      show_msg("Press 1-5 to change mode");
      show_msg(null);

      show_switch_mode();
      show_msg(null);

      while (!finished)
      {
        if (counter != last_counter)
        {
          last_counter = counter;

          acquire_screen();
          textprintf_centre_ex(screen, font, SCREEN_W * 3 / 4, SCREEN_H * 3 / 4,
                   //palette_color[255], palette_color[0],
                   c255, c0,
                   "Time: " + last_counter);
          release_screen();

          acquire_bitmap(graphics_area);
          for (i = 0; i < 10; i++)
            draw_pointless_graphics();
          release_bitmap(graphics_area);
        }

        if (keypressed())
        {
          switch (readkey() & 255)
          {

            case '1':
              set_sw_mode(SWITCH_NONE);
              break;

            case '2':
              set_sw_mode(SWITCH_PAUSE);
              break;

            case '3':
              set_sw_mode(SWITCH_AMNESIA);
              break;

            case '4':
              set_sw_mode(SWITCH_BACKGROUND);
              break;

            case '5':
              set_sw_mode(SWITCH_BACKAMNESIA);
              break;

            case 27:
              finished = true;
              break;
          }
        }

        while (in_callback > 0)
        {
          in_callback--;
          show_msg("SWITCH_IN callback");
          show_msg(null);
        }

        while (out_callback > 0)
        {
          out_callback--;
          show_msg("SWITCH_OUT callback");
          show_msg(null);
        }
      }

      destroy_bitmap(text_area);
      destroy_bitmap(graphics_area);

      return 0;
    }
  }
}
