using System;
using System.Text;

using sharpallegro;

namespace exstream
{
  class exstream : Allegro
  {
    const int BUFFER_SIZE = 1024;


    static unsafe int Main()
    {
      AUDIOSTREAM stream;
      byte* p;
      int updates = 0;
      int pitch = 0;
      int val = 0;
      int i, c;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();

      if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
              allegro_error));
          return 1;
        }
      }

      set_palette(desktop_palette);
      clear_to_color(screen, makecol(255, 255, 255));

      /* install a digital sound driver */
      if (install_sound(DIGI_AUTODETECT, MIDI_NONE, "") != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error initialising sound driver\n{0}\n",
            allegro_error));
        return 1;
      }

      /* we want a _real_ sound driver */
      if (digi_card == DIGI_NONE)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Unable to find a sound driver\n{0}\n",
            allegro_error));
        return 1;
      }

      /* create an audio stream */
      stream = play_audio_stream(BUFFER_SIZE, 8, FALSE, 22050, 255, 128);
      if (!stream)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Error creating audio stream!\n");
        return 1;
      }

      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3 - 24,
         makecol(0, 0, 0), makecol(255, 255, 255),
         "Audio stream is now playing...");
      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3,
         makecol(0, 0, 0), makecol(255, 255, 255),
         string.Format("Driver: {0}", digi_driver.name));
      textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 3 + 24,
         makecol(0, 0, 0), makecol(255, 255, 255),
         "Press [space] to stop/resume");

      while (true)
      {
        if (keypressed())
        {
          c = readkey();
          if ((c >> 8) == KEY_SPACE)
          {
            voice_stop(stream.voice);
            while (true)
            {
              c = readkey();
              if ((c >> 8) == KEY_ESC)
                goto End;
              else if ((c >> 8) == KEY_SPACE)
                break;
            }
            voice_start(stream.voice);
          }
          else if ((c >> 8) == KEY_ESC)
            break;
        }

        /* does the stream need any more data yet? */
        p = get_audio_stream_buffer(stream);

        if (p != null)
        {
          /* if so, generate a bit more of our waveform... */
          textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H * 2 / 3,
                   makecol(0, 0, 0), makecol(255, 255, 255),
                   string.Format("update #{0}", updates++));

          for (i = 0; i < BUFFER_SIZE; i++)
          {
            /* this is just a sawtooth wave that gradually increases in 
             * pitch. Obviously you would want to do something a bit more
             * interesting here, for example you could fread() the next
             * buffer of data in from a disk file...
             */
            p[i] = (byte)((val >> 16) & 0xFF);
            val += pitch;
            pitch++;
            if (pitch > 0x40000)
              pitch = 0x10000;
          }

          free_audio_stream_buffer(stream);
        }
      }

    End:
      stop_audio_stream(stream);

      return 0;
    }
  }
}
