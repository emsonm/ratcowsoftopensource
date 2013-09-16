using sharpallegro;

namespace exexedat
{
    class exexedat : Allegro
    {
        /* the grabber produces this header, which contains defines for the names
         * of all the objects in the datafile (BIG_FONT, SILLY_BITMAP, etc). We
         * still need to keep this, since we want to know the names of the objects.
         */
        const int BIG_FONT = 0;        /* FONT */
        const int SILLY_BITMAP = 1;        /* BMP  */
        const int THE_PALETTE = 2;        /* PAL  */



        static int Main(string[] argv)
        {
            DATAFILE datafile;

            if (allegro_init() != 0)
                return 1;
            install_keyboard();

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

            /* we still don't have a palette => Don't let Allegro twist colors */
            set_color_conversion(COLORCONV_NONE);

            /* load the datafile into memory */
            datafile = load_datafile("#");
            if (!datafile)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message("Unable to load the appended datafile!\n\nThis program" +
                        " reads graphics from the end of the executable file.\n" +
                        "Before running it, you must append this data with the " +
                        "exedat utility.\n\nExample command line:\n\n" +
#if (ALLEGRO_DOS) || (ALLEGRO_WINDOWS)
 "\texedat exexedat.exe example.dat\n\n" +
#else
		      "\texedat exexedat example.dat\n\n" +
#endif
 "To compress the appended data, pass the -c switch " +
                        "to exedat.\n");
                return 1;
            }

            /* select the palette which was loaded from the datafile */
            set_palette(datafile[THE_PALETTE].dat);

            /* aha, set a palette and let Allegro convert colors when blitting */
            set_color_conversion(COLORCONV_TOTAL);

            /* display the bitmap from the datafile */
            textout_ex(screen, font, "This is the bitmap:", 32, 16,
                   makecol(255, 255, 255), -1);
            blit(datafile[SILLY_BITMAP].dat, screen, 0, 0, 64, 32, 64, 64);

            /* and use the font from the datafile */
            textout_ex(screen, datafile[BIG_FONT].dat, "And this is a big font!",
                   32, 128, makecol(0, 255, 0), -1);

            readkey();

            /* unload the datafile when we are finished with it */
            unload_datafile(datafile);

            return 0;
        }
    }
}
