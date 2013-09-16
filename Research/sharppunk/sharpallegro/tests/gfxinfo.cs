#define ALLEGRO_USE_CONSOLE

using System;

using sharpallegro;

namespace gfxinfo
{
    class gfxinfo : Allegro
    {
        static string al_un_id(char[] _out, int driver_id)
        {
            _out[0] = (char)((driver_id >> 24) & 0xff);
            _out[1] = (char)((driver_id >> 16) & 0xff);
            _out[2] = (char)((driver_id >> 8) & 0xff);
            _out[3] = (char)((driver_id) & 0xff);
            _out[4] = '\0';
            return new string(_out);
        }



        static int al_id_safe(string s)
        {
            int l = s.Length;
            return AL_ID((l > 0) ? s[0] : ' ',
                         (l > 1) ? s[1] : ' ',
                         (l > 2) ? s[2] : ' ',
                         (l > 3) ? s[3] : ' ');
        }



        static unsafe int Main(string[] argv)
        {
            _DRIVER_INFO[] driver_info;
            GFX_DRIVER[] gfx_driver;
            GFX_MODE_LIST gfx_mode_list;
            int driver_count, i, id;
            char[] buf = new char[5];

            if (allegro_init() != 0)
                return 1;

            Console.Write("Allegro graphics info utility " + ALLEGRO_VERSION_STR + ", " + ALLEGRO_PLATFORM_STR + "\n");
            Console.Write("By Lorenzo Petrone, " + ALLEGRO_DATE_STR + "\n\n");

            if (system_driver.gfx_drivers != NULL)
                driver_info = system_driver._gfx_drivers();
            else
                driver_info = _gfx_driver_list;

            driver_count = 0;
            while (driver_info[driver_count].driver != NULL)
                driver_count++;

            //gfx_driver = malloc(sizeof(GFX_DRIVER *) * driver_count);
            gfx_driver = new GFX_DRIVER[driver_count];
            //if (!gfx_driver)
            if (gfx_driver == null)
                return 1;
            for (i = 0; i < driver_count; i++)
                gfx_driver[i] = driver_info[i].driver;

            if (argv.Length == 0)
            {
                Console.Write("No graphics driver specified.  These are available:\n\n");
                for (i = 0; i < driver_count; i++)
                    Console.Write(string.Format("{0} : {1}\n", al_un_id(buf, gfx_driver[i].id), gfx_driver[i].ascii_name));
                Console.Write("\n");
                goto end;
            }

            id = al_id_safe(argv[0]);
            for (i = 0; i < driver_count; i++)
            {
                if ((string.Compare(gfx_driver[i].ascii_name, argv[0]) == 0) || (gfx_driver[i].id == id))
                {
                    Console.Write(string.Format("Name: {0}; driver ID: {1}\n", gfx_driver[i].ascii_name, al_un_id(buf, gfx_driver[i].id)));
                    break;
                }
            }

            if (i == driver_count)
            {
                Console.Write(string.Format("Unknown graphics driver {0}.\n" +
                       "Run gfxinfo without parameters to see a list.\n\n", argv[0]));
                goto end;
            }

            if (gfx_driver[i].windowed != 0)
            {
                Console.Write("This is a windowed mode driver. You can use any resolution you want.\n\n");
                goto end;
            }

            gfx_mode_list = get_gfx_mode_list(gfx_driver[i].id);
            if (gfx_mode_list.p == NULL)
            {
                Console.Write("Failed to retrieve mode list. This might be because the driver\n" +
                       "doesn't support mode fetching.\n\n");
                goto end;
            }

            for (i = 0; i < gfx_mode_list.num_modes; i++)
            {
                Console.Write(string.Format("{0:d4}x{1,-4:d} {2:d2} bpp\n",
                       gfx_mode_list.mode[i].width,
                       gfx_mode_list.mode[i].height,
                       gfx_mode_list.mode[i].bpp));
            }
            Console.Write("\n");

            destroy_gfx_mode_list(gfx_mode_list);

        end:

            //free(gfx_driver);
            return 0;
        }
    }
}
