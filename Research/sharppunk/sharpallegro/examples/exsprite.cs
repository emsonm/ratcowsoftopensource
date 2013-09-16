using System;
using System.Text;

using sharpallegro;

namespace exsprite
{
  class exsprite : Allegro
  {
    const int FRAME_01 = 0;      /* BMP  */
    const int FRAME_02 = 1;  /* BMP  */
    const int FRAME_03 = 2;  /* BMP  */
    const int FRAME_04 = 3;  /* BMP  */
    const int FRAME_05 = 4;  /* BMP  */
    const int FRAME_06 = 5;   /* BMP  */
    const int FRAME_07 = 6;  /* BMP  */
    const int FRAME_08 = 7;   /* BMP  */
    const int FRAME_09 = 8;    /* BMP  */
    const int FRAME_10 = 9;     /* BMP  */
    const int PALETTE_001 = 10;      /* PAL  */
    const int SOUND_01 = 11;       /* SAMP */

    const int FRAMES_PER_SECOND = 30;

    /* set up a timer for animation */
    volatile static int ticks = 0;
    static void ticker()
    {
      ticks++;
    }

    static TimerHandler t_ticker = new TimerHandler(ticker);


    /* pointer to data file */
    static DATAFILE running_data;

    /* current sprite frame number */
    static int frame, frame_number = 0;

    /* pointer to a sprite buffer, where sprite will be drawn */
    static BITMAP sprite_buffer;

    /* a boolean - if true, skip to next part */
    static bool next;



    static void animate()
    {
      /* Wait for animation timer. */
      while (frame > ticks)
      {
        /* Avoid busy wait. */
        rest(1);
      }

      /* Ideally, instead of using a timer, we would set the monitor refresh rate
       * to a multiple of the animation speed, and synchronize with the vertical
       * blank interrupt (vsync) - to get a completely smooth animation. But this
       * doesn't work on all setups (e.g. in X11 windowed modes), so should only
       * be used after performing some tests first or letting the user enable it.
       * Too much for this simple example
       */

      frame++;

      /* blits sprite buffer to screen */
      blit(sprite_buffer, screen, 0, 0, (SCREEN_W - sprite_buffer.w) / 2,
     (SCREEN_H - sprite_buffer.h) / 2, sprite_buffer.w, sprite_buffer.h);

      /* clears sprite buffer with color 0 */
      clear_bitmap(sprite_buffer);

      /* if key pressed set a next flag */
      if (keypressed())
        next = true;
      else
        next = false;

      if (frame_number == 0)
        play_sample(running_data[SOUND_01].dat, 128, 128, 1000, FALSE);

      /* increase frame number, or if it's equal 9 (last frame) set it to 0 */
      if (frame_number == 9)
        frame_number = 0;
      else
        frame_number++;
    }


    static int Main(string[] argv)
    {
      byte[] datafile_name = new byte[256];
      int angle = 0;
      int x, y;
      int text_y;
      int color;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_sound(DIGI_AUTODETECT, MIDI_NONE, null);
      install_timer();
      LOCK_FUNCTION(t_ticker);
      LOCK_VARIABLE(ticks);
      install_int_ex(ticker, BPS_TO_TIMER(30));

      if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message("Unable to set any graphic mode\n" +
              allegro_error);
          return 1;
        }
      }

      /* loads datafile and sets user palette saved in datafile */
      replace_filename(datafile_name, "./", "running.dat",
           256);
      running_data = load_datafile(Encoding.ASCII.GetString((datafile_name)));
      if (!running_data)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Error loading " + datafile_name + "!\n");
        return 1;
      }

      /* select the palette which was loaded from the datafile */
      set_palette(running_data[PALETTE_001].dat);

      /* create and clear a bitmap for sprite buffering, big
       * enough to hold the diagonal(sqrt(2)) when rotating */
      sprite_buffer = create_bitmap((int)(82 * Math.Sqrt(2) + 2),
         (int)(82 * Math.Sqrt(2) + 2));
      clear_bitmap(sprite_buffer);

      x = (sprite_buffer.w - 82) / 2;
      y = (sprite_buffer.h - 82) / 2;
      color = makecol(0, 80, 0);
      text_y = SCREEN_H - 10 - text_height(font);

      frame = ticks;

      /* write current sprite drawing method */
      textout_centre_ex(screen, font, "Press a key for next part...",
           SCREEN_W / 2, 10, palette_color[1], -1);
      textout_centre_ex(screen, font, "Using draw_sprite",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        hline(sprite_buffer, 0, y + 82, sprite_buffer.w - 1, color);
        draw_sprite(sprite_buffer, running_data[frame_number].dat, x, y);
        animate();
      } while (!next);

      clear_keybuf();
      rectfill(screen, 0, text_y, SCREEN_W, SCREEN_H, 0);
      textout_centre_ex(screen, font, "Using draw_sprite_h_flip",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        hline(sprite_buffer, 0, y + 82, sprite_buffer.w - 1, color);
        draw_sprite_h_flip(sprite_buffer, running_data[frame_number].dat, x, y);
        animate();
      } while (!next);

      clear_keybuf();
      rectfill(screen, 0, text_y, SCREEN_W, SCREEN_H, 0);
      textout_centre_ex(screen, font, "Using draw_sprite_v_flip",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        hline(sprite_buffer, 0, y - 1, sprite_buffer.w - 1, color);
        draw_sprite_v_flip(sprite_buffer, running_data[frame_number].dat, x, y);
        animate();
      } while (!next);

      clear_keybuf();
      rectfill(screen, 0, text_y, SCREEN_W, SCREEN_H, 0);
      textout_centre_ex(screen, font, "Using draw_sprite_vh_flip",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        hline(sprite_buffer, 0, y - 1, sprite_buffer.w - 1, color);
        draw_sprite_vh_flip(sprite_buffer, running_data[frame_number].dat, x, y);
        animate();
      } while (!next);

      clear_keybuf();
      rectfill(screen, 0, text_y, SCREEN_W, SCREEN_H, 0);
      textout_centre_ex(screen, font, "Now with rotating - pivot_sprite",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        /* The last argument to pivot_sprite() is a fixed point type,
         * so I had to use itofix() routine (integer to fixed).
         */
        circle(sprite_buffer, x + 41, y + 41, 47, color);
        pivot_sprite(sprite_buffer, running_data[frame_number].dat, sprite_buffer.w / 2,
     sprite_buffer.h / 2, 41, 41, itofix(angle));
        animate();
        angle -= 4;
      } while (!next);

      clear_keybuf();
      rectfill(screen, 0, text_y, SCREEN_W, SCREEN_H, 0);
      textout_centre_ex(screen, font, "Now using pivot_sprite_v_flip",
            SCREEN_W / 2, text_y, palette_color[15], -1);

      do
      {
        /* The last argument to pivot_sprite_v_flip() is a fixed point type,
         * so I had to use itofix() routine (integer to fixed).
         */
        circle(sprite_buffer, x + 41, y + 41, 47, color);
        pivot_sprite_v_flip(sprite_buffer, running_data[frame_number].dat,
     sprite_buffer.w / 2, sprite_buffer.h / 2, 41, 41, itofix(angle));
        animate();
        angle += 4;
      } while (!next);

      unload_datafile(running_data);
      destroy_bitmap(sprite_buffer);
      return 0;
    }
  }
}
