using System;
using System.Text;

using sharpallegro;

namespace exflame
{
  class exflame : Allegro
  {
    /* The fire is formed from several 'hotspots' which are moved randomly
     * across the bottom of the screen.
     */
    const int FIRE_HOTSPOTS = 48;

    static int[] hotspot = new int[FIRE_HOTSPOTS];

    static byte[] temp;



    /* This function updates the bottom line of the screen with a pattern
     * of varying intensities which are then moved upwards and faded out
     * by the code in main().
     */
    static void draw_bottom_line_of_fire()
    {
      int c, c2;

      /* zero the buffer */
      for (c = 0; c < SCREEN_W; c++)
        temp[c] = 0;

      for (c = 0; c < FIRE_HOTSPOTS; c++)
      {
        /* display the hotspots */
        for (c2 = hotspot[c] - 20; c2 < hotspot[c] + 20; c2++)
          if ((c2 >= 0) && (c2 < SCREEN_W))
            temp[c2] = (byte)MIN(temp[c2] + 20 - ABS(hotspot[c] - c2), 192);

        /* move the hotspots */
        hotspot[c] += (AL_RAND() & 7) - 3;
        if (hotspot[c] < 0)
          hotspot[c] += SCREEN_W;
        else
          if (hotspot[c] >= SCREEN_W)
            hotspot[c] -= SCREEN_W;
      }

      /* display the buffer */
      for (c = 0; c < SCREEN_W; c++)
        putpixel(screen, c, SCREEN_H - 1, temp[c]);
    }



    unsafe static int Main()
    {
      PALETTE palette = new PALETTE();
      uint address;
      int x, y, c;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      if (set_gfx_mode(GFX_AUTODETECT, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
        {
          allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
          return 1;
        }
      }

      temp = new byte[SCREEN_W];

      if (temp == null)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Not enough memory? This is a joke right!?!\n");
        return 0;
      }

      for (c = 0; c < FIRE_HOTSPOTS; c++)
        hotspot[c] = AL_RAND() % SCREEN_W;

      /* fill our palette with a gradually altering sequence of colors */
      for (c = 0; c < 64; c++)
      {
        palette[c].r = (byte)c;
        palette[c].g = 0;
        palette[c].b = 0;
      }
      for (c = 64; c < 128; c++)
      {
        palette[c].r = 63;
        palette[c].g = (byte)(c - 64);
        palette[c].b = 0;
      }
      for (c = 128; c < 192; c++)
      {
        palette[c].r = 63;
        palette[c].g = 63;
        palette[c].b = (byte)(c - 128);
      }
      for (c = 192; c < 256; c++)
      {
        palette[c].r = 63;
        palette[c].g = 63;
        palette[c].b = 63;
      }

      set_palette(palette);

      textout_ex(screen, font, "Using get/putpixel()", 0, 0, makecol(255, 255, 255), makecol(0, 0, 0));

      /* using getpixel() and putpixel() is slow :-) */
      while (!keypressed())
      {
        acquire_screen();

        draw_bottom_line_of_fire();

        for (y = 64; y < SCREEN_H - 1; y++)
        {
          /* read line */
          for (x = 0; x < SCREEN_W; x++)
          {
            c = getpixel(screen, x, y + 1);

            if (c > 0)
              c--;

            putpixel(screen, x, y, c);
          }
        }
        release_screen();
      }

      clear_keybuf();
      textout_ex(screen, font, "Using direct memory writes", 0, 0, makecol(255, 255, 255), makecol(0, 0, 0));

      /* It is much faster if we access the screen memory directly. This
       * time we read an entire line of the screen into our own buffer,
       * modify it there, and then write the whole line back in one go.
       * That is to avoid having to keep switching back and forth between
       * different scanlines: if we only copied one pixel at a time, we
       * would have to call bmp_write_line() for every single pixel rather
       * than just twice per line.
       */
      while (!keypressed())
      {
        acquire_screen();
        draw_bottom_line_of_fire();

        bmp_select(screen);

        for (y = 64; y < SCREEN_H - 1; y++)
        {
          /* get an address for reading line y+1 */
          address = (uint)bmp_read_line(screen, y + 1);

          /* read line with farptr functions */
          for (x = 0; x < SCREEN_W; x++)
            temp[x] = bmp_read8((int)(address + x));

          /* adjust it */
          for (x = 0; x < SCREEN_W; x++)
            if (temp[x] > 0)
              temp[x]--;

          /* get an address for writing line y */
          address = bmp_write_line(screen, y);

          /* write line with farptr functions */
          for (x = 0; x < SCREEN_W; x++)
            bmp_write8((int)(address + x), temp[x]);
        }

        bmp_unwrite_line(screen);
        release_screen();
      }

      clear_keybuf();
      textout_ex(screen, font, "Using block data transfers", 0, 0, makecol(255, 255, 255), makecol(0, 0, 0));

      /* It is even faster if we transfer the data in 32 bit chunks, rather
       * than only one pixel at a time. This method may not work on really
       * unusual machine architectures, but should be ok on just about
       * anything that you are practically likely to come across.
       */
      while (!keypressed())
      {
        acquire_screen();
        draw_bottom_line_of_fire();

        bmp_select(screen);

        for (y = 64; y < SCREEN_H - 1; y++)
        {
          /* get an address for reading line y+1 */
          address = (uint)bmp_read_line(screen, y + 1);

          /* read line in 32 bit chunks */
          for (x = 0; x < SCREEN_W; x += sizeof(uint))
            //*((uint*)&temp[x]) = (uint)bmp_read32((int)(address + x));
            fixed (byte* addr = temp)
            {
              *((uint*)(addr + x)) = (uint)bmp_read32((int)(address + x));
            }

          /* adjust it */
          for (x = 0; x < SCREEN_W; x++)
            if (temp[x] > 0)
              temp[x]--;

          /* get an address for writing line y */
          address = bmp_write_line(screen, y);

          /* write line in 32 bit chunks */
          for (x = 0; x < SCREEN_W; x += sizeof(uint))
            //bmp_write32((int)(address + x), *((uint*)&temp[x]));
            fixed(byte* addr = temp)
            {
              bmp_write32((uint)(address + x), (int)*((uint*)(addr + x)));
            }
        }

        bmp_unwrite_line(screen);
        release_screen();
      }

      //free(temp);

      return 0;
    }
  }
}
