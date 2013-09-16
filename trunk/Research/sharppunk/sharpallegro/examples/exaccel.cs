using System;
using System.Text;

using sharpallegro;
using System.Runtime.InteropServices;

namespace exaccel
{
  class exaccel : Allegro
  {
    const int MAX_IMAGES = 256;

    /* structure to hold the current position and velocity of an image */
    public struct IMAGE
    {
      public float x, y;
      public float dx, dy;
    }

    /* initialises an image structure to a random position and velocity */
    public static void init_image(ref IMAGE image)
    {
      image.x = (float)(AL_RAND() % 704);
      image.y = (float)(AL_RAND() % 568);
      image.dx = (float)(((AL_RAND() % 255) - 127) / 32.0);
      image.dy = (float)(((AL_RAND() % 255) - 127) / 32.0);
    }



    /* called once per frame to bounce an image around the screen */
    public static void update_image(ref IMAGE image)
    {
      image.x += image.dx;
      image.y += image.dy;

      if (((image.x < 0) && (image.dx < 0)) ||
          ((image.x > 703) && (image.dx > 0)))
        image.dx *= -1;

      if (((image.y < 0) && (image.dy < 0)) ||
          ((image.y > 567) && (image.dy > 0)))
        image.dy *= -1;
    }



    public static int Main(string[] argv)
    {
      byte[] buf = new byte[256];
      PALETTE pal = new PALETTE();
      BITMAP image;
      BITMAP[] page = new BITMAP[2];
      BITMAP vimage;
      IMAGE[] images = new IMAGE[MAX_IMAGES];
      int num_images = 4;
      int page_num = 1;
      bool done = false;
      int i;

      if (allegro_init() != 0)
        return 1;
      install_keyboard();
      install_timer();

      /* see comments in exflip.c */
#if ALLEGRO_VRAM_SINGLE_SURFACE
      if (set_gfx_mode(GFX_AUTODETECT, 1024, 768, 0, 2 * 768 + 200) != 0) {
#else
      if (set_gfx_mode(GFX_AUTODETECT, 1024, 768, 0, 0) != 0)
      {
#endif
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
        return 1;
      }

      /* read in the source graphic */
      replace_filename(buf, "./", "mysha.pcx", buf.Length);
      image = load_bitmap(Encoding.ASCII.GetString(buf), pal);
      if (!image)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error reading {0}!\n", buf));
        return 1;
      }

      set_palette(pal);

      /* initialise the images to random positions */
      for (i = 0; i < MAX_IMAGES; i++)
        init_image(ref images[i]);

      /* create two video memory bitmaps for page flipping */
      page[0] = create_video_bitmap(SCREEN_W, SCREEN_H);
      page[1] = create_video_bitmap(SCREEN_W, SCREEN_H);

      /* create a video memory bitmap to store our picture */
      vimage = create_video_bitmap(image.w, image.h);

      if ((!page[0]) || (!page[1]) || (!vimage))
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message("Not enough video memory (need two 1024x768 pages " +
            "and a 320x200 image)\n");
        return 1;
      }

      /* copy the picture into offscreen video memory */
      blit(image, vimage, 0, 0, 0, 0, image.w, image.h);

      while (!done)
      {
        acquire_bitmap(page[page_num]);

        /* clear the screen */
        clear_bitmap(page[page_num]);

        /* draw onto it */
        for (i = 0; i < num_images; i++)
          blit(vimage, page[page_num], 0, 0, (int)images[i].x, (int)images[i].y,
               vimage.w, vimage.h);

        textprintf_ex(page[page_num], font, 0, 0, 255, -1,
          string.Format("Images: {0} (arrow keys to change)", num_images));

        /* tell the user which functions are being done in hardware */
        if ((gfx_capabilities & GFX_HW_FILL) > 0)
          textout_ex(page[page_num], font, "Clear: hardware accelerated",
               0, 16, 255, -1);
        else
          textout_ex(page[page_num], font, "Clear: software (urgh, this " +
               "is not good!)", 0, 16, 255, -1);

        if ((gfx_capabilities & GFX_HW_VRAM_BLIT) > 0)
          textout_ex(page[page_num], font, "Blit: hardware accelerated",
               0, 32, 255, -1);
        else
          textout_ex(page[page_num], font, "Blit: software (urgh, this program " +
               "will run too sloooooowly without hardware acceleration!)",
               0, 32, 255, -1);

        release_bitmap(page[page_num]);

        /* page flip */
        show_video_bitmap(page[page_num]);
        page_num = 1 - page_num;

        /* deal with keyboard input */
        while (keypressed())
        {
          switch (readkey() >> 8)
          {

            case KEY_UP:
            case KEY_RIGHT:
              if (num_images < MAX_IMAGES)
                num_images++;
              break;

            case KEY_DOWN:
            case KEY_LEFT:
              if (num_images > 0)
                num_images--;
              break;

            case KEY_ESC:
              done = true;
              break;
          }
        }

        /* bounce the images around the screen */
        for (i = 0; i < num_images; i++)
          update_image(ref images[i]);
      }

      destroy_bitmap(image);
      destroy_bitmap(vimage);
      destroy_bitmap(page[0]);
      destroy_bitmap(page[1]);

      return 0;
    }
  }
}
