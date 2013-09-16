using System.Text;

using sharpallegro;

namespace exdata
{
    class exdata : Allegro
    {
        const int BIG_FONT = 0; /* FONT */
        const int SILLY_BITMAP = 1; /* BMP  */
        const int THE_PALETTE = 2; /* PAL  */

        static int Main(string[] argv)
        {
            DATAFILE datafile;
            byte[] buf = new byte[256];

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
            replace_filename(buf, "./", "example.dat", buf.Length);
            datafile = load_datafile(Encoding.ASCII.GetString(buf));
            if (!datafile)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error loading {0}!\n", buf));
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
