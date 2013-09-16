using System.Runtime.InteropServices;

using sharpallegro;

namespace playfli
{
    class playfli : Allegro
    {
        static void usage()
        {
            allegro_message("\nFLI player test program for Allegro " + ALLEGRO_VERSION_STR + ", " + ALLEGRO_PLATFORM_STR + "\n" +
                    "By Shawn Hargreaves, " + ALLEGRO_DATE_STR + "\n\n" +
                    "Usage: playfli [options] filename.(fli|flc)\n\n" +
                    "Options:\n" +
                    "\t'-loop' cycles the animation until a key is pressed\n" +
                    "\t'-step' selects single-step mode\n" +
                    "\t'-mode screen_w screen_h' sets the screen mode\n");
        }



        static int loop = FALSE;
        static int step = FALSE;



        static int key_checker()
        {
            if (step == TRUE)
            {
                if ((readkey() & 0xFF) == 27)
                    return 1;
                else
                    return 0;
            }
            else
            {
                if (keypressed())
                    return 1;
                else
                    return 0;
            }
        }
        static FLICCallback c_key_checker = new FLICCallback(key_checker);



        static int Main(string[] argv)
        {
            int w = 320;
            int h = 200;
            int c, ret;

            if (allegro_init() != 0)
                return 1;

            if (argv.Length < 1)
            {
                usage();
                return 1;
            }

            for (c = 0; c < argv.Length - 1; c++)
            {
                if (string.Compare(argv[c], "-loop") == 0)
                    loop = TRUE;
                else if (string.Compare(argv[c], "-step") == 0)
                    step = TRUE;
                else if ((string.Compare(argv[c], "-mode") == 0) && (c < argv.Length - 2))
                {
                    w = int.Parse(argv[c + 1]);
                    h = int.Parse(argv[c + 2]);
                    c += 2;
                }
                else
                {
                    usage();
                    return 1;
                }
            }

            install_keyboard();
            install_timer();

            if (set_gfx_mode(GFX_AUTODETECT, w, h, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
                return 1;
            }

            ret = play_fli(argv[c], screen, loop, Marshal.GetFunctionPointerForDelegate(c_key_checker));

            if (ret < 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message("Error playing FLI file\n");
                return 1;
            }

            return 0;
        }
    }
}
