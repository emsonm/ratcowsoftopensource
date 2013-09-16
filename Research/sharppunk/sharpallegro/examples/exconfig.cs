using sharpallegro;

namespace exconfig
{
    class exconfig : Allegro
    {
        static int Main()
        {
            int w, h, bpp;
            int windowed;
            int count = -1;
            string[] data;

            string title;
            string filename;
            int r, g, b;

            BITMAP background;
            int display;
            //RGB[] pal = new RGB[256];
            PALETTE pal = new PALETTE();

            int x, y;

            /* you should always do this at the start of Allegro programs */
            if (allegro_init() != 0)
                return 1;
            /* set up the keyboard handler */
            install_keyboard();

            /* save the current ini file, then set the program specific one */
            push_config_state();
            set_config_file("exconfig.ini");

            /* the gfx mode is stored like this:
             *    640  480 16
             * the get_config_argv() function returns a pointer to a char
             * array, and stores the size of the char array in an int
             */
            data = get_config_argv("graphics", "mode", ref count);
            if (count != 3)
            {
                /* We expect only 3 parameters */
                allegro_message(string.Format("Found {0} parameters in graphics.mode instead of " +
                        "the 3 expected.\n", count));
                w = 320;
                h = 200;
                bpp = 8;
            }
            else
            {
                //w = atoi(data[0]);
                w = int.Parse(data[0]);
                //h = atoi(data[1]);
                h = int.Parse(data[1]);
                //bpp = atoi(data[2]);       
                bpp = int.Parse(data[2]);
            }

            /* Should we use a windowed mode? 
             * In the config file this is stored as either FALSE or TRUE.
             * So we need to read a string and see what it contains.
             * If the entry is not found, we use "FALSE" by default
             */
            //if (ustricmp(get_config_string("graphics", "windowed", "FALSE"), "FALSE") == 0)
            if (get_config_string("graphics", "windowed", "FALSE").ToUpper() == "FALSE")
                windowed = GFX_AUTODETECT_FULLSCREEN;
            else
                windowed = GFX_AUTODETECT_WINDOWED;

            /* the title string 
             * The string returned is stored inside of the config system
             * and would be lost if we call pop_config_state(), so we create
             * a copy of it.
             */
            //title = ustrdup(get_config_string("content", "headline", "<no headline>"));
            title = get_config_string("content", "headline", "<no headline>");

            /* the title color 
             * once again this is stored as three ints in one line
             */
            data = get_config_argv("content", "headercolor", ref count);
            if (count != 3)
            {
                /* We expect only 3 parameters */
                allegro_message(string.Format("Found {0} parameters in content.headercolor " +
                        "instead of the 3 expected.\n", count));
                r = g = b = 255;
            }
            else
            {
                //r = atoi(data[0]);
                r = int.Parse(data[0]);
                //g = atoi(data[1]);
                g = int.Parse(data[1]);
                //b = atoi(data[2]);
                b = int.Parse(data[2]);
            }

            /* The image file to read 
             * The string returned is stored inside of the config system
             * and would be lost if we call pop_config_state(), so we create
             * a copy of it.
             */
            //filename = ustrdup(get_config_string("content", "image", "mysha.pcx"));
            filename = get_config_string("content", "image", "mysha.pcx");

            /* and it's tiling mode */
            display = get_config_int("content", "display", 0);
            if (display < 0 || display > 2)
            {
                allegro_message("content.display must be within 0..2\n");
                display = 0;
            }

            /* restore the old config file */
            pop_config_state();


            /* set the graphics mode */
            set_color_depth(bpp);
            if (set_gfx_mode(windowed, w, h, 0, 0) != 0)
            {
                allegro_message(string.Format("Unable to set mode {0}x{1} with {2}bpp\n", w, h, bpp));
                //free(filename);
                //free(title);
                //exit(-1);           
                return -1;
            }

            /* Clear the screen */
            clear_bitmap(screen);

            /* load the image */
            background = load_bitmap(filename, pal);
            if (background != NULL)
            {
                set_palette(pal);

                switch (display)
                {

                    case 0: /* stretch */
                        stretch_blit(background, screen, 0, 0, background.w,
                             background.h, 0, 0, SCREEN_W, SCREEN_H);
                        break;

                    case 1: /* center */
                        blit(background, screen, 0, 0, (SCREEN_W - background.w) / 2,
                         (SCREEN_H - background.h) / 2, background.w, background.h);
                        break;

                    case 2: /* tile */
                        for (y = 0; y < SCREEN_H; y += background.h)
                            for (x = 0; x < SCREEN_W; x += background.w)
                                blit(background, screen, 0, 0, x, y,
                                     background.w, background.h);
                        break;
                }
            }
            else
            {
                textprintf_centre_ex(screen, font, SCREEN_W / 2, SCREEN_H / 2,
                         makecol(r, g, b), -1, string.Format("{0} not found", filename));
            }

            textout_centre_ex(screen, font, title, SCREEN_W / 2, 20, makecol(r, g, b), -1);

            readkey();

            //free(filename);
            //free(title);

            return 0;
        }
    }
}
