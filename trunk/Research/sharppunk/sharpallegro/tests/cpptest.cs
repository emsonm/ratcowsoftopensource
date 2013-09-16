using System;

using sharpallegro;

namespace cpptest
{
    class cpptest : Allegro
    {
        const int RODENTS = 4;
        static Random rand = new Random();


        /* We define dumb memory management operators in order
         * not to have to link against libstdc++ with GCC 3.x.
         * Don't do that in real-life C++ code unless you know
         * what you're doing.
         */
        //void *operator new(size_t size)
        //{
        //   return malloc(size);
        //}

        //void operator delete(void *ptr)
        //{
        //  if (ptr)
        //    free (ptr);
        //}


        /* Our favorite pet. */
        class rodent
        {
            public rodent()
            {
            }
            public rodent(BITMAP bmp)
            {
                x = rand.Next() % (SCREEN_W - bmp.w);
                y = rand.Next() % (SCREEN_H - bmp.h);

                do
                {
                    delta_x = (rand.Next() % 11) - 5;
                } while (delta_x == 0);

                do
                {
                    delta_y = (rand.Next() % 11) - 5;
                } while (delta_y == 0);

                sprite = bmp;
            }
            public void move()
            {
                if ((x + sprite.w + delta_x >= SCREEN_W) || (x + delta_x < 0)) delta_x = -delta_x;
                if ((y + sprite.h + delta_y >= SCREEN_H) || (y + delta_y < 0)) delta_y = -delta_y;

                x += delta_x;
                y += delta_y;
            }
            public void draw(BITMAP bmp)
            {
                draw_sprite(bmp, sprite, x, y);
            }

            private int x, y;
            private int delta_x, delta_y;
            private BITMAP sprite;
        }

        /* A little counter to waste your time. */
        static volatile int counter = 0;

        static void my_timer_handler()
        {
            counter++;
        }
        //END_OF_FUNCTION(my_timer_handler)
        static TimerHandler t_my_timer_handler = new TimerHandler(my_timer_handler);



        /* Yup, you read correctly, we're creating the World here. */
        class world
        {
            /* Genesis */
            public world()
            {
                PALETTE pal = new PALETTE();
                active = TRUE;

                dbuffer = create_bitmap(SCREEN_W, SCREEN_H);
                mouse_sprite = load_bitmap("../examples/mysha.pcx", pal);

                if (!mouse_sprite)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Error loading bitmap\n{0}\n", allegro_error));
                    //exit(1);
                }

                set_palette(pal);

                for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                    mouse[what_mouse] = new rodent(mouse_sprite);
            }
            /* Apocalypse */
            ~world()
            {
                destroy_bitmap(dbuffer);
                destroy_bitmap(mouse_sprite);

                //for(int what_mouse=0; what_mouse < RODENTS; what_mouse++)
                //   delete mouse[what_mouse];
            }
            public void draw()
            {
                clear_bitmap(dbuffer);

                for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                    mouse[what_mouse].draw(dbuffer);

                blit(dbuffer, screen, 0, 0, 0, 0, SCREEN_W, SCREEN_H);
            }
            public void logic()
            {
                if (key[KEY_ESC]) active = FALSE;

                for (int what_mouse = 0; what_mouse < RODENTS; what_mouse++)
                    mouse[what_mouse].move();
            }
            public void loop()
            {
                install_int_ex(t_my_timer_handler, BPS_TO_TIMER(10));

                while (active == TRUE)
                {
                    while (counter > 0)
                    {
                        counter--;
                        logic();
                    }
                    draw();
                }

                remove_int(t_my_timer_handler);
            }

            private static BITMAP dbuffer;
            private static BITMAP mouse_sprite;
            private static int active;
            private static rodent[] mouse = new rodent[RODENTS];
        }

        static int Main()
        {
            allegro_init();

            install_keyboard();
            install_timer();

            //srand(time(NULL));
            LOCK_VARIABLE(counter);
            LOCK_FUNCTION(t_my_timer_handler);

            if (set_gfx_mode(GFX_AUTODETECT, 1024, 768, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error setting graphics mode\n{0}\n", allegro_error));
                return 1;
            }

            world game = new world();  /* America! America! */
            game.loop();
            //delete game;

            return 0;
        }
    }
}
