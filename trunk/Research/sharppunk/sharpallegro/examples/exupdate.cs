using System;
using System.Text;

using sharpallegro;

namespace exupdate
{
  class exupdate : Allegro
  {
    /* number of video memory pages:
     * 1 = double buffered
     * 2 = page flipping
     * 3 = triple buffered
     */
    static int num_pages;

    static bool use_system_bitmaps;



    /* counters for speed control and frame rate measurement */
    volatile static int update_count = 0;
    volatile static int frame_count = 0;
    volatile static int fps = 0;



    /* timer callback for controlling the program speed */
    static void timer_proc()
    {
      update_count++;
    }

    static TimerHandler t_timer = new TimerHandler(timer_proc);



    /* timer callback for measuring the frame rate */
    static void fps_proc()
    {
      fps = frame_count;
      frame_count = 0;
    }

    static TimerHandler t_fps = new TimerHandler(fps_proc);



    /* some rotation values for the graphical effect */
    static int r1 = 0;
    static int r2 = 0;
    static int r3 = 0;
    static int r4 = 0;



    /* helper to draw four mirrored versions of a triangle */
    static void kalid(BITMAP bmp, int x1, int y1, int x2, int y2, int x3, int y3,
         int r, int g, int b)
    {
      triangle(bmp, SCREEN_W / 2 + x1, SCREEN_H / 2 + y1, SCREEN_W / 2 + x2, SCREEN_H / 2 + y2,
         SCREEN_W / 2 + x3, SCREEN_H / 2 + y3, makecol(r, g, b));
      triangle(bmp, SCREEN_W / 2 - x1, SCREEN_H / 2 + y1, SCREEN_W / 2 - x2, SCREEN_H / 2 + y2,
         SCREEN_W / 2 - x3, SCREEN_H / 2 + y3, makecol(r, g, b));
      triangle(bmp, SCREEN_W / 2 - x1, SCREEN_H / 2 - y1, SCREEN_W / 2 - x2, SCREEN_H / 2 - y2,
         SCREEN_W / 2 - x3, SCREEN_H / 2 - y3, makecol(r, g, b));
      triangle(bmp, SCREEN_W / 2 + x1, SCREEN_H / 2 - y1, SCREEN_W / 2 + x2, SCREEN_H / 2 - y2,
         SCREEN_W / 2 + x3, SCREEN_H / 2 - y3, makecol(r, g, b));
    }



    /* draws the current animation frame into the specified bitmap */
    static void draw_screen(BITMAP bmp)
    {
      int c1 = fixcos(r1);
      int c2 = fixcos(r2);
      int c3 = fixcos(r3);
      int c4 = fixcos(r4);
      int s1 = fixsin(r1);
      int s2 = fixsin(r2);
      int s3 = fixsin(r3);
      int s4 = fixsin(r4);

      acquire_bitmap(bmp);

      clear_bitmap(bmp);

      xor_mode(TRUE);

      kalid(bmp, fixtoi(c1 * SCREEN_W / 3), fixtoi(s1 * SCREEN_H / 3),
           fixtoi(c2 * SCREEN_W / 3), fixtoi(s2 * SCREEN_H / 3),
           fixtoi(c3 * SCREEN_W / 3), fixtoi(s3 * SCREEN_H / 3),
           127 + fixtoi(c1 * 127), 127 + fixtoi(c2 * 127), 127 + fixtoi(c3 * 127));

      kalid(bmp, fixtoi(s1 * SCREEN_W / 3), fixtoi(c2 * SCREEN_H / 3),
           fixtoi(s3 * SCREEN_W / 3), fixtoi(c4 * SCREEN_H / 3),
           fixtoi(c3 * SCREEN_W / 3), fixtoi(s4 * SCREEN_H / 3),
           127 + fixtoi(s1 * 127), 127 + fixtoi(c4 * 127), 127 + fixtoi(s4 * 127));

      kalid(bmp, fixtoi(fixmul(s2, c3) * SCREEN_W / 3), fixtoi(c1 * SCREEN_H / 3),
           fixtoi(c4 * SCREEN_W / 3), fixtoi(fixmul(c2, s3) * SCREEN_H / 3),
           fixtoi(fixmul(c3, s4) * SCREEN_W / 3), fixtoi(s1 * SCREEN_H / 3),
           127 + fixtoi(s2 * 127), 127 + fixtoi(c3 * 127), 127 + fixtoi(s3 * 127));

      xor_mode(FALSE);

      if (num_pages == 1)
      {
        if (use_system_bitmaps)
          textout_ex(bmp, font, "Double buffered (system bitmap)", 0, 0,
               makecol(255, 255, 255), -1);
        else
          textout_ex(bmp, font, "Double buffered (memory bitmap)", 0, 0,
               makecol(255, 255, 255), -1);
      }
      else if (num_pages == 2)
        textout_ex(bmp, font, "Page flipping (two pages of vram)", 0, 0,
       makecol(255, 255, 255), -1);
      else
        textout_ex(bmp, font, "Triple buffered (three pages of vram)", 0, 0,
       makecol(255, 255, 255), -1);

      textout_ex(bmp, font, gfx_driver.name, 0, 16, makecol(255, 255, 255), -1);

      textprintf_ex(bmp, font, 0, 32, makecol(255, 255, 255), -1, string.Format("FPS: {0}", fps));

      release_bitmap(bmp);
    }
    /* called at a regular speed to update the program state */
    static void do_update()
    {
      r1 += ftofix(0.5);
      r2 += ftofix(0.6);
      r3 += ftofix(0.11);
      r4 += ftofix(0.13);
    }

    static int Main(string[] argv)
    {
      PALETTE pal = new PALETTE();
      BITMAP[] bmp = new BITMAP[3];
      int card = GFX_AUTODETECT;
      int w, h, bpp, page, i;

      if (allegro_init() != 0)
        return 1;

      /* check command line arguments */
      if (argv.Length == 1)
        num_pages = int.Parse(argv[0]);
      else
        num_pages = 0;

      if (num_pages == 4)
      {
        num_pages = 1;
        use_system_bitmaps = true;
      }
      else
        use_system_bitmaps = false;

      if ((num_pages < 1) || (num_pages > 3))
      {
        allegro_message("\nUsage: 'exupdate num_pages', where num_pages is one of:\n\n" +
            "1 = double buffered (memory bitmap)\n\n" +
            "    + easy, reliable\n" +
            "    + drawing onto a memory bitmap is very fast\n" +
            "    - blitting the finished image to the screen can be quite slow\n\n" +
            "2 = page flipping (two pages of video memory)\n\n" +
            "    + avoids the need for a memory to screen blit of the completed image\n" +
            "    + can use hardware acceleration when it is available\n" +
            "    - drawing directly to vram can be slower than using a memory bitmap\n" +
            "    - requires a card with enough video memory for two screen pages\n\n" +
            "3 = triple buffered (three pages of video memory)\n\n" +
            "    + like page flipping, but faster and smoother\n" +
            "    - requires a card with enough video memory for three screen pages\n" +
            "    - only possible with some graphics drivers\n\n" +
            "4 = double buffered (system bitmap)\n\n" +
            "    + as easy as normal double buffering\n" +
            "    + system bitmaps can be hardware accelerated on some platforms\n" +
            "    - drawing to system bitmaps is sometimes slower than memory bitmaps\n\n");
        return 1;
      }

      /* set up Allegro */
      install_keyboard();
      install_timer();
      install_mouse();
      if (set_gfx_mode(GFX_SAFE, 320, 200, 0, 0) != 0)
      {
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
        return 1;
      }
      set_palette(desktop_palette);

      w = SCREEN_W;
      h = SCREEN_H;
      bpp = bitmap_color_depth(screen);
      if (gfx_mode_select_ex(ref card, ref w, ref h, ref bpp) == 0)
        return 0;

      set_color_depth(bpp);

#if ALLEGRO_VRAM_SINGLE_SURFACE
   if (set_gfx_mode(card, w, h, w, num_pages*h) != 0) {
#else
      if (set_gfx_mode(card, w, h, 0, 0) != 0)
      {
#endif
        set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
        allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
        return 1;
      }

      generate_332_palette(pal);
      pal[0].r = pal[0].g = pal[0].b = 0;
      set_palette(pal);

      switch (num_pages)
      {

        case 1:
          /* double buffering setup */
          if (use_system_bitmaps)
            bmp[0] = create_system_bitmap(SCREEN_W, SCREEN_H);
          else
            bmp[0] = create_bitmap(SCREEN_W, SCREEN_H);
          break;

        case 2:
          /* page flipping setup */
          bmp[0] = create_video_bitmap(SCREEN_W, SCREEN_H);
          bmp[1] = create_video_bitmap(SCREEN_W, SCREEN_H);
          break;

        case 3:
          /* triple buffering setup */
          if ((gfx_capabilities & GFX_CAN_TRIPLE_BUFFER) == 0)
            enable_triple_buffer();

          if ((gfx_capabilities & GFX_CAN_TRIPLE_BUFFER) == 0)
          {
            set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);

#if ALLEGRO_DOS
	    allegro_message("This driver does not support triple buffering\n"
			    "\nTry using mode-X in clean DOS mode (not under "
			    "Windows)\n");
#else
            allegro_message("This driver does not support triple buffering\n");
#endif

            return 1;
          }

          bmp[0] = create_video_bitmap(SCREEN_W, SCREEN_H);
          bmp[1] = create_video_bitmap(SCREEN_W, SCREEN_H);
          bmp[2] = create_video_bitmap(SCREEN_W, SCREEN_H);
          break;
      }

      for (i = 0; i < num_pages; i++)
      {
        if (!bmp[i])
        {
          set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
          allegro_message(string.Format("Unable to create {0} video memory pages\n",
              num_pages));
          return 1;
        }
      }

      /* install timer handlers to control and measure the program speed */
      LOCK_VARIABLE(update_count);
      LOCK_VARIABLE(frame_count);
      LOCK_VARIABLE(fps);
      LOCK_FUNCTION(t_timer);
      LOCK_FUNCTION(t_fps);

      install_int_ex(timer_proc, BPS_TO_TIMER(60));
      install_int_ex(fps_proc, BPS_TO_TIMER(1));

      page = 1;

      /* main program loop */
      while (!keypressed())
      {

        /* draw the next frame of graphics */
        switch (num_pages)
        {

          case 1:
            /* draw onto a memory bitmap, then blit to the screen */
            draw_screen(bmp[0]);
            vsync();
            blit(bmp[0], screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
            break;

          case 2:
            /* flip back and forth between two pages of video memory */
            draw_screen(bmp[page]);
            show_video_bitmap(bmp[page]);
            page = 1 - page;
            break;

          case 3:
            /* triple buffering */
            draw_screen(bmp[page]);

            do
            {
            } while (poll_scroll());

            request_video_bitmap(bmp[page]);
            page = (page + 1) % 3;
            break;
        }

        /* update the program state */
        while (update_count > 0)
        {
          do_update();
          update_count--;
        }

        frame_count++;
      }

      for (i = 0; i < num_pages; i++)
        destroy_bitmap(bmp[i]);

      return 0;
    }
  }
}
