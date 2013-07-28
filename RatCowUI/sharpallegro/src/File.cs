using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FClose(IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetC(IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int UngetC(int c, IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FRead(IntPtr p, int n, IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PutC(int c, IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FWrite(IntPtr p, int n, IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FSeek(IntPtr userdata, int offset);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FEOF(IntPtr userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FError(IntPtr userdata);

    //AL_METHOD(int, pf_fclose, (void *userdata));
    //AL_METHOD(int, pf_getc, (void *userdata));
    //AL_METHOD(int, pf_ungetc, (int c, void *userdata));
    //AL_METHOD(long, pf_fread, (void *p, long n, void *userdata));
    //AL_METHOD(int, pf_putc, (int c, void *userdata));
    //AL_METHOD(long, pf_fwrite, (AL_CONST void *p, long n, void *userdata));
    //AL_METHOD(int, pf_fseek, (void *userdata, int offset));
    //AL_METHOD(int, pf_feof, (void *userdata));
    //AL_METHOD(int, pf_ferror, (void *userdata));

    public class PACKFILE : ManagedPointer                           /* our very own FILE structure... */
    {
        public PACKFILE()
            : base(Alloc(4 * sizeof(Int32)))
        {
        }

        public PACKFILE(IntPtr pointer)
            : base(pointer)
        {
        }

        public IntPtr vtable
        {
            get
            {
                return ReadPointer(0);
            }
        }

        public IntPtr userdata
        {
            get
            {
                return ReadPointer(sizeof(Int32));
            }
        }

        public int is_normal_packfile
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }
        }

        /* The following is only to be used for the "normal" PACKFILE vtable,
         * i.e. what is implemented by Allegro itself. If is_normal_packfile is
         * false then the following is not even allocated. This must be the last
         * member in the structure.
         */
        public IntPtr normal
        {
            get
            {
                return ReadPointer(3 * sizeof(Int32));
            }
        }

        public static implicit operator PACKFILE(IntPtr pointer)
        {
            return new PACKFILE(pointer);
        }
    }

    public class PACKFILE_VTABLE : ManagedPointer
    {
        public PACKFILE_VTABLE()
            : base(Alloc(9 * sizeof(Int32)))
        {
        }

        public PACKFILE_VTABLE(IntPtr pf_pclose, IntPtr pf_getc, IntPtr pf_ungetc, IntPtr pf_fread, IntPtr pf_putc, IntPtr pf_fwrite, IntPtr pf_fseek, IntPtr pf_feof, IntPtr pf_ferror)
            : base(Alloc(9 * sizeof(Int32)))
        {
            this.pf_pclose = pf_pclose;
            this.pf_getc = pf_getc;
            this.pf_ungetc = pf_ungetc;
            this.pf_fread = pf_fread;
            this.pf_putc = pf_putc;
            this.pf_fwrite = pf_fwrite;
            this.pf_fseek = pf_fseek;
            this.pf_feof = pf_feof;
            this.pf_ferror = pf_ferror;
        }

        public IntPtr pf_pclose
        {
            set
            {
                WritePointer(0, value);
            }
        }

        public IntPtr pf_getc
        {
            set
            {
                WritePointer(sizeof(Int32), value);
            }
        }

        public IntPtr pf_ungetc
        {
            set
            {
                WritePointer(2 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_fread
        {
            set
            {
                WritePointer(3 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_putc
        {
            set
            {
                WritePointer(4 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_fwrite
        {
            set
            {
                WritePointer(5 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_fseek
        {
            set
            {
                WritePointer(6 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_feof
        {
            set
            {
                WritePointer(7 * sizeof(Int32), value);
            }
        }

        public IntPtr pf_ferror
        {
            set
            {
                WritePointer(8 * sizeof(Int32), value);
            }
        }

        //AL_METHOD(int, pf_fclose, (void *userdata));
        //AL_METHOD(int, pf_getc, (void *userdata));
        //AL_METHOD(int, pf_ungetc, (int c, void *userdata));
        //AL_METHOD(long, pf_fread, (void *p, long n, void *userdata));
        //AL_METHOD(int, pf_putc, (int c, void *userdata));
        //AL_METHOD(long, pf_fwrite, (AL_CONST void *p, long n, void *userdata));
        //AL_METHOD(int, pf_fseek, (void *userdata, int offset));
        //AL_METHOD(int, pf_feof, (void *userdata));
        //AL_METHOD(int, pf_ferror, (void *userdata));
    };
}
