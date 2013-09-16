using System;
using System.Runtime.InteropServices;
using System.Text;

using sharpallegro;

namespace exgui
{
    class exgui : Allegro
    {
        const int BIG_FONT = 0; /* FONT */
        const int SILLY_BITMAP = 1; /* BMP  */
        const int THE_PALETTE = 2; /* PAL  */

        /* maximum number of bytes a single (Unicode) character can have */
        const int MAX_BYTES_PER_CHAR = 4;

        /* for the d_edit_proc object */
        const int LEN = 32;
        //char the_string[(LEN + 1) * MAX_BYTES_PER_CHAR] = "Change Me!";
        static string the_string = "Change Me!";

        /* for the d_text_box_proc object */
        //char the_text[] =
        static string the_text =
           @"I'm text inside a text box.\n\n
           I can have multiple lines.\n\n
           If I grow too big to fit into my box, I get a scrollbar to 
           the right, so you can scroll me in the vertical direction. I will never 
           let you scroll in the horizontal direction, but instead I will try to 
           word wrap the text.";

        /* for the multiple selection list */
        //char sel[10];
        static ManagedPointer sel = new ManagedPointer(10 * sizeof(char));

        /* for the example bitmap */
        static DATAFILE datafile;



        /* callback function to specify the contents of the listbox */
        static string listbox_getter(int index, IntPtr list_size)
        {
            string[] strings =
           {
              "Zero",  "One",   "Two",   "Three", "Four",  "Five", 
              "Six",   "Seven", "Eight", "Nine",  "Ten"
           };

            if (index < 0)
            {
                //*list_size = 11;
                Marshal.WriteInt32(list_size, 11);
                return null;
            }
            else
            {
                return strings[index];
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string ListboxGetter(int index, IntPtr list_size);
        static ListboxGetter d_listbox_getter = new ListboxGetter(listbox_getter);



        /* Used as a menu-callback, and by the quit button. */
        static int quit()
        {
            if (alert("Really Quit?", null, null, "&Yes", "&No", 'y', 'n') == 1)
                return D_CLOSE;
            else
                return D_O_K;
        }
        static MenuCallback d_quit = new MenuCallback(quit);



        /* A custom dialog procedure, derived from d_button_proc. It intercepts
         * the D_CLOSE return of d_button_proc, and calls the function in dp3.
         */
        static int my_button_proc(int msg, IntPtr d, int c)
        {
            int ret = d_button_proc(msg, d, c);
            if (ret == D_CLOSE && ((DIALOG)d).dp3 != NULL)
                return ((MenuCallback)Marshal.GetDelegateForFunctionPointer(((DIALOG)d).dp3, typeof(MenuCallback)))();
            return ret;
        }
        static DIALOG_PROC d_my_button_proc = new DIALOG_PROC(my_button_proc);



        /* Our about box. */
        static int about()
        {
            alert("* exgui *",
                  "",
                  "Allegro GUI Example",
                  "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_about = new MenuCallback(about);



        /* Another menu callback. */
        static int menu_callback()
        {
            //char str[256];
            string str = string.Empty;

            //ustrzcpy(str, sizeof str, active_menu->text);
            str = active_menu.text;
            //alert("Selected menu item:", "", ustrtok(str, "\t"), "Ok", NULL, 0, 0);
            alert("Selected menu item:", "", str.Split(new char[] { '\t' })[0], "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_menu_callback = new MenuCallback(menu_callback);



        /* Menu callback which toggles the checked status. */
        static int check_callback()
        {
            active_menu.flags ^= D_SELECTED;
            if ((active_menu.flags & D_SELECTED) > 0)
                active_menu.text = "Checked";
            else
                active_menu.text = "Unchecked";

            alert("Menu item has been toggled!", null, null, "Ok", null, 0, 0);
            return D_O_K;
        }



        //        /* the submenu */
        //        MENU[] submenu =
        //{
        //   new MENU( "Submenu",            NULL,   NULL, D_DISABLED,       NULL  ),
        //   new MENU( "",                   NULL,   NULL, 0,                NULL  ),
        //   new MENU( "Checked",  check_callback,   NULL, D_SELECTED,       NULL  ),
        //   new MENU( "Disabled",           NULL,   NULL, D_DISABLED,       NULL  ),
        //   new MENU( null,                 NULL,   NULL, 0,                NULL  )
        //};


        //        /* the first menu in the menubar */
        //        MENU[] menu1 =
        //{
        //   new MENU( "Test &1 \t1", menu_callback,  NULL,      0,  NULL  ),
        //   new MENU( "Test &2 \t2", menu_callback,  NULL,      0,  NULL  ),
        //   new MENU( "&Quit \tq/Esc",        quit,  NULL,      0,  NULL  ),
        //   new MENU( null,                   NULL,  NULL,      0,  NULL  )
        //};



        //        /* the second menu in the menubar */
        //        MENU[] menu2 =
        //{
        //   new MENU( "&Test",    menu_callback,     NULL,   0,  NULL  ),
        ////   new MENU( "&Submenu",          NULL,  submenu,   0,  NULL  ),
        //   new MENU( null,                NULL,     NULL,   0,  NULL  )
        //};



        //        /* the help menu */
        //        MENU[] helpmenu =
        //{
        //   new MENU( "&About \tF1",     about,  NULL,      0,  NULL  ),
        //   new MENU( null,               NULL,  NULL,      0,  NULL  )
        //};



        //        /* the main menu-bar */
        //        MENU[] the_menu =
        //{
        //   //new MENU( "&First",  NULL,   menu1,          0,      NULL  ),
        //   //new MENU( "&Second", NULL,   menu2,          0,      NULL  ),
        //   //new MENU( "&Help",   NULL,   helpmenu,       0,      NULL  ),
        //   new MENU( null,      NULL,   NULL,           0,      NULL  )
        //};



        //extern int info1(void);
        //extern int info2(void);
        //extern int info3(void);

        const int LIST_OBJECT = 26;
        const int TEXTLIST_OBJECT = 27;
        const int SLIDER_OBJECT = 29;
        const int BITMAP_OBJECT = 32;
        const int ICON_OBJECT = 33;

        /* here it comes - the big bad ugly DIALOG array for our main dialog */
        static DIALOGS the_dialog = new DIALOGS(41);
        //{
        /* (dialog proc)     (x)   (y)   (w)   (h) (fg)(bg) (key) (flags)     (d1) (d2)    (dp)                   (dp2) (dp3) */

        /* this element just clears the screen, therefore it should come before the others */
        //new DIALOG( d_clear_proc,        0,   0,    0,    0,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),

        /* these just display text, either left aligned, centered, or right aligned */
        //new DIALOG( "d_text_proc",         0,  20,    0,    0,   0,  0,    0,      0,       0,   0,    "d_text_proc",          NULL, NULL  ),
        //new DIALOG( "d_ctext_proc",      318,  20,    0,    0,   0,  0,    0,      0,       0,   0,    "d_ctext_proc",         NULL, NULL  ),
        //new DIALOG( "d_rtext_proc",      636,  20,    0,    0,   0,  0,    0,      0,       0,   0,    "d_rtext_proc",         NULL, NULL  ),

        ///* lots of descriptive text elements */
        //new DIALOG( "d_text_proc",         0,   0,    0,    0,   0,  0,    0,      0,       0,   0,    "d_menu_proc->",        NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0,  40,    0,    0,   0,  0,    0,      0,       0,   0,    "d_button_proc->",      NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0,  70,    0,    0,   0,  0,    0,      0,       0,   0,    "d_check_proc->",       NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 100,    0,    0,   0,  0,    0,      0,       0,   0,    "d_radio_proc->",       NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 130,    0,    0,   0,  0,    0,      0,       0,   0,    "d_edit_proc->",        NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 150,    0,    0,   0,  0,    0,      0,       0,   0,    "d_list_proc->",        NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 200,    0,    0,   0,  0,    0,      0,       0,   0,    "d_text_list_proc->",   NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 250,    0,    0,   0,  0,    0,      0,       0,   0,    "d_textbox_proc->",     NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 300,    0,    0,   0,  0,    0,      0,       0,   0,    "d_slider_proc->",      NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 330,    0,    0,   0,  0,    0,      0,       0,   0,    "d_box_proc->",         NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 360,    0,    0,   0,  0,    0,      0,       0,   0,    "d_shadow_box_proc->",  NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 390,    0,    0,   0,  0,    0,      0,       0,   0,    "d_keyboard_proc. Press F1 to see me trigger the about box.", NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 410,    0,    0,   0,  0,    0,      0,       0,   0,    "d_clear_proc. I draw the white background.", NULL, NULL  ),
        //new DIALOG( "d_text_proc",         0, 430,    0,    0,   0,  0,    0,      0,       0,   0,    "d_yield_proc. I make us play nice with the OS scheduler.", NULL, NULL  ),
        //new DIALOG( "d_rtext_proc",      636,  40,    0,    0,   0,  0,    0,      0,       0,   0,    "<-d_bitmap_proc",      NULL, NULL  ),
        //new DIALOG( "d_rtext_proc",      636,  80,    0,    0,   0,  0,    0,      0,       0,   0,    "<-d_icon_proc",        NULL, NULL  ),

        ///* a menu bar - note how it auto-calculates its dimension if they are not given */
        //new DIALOG( "d_menu_proc",       160,   0,    0,    0,   0,  0,    0,      0,       0,   0,    the_menu,               NULL, NULL  ),

        ///* some more GUI elements, all of which require you to specify their dimensions */
        //new DIALOG( "d_button_proc",     160,  40,  160,   20,   0,  0,  't',      0,       0,   0,    "&Toggle Me!",          NULL, NULL  ),
        //new DIALOG( "d_check_proc",      160,  70,  160,   20,   0,  0,  'c',      0,       0,   0,    "&Check Me!",           NULL, NULL  ),
        //new DIALOG( "d_radio_proc",      160, 100,  160,   19,   0,  0,  's',      0,       0,   0,    "&Select Me!",          NULL, NULL  ),
        //new DIALOG( "d_radio_proc",      320, 100,  160,   19,   0,  0,  'o',      0,       0,   0,    "&Or Me!",              NULL, NULL  ),
        //new DIALOG( "d_edit_proc",       160, 130,  160,    8,   0,  0,    0,      0,     LEN,   0,    the_string,             NULL, NULL  ),
        //new DIALOG( "d_list_proc",       160, 150,  160,   44,   0,  0,    0,      0,       0,   0,    (void *)listbox_getter, sel,  NULL  ),
        //new DIALOG( "d_text_list_proc",  160, 200,  160,   44,   0,  0,    0,      0,       0,   0,    (void *)listbox_getter, NULL, NULL  ),
        //new DIALOG( "d_textbox_proc",    160, 250,  160,   48,   0,  0,    0,      0,       0,   0,    (void *)the_text,       NULL, NULL  ),
        //new DIALOG( "d_slider_proc",     160, 300,  160,   12,   0,  0,    0,      0,     100,   0,    NULL,                   NULL, NULL  ),
        //new DIALOG( "d_box_proc",        160, 330,  160,   20,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),
        //new DIALOG( "d_shadow_box_proc", 160, 360,  160,   20,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),

        ///* note how we don't fill in the dp field yet, because we first need to load the bitmap */
        //new DIALOG( "d_bitmap_proc",     480,  40,   30,   30,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),
        //new DIALOG( "d_icon_proc",       480,  80,   30,   30,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),

        ///* the quit and info buttons use our customized dialog procedure, using dp3 as callback */
        //new DIALOG( my_button_proc,      0, 450,  160,   20,   0,  0,  'q', D_EXIT,       0,   0,    "&Quit",                NULL, (void *)quit  ),
        //new DIALOG( my_button_proc,    400, 150,  160,   20,   0,  0,  'i', D_EXIT,       0,   0,    "&Info",                NULL, (void *)info1 ),
        //new DIALOG( my_button_proc,    400, 200,  160,   20,   0,  0,  'n', D_EXIT,       0,   0,    "I&nfo",                NULL, (void *)info2 ),
        //new DIALOG( my_button_proc,    400, 300,  160,   20,   0,  0,  'f', D_EXIT,       0,   0,    "In&fo",                NULL, (void *)info3 ),

        ///* the next two elements don't draw anything */
        //new DIALOG( "d_keyboard_proc",     0,   0,    0,    0,   0,  0,    0,      0,  KEY_F1,   0,    (void *)about,          NULL, NULL  ),
        //   new DIALOG( "d_yield_proc",        0,   0,    0,    0,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  ),
        //   new DIALOG( NULL,                0,   0,    0,    0,   0,  0,    0,      0,       0,   0,    NULL,                   NULL, NULL  )
        //};



        /* These three functions demonstrate how to query dialog elements.
         */
        static int info1()
        {
            //char buf1[256];
            //char buf2[256] = "";
            string buf1 = string.Empty;
            string buf2 = string.Empty;
            //int i, s = 0, n;
            int i, s = 0;
            ManagedPointer n = new ManagedPointer(1);

            //listbox_getter(-1, &n);
            listbox_getter(-1, n);
            ///* query the list proc */
            for (i = 0; i < n.ReadInt(0); i++)
            {
                //if (sel[i]) {
                if (sel.ReadByte(i * sizeof(byte)) > 0)
                {
                    //uszprintf(buf1, sizeof buf1, "%i ", i);
                    buf1 = string.Format("{0} ", i);
                    //ustrzcat(buf2, sizeof buf2, buf1);
                    buf2 += buf1;
                    s = 1;
                }
            }
            if (s > 0)
                //   ustrzcat(buf2, sizeof buf2, "are in the multiple selection!");
                buf2 += "are in the multiple selection!";
            else
                //   ustrzcat(buf2, sizeof buf2, "There is no multiple selection!");
                buf2 += "There is no multiple selection!";
            //uszprintf(buf1, sizeof buf1, "Item number %i is selected!",
            //   the_dialog[LIST_OBJECT].d1);
            buf1 = string.Format("Item number {0} is selected!", the_dialog[LIST_OBJECT].d1);
            alert("Info about the list:", buf1, buf2, "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_info1 = new MenuCallback(info1);

        static int info2()
        {
            //char buf[256];
            string buf = string.Empty;

            ///* query the textlist proc */
            //uszprintf(buf, sizeof buf, "Item number %i is selected!",
            //   the_dialog[TEXTLIST_OBJECT].d1);
            buf = string.Format("Item number {0} is selected!", the_dialog[TEXTLIST_OBJECT].d1);
            alert("Info about the text list:", null, buf, "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_info2 = new MenuCallback(info2);

        static int info3()
        {
            //char buf[256];
            string buf = string.Empty;

            ///* query the slider proc */
            //uszprintf(buf, sizeof buf, "Slider position is %i!",
            //  the_dialog[SLIDER_OBJECT].d2);
            buf = string.Format("Slider position is {0}!", the_dialog[SLIDER_OBJECT].d2);
            alert("Info about the slider:", null, buf, "Ok", null, 0, 0);
            return D_O_K;
        }
        static MenuCallback d_info3 = new MenuCallback(info3);



        static int Main(string[] argv)
        {
            //        /* the submenu */
            MENUS submenu = new MENUS(5);
            submenu[0] = new MENU("Submenu", NULL, NULL, D_DISABLED, NULL);
            submenu[1] = new MENU("", NULL, NULL, 0, NULL);
            submenu[2] = new MENU("Checked", check_callback, NULL, D_SELECTED, NULL);
            submenu[3] = new MENU("Disabled", NULL, NULL, D_DISABLED, NULL);
            submenu[4] = new MENU(null, NULL, NULL, 0, NULL);

            /* the first menu in the menubar */
            MENUS menu1 = new MENUS(4);
            menu1[0] = new MENU("Test &1 \t1", menu_callback, NULL, 0, NULL);
            menu1[1] = new MENU("Test &2 \t2", menu_callback, NULL, 0, NULL);
            menu1[2] = new MENU("&Quit \tq/Esc", Marshal.GetFunctionPointerForDelegate(d_quit), NULL, 0, NULL);
            menu1[3] = new MENU(null, NULL, NULL, 0, NULL);

            /* the second menu in the menubar */
            MENUS menu2 = new MENUS(3);
            menu2[0] = new MENU("&Test", menu_callback, NULL, 0, NULL);
            menu2[1] = new MENU("&Submenu", NULL, submenu, 0, NULL);
            menu2[2] = new MENU(null, NULL, NULL, 0, NULL);

            /* the help menu */
            MENUS helpmenu = new MENUS(2);
            helpmenu[0] = new MENU("&About \tF1", Marshal.GetFunctionPointerForDelegate(d_about), NULL, 0, NULL);
            helpmenu[1] = new MENU(null, NULL, NULL, 0, NULL);

            /* the main menu-bar */
            MENUS the_menu = new MENUS(4);
            the_menu[0] = new MENU("&First", NULL, menu1, 0, NULL);
            the_menu[1] = new MENU("&Second", NULL, menu2, 0, NULL);
            the_menu[2] = new MENU("&Help", NULL, helpmenu, 0, NULL);
            the_menu[3] = new MENU(null, NULL, NULL, 0, NULL);

            the_dialog[0] = new DIALOG(d_clear_proc, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);

            /* these just display text, either left aligned, centered, or right aligned */
            the_dialog[1] = new DIALOG("d_text_proc", 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_text_proc"), NULL, NULL);
            the_dialog[2] = new DIALOG("d_ctext_proc", 318, 20, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_ctext_proc"), NULL, NULL);
            the_dialog[3] = new DIALOG("d_rtext_proc", 636, 20, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_rtext_proc"), NULL, NULL);

            ///* lots of descriptive text elements */
            the_dialog[4] = new DIALOG("d_text_proc", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_menu_proc->"), NULL, NULL);
            the_dialog[5] = new DIALOG("d_text_proc", 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_button_proc->"), NULL, NULL);
            the_dialog[6] = new DIALOG("d_text_proc", 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_check_proc->"), NULL, NULL);
            the_dialog[7] = new DIALOG("d_text_proc", 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_radio_proc->"), NULL, NULL);
            the_dialog[8] = new DIALOG("d_text_proc", 0, 130, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_edit_proc->"), NULL, NULL);
            the_dialog[9] = new DIALOG("d_text_proc", 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_list_proc->"), NULL, NULL);
            the_dialog[10] = new DIALOG("d_text_proc", 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_text_list_proc->"), NULL, NULL);
            the_dialog[11] = new DIALOG("d_text_proc", 0, 250, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_textbox_proc->"), NULL, NULL);
            the_dialog[12] = new DIALOG("d_text_proc", 0, 300, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_slider_proc->"), NULL, NULL);
            the_dialog[13] = new DIALOG("d_text_proc", 0, 330, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_box_proc->"), NULL, NULL);
            the_dialog[14] = new DIALOG("d_text_proc", 0, 360, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_shadow_box_proc->"), NULL, NULL);
            the_dialog[15] = new DIALOG("d_text_proc", 0, 390, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_keyboard_proc. Press F1 to see me trigger the about box."), NULL, NULL);
            the_dialog[16] = new DIALOG("d_text_proc", 0, 410, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_clear_proc. I draw the white background."), NULL, NULL);
            the_dialog[17] = new DIALOG("d_text_proc", 0, 430, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("d_yield_proc. I make us play nice with the OS scheduler."), NULL, NULL);
            the_dialog[18] = new DIALOG("d_rtext_proc", 636, 40, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("<-d_bitmap_proc"), NULL, NULL);
            the_dialog[19] = new DIALOG("d_rtext_proc", 636, 80, 0, 0, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi("<-d_icon_proc"), NULL, NULL);

            ///* a menu bar - note how it auto-calculates its dimension if they are not given */
            the_dialog[20] = new DIALOG("d_menu_proc", 160, 0, 0, 0, 0, 0, 0, 0, 0, 0, the_menu, NULL, NULL);


            ///* some more GUI elements, all of which require you to specify their dimensions */
            the_dialog[21] = new DIALOG("d_button_proc", 160, 40, 160, 20, 0, 0, 't', 0, 0, 0, Marshal.StringToCoTaskMemAnsi("&Toggle Me!"), NULL, NULL);
            the_dialog[22] = new DIALOG("d_check_proc", 160, 70, 160, 20, 0, 0, 'c', 0, 0, 0, Marshal.StringToCoTaskMemAnsi("&Check Me!"), NULL, NULL);
            the_dialog[23] = new DIALOG("d_radio_proc", 160, 100, 160, 19, 0, 0, 's', 0, 0, 0, Marshal.StringToCoTaskMemAnsi("&Select Me!"), NULL, NULL);
            the_dialog[24] = new DIALOG("d_radio_proc", 320, 100, 160, 19, 0, 0, 'o', 0, 0, 0, Marshal.StringToCoTaskMemAnsi("&Or Me!"), NULL, NULL);
            the_dialog[25] = new DIALOG("d_edit_proc", 160, 130, 160, 8, 0, 0, 0, 0, LEN, 0, Marshal.StringToCoTaskMemAnsi(the_string), NULL, NULL);
            the_dialog[26] = new DIALOG("d_list_proc", 160, 150, 160, 44, 0, 0, 0, 0, 0, 0, Marshal.GetFunctionPointerForDelegate(d_listbox_getter), sel, NULL);
            the_dialog[27] = new DIALOG("d_text_list_proc", 160, 200, 160, 44, 0, 0, 0, 0, 0, 0, Marshal.GetFunctionPointerForDelegate(d_listbox_getter), NULL, NULL);
            the_dialog[28] = new DIALOG("d_textbox_proc", 160, 250, 160, 48, 0, 0, 0, 0, 0, 0, Marshal.StringToCoTaskMemAnsi(the_text), NULL, NULL);
            the_dialog[29] = new DIALOG("d_slider_proc", 160, 300, 160, 12, 0, 0, 0, 0, 100, 0, NULL, NULL, NULL);
            the_dialog[30] = new DIALOG("d_box_proc", 160, 330, 160, 20, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dialog[31] = new DIALOG("d_shadow_box_proc", 160, 360, 160, 20, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);

            ///* note how we don't fill in the dp field yet, because we first need to load the bitmap */
            the_dialog[32] = new DIALOG("d_bitmap_proc", 480, 40, 30, 30, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dialog[33] = new DIALOG("d_icon_proc", 480, 80, 30, 30, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);

            ///* the quit and info buttons use our customized dialog procedure, using dp3 as callback */
            the_dialog[34] = new DIALOG(d_my_button_proc, 0, 450, 160, 20, 0, 0, 'q', D_EXIT, 0, 0, Marshal.StringToCoTaskMemAnsi("&Quit"), NULL, Marshal.GetFunctionPointerForDelegate(d_quit));
            the_dialog[35] = new DIALOG(d_my_button_proc, 400, 150, 160, 20, 0, 0, 'i', D_EXIT, 0, 0, Marshal.StringToCoTaskMemAnsi("&Info"), NULL, Marshal.GetFunctionPointerForDelegate(d_info1));
            the_dialog[36] = new DIALOG(d_my_button_proc, 400, 200, 160, 20, 0, 0, 'n', D_EXIT, 0, 0, Marshal.StringToCoTaskMemAnsi("I&nfo"), NULL, Marshal.GetFunctionPointerForDelegate(d_info2));
            the_dialog[37] = new DIALOG(d_my_button_proc, 400, 300, 160, 20, 0, 0, 'f', D_EXIT, 0, 0, Marshal.StringToCoTaskMemAnsi("In&fo"), NULL, Marshal.GetFunctionPointerForDelegate(d_info3));

            ///* the next two elements don't draw anything */
            the_dialog[38] = new DIALOG("d_keyboard_proc", 0, 0, 0, 0, 0, 0, 0, 0, KEY_F1, 0, Marshal.GetFunctionPointerForDelegate(d_about), NULL, NULL);
            the_dialog[39] = new DIALOG("d_yield_proc", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);
            the_dialog[the_dialog.Length - 1] = new DIALOG(NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL);

            //char buf[256];
            byte[] buf = new byte[256];
            int i;

            /* initialise everything */
            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            install_mouse();
            install_timer();

            if (set_gfx_mode(GFX_AUTODETECT, 640, 480, 0, 0) != 0)
            {
                if (set_gfx_mode(GFX_SAFE, 640, 480, 0, 0) != 0)
                {
                    set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                    allegro_message(string.Format("Unable to set any graphic mode\n{0}\n", allegro_error));
                    return 1;
                }
            }

            /* load the datafile */
            //replace_filename(buf, argv[0], "example.dat", sizeof(buf));
            //datafile = load_datafile(buf);
            replace_filename(buf, "./", "example.dat", buf.Length);
            datafile = load_datafile(Encoding.ASCII.GetString(buf));
            if (!datafile)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Error loading {0}!\n", buf));
                return 1;
            }

            set_palette(datafile[THE_PALETTE].dat);

            /* set up colors */
            gui_fg_color = makecol(0, 0, 0);
            gui_mg_color = makecol(128, 128, 128);
            gui_bg_color = makecol(200, 240, 200);
            set_dialog_color(the_dialog, gui_fg_color, gui_bg_color);

            /* white color for d_clear_proc and the d_?text_procs */
            the_dialog[0].bg = makecol(255, 255, 255);
            for (i = 4; the_dialog[i].proc != NULL; i++)
            {
                if (the_dialog[i].proc == GetAddress("d_text_proc") ||
                    the_dialog[i].proc == GetAddress("d_ctext_proc") ||
                    the_dialog[i].proc == GetAddress("d_rtext_proc"))
                {
                    the_dialog[i].bg = the_dialog[0].bg;
                }
            }

            /* fill in bitmap pointers */
            the_dialog[BITMAP_OBJECT].dp = datafile[SILLY_BITMAP].dat;
            the_dialog[ICON_OBJECT].dp = datafile[SILLY_BITMAP].dat;

            /* shift the dialog 2 pixels away from the border */
            position_dialog(the_dialog, 2, 2);

            /* do the dialog */
            do_dialog(the_dialog, -1);

            unload_datafile(datafile);

            return 0;
        }
    }
}
