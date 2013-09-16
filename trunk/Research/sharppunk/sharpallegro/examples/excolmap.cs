using System;

using sharpallegro;
using System.Runtime.InteropServices;

namespace exzcolmap
{
  class excolmap : Allegro
  {
    /* RGB -> color mapping table. Not needed, but speeds things up */
    static RGB_MAP rgb_table = new RGB_MAP();

    /* greyscale & negative color mapping table */
    static COLOR_MAP greyscale_table = new COLOR_MAP(), negative_table = new COLOR_MAP();

    static PALETTE pal = new PALETTE();
    static BITMAP background;
    static BITMAP temp;



    /* Here comes our custom function. It's designed to take the input colors
     * (red, green & blue) and return a greyscale color for it. This way, when
     * any drawing function draws say over green, it draws the greyscale color
     * for green.
     * 'pal' is the palette we are looking in to find the colors.
     * Now, imagine we want to draw a pixel with color A, over color B.
     * Once the table is created, set, and the drawing mode is TRANSLUCENT, then
     * A is the 'x' color passed to the function and B is the 'y' color passed
     * to the function.
     * Since we want a greyscale effect with no matter what A (or 'x') color, we
     * ignore it and use y to look at the palette.
     * NOTE:
     * When you return the rgb value, you don't need to search the palette for
     * the nearest color, Allegro does this automatically.
     */

    static void return_grey_color(IntPtr ppal, int x, int y, IntPtr prgb)
    {
      int c;

      // TODO: are the following 2 lines necessary?
      PALETTE pal = ppal;
      RGB rgb = prgb;

      /* first create the greyscale color */
      c = (int)(pal[y].r * 0.3 + pal[y].g * 0.5 + pal[y].b * 0.2);

      /* now assign to our rgb triplet the palette greyscale color... */
      rgb.r = rgb.g = rgb.b = (byte)c;
    }

    static BlendCallback b_grey = new BlendCallback(return_grey_color);



    /* The negative_color function is quite the same like the grayscale one,
     * since we are ignoring the value of the drawn color (aka x).
     */

    static void return_negative_color(IntPtr ppal, int x, int y, IntPtr prgb)
    {
      // TODO: are the following 2 lines necessary?
      PALETTE pal = ppal;
      RGB rgb = prgb;
      /* To get the negative color, subtract the color values of red, green
       * and blue from the full (63) color value.
       */
      rgb.r = (byte)(63 - pal[y].r);
      rgb.g = (byte)(63 - pal[y].g);
      rgb.b = (byte)(63 - pal[y].b);
    }

    static BlendCallback b_negative = new BlendCallback(return_negative_color);



    static void generate_background()
    {
      int i;

      /* First get some usual colors. */
      generate_332_palette(pal);

      /* Now remap the first 64 for a perfect greyscale gradient. */
      for (i = 0; i < 64; i++)
      {
        pal[i].r = (byte)i;
        pal[i].g = (byte)i;
        pal[i].b = (byte)i;
      }

      /* Draws some things on the screen using not-greyscale colors. */
      for (i = 0; i < 3000; i++)
        circlefill(background, AL_RAND() % 320, AL_RAND() % 200,
       AL_RAND() % 25, 64 + AL_RAND() % 192);
    }



    static int Main()
    {
      int x, y, deltax = 1, deltay = 1;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();

      temp = create_bitmap(320, 200);
      background = create_bitmap(320, 200);

      generate_background();

      /* This isn't needed, but it speeds up the color table calculations. */
      create_rgb_table(rgb_table, pal, NULL);
      rgb_map = rgb_table;

      /* Build a color lookup table for greyscale effect. */
      create_color_table(greyscale_table, pal, Marshal.GetFunctionPointerForDelegate(b_grey), NULL);

      /* Build a color lookup table for negative effect. */
      create_color_table(negative_table, pal, Marshal.GetFunctionPointerForDelegate(b_negative), NULL);

      if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 320, 200, 0, 0) != 0)
      {
        if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to set any graphic mode\n{0}\n",
              allegro_error));
          return 1;
        }
      }

      set_palette(pal);

      /* We have set the drawing mode to TRANS. This makes all the drawing
       * functions use the general color_map table, which is _NOT_ translucent,
       * since we are using a custom color_map table.
       */
      drawing_mode(DRAW_MODE_TRANS, NULL, 0, 0);

      /* select the greyscale table */
      color_map = greyscale_table;

      x = y = 50;
      blit(background, temp, 0, 0, 0, 0, 320, 200);
      rectfill(temp, x, y, x + 50, y + 50, 0);

      blit(temp, screen, 0, 0, 0, 0, 320, 200);

      while (!keypressed())
      {
        x += deltax;
        y += deltay;

        if ((x < 1) || (x > 320 - 50))
          deltax *= -1;

        if ((y < 1) || (y > 200 - 50))
          deltay *= -1;

        blit(background, temp, 0, 0, 0, 0, 320, 200);
        textout_centre_ex(temp, font, "Greyscale effect",
        SCREEN_W / 2, SCREEN_H / 2, makecol(0, 0, 255), -1);
        rectfill(temp, x, y, x + 50, y + 50, 0);
        vsync();
        blit(temp, screen, 0, 0, 0, 0, 320, 200);
      }

      clear_keybuf();

      /* Now it's time for the negative part. The negative example is easier to
       * see with greyscale colors. Therefore we will change the color of the
       * background to a greyscale one, but only in a restricted area...
       */

      rectfill(background, SCREEN_H / 4, SCREEN_H / 4,
         background.w - SCREEN_H / 4, background.h - SCREEN_H / 4, 0);

      /* this should go inside the next loop, but since we won't use the
       * background image any more, we can optimize it's speed printing the
       * text now.
       */
      textout_centre_ex(background, font, "Negative effect",
            SCREEN_W / 2, SCREEN_H / 2, makecol(0, 0, 0), -1);

      /* switch the active color table... */
      color_map = negative_table;

      blit(background, temp, 0, 0, 0, 0, 320, 200);
      rectfill(temp, x, y, x + 50, y + 50, 0);

      blit(temp, screen, 0, 0, 0, 0, 320, 200);

      while (!keypressed())
      {
        x += deltax;
        y += deltay;

        if ((x < 1) || (x > 320 - 50))
          deltax *= -1;

        if ((y < 1) || (y > 200 - 50))
          deltay *= -1;

        blit(background, temp, 0, 0, 0, 0, 320, 200);
        rectfill(temp, x, y, x + 50, y + 50, 0);
        vsync();
        blit(temp, screen, 0, 0, 0, 0, 320, 200);
      }

      destroy_bitmap(background);
      destroy_bitmap(temp);

      return 0;
    }

  }
}
