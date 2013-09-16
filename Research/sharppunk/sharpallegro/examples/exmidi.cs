using System;
using System.Text;

using sharpallegro;

namespace exmidi
{
  class exmidi : Allegro
  {
    static int Main(string[] argv)
    {
      MIDI the_music;
      int length, pos;
      int beats, beat;
      int x, y, tw, th;
      int background_color;
      int text_color;
      bool paused = false;
      bool done = false;

      if (allegro_init() != 0)
        return 1;

      if (argv.Length != 1)
      {
        allegro_message("Usage: 'exmidi filename.mid'\n");
        return 1;
      }

      install_keyboard();
      install_timer();

      /* install a MIDI sound driver */
      if (install_sound(DIGI_AUTODETECT, MIDI_AUTODETECT, "") != 0)
      {
        allegro_message(string.Format("Error initialising sound system\n{0}\n", allegro_error));
        return 1;
      }

      /* read in the MIDI file */
      the_music = load_midi(argv[0]);
      if (!the_music)
      {
        allegro_message(string.Format("Error reading MIDI file '{0}'\n", argv[0]));
        return 1;
      }
      length = get_midi_length(the_music);
      beats = (int)-midi_pos; /* get_midi_length updates midi_pos to the negative
                         number of beats (quarter notes) in the midi. */

      if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
          return 1;
        }
      }

      /* try to continue in the background */
      if (set_display_switch_mode(SWITCH_BACKGROUND) > 0)
        set_display_switch_mode(SWITCH_BACKAMNESIA);

      set_palette(desktop_palette);
      background_color = makecol(255, 255, 255);
      text_color = makecol(0, 0, 0);
      clear_to_color(screen, background_color);
      th = text_height(font);
      x = SCREEN_W / 2;

      textprintf_centre_ex(screen, font, x, SCREEN_H / 3, text_color, -1,
         string.Format("Driver: {0}", midi_driver.name));
      textprintf_centre_ex(screen, font, x, SCREEN_H / 2, text_color, -1,
         string.Format("Playing {0}", get_filename(argv[0])));

      /* start up the MIDI file */
      play_midi(the_music, TRUE);

      y = 2 * SCREEN_H / 3;
      tw = text_length(font, "0000:00 / 0000:00");
      /* wait for a key press */
      while (!done)
      {
        /* P key pauses/resumes, any other key exits. */
        while (keypressed())
        {
          int k = readkey() & 255;
          if (k == 'p')
          {
            paused = !paused;
            if (paused)
            {
              midi_pause();
              textprintf_centre_ex(screen, font, x, y + th * 3,
           text_color, -1, "P A U S E D");
            }
            else
            {
              midi_resume();
              rectfill(screen, x - tw / 2, y + th * 3, x + tw / 2,
           y + th * 4, background_color);
            }
          }
          else
            done = true;
        }
        pos = (int)midi_time;
        beat = (int)midi_pos;
        rectfill(screen, x - tw / 2, y, x + tw / 2, y + th * 2, background_color);
        textprintf_centre_ex(screen, font, x, y, text_color, -1,
          string.Format("{0}:{1:00} / {2}:{3:00}", pos / 60, pos % 60, length / 60, length % 60));
        textprintf_centre_ex(screen, font, x, y + th, text_color, -1,
          string.Format("beat {0} / {1}", beat, beats));
        /* We have nothing else to do. */
        rest(100);
      }

      /* destroy the MIDI file */
      destroy_midi(the_music);

      return 0;
    }
  }
}
