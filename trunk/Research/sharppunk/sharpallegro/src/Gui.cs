using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int DIALOG_PROC(int msg, IntPtr d, int c);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int MenuCallback();

    public class DIALOGS : ManagedPointer
    {
        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public DIALOGS(int length)
            : base(Alloc(length * 14 * sizeof(Int32)))
        {
            this.length = length;
        }

        new public DIALOG this[int index]
        {
            get
            {
                return new DIALOG(Offset(index * 14 * sizeof(Int32)));
            }
            set
            {
                this[index].proc = value.proc;
                this[index].x = value.x;
                this[index].y = value.y;
                this[index].w = value.w;
                this[index].h = value.h;
                this[index].fg = value.fg;
                this[index].bg = value.bg;
                this[index].key = value.key;
                this[index].flags = value.flags;
                this[index].d1 = value.d1;
                this[index].d2 = value.d2;
                this[index].dp = value.dp;
                this[index].dp2 = value.dp2;
                this[index].dp3 = value.dp3;
                //value.pointer = Offset(index * 14 * sizeof(Int32));
            }
        }
    }

    public class DIALOG : ManagedPointer
    {
        public DIALOG(IntPtr pointer)
            : base(pointer)
        {
        }

        public DIALOG(IntPtr proc, int x, int y, int w, int h, int fg, int bg, int key, int flags, int d1, int d2, IntPtr dp, IntPtr dp2, IntPtr dp3)
            : base(Alloc(14 * sizeof(Int32)))
        {
            this.proc = proc;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.fg = fg;
            this.bg = bg;
            this.key = key;
            this.flags = flags;
            this.d1 = d1;
            this.d2 = d2;
            this.dp = dp;
            this.dp2 = dp2;
            this.dp3 = dp3;
        }

        public DIALOG(DIALOG_PROC proc, int x, int y, int w, int h, int fg, int bg, int key, int flags, int d1, int d2, IntPtr dp, IntPtr dp2, IntPtr dp3)
            : this(Marshal.GetFunctionPointerForDelegate(proc), x, y, w, h, fg, bg, key, flags, d1, d2, dp, dp2, dp3)
        {
        }

        public DIALOG(string proc, int x, int y, int w, int h, int fg, int bg, int key, int flags, int d1, int d2, IntPtr dp, IntPtr dp2, IntPtr dp3)
            : this(AllegroAPI.GetAddress(proc), x, y, w, h, fg, bg, key, flags, d1, d2, dp, dp2, dp3)
        {
        }

        public IntPtr proc
        {
            get
            {
                return ReadPointer(0);
            }
            set
            {
                WritePointer(0, value);
            }
        }

        public int callback
        {
            set
            {
                WriteInt(0, value);
            }
        }

        public int x
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int y
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }

            set
            {
                WriteInt(2 * sizeof(Int32), value);
            }
        }

        public int w
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }

            set
            {
                WriteInt(3 * sizeof(Int32), value);
            }
        }

        public int h
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }

            set
            {
                WriteInt(4 * sizeof(Int32), value);
            }
        }

        public int fg
        {
            get
            {
                return ReadInt(5 * sizeof(Int32));
            }

            set
            {
                WriteInt(5 * sizeof(Int32), value);
            }
        }

        public int bg
        {
            get
            {
                return ReadInt(6 * sizeof(Int32));
            }

            set
            {
                WriteInt(6 * sizeof(Int32), value);
            }
        }

        public int key
        {
            get
            {
                return ReadInt(7 * sizeof(Int32));
            }

            set
            {
                WriteInt(7 * sizeof(Int32), value);
            }
        }

        public int flags
        {
            get
            {
                return ReadInt(8 * sizeof(Int32));
            }

            set
            {
                WriteInt(8 * sizeof(Int32), value);
            }
        }

        public int d1
        {
            get
            {
                return ReadInt(9 * sizeof(Int32));
            }

            set
            {
                WriteInt(9 * sizeof(Int32), value);
            }
        }

        public int d2
        {
            get
            {
                return ReadInt(10 * sizeof(Int32));
            }

            set
            {
                WriteInt(10 * sizeof(Int32), value);
            }
        }

        public IntPtr dp
        {
            get
            {
                return ReadPointer(11 * sizeof(Int32));
            }

            set
            {
                WritePointer(11 * sizeof(Int32), value);
            }
        }

        public IntPtr dp2
        {
            get
            {
                return ReadPointer(12 * sizeof(Int32));
            }

            set
            {
                WritePointer(12 * sizeof(Int32), value);
            }
        }

        public IntPtr dp3
        {
            get
            {
                return ReadPointer(13 * sizeof(Int32));
            }

            set
            {
                WritePointer(13 * sizeof(Int32), value);
            }
        }

        public static implicit operator DIALOG(IntPtr pointer)
        {
            return new DIALOG(pointer);
        }
    }

    /* stored information about the state of an active GUI dialog */

    public class DIALOG_PLAYER : ManagedPointer
    {
        public DIALOG_PLAYER(IntPtr pointer)
            : base(pointer)
        {
        }

        public int obj
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int res
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }

            set
            {
                WriteInt(2 * sizeof(Int32), value);
            }
        }

        public int mouse_obj
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }

            set
            {
                WriteInt(3 * sizeof(Int32), value);
            }
        }

        public int focus_obj
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int joy_on
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int click_wait
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int mouse_ox
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int mouse_oy
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int mouse_oz
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public int mouse_b
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }

            set
            {
                WriteInt(sizeof(Int32), value);
            }
        }

        public IntPtr dialog
        {
            get
            {
                return new IntPtr(ReadInt(sizeof(Int32)));
            }

            set
            {
                WriteInt(sizeof(Int32), value.ToInt32());
            }
        }

        public static implicit operator DIALOG_PLAYER(IntPtr pointer)
        {
            return new DIALOG_PLAYER(pointer);
        }
    }

    public class MENUS : ManagedPointer
    {
        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public MENUS(int length)
            : base(Alloc(length * 5 * sizeof(Int32)))
        {
            this.length = length;
        }

        new public MENU this[int index]
        {
            get
            {
                return new MENU(Offset(index * 5 * sizeof(Int32)));
            }
            set
            {
                this[index].text = value.text;
                this[index].proc = value.proc;
                this[index].child = value.child;
                this[index].flags = value.flags;
            }
        }
    }

    public class MENU : ManagedPointer
    {
        public MENU()
            : base(5 * sizeof(Int32))
        {
        }

        public MENU(IntPtr pointer)
            : base(pointer)
        {
        }

        public MENU(string text, IntPtr proc, IntPtr child, int flags, IntPtr dp)
            : this()
        {
            this.text = text;
            this.proc = proc;
            this.child = child;
            this.flags = flags;
            this.dp = dp;
        }

        public MENU(string text, MenuCallback proc, IntPtr child, int flags, IntPtr dp)
            : this(text, Marshal.GetFunctionPointerForDelegate(proc), child, flags, dp)
        {
        }

        public string text
        {
            get
            {
                return ReadString(0);
            }

            set
            {
                WriteString(0, value);
            }
        }

        public IntPtr proc
        {
            get
            {
                return ReadPointer(sizeof(Int32));
            }

            set
            {
                WritePointer(sizeof(Int32), value);
            }
        }

        public IntPtr child
        {
            get
            {
                return ReadPointer(2 * sizeof(Int32));
            }

            set
            {
                WritePointer(2 * sizeof(Int32), value);
            }
        }

        public int flags
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }

            set
            {
                WriteInt(3 * sizeof(Int32), value);
            }
        }

        public IntPtr dp
        {
            get
            {
                return ReadPointer(4 * sizeof(Int32));
            }

            set
            {
                WritePointer(4 * sizeof(Int32), value);
            }
        }

        public static implicit operator MENU(IntPtr pointer)
        {
            return new MENU(pointer);
        }

        //char *text;                   /* menu item text */
        //AL_METHOD(int, proc, (void)); /* callback function */
        //struct MENU *child;           /* to allow nested menus */
        //int flags;                    /* flags about the menu state */
        //void *dp;                     /* any data the menu might require */
    }
}