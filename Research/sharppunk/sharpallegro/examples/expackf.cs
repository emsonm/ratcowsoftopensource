using System;
using System.Text;

using sharpallegro;
using System.Runtime.InteropServices;

namespace expackf
{
    class expackf : Allegro
    {
        const int SEEK_CUR = 1;
        [DllImport("msvcr71")]
        static extern IntPtr fopen(string filename, string mode);
        [DllImport("msvcr71")]
        static extern IntPtr freopen(string path, string mode, IntPtr stream);
        [DllImport("msvcr71")]
        static extern int fclose(IntPtr file);
        [DllImport("msvcr71")]
        static extern int fgetc(IntPtr stream);
        [DllImport("msvcr71")]
        static extern int ungetc(int c, IntPtr stream);
        [DllImport("msvcr71")]
        static extern int fread(IntPtr buffer, int size, int count, IntPtr stream);
        [DllImport("msvcr71")]
        static extern int fputc(int c, IntPtr stream);
        [DllImport("msvcr71")]
        static extern int fwrite(IntPtr buffer, int size, int count, IntPtr stream);
        [DllImport("msvcr71")]
        static extern int fseek(IntPtr stream, int offset, int origin);
        [DllImport("msvcr71")]
        static extern int feof(IntPtr stream);
        [DllImport("msvcr71")]
        static extern int ferror(IntPtr stream);

        /*----------------------------------------------------------------------*/
        /*                memory vtable                                         */
        /*----------------------------------------------------------------------*/

        /* The packfile data for our memory reader. */
        public class MEMREAD_INFO : ManagedPointer
        {
            public MEMREAD_INFO()
                : base(Alloc(3 * sizeof(Int32)))
            {
            }

            public MEMREAD_INFO(IntPtr pointer)
                : base(pointer)
            {
            }

            public ManagedBytePointerArray block
            {
                get
                {
                    return new ManagedBytePointerArray(ReadPointer(0));
                }
                set
                {
                    WritePointer(0, value.pointer);
                }
            }

            public int length
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

            public int offset
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

            public static implicit operator MEMREAD_INFO(IntPtr pointer)
            {
                return new MEMREAD_INFO(pointer);
            }
        }

        static int memread_getc(IntPtr userdata)
        {
            MEMREAD_INFO info = userdata;
            //ASSERT(info);
            //ASSERT(info.offset <= info.length);

            if (info.offset == info.length)
                return EOF;
            else
            {
                return info.block[info.offset++];
            }
        }
        static GetC pf_memread_getc = new GetC(memread_getc);

        static int memread_ungetc(int c, IntPtr userdata)
        {
            MEMREAD_INFO info = userdata;
            byte ch = (byte)c;

            if ((info.offset > 0) && (info.block[info.offset - 1] == ch))
                return ch;
            else
                return EOF;
        }
        static UngetC pf_memread_ungetc = new UngetC(memread_ungetc);

        static int memread_putc(int c, IntPtr userdata)
        {
            return EOF;
        }
        static PutC pf_memread_putc = new PutC(memread_putc);

        static int memread_fread(IntPtr p, int n, IntPtr userdata)
        {
            MEMREAD_INFO info = userdata;
            //size_t actual;
            int actual;
            //ASSERT(info);
            //ASSERT(info.offset <= info.length);

            actual = MIN((int)n, info.length - info.offset);

            //memcpy(p, info.block + info.offset, actual);

            info.offset += actual;

            //ASSERT(info.offset <= info.length);

            return actual;
        }
        static FRead pf_memread_fread = new FRead(memread_fread);

        static int memread_fwrite(IntPtr p, int n, IntPtr userdata)
        {
            return 0;
        }
        static FWrite pf_memread_fwrite = new FWrite(memread_fwrite);

        static int memread_seek(IntPtr userdata, int offset)
        {
            MEMREAD_INFO info = userdata;
            int actual;
            //ASSERT(info);
            //ASSERT(info.offset <= info.length);

            actual = MIN(offset, info.length - info.offset);

            info.offset += actual;

            //ASSERT(info.offset <= info.length);

            if (offset == actual)
                return 0;
            else
                return -1;
        }
        static FSeek pf_memread_seek = new FSeek(memread_seek);

        static int memread_fclose(IntPtr userdata)
        {
            return 0;
        }
        static FClose pf_memread_fclose = new FClose(memread_fclose);

        static int memread_feof(IntPtr userdata)
        {
            MEMREAD_INFO info = userdata;

            return info.offset >= info.length ? 1 : 0;
        }
        static FEOF pf_memread_feof = new FEOF(memread_feof);

        static int memread_ferror(IntPtr userdata)
        {
            //(void)userdata;

            return FALSE;
        }
        static FError pf_memread_ferror = new FError(memread_ferror);

        /* The actual vtable. Note that writing is not supported, the functions for
         * writing above are only placeholders.
         */
        static PACKFILE_VTABLE memread_vtable = new PACKFILE_VTABLE(
            Marshal.GetFunctionPointerForDelegate(pf_memread_fclose),
            Marshal.GetFunctionPointerForDelegate(pf_memread_getc),
            Marshal.GetFunctionPointerForDelegate(pf_memread_ungetc),
            Marshal.GetFunctionPointerForDelegate(pf_memread_fread),
            Marshal.GetFunctionPointerForDelegate(pf_memread_putc),
            Marshal.GetFunctionPointerForDelegate(pf_memread_fwrite),
            Marshal.GetFunctionPointerForDelegate(pf_memread_seek),
            Marshal.GetFunctionPointerForDelegate(pf_memread_feof),
            Marshal.GetFunctionPointerForDelegate(pf_memread_ferror));

        /*----------------------------------------------------------------------*/
        /*                stdio vtable                                          */
        /*----------------------------------------------------------------------*/

        static int stdio_fclose(IntPtr userdata)
        {
            IntPtr fp = userdata;
            return fclose(fp);
        }
        static FClose pf_stdio_fclose = new FClose(stdio_fclose);

        static int stdio_getc(IntPtr userdata)
        {
            IntPtr fp = userdata;
            return fgetc(fp);
        }
        static GetC pf_stdio_getc = new GetC(stdio_getc);

        static int stdio_ungetc(int c, IntPtr userdata)
        {
            IntPtr fp = userdata;
            return ungetc(c, fp);
        }
        static UngetC pf_stdio_ungetc = new UngetC(stdio_ungetc);

        static int stdio_fread(IntPtr p, int n, IntPtr userdata)
        {
            IntPtr fp = userdata;
            return fread(p, 1, n, fp);
        }
        static FRead pf_stdio_fread = new FRead(stdio_fread);

        static int stdio_putc(int c, IntPtr userdata)
        {
            IntPtr fp = userdata;
            return fputc(c, fp);
        }
        static PutC pf_stdio_putc = new PutC(stdio_putc);

        static int stdio_fwrite(IntPtr p, int n, IntPtr userdata)
        {
            IntPtr fp = userdata;
            return fwrite(p, 1, n, fp);
        }
        static FWrite pf_stdio_fwrite = new FWrite(stdio_fwrite);

        static int stdio_seek(IntPtr userdata, int n)
        {
            IntPtr fp = userdata;
            return fseek(fp, n, SEEK_CUR);
        }
        static FSeek pf_stdio_seek = new FSeek(stdio_seek);

        static int stdio_feof(IntPtr userdata)
        {
            IntPtr fp = userdata;
            return feof(fp);
        }
        static FEOF pf_stdio_feof = new FEOF(stdio_feof);

        static int stdio_ferror(IntPtr userdata)
        {
            IntPtr fp = userdata;
            return ferror(fp);
        }
        static FError pf_stdio_ferror = new FError(stdio_ferror);

        /* The actual vtable. */
        static PACKFILE_VTABLE stdio_vtable = new PACKFILE_VTABLE(
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_fclose),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_getc),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_ungetc),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_fread),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_putc),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_fwrite),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_seek),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_feof),
                  Marshal.GetFunctionPointerForDelegate(pf_stdio_ferror));

        /*----------------------------------------------------------------------*/
        /*                tests                                                 */
        /*----------------------------------------------------------------------*/

        static void next()
        {
            textprintf_centre_ex(screen, font, SCREEN_W / 2,
               SCREEN_H - text_height(font), -1, -1, "Press a key to continue");
            readkey();
            clear_bitmap(screen);
        }

        static void CHECK(Int64 x, string err)
        {
            do
            {
                if (x == 0)
                {
                    alert("Error", err, null, "Ok", null, 0, 0);
                    return;
                }
            } while (false);
        }

        /* This reads the files mysha.pcx and allegro.pcx into a memory block as
         * binary data, and then uses the memory vtable to read the bitmaps out of
         * the memory block.
         */
        static void memread_test()
        {
            PACKFILE f;
            MEMREAD_INFO memread_info = new MEMREAD_INFO();
            BITMAP bmp, bmp2;
            IntPtr block;
            int l1, l2;
            PACKFILE f1, f2;

            l1 = file_size_ex("allegro.pcx");
            l2 = file_size_ex("mysha.pcx");

            //block = malloc(l1 + l2);
            block = Marshal.AllocHGlobal(l1 + l2);

            /* Read mysha.pcx into the memory block. */
            f1 = pack_fopen("allegro.pcx", "rb");
            CHECK(f1.pointer.ToInt32(), "opening allegro.pcx");
            pack_fread(block, l1, f1);
            pack_fclose(f1);

            /* Read allegro.pcx into the memory block. */
            f2 = pack_fopen("mysha.pcx", "rb");
            CHECK(f2.pointer.ToInt32(), "opening mysha.pcx");
            pack_fread(new ManagedPointer(block).Offset(l1), l2, f2);
            pack_fclose(f2);

            /* Open the memory block as PACKFILE, using our memory vtable. */
            memread_info.block = block;
            memread_info.length = l1 + l2;
            memread_info.offset = 0;
            f = pack_fopen_vtable(memread_vtable, memread_info);
            CHECK(f.pointer.ToInt32(), "reading from memory block");

            /* Read the bitmaps out of the memory block. */
            // TODO: check why f gets nulled
            PACKFILE t = f, u = f;
            bmp = load_pcx_pf(f, NULL);
            CHECK(bmp.pointer.ToInt32(), "load_pcx_pf");
            //bmp2 = load_pcx_pf(f, NULL);
            bmp2 = load_pcx_pf(t, NULL);
            CHECK(bmp2.pointer.ToInt32(), "load_pcx_pf");

            blit(bmp, screen, 0, 0, 0, 0, bmp.w, bmp.h);
            textprintf_ex(screen, font, bmp.w + 8, 8, -1, -1,
               "\"allegro.pcx\"");
            textprintf_ex(screen, font, bmp.w + 8, 8 + 20, -1, -1,
               "read out of a memory file");

            blit(bmp2, screen, 0, 0, 0, bmp.h + 8, bmp2.w, bmp2.h);
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8, -1, -1,
               "\"mysha.pcx\"");
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8 + 20, -1, -1,
               "read out of a memory file");

            destroy_bitmap(bmp);
            destroy_bitmap(bmp2);
            //pack_fclose(f);
            pack_fclose(u);

            next();
        }

        /* This reads in allegro.pcx, but it does so by using the stdio vtable. */
        static void stdio_read_test()
        {
            IntPtr fp, fp2;
            PACKFILE f;
            BITMAP bmp, bmp2;

            /* Simply open the file with the libc fopen. */
            fp = fopen("allegro.pcx", "rb");
            IntPtr t = fp;
            CHECK(fp.ToInt32(), "opening allegro.pcx");

            /* Create a PACKFILE, with our custom stdio vtable. */
            f = pack_fopen_vtable(stdio_vtable, fp);
            CHECK(f.pointer.ToInt32(), "reading with stdio");

            ///* Now read in the bitmap. */
            bmp = load_pcx_pf(f, NULL);
            CHECK(bmp.pointer.ToInt32(), "load_pcx_pf");

            ///* A little bit hackish, we re-assign the file pointer in our PACKFILE
            // * to another file.
            // */
            //fp2 = freopen("mysha.pcx", "rb", fp);
            fp2 = freopen("mysha.pcx", "rb", t);
            CHECK(fp2.ToInt32(), "opening mysha.pcx");

            ///* Read in the other bitmap. */
            bmp2 = load_pcx_pf(f, NULL);
            CHECK(bmp.pointer.ToInt32(), "load_pcx_pf");

            blit(bmp, screen, 0, 0, 0, 0, bmp.w, bmp.h);
            textprintf_ex(screen, font, bmp2.w + 8, 8, -1, -1,
               "\"allegro.pcx\"");
            textprintf_ex(screen, font, bmp2.w + 8, 8 + 20, -1, -1,
               "read with stdio functions");

            blit(bmp2, screen, 0, 0, 0, bmp.h + 8, bmp2.w, bmp2.h);
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8, -1, -1,
               "\"mysha.pcx\"");
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8 + 20, -1, -1,
               "read with stdio functions");

            destroy_bitmap(bmp);
            destroy_bitmap(bmp2);
            pack_fclose(f);

            next();
        }

        /* This demonstrates seeking. It opens expackf.c, and reads some characters
         * from it.
         */
        static void stdio_seek_test()
        {
            IntPtr fp;
            PACKFILE f;
            IntPtr str = Marshal.AllocHGlobal(8);

            fp = fopen("expackf.c", "rb");
            CHECK(fp.ToInt32(), "opening expackf.c");
            f = pack_fopen_vtable(stdio_vtable, fp);
            CHECK(f.pointer.ToInt32(), "reading with stdio");

            pack_fseek(f, 33);
            pack_fread(str, 7, f);
            //str[7] = '\0';

            textprintf_ex(screen, font, 0, 0, -1, -1, "Reading from \"expackf.c\" with stdio.");
            textprintf_ex(screen, font, 0, 20, -1, -1, "Seeking to byte 33, reading 7 bytes:");
            textprintf_ex(screen, font, 0, 40, -1, -1, string.Format("\"{0}\"", Marshal.PtrToStringAnsi(str)));
            textprintf_ex(screen, font, 0, 60, -1, -1, "(Should be \"Allegro\")");

            pack_fclose(f);

            next();
        }

        /* This demonstrates writing. It simply saves the two bitmaps into a binary
         * file.
         */
        static void stdio_write_test()
        {
            IntPtr fp;
            PACKFILE f;
            BITMAP bmp, bmp2;

            /* Read the bitmaps. */
            bmp = load_pcx("allegro.pcx", NULL);
            CHECK(bmp.pointer.ToInt32(), "load_pcx");
            bmp2 = load_pcx("mysha.pcx", NULL);
            CHECK(bmp2.pointer.ToInt32(), "load_pcx");

            /* Write them with out custom vtable. */
            fp = fopen("expackf.out", "wb");
            CHECK(fp.ToInt32(), "writing expackf.out");
            f = pack_fopen_vtable(stdio_vtable, fp);
            CHECK(f.pointer.ToInt32(), "writing with stdio");

            // TODO: check why saving something other than a pcx causes engine exception while reading back
            //save_tga_pf(f, bmp, NULL);
            save_pcx_pf(f, bmp, NULL);
            //save_bmp_pf(f, bmp2, NULL);
            save_pcx_pf(f, bmp2, NULL);

            destroy_bitmap(bmp);
            destroy_bitmap(bmp2);
            pack_fclose(f);

            /* Now read them in again with our custom vtable. */
            fp = fopen("expackf.out", "rb");
            CHECK(fp.ToInt32(), "fopen");
            f = pack_fopen_vtable(stdio_vtable, fp);
            CHECK(f.pointer.ToInt32(), "reading from stdio");

            /* Note: in general you would need to implement a "chunking" system
             * that knows where the boundary of each file is. Many file format
             * loaders will happily read everything to the end of the file,
             * whereas others stop reading as soon as they have all the essential
             * data (e.g. there may be some metadata at the end of the file).
             * Concatenating bare files together only works in examples programs.
             */
            //bmp = load_tga_pf(f, NULL);
            bmp = load_pcx_pf(f, NULL);
            CHECK(bmp.pointer.ToInt32(), "load_tga_pf");
            //bmp2 = load_bmp_pf(f, NULL);
            bmp2 = load_pcx_pf(f, NULL);
            CHECK(bmp2.pointer.ToInt32(), "load_bmp_pf");

            blit(bmp, screen, 0, 0, 0, 0, bmp.w, bmp.h);
            textprintf_ex(screen, font, bmp2.w + 8, 8, -1, -1,
               "\"allegro.pcx\" (as tga)");
            textprintf_ex(screen, font, bmp2.w + 8, 8 + 20, -1, -1,
               "wrote with stdio functions");

            blit(bmp2, screen, 0, 0, 0, bmp.h + 8, bmp2.w, bmp2.h);
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8, -1, -1,
               "\"mysha.pcx\" (as bmp)");
            textprintf_ex(screen, font, bmp2.w + 8, bmp.h + 8 + 20, -1, -1,
               "wrote with stdio functions");

            destroy_bitmap(bmp);
            destroy_bitmap(bmp2);
            pack_fclose(f);

            next();
        }

        /*----------------------------------------------------------------------*/

        static int Main()
        {
            if (allegro_init() != 0)
                return 1;
            install_keyboard();
            set_color_depth(32);
            if (set_gfx_mode(GFX_AUTODETECT_WINDOWED, 640, 480, 0, 0) != 0)
            {
                set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
                allegro_message(string.Format("Unable to set a 640x480x32 windowed mode\n{0}\n", allegro_error));
                return 1;
            }

            memread_test();

            stdio_read_test();

            stdio_seek_test();

            stdio_write_test();

            return 0;
        }
    }
}
