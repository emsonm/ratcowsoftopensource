using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    public class GFX_DRIVER : ManagedPointer
    {
        public GFX_DRIVER(IntPtr pointer)
            : base(pointer)
        {
        }

        public int id
        {
            get
            {
                return ReadInt(0);
            }
        }

        public string name
        {
            get
            {
                return ReadString(sizeof(Int32));
            }
        }

        public string desc
        {
            get
            {
                return ReadString(2 * sizeof(Int32));
            }
        }

        public string ascii_name
        {
            get
            {
                return ReadString(3 * sizeof(Int32));
            }
        }

        /* physical (not virtual!) screen size */
        public int w
        {
            get
            {
                return ReadInt(27 * sizeof(Int32));
            }
        }

        public int h
        {
            get
            {
                return ReadInt(28 * sizeof(Int32));
            }
        }

        /* true if video memory is linear */
        public int linear
        {
            get
            {
                return ReadInt(29 * sizeof(Int32));
            }
        }

        /* bank size, in bytes */
        public int bank_size
        {
            get
            {
                return ReadInt(30 * sizeof(Int32));
            }
        }

        /* bank granularity, in bytes */
        public int bank_gran
        {
            get
            {
                return ReadInt(31 * sizeof(Int32));
            }
        }

        /* video memory size, in bytes */
        public int vid_mem
        {
            get
            {
                return ReadInt(32 * sizeof(Int32));
            }
        }

        /* physical address of video memory */
        public int vid_phys_base
        {
            get
            {
                return ReadInt(33 * sizeof(Int32));
            }
        }

        /* true if driver runs windowed */
        public int windowed
        {
            get
            {
                return ReadInt(34 * sizeof(Int32));
            }
        }

        public static implicit operator GFX_DRIVER(IntPtr pointer)
        {
            return new GFX_DRIVER(pointer);
        }
    }

    /// <summary>
    /// Single palette entry.
    /// </summary>
    public class RGB : ManagedPointer
    {
        public RGB()
            : this(0, 0, 0, 0)
        {
        }

        public RGB(byte r, byte g, byte b, byte filler)
            : base(Alloc(4 * sizeof(byte)))
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.filler = filler;
        }

        public RGB(IntPtr pointer)
            : base(pointer)
        {
        }

        public byte r
        {
            get
            {
                return ReadByte(0);
            }

            set
            {
                WriteByte(0, value);
            }
        }

        public byte g
        {
            get
            {
                return ReadByte(sizeof(byte));
            }

            set
            {
                WriteByte(sizeof(byte), value);
            }
        }

        public byte b
        {
            get
            {
                return ReadByte(2 * sizeof(byte));
            }

            set
            {
                WriteByte(2 * sizeof(byte), value);
            }
        }

        public byte filler
        {
            get
            {
                return ReadByte(3 * sizeof(byte));
            }

            set
            {
                WriteByte(3 * sizeof(byte), value);
            }
        }

        public static implicit operator RGB(IntPtr pointer)
        {
            return new RGB(pointer);
        }
    }

    public class RGB_MAP : ManagedPointerArray
    {
        public RGB_MAP()
            : base(Alloc(32 * 32 * 32 * sizeof(byte)))
        {
        }
    }

    public class COLOR_MAP : ManagedPointer
    {
        public COLOR_MAP()
            : base(Alloc(256 * 256 * sizeof(byte)))
        {
        }

        public COLOR_MAP(IntPtr pointer)
            : base(pointer)
        {
        }

        public ManagedPointerBidimensionalArray data
        {
            get
            {
                return new ManagedPointerBidimensionalArray(pointer, 256, 256);
            }
        }
    }

    /// <summary>
    /// Stores palette information.
    /// </summary>
    public class PALETTE : ManagedPointer
    {
        public PALETTE()
            : base(Alloc(256 * 4 * sizeof(byte)))
        {
        }

        public PALETTE(IntPtr pointer)
            : base(pointer)
        {
        }

        public RGB this[int index]
        {
            get
            {
                return new RGB(Offset(pointer, index * 4 * sizeof(byte)));
            }

            set
            {
                this[index].r = value.r;
                this[index].g = value.g;
                this[index].b = value.b;
            }
        }

        public static implicit operator PALETTE(IntPtr pointer)
        {
            return new PALETTE(pointer);
        }
    }

    public class ZBUFFER : BITMAP
    {
        public ZBUFFER(IntPtr pointer)
            : base(pointer)
        {
        }

        public static implicit operator ZBUFFER(IntPtr pointer)
        {
            return new ZBUFFER(pointer);
        }
    }

    /// <summary>
    /// Stores the contents of a compiled sprite.
    /// </summary>
    public class COMPILED_SPRITE : RLE_SPRITE
    {
        public COMPILED_SPRITE(IntPtr pointer)
            : base(pointer)
        {
        }
        /*
        /// <summary>
        /// set if it's a planar (mode-X) sprite
        /// </summary>
        public short planar
        {
            get
            {
                return ReadShort(0);
            }
            set
            {
                WriteShort(0, value);
            }
        }

        /// <summary>
        /// color depth of the image
        /// </summary>
        public short color_depth
        {
            get
            {
                return ReadShort(sizeof(Int16));
            }
            set
            {
                WriteShort(sizeof(Int16), value);
            }
        }

        /// <summary>
        /// size of the sprite
        /// </summary>
        public short w
        {
            get
            {
                return ReadShort(2 * sizeof(Int16));
            }
            set
            {
                WriteShort(2 * sizeof(Int16), value);
            }
        }

        /// <summary>
        /// size of the sprite
        /// </summary>
        public short h
        {
            get
            {
                return ReadShort(4 * sizeof(Int16));
            }
            set
            {
                WriteShort(4 * sizeof(Int16), value);
            }
        }*/

        public static implicit operator COMPILED_SPRITE(IntPtr pointer)
        {
            return new COMPILED_SPRITE(pointer);
        }
    }

    /// <summary>
    /// Stores the contents of an RLE sprite.
    /// </summary>
    public class RLE_SPRITE : ManagedPointer
    {
        public RLE_SPRITE(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// width in pixels
        /// </summary>
        public int w
        {
            get
            {
                return ReadInt(0);
            }
            set
            {
                WriteInt(0, value);
            }
        }

        /// <summary>
        /// height in pixels
        /// </summary>
        public int h
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

        /// <summary>
        /// color depth of the image
        /// </summary>
        public int color_depth
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

        public static implicit operator RLE_SPRITE(IntPtr pointer)
        {
            return new RLE_SPRITE(pointer);
        }
    }

    /// <summary>
    /// a bitmap structure
    /// </summary>
    public class BITMAP : ManagedPointer
    {
        public BITMAP(IntPtr pointer)
            : base(pointer)
        {
        }

        /// <summary>
        /// width in pixels
        /// </summary>
        public int w
        {
            get
            {
                return ReadInt(0);
            }

            set
            {
                WriteInt(0, value);
            }
        }

        /// <summary>
        /// height in pixels
        /// </summary>
        public int h
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }

        /// <summary>
        /// flag if clipping is turned on
        /// </summary>
        public int clip
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }
        }

        /// <summary>
        /// clip left value
        /// </summary>
        public int cl
        {
            get
            {
                return ReadInt(3 * sizeof(Int32));
            }
        }

        /// <summary>
        /// clip right value
        /// </summary>
        public int cr
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }
        }

        //clip top value
        public int ct
        {
            get
            {
                return ReadInt(5 * sizeof(Int32));
            }
        }

        /// <summary>
        /// clip bottom value
        /// </summary>
        public int cb
        {
            get
            {
                return ReadInt(6 * sizeof(Int32));
            }
        }

        /// <summary>
        /// drawing functions
        /// </summary>
        public IntPtr vtable
        {
            get
            {
                return ReadPointer(7 * sizeof(Int32));
            }

            set
            {
                WritePointer(7 * sizeof(Int32), value);
            }
        }

        /// <summary>
        /// C func on some machines, asm on i386
        /// </summary>
        public IntPtr write_bank
        {
            get
            {
                return ReadPointer(8 * sizeof(Int32));
            }
        }

        /// <summary>
        /// C func on some machines, asm on i386
        /// </summary>
        public IntPtr read_bank
        {
            get
            {
                return ReadPointer(9 * sizeof(Int32));
            }
        }

        /// <summary>
        /// the memory we allocated for the bitmap
        /// </summary>
        public IntPtr dat
        {
            get
            {
                return ReadPointer(10 * sizeof(Int32));
            }
        }

        /// <summary>
        /// for identifying sub-bitmaps
        /// </summary>
        public ulong id
        {
            get
            {
                return (ulong)ReadInt(11 * sizeof(Int32));
            }
        }

        /// <summary>
        /// points to a structure with more info
        /// </summary>
        public IntPtr extra
        {
            get
            {
                return ReadPointer(12 * sizeof(Int32));
            }
        }

        /// <summary>
        /// horizontal offset (for sub-bitmaps)
        /// </summary>
        public int x_ofs
        {
            get
            {
                return ReadInt(13 * sizeof(Int32));
            }
        }

        /// <summary>
        /// vertical offset (for sub-bitmaps)
        /// </summary>
        public int y_ofs
        {
            get
            {
                return ReadInt(14 * sizeof(Int32));
            }
        }

        /// <summary>
        /// bitmap segment
        /// </summary>
        public int seg
        {
            get
            {
                return ReadInt(15 * sizeof(Int32));
            }
        }

        //public IntPtr line
        //{
        //  get
        //  {
        //    return ReadPointer(12 * sizeof(Int32));
        //  }
        //}

        public ManagedPointerBidimensionalArray line
        {
            get
            {
                return new ManagedPointerBidimensionalArray(ReadPointer(16 * sizeof(Int32)), w, h);
            }
        }

        public static implicit operator BITMAP(IntPtr pointer)
        {
            return new BITMAP(pointer);
        }
    }

    public class GFX_VTABLE : ManagedPointer       /* functions for drawing onto bitmaps */
    {
        public GFX_VTABLE(IntPtr pointer)
            : base(pointer)
        {
        }

        public int color_depth
        {
            get
            {
                return ReadInt(0);
            }

            set
            {
                WriteInt(0, value);
            }
        }

        public int mask_color
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

        /// <summary>
        /// C function on some machines, asm on i386
        /// </summary>
        public IntPtr unwrite_bank
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

        //AL_METHOD(void, set_clip, (struct BITMAP *bmp));
        //AL_METHOD(void, acquire, (struct BITMAP *bmp));
        //AL_METHOD(void, release, (struct BITMAP *bmp));
        //AL_METHOD(struct BITMAP *, create_sub_bitmap, (struct BITMAP *parent, int x, int y, int width, int height));
        //AL_METHOD(void, created_sub_bitmap, (struct BITMAP *bmp, struct BITMAP *parent));
        //AL_METHOD(int,  getpixel, (struct BITMAP *bmp, int x, int y));
        //AL_METHOD(void, putpixel, (struct BITMAP *bmp, int x, int y, int color));
        //AL_METHOD(void, vline, (struct BITMAP *bmp, int x, int y_1, int y2, int color));
        //AL_METHOD(void, hline, (struct BITMAP *bmp, int x1, int y, int x2, int color));
        //AL_METHOD(void, hfill, (struct BITMAP *bmp, int x1, int y, int x2, int color));
        //AL_METHOD(void, line, (struct BITMAP *bmp, int x1, int y_1, int x2, int y2, int color));
        //AL_METHOD(void, fastline, (struct BITMAP *bmp, int x1, int y_1, int x2, int y2, int color));
        //AL_METHOD(void, rectfill, (struct BITMAP *bmp, int x1, int y_1, int x2, int y2, int color));
        //AL_METHOD(void, triangle, (struct BITMAP *bmp, int x1, int y_1, int x2, int y2, int x3, int y3, int color));
        //AL_METHOD(void, draw_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_256_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_sprite_v_flip, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_sprite_h_flip, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_sprite_vh_flip, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_trans_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_trans_rgba_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y));
        //AL_METHOD(void, draw_lit_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y, int color));
        //AL_METHOD(void, draw_rle_sprite, (struct BITMAP *bmp, AL_CONST struct RLE_SPRITE *sprite, int x, int y));
        //AL_METHOD(void, draw_trans_rle_sprite, (struct BITMAP *bmp, AL_CONST struct RLE_SPRITE *sprite, int x, int y));
        //AL_METHOD(void, draw_trans_rgba_rle_sprite, (struct BITMAP *bmp, AL_CONST struct RLE_SPRITE *sprite, int x, int y));
        //AL_METHOD(void, draw_lit_rle_sprite, (struct BITMAP *bmp, AL_CONST struct RLE_SPRITE *sprite, int x, int y, int color));
        //AL_METHOD(void, draw_character, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y, int color, int bg));
        //AL_METHOD(void, draw_glyph, (struct BITMAP *bmp, AL_CONST struct FONT_GLYPH *glyph, int x, int y, int color, int bg));
        //AL_METHOD(void, blit_from_memory, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_to_memory, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_from_system, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_to_system, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_to_self, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_to_self_forward, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_to_self_backward, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, blit_between_formats, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, masked_blit, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height));
        //AL_METHOD(void, clear_to_color, (struct BITMAP *bitmap, int color));
        //AL_METHOD(void, pivot_scaled_sprite_flip, (struct BITMAP *bmp, struct BITMAP *sprite, fixed x, fixed y, fixed cx, fixed cy, fixed angle, fixed scale, int v_flip));
        //AL_METHOD(void, do_stretch_blit, (struct BITMAP *source, struct BITMAP *dest, int source_x, int source_y, int source_width, int source_height, int dest_x, int dest_y, int dest_width, int dest_height, int masked));
        //AL_METHOD(void, draw_gouraud_sprite, (struct BITMAP *bmp, struct BITMAP *sprite, int x, int y, int c1, int c2, int c3, int c4));
        //AL_METHOD(void, draw_sprite_end, (void));
        //AL_METHOD(void, blit_end, (void));
        //AL_METHOD(void, polygon, (struct BITMAP *bmp, int vertices, AL_CONST int *points, int color));
        //AL_METHOD(void, rect, (struct BITMAP *bmp, int x1, int y_1, int x2, int y2, int color));
        //AL_METHOD(void, circle, (struct BITMAP *bmp, int x, int y, int radius, int color));
        //AL_METHOD(void, circlefill, (struct BITMAP *bmp, int x, int y, int radius, int color));
        //AL_METHOD(void, ellipse, (struct BITMAP *bmp, int x, int y, int rx, int ry, int color));
        //AL_METHOD(void, ellipsefill, (struct BITMAP *bmp, int x, int y, int rx, int ry, int color));
        //AL_METHOD(void, arc, (struct BITMAP *bmp, int x, int y, fixed ang1, fixed ang2, int r, int color));
        //AL_METHOD(void, spline, (struct BITMAP *bmp, AL_CONST int points[8], int color));
        //AL_METHOD(void, floodfill, (struct BITMAP *bmp, int x, int y, int color));
        //AL_METHOD(void, polygon3d, (struct BITMAP *bmp, int type, struct BITMAP *texture, int vc, V3D *vtx[]));
        //AL_METHOD(void, polygon3d_f, (struct BITMAP *bmp, int type, struct BITMAP *texture, int vc, V3D_f *vtx[]));
        //AL_METHOD(void, triangle3d, (struct BITMAP *bmp, int type, struct BITMAP *texture, V3D *v1, V3D *v2, V3D *v3));
        //AL_METHOD(void, triangle3d_f, (struct BITMAP *bmp, int type, struct BITMAP *texture, V3D_f *v1, V3D_f *v2, V3D_f *v3));
        //AL_METHOD(void, quad3d, (struct BITMAP *bmp, int type, struct BITMAP *texture, V3D *v1, V3D *v2, V3D *v3, V3D *v4));
        //AL_METHOD(void, quad3d_f, (struct BITMAP *bmp, int type, struct BITMAP *texture, V3D_f *v1, V3D_f *v2, V3D_f *v3, V3D_f *v4));

        public static implicit operator GFX_VTABLE(IntPtr pointer)
        {
            return new GFX_VTABLE(pointer);
        }
    }

}
