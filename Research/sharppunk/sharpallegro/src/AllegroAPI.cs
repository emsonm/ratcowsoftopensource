/*
 * SharpAllegro: a C# wrapper around Allegro game library.
 * Copyright (C) 2007  Eugenio Favalli
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation;
 * version 2.1 of the License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 *
 * $Id: AllegroAPI.cs 106 2011-01-04 15:58:32Z eugeniofavalli $
 *
 */

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace sharpallegro
{
    public unsafe class Keys
    {
        public static unsafe byte* key;

        public bool this[int index]
        {
            get
            {
                return key[index] != 0;
            }
        }
    }

    public class AllegroAPI
    {
        public const string ALLEG_DLL = "alleg44.dll";

        #region Constants

        public const int SYSTEM_AUTODETECT = 0;
        public const int SYSTEM_NONE = ((('N') << 24) | (('O') << 16) | (('N') << 8) | ('E'));

        public const int GFX_TEXT = -1;
        public const int GFX_AUTODETECT = 0;
        public const int GFX_AUTODETECT_FULLSCREEN = 1;
        public const int GFX_AUTODETECT_WINDOWED = 2;
        public const int GFX_SAFE = ((('S') << 24) | (('A') << 16) | (('F') << 8) | ('E'));
        public const int GFX_DIRECTX_ACCEL = ((('D') << 24) | (('X') << 16) | (('A') << 8) | ('C'));
        public const int GFX_DIRECTX_WIN = ((('D') << 24) | (('X') << 16) | (('W') << 8) | ('N'));
        public const int GFX_DIRECTX = ((('D') << 24) | (('X') << 16) | (('A') << 8) | ('C'));
        public const int GFX_DIRECTX_SAFE = ((('D') << 24) | (('X') << 16) | (('S') << 8) | ('A'));
        public const int GFX_DIRECTX_SOFT = ((('D') << 24) | (('X') << 16) | (('S') << 8) | ('O'));
        public const int GFX_DIRECTX_OVL = ((('D') << 24) | (('X') << 16) | (('O') << 8) | ('V'));
        public const int GFX_GDI = ((('G') << 24) | (('D') << 16) | (('I') << 8) | ('B'));

        protected const int KB_SHIFT_FLAG = 0x0001;
        protected const int KB_CTRL_FLAG = 0x0002;
        protected const int KB_ALT_FLAG = 0x0004;
        protected const int KB_LWIN_FLAG = 0x0008;
        protected const int KB_RWIN_FLAG = 0x0010;
        protected const int KB_MENU_FLAG = 0x0020;
        protected const int KB_COMMAND_FLAG = 0x0040;
        protected const int KB_SCROLOCK_FLAG = 0x0100;
        protected const int KB_NUMLOCK_FLAG = 0x0200;
        protected const int KB_CAPSLOCK_FLAG = 0x0400;
        protected const int KB_INALTSEQ_FLAG = 0x0800;
        protected const int KB_ACCENT1_FLAG = 0x1000;
        protected const int KB_ACCENT2_FLAG = 0x2000;
        protected const int KB_ACCENT3_FLAG = 0x4000;
        protected const int KB_ACCENT4_FLAG = 0x8000;

        public const int KEY_A = 1;
        public const int KEY_B = 2;
        public const int KEY_C = 3;
        public const int KEY_D = 4;
        public const int KEY_E = 5;
        public const int KEY_F = 6;
        public const int KEY_G = 7;
        public const int KEY_H = 8;
        public const int KEY_I = 9;
        public const int KEY_J = 10;
        public const int KEY_K = 11;
        public const int KEY_L = 12;
        public const int KEY_M = 13;
        public const int KEY_N = 14;
        public const int KEY_O = 15;
        public const int KEY_P = 16;
        public const int KEY_Q = 17;
        public const int KEY_R = 18;
        public const int KEY_S = 19;
        public const int KEY_T = 20;
        public const int KEY_U = 21;
        public const int KEY_V = 22;
        public const int KEY_W = 23;
        public const int KEY_X = 24;
        public const int KEY_Y = 25;
        public const int KEY_Z = 26;
        public const int KEY_0 = 27;
        public const int KEY_1 = 28;
        public const int KEY_2 = 29;
        public const int KEY_3 = 30;
        public const int KEY_4 = 31;
        public const int KEY_5 = 32;
        public const int KEY_6 = 33;
        public const int KEY_7 = 34;
        public const int KEY_8 = 35;
        public const int KEY_9 = 36;
        public const int KEY_0_PAD = 37;
        public const int KEY_1_PAD = 38;
        public const int KEY_2_PAD = 39;
        public const int KEY_3_PAD = 40;
        public const int KEY_4_PAD = 41;
        public const int KEY_5_PAD = 42;
        public const int KEY_6_PAD = 43;
        public const int KEY_7_PAD = 44;
        public const int KEY_8_PAD = 45;
        public const int KEY_9_PAD = 46;
        public const int KEY_F1 = 47;
        public const int KEY_F2 = 48;
        public const int KEY_F3 = 49;
        public const int KEY_F4 = 50;
        public const int KEY_F5 = 51;
        public const int KEY_F6 = 52;
        public const int KEY_F7 = 53;
        public const int KEY_F8 = 54;
        public const int KEY_F9 = 55;
        public const int KEY_F10 = 56;
        public const int KEY_F11 = 57;
        public const int KEY_F12 = 58;
        public const int KEY_ESC = 59;
        public const int KEY_TILDE = 60;
        public const int KEY_MINUS = 61;
        public const int KEY_EQUALS = 62;
        public const int KEY_BACKSPACE = 63;
        public const int KEY_TAB = 64;
        public const int KEY_OPENBRACE = 65;
        public const int KEY_CLOSEBRACE = 66;
        public const int KEY_ENTER = 67;
        public const int KEY_COLON = 68;
        public const int KEY_QUOTE = 69;
        public const int KEY_BACKSLASH = 70;
        public const int KEY_BACKSLASH2 = 71;
        public const int KEY_COMMA = 72;
        public const int KEY_STOP = 73;
        public const int KEY_SLASH = 74;
        public const int KEY_SPACE = 75;
        public const int KEY_INSERT = 76;
        public const int KEY_DEL = 77;
        public const int KEY_HOME = 78;
        public const int KEY_END = 79;
        public const int KEY_PGUP = 80;
        public const int KEY_PGDN = 81;
        public const int KEY_LEFT = 82;
        public const int KEY_RIGHT = 83;
        public const int KEY_UP = 84;
        public const int KEY_DOWN = 85;
        public const int KEY_SLASH_PAD = 86;
        public const int KEY_ASTERISK = 87;
        public const int KEY_MINUS_PAD = 88;
        public const int KEY_PLUS_PAD = 89;
        public const int KEY_DEL_PAD = 90;
        public const int KEY_ENTER_PAD = 91;
        public const int KEY_PRTSCR = 92;
        public const int KEY_PAUSE = 93;
        public const int KEY_ABNT_C1 = 94;
        public const int KEY_YEN = 95;
        public const int KEY_KANA = 96;
        public const int KEY_CONVERT = 97;
        public const int KEY_NOCONVERT = 98;
        public const int KEY_AT = 99;
        public const int KEY_CIRCUMFLEX = 100;
        public const int KEY_COLON2 = 101;
        public const int KEY_KANJI = 102;
        public const int KEY_EQUALS_PAD = 103;  /* MacOS X */
        public const int KEY_BACKQUOTE = 104;  /* MacOS X */
        public const int KEY_SEMICOLON = 105;  /* MacOS X */
        public const int KEY_COMMAND = 106;  /* MacOS X */
        public const int KEY_UNKNOWN1 = 107;
        public const int KEY_UNKNOWN2 = 108;
        public const int KEY_UNKNOWN3 = 109;
        public const int KEY_UNKNOWN4 = 110;
        public const int KEY_UNKNOWN5 = 111;
        public const int KEY_UNKNOWN6 = 112;
        public const int KEY_UNKNOWN7 = 113;
        public const int KEY_UNKNOWN8 = 114;

        public const int KEY_MODIFIERS = 115;

        public const int KEY_LSHIFT = 115;
        public const int KEY_RSHIFT = 116;
        public const int KEY_LCONTROL = 117;
        public const int KEY_RCONTROL = 118;
        public const int KEY_ALT = 119;
        public const int KEY_ALTGR = 120;
        public const int KEY_LWIN = 121;
        public const int KEY_RWIN = 122;
        public const int KEY_MENU = 123;
        public const int KEY_SCRLOCK = 124;
        public const int KEY_NUMLOCK = 125;
        public const int KEY_CAPSLOCK = 126;

        public const int KEY_MAX = 127;

        protected const int SWITCH_NONE = 0;
        protected const int SWITCH_PAUSE = 1;
        protected const int SWITCH_AMNESIA = 2;
        protected const int SWITCH_BACKGROUND = 3;
        protected const int SWITCH_BACKAMNESIA = 4;

        protected const int SWITCH_IN = 0;
        protected const int SWITCH_OUT = 1;

        protected const int GFX_CAN_SCROLL = 0x00000001;
        protected const int GFX_CAN_TRIPLE_BUFFER = 0x00000002;
        protected const int GFX_HW_CURSOR = 0x00000004;
        protected const int GFX_HW_HLINE = 0x00000008;
        protected const int GFX_HW_HLINE_XOR = 0x00000010;
        protected const int GFX_HW_HLINE_SOLID_PATTERN = 0x00000020;
        protected const int GFX_HW_HLINE_COPY_PATTERN = 0x00000040;
        protected const int GFX_HW_FILL = 0x00000080;
        protected const int GFX_HW_FILL_XOR = 0x00000100;
        protected const int GFX_HW_FILL_SOLID_PATTERN = 0x00000200;
        protected const int GFX_HW_FILL_COPY_PATTERN = 0x00000400;
        protected const int GFX_HW_LINE = 0x00000800;
        protected const int GFX_HW_LINE_XOR = 0x00001000;
        protected const int GFX_HW_TRIANGLE = 0x00002000;
        protected const int GFX_HW_TRIANGLE_XOR = 0x00004000;
        protected const int GFX_HW_GLYPH = 0x00008000;
        protected const int GFX_HW_VRAM_BLIT = 0x00010000;
        protected const int GFX_HW_VRAM_BLIT_MASKED = 0x00020000;
        protected const int GFX_HW_MEM_BLIT = 0x00040000;
        protected const int GFX_HW_MEM_BLIT_MASKED = 0x00080000;
        protected const int GFX_HW_SYS_TO_VRAM_BLIT = 0x00100000;
        protected const int GFX_HW_SYS_TO_VRAM_BLIT_MASKED = 0x00200000;
        protected const int GFX_SYSTEM_CURSOR = 0x00400000;
        protected const int GFX_HW_VRAM_STRETCH_BLIT = 0x00800000;
        protected const int GFX_HW_VRAM_STRETCH_BLIT_MASKED = 0x01000000;
        protected const int GFX_HW_SYS_STRETCH_BLIT = 0x02000000;
        protected const int GFX_HW_SYS_STRETCH_BLIT_MASKED = 0x04000000;

        protected const int COLORCONV_NONE = 0;

        protected const int COLORCONV_8_TO_15 = 1;
        protected const int COLORCONV_8_TO_16 = 2;
        protected const int COLORCONV_8_TO_24 = 4;
        protected const int COLORCONV_8_TO_32 = 8;

        protected const int COLORCONV_15_TO_8 = 0x10;
        protected const int COLORCONV_15_TO_16 = 0x20;
        protected const int COLORCONV_15_TO_24 = 0x40;
        protected const int COLORCONV_15_TO_32 = 0x80;

        protected const int COLORCONV_16_TO_8 = 0x100;
        protected const int COLORCONV_16_TO_15 = 0x200;
        protected const int COLORCONV_16_TO_24 = 0x400;
        protected const int COLORCONV_16_TO_32 = 0x800;

        protected const int COLORCONV_24_TO_8 = 0x1000;
        protected const int COLORCONV_24_TO_15 = 0x2000;
        protected const int COLORCONV_24_TO_16 = 0x4000;
        protected const int COLORCONV_24_TO_32 = 0x8000;

        protected const int COLORCONV_32_TO_8 = 0x10000;
        protected const int COLORCONV_32_TO_15 = 0x20000;
        protected const int COLORCONV_32_TO_16 = 0x40000;
        protected const int COLORCONV_32_TO_24 = 0x80000;

        protected const int COLORCONV_32A_TO_8 = 0x100000;
        protected const int COLORCONV_32A_TO_15 = 0x200000;
        protected const int COLORCONV_32A_TO_16 = 0x400000;
        protected const int COLORCONV_32A_TO_24 = 0x800000;

        protected const int COLORCONV_DITHER_PAL = 0x1000000;
        protected const int COLORCONV_DITHER_HI = 0x2000000;
        protected const int COLORCONV_KEEP_TRANS = 0x4000000;

        protected const int COLORCONV_DITHER = (COLORCONV_DITHER_PAL | COLORCONV_DITHER_HI);

        protected const int COLORCONV_EXPAND_256 = (COLORCONV_8_TO_15 | COLORCONV_8_TO_16 | COLORCONV_8_TO_24 | COLORCONV_8_TO_32);

        protected const int COLORCONV_REDUCE_TO_256 = (COLORCONV_15_TO_8 | COLORCONV_16_TO_8 | COLORCONV_24_TO_8 | COLORCONV_32_TO_8 | COLORCONV_32A_TO_8);

        protected const int COLORCONV_EXPAND_15_TO_16 = COLORCONV_15_TO_16;

        protected const int COLORCONV_REDUCE_16_TO_15 = COLORCONV_16_TO_15;

        protected const int COLORCONV_EXPAND_HI_TO_TRUE = (COLORCONV_15_TO_24 | COLORCONV_15_TO_32 | COLORCONV_16_TO_24 | COLORCONV_16_TO_32);

        protected const int COLORCONV_REDUCE_TRUE_TO_HI = (COLORCONV_24_TO_15 | COLORCONV_24_TO_16 | COLORCONV_32_TO_15 | COLORCONV_32_TO_16);

        protected const int COLORCONV_24_EQUALS_32 = (COLORCONV_24_TO_32 | COLORCONV_32_TO_24);

        protected const int COLORCONV_TOTAL = (COLORCONV_EXPAND_256 | COLORCONV_REDUCE_TO_256 | COLORCONV_EXPAND_15_TO_16 | COLORCONV_REDUCE_16_TO_15 | COLORCONV_EXPAND_HI_TO_TRUE | COLORCONV_REDUCE_TRUE_TO_HI | COLORCONV_24_EQUALS_32 | COLORCONV_32A_TO_15 | COLORCONV_32A_TO_16 | COLORCONV_32A_TO_24);

        protected const int COLORCONV_PARTIAL = (COLORCONV_EXPAND_15_TO_16 | COLORCONV_REDUCE_16_TO_15 | COLORCONV_24_EQUALS_32);

        protected const int COLORCONV_MOST = (COLORCONV_EXPAND_15_TO_16 | COLORCONV_REDUCE_16_TO_15 | COLORCONV_EXPAND_HI_TO_TRUE | COLORCONV_REDUCE_TRUE_TO_HI | COLORCONV_24_EQUALS_32);

        protected const int COLORCONV_KEEP_ALPHA = (COLORCONV_TOTAL & ~(COLORCONV_32A_TO_8 | COLORCONV_32A_TO_15 | COLORCONV_32A_TO_16 | COLORCONV_32A_TO_24));

        /// <summary>
        /// flags for drawing_mode()
        /// </summary>
        protected const int DRAW_MODE_SOLID = 0;

        protected const int DRAW_MODE_XOR = 1;
        protected const int DRAW_MODE_COPY_PATTERN = 2;
        protected const int DRAW_MODE_SOLID_PATTERN = 3;
        protected const int DRAW_MODE_MASKED_PATTERN = 4;
        protected const int DRAW_MODE_TRANS = 5;

        protected const int OSTYPE_UNKNOWN = 0;
        protected static readonly int OSTYPE_WIN3 = AL_ID('W', 'I', 'N', '3');
        protected static readonly int OSTYPE_WIN95 = AL_ID('W', '9', '5', ' ');
        protected static readonly int OSTYPE_WIN98 = AL_ID('W', '9', '8', ' ');
        protected static readonly int OSTYPE_WINME = AL_ID('W', 'M', 'E', ' ');
        protected static readonly int OSTYPE_WINNT = AL_ID('W', 'N', 'T', ' ');
        protected static readonly int OSTYPE_WIN2000 = AL_ID('W', '2', 'K', ' ');
        protected static readonly int OSTYPE_WINXP = AL_ID('W', 'X', 'P', ' ');
        protected static readonly int OSTYPE_WIN2003 = AL_ID('W', '2', 'K', '3');
        protected static readonly int OSTYPE_WINVISTA = AL_ID('W', 'V', 'S', 'T');
        protected static readonly int OSTYPE_OS2 = AL_ID('O', 'S', '2', ' ');
        protected static readonly int OSTYPE_WARP = AL_ID('W', 'A', 'R', 'P');
        protected static readonly int OSTYPE_DOSEMU = AL_ID('D', 'E', 'M', 'U');
        protected static readonly int OSTYPE_OPENDOS = AL_ID('O', 'D', 'O', 'S');
        protected static readonly int OSTYPE_LINUX = AL_ID('T', 'U', 'X', ' ');
        protected static readonly int OSTYPE_SUNOS = AL_ID('S', 'U', 'N', ' ');
        protected static readonly int OSTYPE_FREEBSD = AL_ID('F', 'B', 'S', 'D');
        protected static readonly int OSTYPE_NETBSD = AL_ID('N', 'B', 'S', 'D');
        protected static readonly int OSTYPE_OPENBSD = AL_ID('O', 'B', 'S', 'D');
        protected static readonly int OSTYPE_IRIX = AL_ID('I', 'R', 'I', 'X');
        protected static readonly int OSTYPE_DARWIN = AL_ID('D', 'A', 'R', 'W');
        protected static readonly int OSTYPE_QNX = AL_ID('Q', 'N', 'X', ' ');
        protected static readonly int OSTYPE_UNIX = AL_ID('U', 'N', 'I', 'X');
        protected static readonly int OSTYPE_BEOS = AL_ID('B', 'E', 'O', 'S');
        protected static readonly int OSTYPE_MACOS = AL_ID('M', 'A', 'C', ' ');
        protected static readonly int OSTYPE_MACOSX = AL_ID('M', 'A', 'C', 'X');

        // Mouse cursors
        public const int MOUSE_CURSOR_NONE = 0;

        public const int MOUSE_CURSOR_ALLEGRO = 1;
        public const int MOUSE_CURSOR_ARROW = 2;
        public const int MOUSE_CURSOR_BUSY = 3;
        public const int MOUSE_CURSOR_QUESTION = 4;
        public const int MOUSE_CURSOR_EDIT = 5;
        public const int AL_NUM_MOUSE_CURSORS = 6;

        public const int TIMERS_PER_SECOND = 1193181;

        public const int DIGI_AUTODETECT = -1;       /* for passing to install_sound() */
        public const int DIGI_NONE = 0;

        public const int MIDI_AUTODETECT = -1;
        public const int MIDI_NONE = 0;
        public static readonly int MIDI_DIGMID = AL_ID('D', 'I', 'G', 'I');

        public const int POLYTYPE_FLAT = 0;
        public const int POLYTYPE_GCOL = 1;
        public const int POLYTYPE_GRGB = 2;
        public const int POLYTYPE_ATEX = 3;
        public const int POLYTYPE_PTEX = 4;
        public const int POLYTYPE_ATEX_MASK = 5;
        public const int POLYTYPE_PTEX_MASK = 6;
        public const int POLYTYPE_ATEX_LIT = 7;
        public const int POLYTYPE_PTEX_LIT = 8;
        public const int POLYTYPE_ATEX_MASK_LIT = 9;
        public const int POLYTYPE_PTEX_MASK_LIT = 10;
        public const int POLYTYPE_ATEX_TRANS = 11;
        public const int POLYTYPE_PTEX_TRANS = 12;
        public const int POLYTYPE_ATEX_MASK_TRANS = 13;
        public const int POLYTYPE_PTEX_MASK_TRANS = 14;
        public const int POLYTYPE_MAX = 15;
        public const int POLYTYPE_ZBUF = 16;

        /* bits for the flags field */
        public const int D_EXIT = 1;   /* object makes the dialog exit */
        public const int D_SELECTED = 2;   /* object is selected */
        public const int D_GOTFOCUS = 4;   /* object has the input focus */
        public const int D_GOTMOUSE = 8;   /* mouse is on top of object */
        public const int D_HIDDEN = 16;   /* object is not visible */
        public const int D_DISABLED = 32;   /* object is visible but inactive */
        public const int D_DIRTY = 64;   /* object needs to be redrawn */
        public const int D_INTERNAL = 128;   /* reserved for internal use */
        public const int D_USER = 256;   /* from here on is free for your own use */

        /* return values for the dialog procedures */
        public const int D_O_K = 0;  /* normal exit status */
        public const int D_CLOSE = 1;   /* request to close the dialog */
        public const int D_REDRAW = 2;    /* request to redraw the dialog */
        public const int D_REDRAWME = 4;     /* request to redraw this object */
        public const int D_WANTFOCUS = 8;      /* this object wants the input focus */
        public const int D_USED_CHAR = 16;       /* object has used the keypress */
        public const int D_REDRAW_ALL = 32;       /* request to redraw all active dialogs */
        public const int D_DONTWANTMOUSE = 64;       /* this object does not want mouse focus */

        /* messages for the dialog procedures */
        public const int MSG_START = 1;  /* start the dialog, initialise */
        public const int MSG_END = 2;  /* dialog is finished - cleanup */
        public const int MSG_DRAW = 3;  /* draw the object */
        public const int MSG_CLICK = 4;   /* mouse click on the object */
        public const int MSG_DCLICK = 5;    /* double click on the object */
        public const int MSG_KEY = 6;     /* keyboard shortcut */
        public const int MSG_CHAR = 7;      /* other keyboard input */
        public const int MSG_UCHAR = 8;       /* unicode keyboard input */
        public const int MSG_XCHAR = 9;       /* broadcast character to all objects */
        public const int MSG_WANTFOCUS = 10;       /* does object want the input focus? */
        public const int MSG_GOTFOCUS = 11;       /* got the input focus */
        public const int MSG_LOSTFOCUS = 12;       /* lost the input focus */
        public const int MSG_GOTMOUSE = 13;       /* mouse on top of object */
        public const int MSG_LOSTMOUSE = 14;       /* mouse moved away from object */
        public const int MSG_IDLE = 15;       /* update any background stuff */
        public const int MSG_RADIO = 16;       /* clear radio buttons */
        public const int MSG_WHEEL = 17;       /* mouse wheel moved */
        public const int MSG_LPRESS = 18;       /* mouse left button pressed */
        public const int MSG_LRELEASE = 19;       /* mouse left button released */
        public const int MSG_MPRESS = 20;       /* mouse middle button pressed */
        public const int MSG_MRELEASE = 21;       /* mouse middle button released */
        public const int MSG_RPRESS = 22;       /* mouse right button pressed */
        public const int MSG_RRELEASE = 23;       /* mouse right button released */
        public const int MSG_WANTMOUSE = 24;       /* does object want the mouse? */
        public const int MSG_USER = 25;       /* from here on are free... */

        private const int QUAT_SHORT = 0;
        private const int QUAT_LONG = 1;
        private const int QUAT_CW = 2;
        private const int QUAT_CCW = 3;
        private const int QUAT_USER = 4;

        public const int JOY_TYPE_AUTODETECT = -1;
        public const int JOY_TYPE_NONE = 0;

        public const int MAX_JOYSTICKS = 8;
        public const int MAX_JOYSTICK_AXIS = 3;
        public const int MAX_JOYSTICK_STICKS = 5;
        public const int MAX_JOYSTICK_BUTTONS = 32;

        /* joystick status flags */
        public const int JOYFLAG_DIGITAL = 1;
        public const int JOYFLAG_ANALOGUE = 2;
        public const int JOYFLAG_CALIB_DIGITAL = 4;
        public const int JOYFLAG_CALIB_ANALOGUE = 8;
        public const int JOYFLAG_CALIBRATE = 16;
        public const int JOYFLAG_SIGNED = 32;
        public const int JOYFLAG_UNSIGNED = 64;
        /* alternative spellings */
        public const int JOYFLAG_ANALOG = JOYFLAG_ANALOGUE;
        public const int JOYFLAG_CALIB_ANALOG = JOYFLAG_CALIB_ANALOGUE;

        #endregion Constants

        #region Magic region

        public const int INT_MAX = 2147483647;
        public const int EOF = 0;

        public const int FALSE = 0;
        public const int TRUE = 1;
        public static readonly IntPtr NULL = IntPtr.Zero;
        private static Random random = new Random();

        public const int _AL_RAND_MAX = 0xFFFF;

        public delegate int CatExitPtr();

        public delegate void CloseButtonCallback();

        public delegate int IntGetter(string name, int def);

        public delegate string StringGetter(string name, string def);

        public delegate void StringSetter(string name, string value);

        public delegate void MouseCallback(int flags);

        public delegate void TimerHandler();

        public delegate void ParamTimerHandler(IntPtr p);

        public delegate void RestCallback();

        public delegate int KeyPressedCallback();

        public delegate int ReadKeyCallback();

        public delegate int KeyboardCallback(int key);

        public delegate int KeyboardUCallback(int key, ref int scancode);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void KeyboardLowLevelCallback(int scancode);

        public delegate void DisplaySwitchCallback();

        public delegate IntPtr Load(string filename, RGB[] pal);

        public delegate int Save(string filename, IntPtr bmp, RGB[] pal);

        public delegate void LineCallback(IntPtr bmp, int x, int y, int d);

        public delegate void CircleCallback(IntPtr bmp, int x, int y, int d);

        public delegate void EllipseCallback(IntPtr bmp, int x, int y, int d);

        public delegate void ArcCallback(IntPtr bmp, int x, int y, int d);

        public delegate IntPtr SampleLoadCallback(string filename);

        public delegate int SampleSaveCallback(string filename, IntPtr spl);

        public delegate void MidiMsgCallback(int msg, int byte1, int byte2);

        public delegate void MidiMetaCallback(int type, string data, int length);

        public delegate void MidiSysexCallback(string data, int length);

        public delegate void DatafileCallback(IntPtr d);

        public delegate void DatafileLoadCallback(IntPtr f, long size);

        public delegate void DatafileDestroyCallback(IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void BlendCallback(IntPtr pal, int x, int y, IntPtr rgb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FLICCallback();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int GuiMouseCallback();

        public static int ABS(int x)
        {
            return (((x) >= 0) ? (x) : (-(x)));
        }

        public static int AL_RAND()
        {
            return random.Next(_AL_RAND_MAX);
        }

        public class PALETTE_COLOR : ManagedPointer
        {
            public PALETTE_COLOR(IntPtr pointer)
                : base(pointer)
            {
            }

            public int this[int index]
            {
                get
                {
                    return ReadInt(index * sizeof(Int32));
                }
            }
        }

        public static PALETTE_COLOR palette_color
        {
            get
            {
                return new PALETTE_COLOR(Marshal.ReadIntPtr(GetAddress("palette_color")));
            }
        }

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpszLib);

        public static IntPtr GetAddress(string name)
        {
            IntPtr handle = LoadLibrary(ALLEG_DLL);
            if (handle != IntPtr.Zero)
            {
                IntPtr address = GetProcAddress(handle, name);
                if (address != IntPtr.Zero)
                {
                    return address;
                }
            }
            return IntPtr.Zero;
        }

        public static GFX_DRIVER gfx_driver
        {
            get
            {
                return new GFX_DRIVER(Marshal.ReadIntPtr(GetAddress("gfx_driver")));
            }
        }

        public static KEYBOARD_DRIVER keyboard_driver
        {
            get
            {
                return new KEYBOARD_DRIVER(Marshal.ReadIntPtr(GetAddress("keyboard_driver")));
            }
        }

        public static JOYSTICK_DRIVER joystick_driver
        {
            get
            {
                return new JOYSTICK_DRIVER(Marshal.ReadIntPtr(GetAddress("joystick_driver")));
            }
        }

        public static MOUSE_DRIVER mouse_driver
        {
            get
            {
                return new MOUSE_DRIVER(Marshal.ReadIntPtr(GetAddress("mouse_driver")));
            }
        }

        public static TIMER_DRIVER timer_driver
        {
            get
            {
                return new TIMER_DRIVER(Marshal.ReadIntPtr(GetAddress("timer_driver")));
            }
        }

        public static DIGI_DRIVER digi_driver
        {
            get
            {
                return new DIGI_DRIVER(Marshal.ReadIntPtr(GetAddress("digi_driver")));
            }
        }

        public static MIDI_DRIVER midi_driver
        {
            get
            {
                return new MIDI_DRIVER(Marshal.ReadIntPtr(GetAddress("midi_driver")));
            }
        }

        public static int MIN(int x, int y)
        {
            return (((x) < (y)) ? (x) : (y));
        }

        public static int MAX(int x, int y)
        {
            return (((x) > (y)) ? (x) : (y));
        }

        public static int MID(int x, int y, int z)
        {
            return MAX((x), MIN((y), (z)));
        }

        public static int digi_card
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("digi_card"));
            }
        }

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void create_rgb_table(IntPtr table, IntPtr pal, IntPtr callback);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void create_light_table(IntPtr table, IntPtr pal, int r, int g, int b, IntPtr callback);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void create_trans_table(IntPtr table, IntPtr pal, int r, int g, int b, IntPtr callback);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void create_color_table(IntPtr table, IntPtr pal, IntPtr blend, IntPtr callback);

        //AL_FUNC(void, create_blender_table, (COLOR_MAP *table, AL_CONST PALETTE pal, AL_METHOD(void, callback, (int pos))));

        public static RGB_MAP rgb_map
        {
            set
            {
                Marshal.WriteInt32(GetAddress("rgb_map"), value.pointer.ToInt32());
            }
        }

        public static COLOR_MAP color_map
        {
            get
            {
                return new COLOR_MAP(GetAddress("color_map"));
            }

            set
            {
                Marshal.WriteIntPtr(GetAddress("color_map"), value);
            }
        }

        public static readonly int U_ASCII = AL_ID('A', 'S', 'C', '8');
        public static readonly int U_ASCII_CP = AL_ID('A', 'S', 'C', 'P');
        public static readonly int U_UNICODE = AL_ID('U', 'N', 'I', 'C');
        public static readonly int U_UTF8 = AL_ID('U', 'T', 'F', '8');
        public static readonly int U_CURRENT = AL_ID('c', 'u', 'r', '.');

        public static void bmp_write8(int addr, byte c)
        {
            Marshal.WriteByte(new IntPtr(addr), c);
            //(*((uint8_t  *)(addr)) = (c));
        }

        public static void bmp_write32(uint addr, int c)
        {
            Marshal.WriteInt32(new IntPtr(addr), c);
            //(*((uint *)(addr)) = (c));
        }

        public static void bmp_select(IntPtr bmp)
        {
            // TODO: implement this one if necessary
        }

        public static byte bmp_read8(int addr)
        {
            return Marshal.ReadByte(new IntPtr(addr));
            //return (*((byte*)(addr)));
        }

        public static int bmp_read32(int addr)
        {
            return Marshal.ReadInt32(new IntPtr(addr));
            //return (*((uint*)(addr)));
        }

        public const string ALLEGRO_PLATFORM_STR = "MSVC";

        public const string EMPTY_STRING = "\0\0\0";

        public struct _DRIVER_INFO         /* info about a hardware driver */
        {
            public int id;                          /* integer ID */
            public IntPtr driver;                   /* the driver structure */
            public int autodetect;                  /* set to allow autodetection */

            public _DRIVER_INFO(int id, IntPtr driver, int autodetect)
            {
                this.id = id;
                this.driver = driver;
                this.autodetect = autodetect;
            }
        }

        public static SYSTEM_DRIVER system_driver
        {
            get
            {
                return new SYSTEM_DRIVER(Marshal.ReadIntPtr(GetAddress("system_driver")));
            }
        }

        public static _DRIVER_INFO[] _gfx_driver_list
        {
            get
            {
                return get_drivers(GetAddress("_gfx_driver_list"));
            }
        }

        public static _DRIVER_INFO[] _digi_driver_list
        {
            get
            {
                return get_drivers(GetAddress("_digi_driver_list"));
            }
        }

        public static _DRIVER_INFO[] _midi_driver_list
        {
            get
            {
                return get_drivers(GetAddress("_midi_driver_list"));
            }
        }

        public static _DRIVER_INFO[] get_drivers(IntPtr drivers)
        {
            int size = 0;
            ManagedPointer pointer = new ManagedPointer(drivers);
            while (pointer.ReadPointer(sizeof(Int32)) != AllegroAPI.NULL)
            {
                size++;
                pointer = pointer.Offset(3 * sizeof(Int32));
            }
            AllegroAPI._DRIVER_INFO[] ret = new AllegroAPI._DRIVER_INFO[size + 1];
            pointer = new ManagedPointer(drivers);
            for (int i = 0; i < size + 1; i++)
            {
                ret[i].id = pointer.ReadInt(0);
                ret[i].driver = pointer.ReadPointer(sizeof(Int32));
                ret[i].autodetect = pointer.ReadInt(2 * sizeof(Int32));
                pointer = pointer.Offset(3 * sizeof(Int32));
            }
            return ret;
        }

        #endregion Magic region

        #region Using Allegro

        /*
         * install_allegro - Initialise the Allegro library.
         * allegro_init - Macro to initialise the Allegro library.
         * allegro_exit - Closes down the Allegro system.
         * END_OF_MAIN - Macro to put after your main() function.
         * allegro_id - String containing date and version number of Allegro.
         * allegro_error - Stores the last Allegro error message.
         * ALLEGRO_VERSION - Defined to the major version of Allegro.
         * ALLEGRO_SUB_VERSION - Defined to the middle version of Allegro.
         * ALLEGRO_WIP_VERSION - Defined to the minor version of Allegro.
         * ALLEGRO_VERSION_STR - Defined to a string with the full Allegro version number.
         * ALLEGRO_DATE_STR - Defined to a string with the year Allegro was released.
         * ALLEGRO_DATE - Defined to a number with the release date of Allegro.
         * AL_ID - Converts four 8 bit values to a packed 32 bit integer ID.
         * MAKE_VERSION - Create a 32 bit integer from the Allegro version
         * os_type - Stores the detected type of the OS.
         * os_version
         * os_revision - Version of the OS currently running.
         * os_multitasking - Indicates if the OS is multitasking.
         * allegro_message - Used mainly to show error messages to users.
         * set_window_title - Sets the window title of the Allegro program.
         * set_close_button_callback - Handles the user clicking on the close button of the window.
         * desktop_color_depth - Finds out the desktop color depth.
         * get_desktop_resolution - Finds out the desktop resolution.
         * check_cpu - Detects the CPU type.
         * cpu_vendor - Contains the CPU vendor name.
         * cpu_family - Contains the CPU type.
         * cpu_model - Contains the Intel CPU submodel.
         * cpu_capabilities - Contains the capability flags of the CPU.
         */

        /// <summary>
        /// Initialise the Allegro library.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_allegro(int system_id, ref int errno_ptr, CatExitPtr atexit_ptr);

        /// <summary>
        /// Macro to initialise the Allegro library.
        /// </summary>
        public static int allegro_init()
        {
            int errno = 0;
            return install_allegro(SYSTEM_AUTODETECT, ref errno, null);
        }

        /// <summary>
        /// Closes down the Allegro system.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void allegro_exit();

        /// <summary>
        /// String containing date and version number of Allegro.
        /// </summary>
        public static string allegro_id
        {
            get
            {
                return Marshal.PtrToStringAnsi(GetAddress("allegro_id"));
            }
        }

        /// <summary>
        /// Stores the last Allegro error message.
        /// </summary>
        public static string allegro_error
        {
            get
            {
                return Marshal.PtrToStringAnsi(GetAddress("allegro_error"));
            }
        }

        /// <summary>
        /// Defined to the major version of Allegro.
        /// </summary>
        public static string ALLEGRO_VERSION
        {
            get
            {
                Regex regex = new Regex(@"Allegro (?<major>\d*)\.(?<middle>\d*)\.(?<minor>\d*), ([\w])*");
                return regex.Match(allegro_id).Groups["major"].ToString();
            }
        }

        /// <summary>
        /// Defined to the middle version of Allegro.
        /// </summary>
        public static string ALLEGRO_SUB_VERSION
        {
            get
            {
                Regex regex = new Regex(@"Allegro (?<major>\d*)\.(?<middle>\d*)\.(?<minor>\d*), ([\w])*");
                return regex.Match(allegro_id).Groups["middle"].ToString();
            }
        }

        /// <summary>
        /// Defined to the minor version of Allegro.
        /// </summary>
        public static string ALLEGRO_WIP_VERSION
        {
            get
            {
                Regex regex = new Regex(@"Allegro (?<major>\d*)\.(?<middle>\d*)\.(?<minor>\d*), ([\w])*");
                return regex.Match(allegro_id).Groups["minor"].ToString();
            }
        }

        /// <summary>
        /// Defined to a string with the full Allegro version number.
        /// </summary>
        public static string ALLEGRO_VERSION_STR
        {
            get
            {
                // TODO: take into account additional text, i.e. `4.1.16 (CVS)'
                Regex regex = new Regex(@"Allegro (?<version>.*), ([\w])*");
                return regex.Match(allegro_id).Groups["version"].ToString();
            }
        }

        /// <summary>
        /// Defined to a string with the year Allegro was released.
        /// </summary>
        public static string ALLEGRO_DATE_STR
        {
            get
            {
                return ALLEGRO_DATE.Substring(0, 4);
            }
        }

        /// <summary>
        /// Defined to a number with the release date of Allegro.
        /// </summary>
        public static string ALLEGRO_DATE
        {
            get
            {
                // TODO: gather this info
                return "20070722";
            }
        }

        /// <summary>
        /// Converts four 8 bit values to a packed 32 bit integer ID.
        /// </summary>
        public static int AL_ID(char a, char b, char c, char d)
        {
            return (((a) << 24) | ((b) << 16) | ((c) << 8) | (d));
        }

        /// <summary>
        /// Create a 32 bit integer from the Allegro version.
        /// </summary>
        public static int MAKE_VERSION(int a, int b, int c)
        {
            return (((a) << 16) | ((b) << 8) | (c));
        }

        /// <summary>
        /// Stores the detected type of the OS.
        /// </summary>
        public static int os_type
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("os_type"));
            }
        }

        public static int os_version
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("os_version"));
            }
        }

        /// <summary>
        /// Version of the OS currently running.
        /// </summary>
        public static int os_revision
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("os_revision"));
            }
        }

        /// <summary>
        /// Indicates if the OS is multitasking.
        /// </summary>
        public static int os_multitasking
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("os_multitasking"));
            }
        }

        /// <summary>
        /// Used mainly to show error messages to users.
        /// </summary>
        public static void allegro_message(string text_format)
        {
            MessageBox.Show(text_format, "allegro");
        }

        /// <summary>
        /// Sets the window title of the Allegro program.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void set_window_title(string name);

        /// <summary>
        /// Handles the user clicking on the close button of the window.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_close_button_callback(CloseButtonCallback proc);

        /// <summary>
        /// Finds out the desktop color depth.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int desktop_color_depth();

        /// <summary>
        /// Finds out the desktop resolution.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int get_desktop_resolution(out int width, out int height);

        /// <summary>
        /// Detects the CPU type.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void check_cpu();

        /// <summary>
        /// Contains the CPU vendor name.
        /// </summary>
        public static string cpu_vendor
        {
            get
            {
                return Marshal.PtrToStringAnsi(GetAddress("cpu_vendor"));
            }
        }

        /// <summary>
        /// Contains the CPU type.
        /// </summary>
        public static int cpu_family
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("cpu_family"));
            }
        }

        /// <summary>
        /// Contains the Intel CPU submodel.
        /// </summary>
        public static int cpu_model
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("cpu_model"));
            }
        }

        /// <summary>
        /// Contains the capability flags of the CPU.
        /// </summary>
        public static int cpu_capabilities
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("cpu_capabilities"));
            }
        }

        #endregion Using Allegro

        #region Structures and types defined by Allegro

        /// <summary>
        /// Stores an array of GFX_MODE structures.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct GFX_MODE_LIST
        {
            public int num_modes;
            public GFX_MODE* mode;

            // Saves the original unmanaged memory address
            public IntPtr p;
        }

        /// <summary>
        /// Stores video mode information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct GFX_MODE
        {
            public int width, height, bpp;
        }

        /// <summary>
        /// Number of entries in a palette.
        /// </summary>
        public const int PAL_SIZE = 256;

        /// <summary>
        /// Fixed point vertex structure used by 3d functions.
        /// </summary>
        public struct V3D                  /* a 3d point (fixed point version) */
        {
            public V3D(int x, int y, int z, int u, int v, int c)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.u = u;
                this.v = v;
                this.c = c;
            }

            public int x, y, z;                   /* position */
            public int u, v;                      /* texture map coordinates */
            public int c;                           /* color */
        }

        /// <summary>
        /// Floating point vertex structure used by 3d functions.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct V3D_f                /* a 3d point (floating point version) */
        {
            public V3D_f(float x, float y, float z, float u, float v, int c)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.u = u;
                this.v = v;
                this.c = c;
            }

            public float x, y, z;                   /* position */
            public float u, v;                      /* texture map coordinates */
            public int c;                           /* color */
        }

        /// <summary>
        /// Fixed point matrix structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct MATRIX            /* transformation matrix (fixed point) */
        {
            public fixed int v[9];                /* scaling and rotation */
            public fixed int t[3];                   /* translation */
        }

        /// <summary>
        /// Floating point matrix structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct MATRIX_f          /* transformation matrix (floating point) */
        {
            // TODO: replace with bidimensional array
            public fixed float v[9];                /* scaling and rotation */

            public fixed float t[3];                   /* translation */
        }

        /// <summary>
        /// Stores quaternion information.
        /// </summary>
        public struct QUAT
        {
            public float w, x, y, z;
        }

        //public struct DIALOG
        //{
        //  public DIALOG(DIALOG_PROC proc, int x, int y, int w, int h, int fg, int bg, int key, int flags, int d1, int d2, IntPtr dp, IntPtr dp2, IntPtr dp3)
        //  {
        //    this.proc = proc;
        //    this.x = x;
        //    this.y = y;
        //    this.w = w;
        //    this.h = h;
        //    this.fg = fg;
        //    this.bg = bg;
        //    this.key = key;
        //    this.flags = flags;
        //    this.d1 = d1;
        //    this.d2 = d2;
        //    this.dp = dp;
        //    this.dp2 = dp2;
        //    this.dp3 = dp3;
        //  }

        //  public DIALOG_PROC proc;
        //  public int x, y, w, h;               /* position and size of the object */
        //  public int fg, bg;                   /* foreground and background colors */
        //  public int key;                      /* keyboard shortcut (ASCII code) */
        //  public int flags;                    /* flags about the object state */
        //  public int d1, d2;                   /* any data the object might require */
        //  public IntPtr dp, dp2, dp3;          /* pointers to more object data */
        //}

        #endregion Structures and types defined by Allegro

        #region Unicode routines

        /* set_uformat - Set the global current text encoding format.
     * get_uformat - Finds out what text encoding format is currently selected.
     * register_uformat - Installs handler functions for a new text encoding format.
     * set_ucodepage - Sets 8-bit to Unicode conversion tables.
     * need_uconvert - Tells if a string requires encoding conversion.
     * uconvert_size - Number of bytes needed to store a string after conversion.
     * do_uconvert - Converts a string to another encoding format.
     * uconvert - High level string encoding conversion wrapper.
     * uconvert_ascii - Converts string from ASCII into the current format.
     * uconvert_toascii - Converts strings from the current format into ASCII.
     * empty_string - Universal string NULL terminator.
     * ugetc - Low level helper function for reading Unicode text data.
     * ugetx
     * ugetxc - Low level helper function for reading Unicode text data.
     * usetc - Low level helper function for writing Unicode text data.
     * uwidth - Low level helper function for testing Unicode text data.
     * ucwidth - Low level helper function for testing Unicode text data.
     * uisok - Low level helper function for testing Unicode text data.
     * uoffset - Finds the offset of a character in a string.
     * ugetat - Finds out the value of a character in a string.
     * usetat - Replaces a character in a string.
     * uinsert - Inserts a character in a string.
     * uremove - Removes a character from a string.
     * ustrsize - Size of the string in bytes without null terminator.
     * ustrsizez - Size of the string in bytes including null terminator.
     * uwidth_max - Number of bytes a character can occupy.
     * utolower - Converts a letter to lower case.
     * utoupper - Converts a letter to upper case.
     * uisspace - Tells if a character is whitespace.
     * uisdigit - Tells if a character is a digit.
     * ustrdup - Duplicates a string.
     * _ustrdup - Duplicates a string with a custom memory allocator.
     * ustrcpy - Copies a string into another one.
     * ustrzcpy - Copies a string into another one, specifying size.
     * ustrcat - Concatenates a string to another one.
     * ustrzcat - Concatenates a string to another one, specifying size.
     * ustrlen - Tells the number of characters in a string.
     * ustrcmp - Compares two strings.
     * ustrncpy - Copies a string into another one, specifying size.
     * ustrzncpy - Copies a string into another one, specifying size.
     * ustrncat - Concatenates a string to another one, specifying size.
     * ustrzncat - Concatenates a string to another one, specifying size.
     * ustrncmp - Compares up to n letters of two strings.
     * ustricmp - Compares two strings ignoring case.
     * ustrnicmp - Compares up to n letters of two strings ignoring case.
     * ustrlwr - Replaces all letters with lower case.
     * ustrupr - Replaces all letters with upper case.
     * ustrchr - Finds the first occurrence of a character in a string.
     * ustrrchr - Finds the last occurence of a character in a string.
     * ustrstr - Finds the first occurence of a string in another one.
     * ustrpbrk - Finds the first character that matches any in a set.
     * ustrtok - Retrieves tokens from a string.
     * ustrtok_r - Reentrant function to retrieve tokens from a string.
     * uatof - Converts a string into a double.
     * ustrtol - Converts a string into an integer.
     * ustrtod - Converts a string into a floating point number.
     * ustrerror - Returns a string describing errno.
     * usprintf - Writes formatted data into a buffer.
     * uszprintf - Writes formatted data into a buffer, specifying size.
     * uvsprintf - Writes formatted data into a buffer, using variable arguments.
     * uvszprintf - Writes formatted data into a buffer, using size and variable arguments.
     */

        /// <summary>
        /// Set the global current text encoding format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_uformat(int type);

        /// <summary>
        /// High level string encoding conversion wrapper.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string uconvert(string s, int type, StringBuilder buf, int newtype, int size);

        /// <summary>
        /// Converts string from ASCII into the current format.
        /// </summary>
        public static string uconvert_ascii(string s, byte[] buf)
        {
            return uconvert(s, U_ASCII, new StringBuilder(buf.Length), U_CURRENT, buf.Length);
        }

        public static string uconvert_ascii(string s, char[] buf)
        {
            return uconvert(s, U_ASCII, new StringBuilder(new string(buf)), U_CURRENT, buf.Length);
        }

        /// <summary>
        /// Size of the string in bytes without null terminator.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int ustrsize(string s);
        // TODO: restore original implementation
        public static int ustrsize(string s)
        {
            return Encoding.ASCII.GetByteCount(s.ToCharArray()) - 2;
        }

        /// <summary>
        /// Size of the string in bytes including null terminator.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int ustrsizez(string s);
        public static int ustrsizez(string s)
        {
            return Encoding.ASCII.GetByteCount(s.ToCharArray());
        }

        /// <summary>
        /// Copies a string into another one.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string ustrzcpy(StringBuilder dest, int size, string src);

        public static string ustrcpy(StringBuilder dest, string src)
        {
            return ustrzcpy(dest, INT_MAX, src);
        }

        /// <summary>
        /// Concatenates a string to another one.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string ustrzcat(StringBuilder dest, int size, string src);

        public static string ustrcat(StringBuilder dest, string src)
        {
            return ustrzcat(dest, INT_MAX, src);
        }

        #endregion Unicode routines

        #region Configuration routines

        /* set_config_file - Sets the configuration file.
         * set_config_data - Sets a block of configuration data.
         * override_config_file - Specifies a file containing config overrides.
         * override_config_data - Specifies a block of data containing config overrides.
         * push_config_state - Pushes the current configuration state.
         * pop_config_state - Pops a previously pushed configuration state.
         * flush_config_file - Flushes the current config file to disk.
         * reload_config_texts - Reloads translated strings returned by get_config_text().
         * hook_config_section - Hooks a configuration file section with custom handlers.
         * config_is_hooked - Tells if a config section has custom hooks.
         * get_config_string - Retrieves a string from the configuration file.
         * get_config_int - Retrieves an integer from the configuration file.
         * get_config_hex - Retrieves a hexadecimal value from the configuration file.
         * get_config_float - Retrieves a float from the configuration file.
         * get_config_id - Retrieves a driver ID from a configuration file.
         * get_config_argv - Reads a token list from the configuration file.
         * get_config_text - Returns a string translated to the current language.
         * set_config_string - Writes a string in the configuration file.
         * set_config_int - Writes an integer in the configuration file.
         * set_config_hex - Writes a hexadecimal integer in the configuration file.
         * set_config_float - Writes a float in the configuration file.
         * set_config_id - Writes a driver ID in the configuration file.
         * list_config_entries - Lists the names of all entries in a config section
         * list_config_sections - Lists the names of all sections available in the current configuration.
         * free_config_entries - Frees memory allocated for config entry lists.
         */

        /// <summary>
        /// Sets the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_file(string filename);

        /// <summary>
        /// Sets a block of configuration data.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_data(string data, int length);

        /// <summary>
        /// Specifies a file containing config overrides.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void override_config_file(string filename);

        /// <summary>
        /// Specifies a block of data containing config overrides.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void override_config_data(string data, int length);

        /// <summary>
        /// Pushes the current configuration state.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void push_config_state();

        /// <summary>
        /// Pops a previously pushed configuration state.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pop_config_state();

        /// <summary>
        /// Flushes the current config file to disk.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void flush_config_file();

        /// <summary>
        /// Reloads translated strings returned by get_config_text().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void reload_config_texts(string new_language);

        /// <summary>
        /// Hooks a configuration file section with custom handlers.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void hook_config_section(string section, IntGetter intgetter, StringGetter stringgetter, StringSetter stringsetter);

        /// <summary>
        /// Tells if a config section has custom hooks.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int config_is_hooked(string section);

        /// <summary>
        /// Retrieves a string from the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string get_config_string(string section, string name, string def);

        /// <summary>
        /// Retrieves an integer from the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_config_int(string section, string name, int def);

        /// <summary>
        /// Retrieves a hexadecimal value from the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_config_hex(string section, string name, int def);

        /// <summary>
        /// Retrieves a float from the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern float get_config_float(string section, string name, float def);

        /// <summary>
        /// Retrieves a driver ID from a configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_config_id(string section, string name, int def);

        /// <summary>
        /// Reads a token list from the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr get_config_argv(string section, string name, ref int argc);

        /// <summary>
        /// Returns a string translated to the current language.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string get_config_text(string msg);

        /// <summary>
        /// Writes a string in the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_string(string section, string name, string val);

        /// <summary>
        /// Writes an integer in the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_int(string section, string name, int val);

        /// <summary>
        /// Writes a hexadecimal integer in the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_hex(string section, string name, int val);

        /// <summary>
        /// Writes a float in the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_float(string section, string name, float val);

        /// <summary>
        /// Writes a driver ID in the configuration file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_config_id(string section, string name, int val);

        /// <summary>
        /// Lists the names of all entries in a config section.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int list_config_entries(string section, string[] names);

        /// <summary>
        /// Lists the names of all sections available in the current configuration.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int list_config_sections(string[] names);

        /// <summary>
        /// Frees memory allocated for config entry lists.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int free_config_entries(string[] names);

        #endregion Configuration routines

        #region Mouse routines

        /* install_mouse - Installs the Allegro mouse handler.
         * remove_mouse - Removes the mouse handler.
         * poll_mouse - Polls the mouse.
         * mouse_needs_poll - Tells if the mouse driver requires polling.
         * enable_hardware_cursor - Enables the OS hardware cursor.
         * disable_hardware_cursor - Disables the OS hardware cursor.
         * select_mouse_cursor - Tells Allegro to select software or hardware cursor drawing.
         * set_mouse_cursor_bitmap - Changes the image Allegro uses for mouse cursors.
         * mouse_x
         * mouse_y
         * mouse_z
         * mouse_b
         * mouse_pos - Global variable with the mouse position/button state.
         * mouse_sprite
         * mouse_x_focus
         * mouse_y_focus - Global variable with the mouse sprite and focus point.
         * show_mouse - Tells Allegro to display a mouse pointer on the screen.
         * scare_mouse - Helper for hiding the mouse pointer before drawing.
         * scare_mouse_area - Helper for hiding the mouse cursor before drawing in an area.
         * unscare_mouse - Undoes the effect of scare_mouse() or scare_mouse_area().
         * show_os_cursor - Low level function to display the operating system cursor.
         * freeze_mouse_flag - Flag to avoid redrawing the mouse pointer.
         * position_mouse - Moves the mouse to the specified screen position.
         * position_mouse_z - Sets the mouse wheel position global variable.
         * set_mouse_range - Sets the area of the screen restricting mouse movement.
         * set_mouse_speed - Sets the mouse speed.
         * set_mouse_sprite - Sets the mouse sprite.
         * set_mouse_sprite_focus - Sets the mouse sprite focus.
         * get_mouse_mickeys - How far the mouse has moved since the last call to this function.
         * mouse_callback - User specified mouse callback.
         */

        /// <summary>
        /// Installs the Allegro mouse handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_mouse();

        /// <summary>
        /// Removes the mouse handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_mouse();

        /// <summary>
        /// Polls the mouse.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int poll_mouse();

        /// <summary>
        /// Tells if the mouse driver requires polling.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int mouse_needs_poll();

        /// <summary>
        /// Enables the OS hardware cursor.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void enable_hardware_cursor();

        /// <summary>
        /// Disables the OS hardware cursor.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void disable_hardware_cursor();

        /// <summary>
        /// Tells Allegro to select software or hardware cursor drawing.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void select_mouse_cursor(int cursor);

        /// <summary>
        /// Changes the image Allegro uses for mouse cursors.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mouse_cursor_bitmap(int cursor, IntPtr bmp);

        public static int mouse_x
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_x"));
            }
        }

        public static int mouse_y
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_y"));
            }
        }

        public static int mouse_z
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_z"));
            }
        }

        public static int mouse_w
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_w"));
            }
        }

        public static int mouse_b
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_b"));
            }
        }

        /// <summary>
        /// Global variable with the mouse position/button state.
        /// </summary>
        public static int mouse_pos
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_pos"));
            }
        }

        /*public static IntPtr mouse_sprite
        {
          get
          {
            return new IntPtr(Marshal.ReadInt32(GetAddress("mouse_sprite")));
          }

          set
          {
            set_mouse_sprite(value);
          }
        }*/

        public static int mouse_x_focus
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_x_focus"));
            }
        }

        public static int mouse_y_focus
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("mouse_y_focus"));
            }
        }

        /// <summary>
        /// Tells Allegro to display a mouse pointer on the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void show_mouse(IntPtr bmp);

        /// <summary>
        /// Helper for hiding the mouse pointer before drawing.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void scare_mouse();

        /// <summary>
        /// Helper for hiding the mouse cursor before drawing in an area.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void scare_mouse_area(int x, int y, int w, int h);

        /// <summary>
        /// Undoes the effect of scare_mouse() or scare_mouse_area().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unscare_mouse();

        /// <summary>
        /// Low level function to display the operating system cursor.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int show_os_cursor(int cursor);

        /// <summary>
        /// Flag to avoid redrawing the mouse pointer.
        /// </summary>
        public static int freeze_mouse_flag
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("freeze_mouse_flag"));
            }
        }

        /// <summary>
        /// Moves the mouse to the specified screen position.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void position_mouse(int x, int y);

        /// <summary>
        /// Sets the mouse wheel position global variable.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void position_mouse_z(int z);

        /// <summary>
        /// Sets the area of the screen restricting mouse movement.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mouse_range(int x1, int y1, int x2, int y2);

        /// <summary>
        /// Sets the mouse speed.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mouse_speed(int xspeed, int yspeed);

        /// <summary>
        /// Sets the mouse sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mouse_sprite(IntPtr sprite);

        /// <summary>
        /// Sets the mouse sprite focus.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mouse_sprite_focus(int x, int y);

        /// <summary>
        /// How far the mouse has moved since the last call to this function.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_mouse_mickeys(ref int mickeyx, ref int mickeyy);

        /// <summary>
        /// User specified mouse callback.
        /// </summary>
        public static MouseCallback mouse_callback
        {
            set
            {
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(value);
                Marshal.WriteInt32(GetAddress("mouse_callback"), callback.ToInt32());
            }
        }

        #endregion Mouse routines

        #region Timer routines

        /* install_timer - Installs the Allegro timer interrupt handler.
         * remove_timer - Removes the Allegro time handler.
         * install_int - Installs a user timer handler.
         * install_int_ex - Adds or modifies a timer.
         * LOCK_VARIABLE - Locks the memory of a variable used by a timer.
         * LOCK_FUNCTION - Locks the memory of a function used by a timer.
         * END_OF_FUNCTION - Locks the code used by a timer.
         * remove_int - Removes a timers.
         * install_param_int - Installs a timer routine with a customizable parameter.
         * install_param_int_ex - Adds or modifies a timer with a customizable parameter.
         * remove_param_int - Removes a timer with a customizable parameter.
         * retrace_count - Retrace count simulator.
         * rest - Waits a specified number of milliseconds or yields CPU.
         * rest_callback - Like rest(), but calls the callback during the wait.
         */

        /// <summary>
        /// Installs the Allegro timer interrupt handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_timer();

        /// <summary>
        /// Removes the Allegro time handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_timer();

        /// <summary>
        /// Installs a user timer handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_int(TimerHandler proc, int speed);

        /// <summary>
        /// Adds or modifies a timer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_int_ex(TimerHandler proc, int speed);

        /// <summary>
        /// Locks the memory of a variable used by a timer.
        /// </summary>
        public static void LOCK_VARIABLE(object variable_name)
        {
            // NOOP
        }

        /// <summary>
        /// Locks the memory of a function used by a timer.
        /// </summary>
        public static void LOCK_FUNCTION(object variable_name)
        {
            // NOOP
        }

        /// <summary>
        /// Locks the code used by a timer.
        /// </summary>
        public static void END_OF_FUNCTION(object variable_name)
        {
            // NOOP
        }

        /// <summary>
        /// Gives the number of seconds between each tick.
        /// </summary>
        public static int SECS_TO_TIMER(int x)
        {
            return x * TIMERS_PER_SECOND;
        }

        /// <summary>
        /// Gives the number of milliseconds between ticks.
        /// </summary>
        public static int MSEC_TO_TIMER(int x)
        {
            return x * (TIMERS_PER_SECOND / 1000);
        }

        /// <summary>
        /// Gives the number of ticks each second.
        /// </summary>
        public static int BPS_TO_TIMER(int x)
        {
            return TIMERS_PER_SECOND / x;
        }

        /// <summary>
        /// Gives the number of ticks per minute.
        /// </summary>
        public static int BPM_TO_TIMER(int x)
        {
            return (60 * TIMERS_PER_SECOND) / x;
        }

        /// <summary>
        /// Removes a timers.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_int(TimerHandler proc);

        /// <summary>
        /// Installs a timer routine with a customizable parameter.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_param_int(ParamTimerHandler proc, IntPtr param, int speed);

        /// <summary>
        /// Adds or modifies a timer with a customizable parameter.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_param_int_ex(ParamTimerHandler proc, IntPtr param, int speed);

        /// <summary>
        /// Removes a timer with a customizable parameter.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_param_int(ParamTimerHandler proc, IntPtr param);

        /// <summary>
        /// Retrace count simulator.
        /// </summary>
        public static int retrace_count
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("retrace_count"));
            }
        }

        /// <summary>
        /// Waits a specified number of milliseconds or yields CPU.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rest(uint time);

        /// <summary>
        /// Like rest(), but calls the callback during the wait.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rest_callback(long time, RestCallback callback);

        #endregion Timer routines

        #region Keyboard routines

        /*
         * install_keyboard - Installs the Allegro keyboard interrupt handler.
         * remove_keyboard - Removes the Allegro keyboard handler.
         * install_keyboard_hooks - Installs custom keyboard hooks.
         * poll_keyboard - Polls the keyboard.
         * keyboard_needs_poll - Tells if the keyboard needs polling.
         * key - Array of flags indicating key state.
         * key_shifts - Bitmask containing the current state of modifier keys.
         * keypressed - Tells if there are keypresses waiting in the input buffer.
         * readkey - Returns the next character from the keyboard buffer.
         * ureadkey - Returns the next unicode character from the keyboard buffer.
         * scancode_to_ascii - Converts a scancode to an ASCII character.
         * scancode_to_name - Converts a scancode to a key name.
         * simulate_keypress - Stuffs a key into the keyboard buffer.
         * simulate_ukeypress - Stuffs an unicode key into the keyboard buffer.
         * keyboard_callback - User specified keyboard callback handler.
         * keyboard_ucallback - User specified unicode keyboard callback handler.
         * keyboard_lowlevel_callback - User specified low level keyboard event handler.
         * set_leds - Sets the state of the keyboard LED indicators.
         * set_keyboard_rate - Sets the keyboard repeat rate.
         * clear_keybuf - Clears the keyboard buffer.
         * three_finger_flag - Flag to desactivate the emergency exit key combination.
         * key_led_flag - Flag to prevent the keyboard LEDs from being updated.
         */

        /// <summary>
        /// Installs the Allegro keyboard interrupt handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_keyboard();

        /// <summary>
        /// Removes the Allegro keyboard handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_keyboard();

        /// <summary>
        /// Installs custom keyboard hooks.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void install_keyboard_hooks(KeyPressedCallback keypressed, ReadKeyCallback readkey);

        /// <summary>
        /// Polls the keyboard.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int poll_keyboard();

        /// <summary>
        /// Tells if the keyboard needs polling.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int keyboard_needs_poll();

        /// <summary>
        /// Array of flags indicating key state.
        /// </summary>
        public static Keys key;

        /// <summary>
        /// Bitmask containing the current state of modifier keys.
        /// </summary>
        public static int key_shifts
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("key_shifts"));
            }
        }

        /// <summary>
        /// Tells if there are keypresses waiting in the input buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int keypressed();

        /// <summary>
        /// Returns the next character from the keyboard buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int readkey();

        /// <summary>
        /// Returns the next unicode character from the keyboard buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ureadkey(out int scancode);

        /// <summary>
        /// Converts a scancode to an ASCII character.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int scancode_to_ascii(int scancode);

        /// <summary>
        /// Converts a scancode to a key name.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder scancode_to_name(int scancode);

        /// <summary>
        /// Stuffs a key into the keyboard buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void simulate_keypress(int key);

        /// <summary>
        /// Stuffs an unicode key into the keyboard buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void simulate_ukeypress(int key, int scancode);

        /// <summary>
        /// User specified keyboard callback handler.
        /// </summary>
        public static KeyboardCallback keyboard_callback;

        /// <summary>
        /// User specified unicode keyboard callback handler.
        /// </summary>
        public static KeyboardUCallback keyboard_ucallback;

        /// <summary>
        /// User specified low level keyboard event handler.
        /// </summary>
        ///  [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static KeyboardLowLevelCallback keyboard_lowlevel_callback
        {
            set
            {
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(value);
                Marshal.WriteInt32(GetAddress("keyboard_lowlevel_callback"), callback.ToInt32());
            }
        }

        /// <summary>
        /// Sets the state of the keyboard LED indicators.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_leds(int leds);

        /// <summary>
        /// Sets the keyboard repeat rate.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_keyboard_rate(int delay, int repeat);

        /// <summary>
        /// Clears the keyboard buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clear_keybuf();

        /// <summary>
        /// Flag to desactivate the emergency exit key combination.
        /// </summary>
        public static int three_finger_flag;

        /// <summary>
        /// Flag to prevent the keyboard LEDs from being updated.
        /// </summary>
        public static int key_led_flag;

        #endregion Keyboard routines

        #region Joystick routines

        /* install_joystick - Initialises the joystick.
         * remove_joystick - Removes the joystick handler.
         * poll_joystick - Polls the joystick.
         * num_joysticks - Global variable saying how many joysticks there are.
         * joy - Global array of joystick state information.
         * calibrate_joystick_name - Returns the next calibration text string.
         * calibrate_joystick - Calibrates the specified joystick.
         * save_joystick_data - Saves joystick calibration data.
         * load_joystick_data - Loads joystick calibration data.
         * initialise_joystick - Deprecated version of install_joystick().
         */

        /// <summary>
        /// Initialises the joystick.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_joystick(int type);

        /// <summary>
        /// Removes the joystick handler.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_joystick();

        /// <summary>
        /// Polls the joystick.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int poll_joystick();

        /// <summary>
        /// Global variable saying how many joysticks there are.
        /// </summary>
        public static int num_joysticks
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("num_joysticks"));
            }
        }

        /// <summary>
        /// Global array of joystick state information.
        /// </summary>
        public static IntPtr joy
        {
            get
            {
                return GetAddress("joy");
            }
        }

        /// <summary>
        /// Returns the next calibration text string.
        /// </summary>
        // TODO: replace string with StringBuilder and test
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string calibrate_joystick_name(int n);

        /// <summary>
        /// Calibrates the specified joystick.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int calibrate_joystick(int n);

        /// <summary>
        /// Saves joystick calibration data.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_joystick_data(string filename);

        /// <summary>
        /// Loads joystick calibration data.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int load_joystick_data(string filename);

        /// <summary>
        /// Deprecated version of install_joystick().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int initialise_joystick();

        #endregion Joystick routines

        #region Graphics modes

        /*
         * set_color_depth - Sets the global pixel color depth.
         * get_color_depth - Returns the current pixel color depth.
         * request_refresh_rate - Requests a specific refresh rate during graphic mode switch.
         * get_refresh_rate - Returns the current refresh rate.
         * get_gfx_mode_list - Obtains a list of available video modes.
         * destroy_gfx_mode_list - Frees the list created by get_gfx_mode_list().
         * set_gfx_mode - Sets a graphic video mode.
         * set_display_switch_mode - Tells Allegro how the program handles background switching.
         * set_display_switch_callback - Installs a switching notification callback.
         * remove_display_switch_callback - Removes a switching notification callback.
         * get_display_switch_mode - Returns the current display switching mode.
         * is_windowed_mode - Tells if you are running in windowed mode.
         * gfx_capabilities - Bitfield describing video hardware capabilities.
         * enable_triple_buffer - Enables triple buffering.
         * scroll_screen - Requests a hardware scroll request.
         * request_scroll - Queues a hardware scroll request with triple buffering.
         * poll_scroll - Checks the status of a scroll request with triple buffering.
         * show_video_bitmap - Flips the hardware screen to use the specified page.
         * request_video_bitmap - Triple buffering page flip request.
         * vsync - Waits for a vertical retrace to begin.
         */

        /// <summary>
        /// Sets the global pixel color depth.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_color_depth(int depth);

        /// <summary>
        /// Returns the current pixel color depth.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_color_depth();

        /// <summary>
        /// Requests a specific refresh rate during graphic mode switch.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void request_refresh_rate(int rate);

        /// <summary>
        /// Returns the current refresh rate.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_refresh_rate();

        /// <summary>
        /// Obtains a list of available video modes.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr get_gfx_mode_list(int card);

        /// <summary>
        /// Frees the list created by get_gfx_mode_list().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void destroy_gfx_mode_list(IntPtr mode_list);

        /// <summary>
        /// Sets a graphic video mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int set_gfx_mode(int card, int w, int h, int v_w, int v_h);

        /// <summary>
        /// Tells Allegro how the program handles background switching.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_display_switch_mode(int mode);

        /// <summary>
        /// Installs a switching notification callback.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_display_switch_callback(int dir, DisplaySwitchCallback cb);

        /// <summary>
        /// Removes a switching notification callback.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_display_switch_callback(DisplaySwitchCallback cb);

        /// <summary>
        /// Returns the current display switching mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_display_switch_mode();

        /// <summary>
        /// Tells if you are running in windowed mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_windowed_mode();

        /// <summary>
        /// Bitfield describing video hardware capabilities.
        /// </summary>
        public static int gfx_capabilities
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gfx_capabilities"));
            }
        }

        /// <summary>
        /// Enables triple buffering.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int enable_triple_buffer();

        /// <summary>
        /// Requests a hardware scroll request.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int scroll_screen(int x, int y);

        /// <summary>
        /// Queues a hardware scroll request with triple buffering.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int request_scroll(int x, int y);

        /// <summary>
        /// Checks the status of a scroll request with triple buffering.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int poll_scroll();

        /// <summary>
        /// Flips the hardware screen to use the specified page.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int show_video_bitmap(IntPtr bitmap);

        /// <summary>
        /// Triple buffering page flip request.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int request_video_bitmap(IntPtr bitmap);

        /// <summary>
        /// Waits for a vertical retrace to begin.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void vsync();

        #endregion Graphics modes

        #region Bitmap objects

        /* screen - Global pointer to the screen hardware video memory.
         * SCREEN_W
         * SCREEN_H - Global define to obtain the size of the screen.
         * VIRTUAL_W
         * VIRTUAL_H - Global define to obtain the virtual size of the screen.
         * create_bitmap - Creates a memory bitmap.
         * create_bitmap_ex - Creates a memory bitmap specifying color depth.
         * create_sub_bitmap - Creates a memory sub bitmap.
         * create_video_bitmap - Creates a video memory bitmap.
         * create_system_bitmap - Creates a system memory bitmap.
         * destroy_bitmap - Destroys any type of created bitmap.
         * lock_bitmap - Locks the memory used by a bitmap.
         * bitmap_color_depth - Returns the color depth of the specified bitmap.
         * bitmap_mask_color - Returns the mask color of the specified bitmap.
         * is_same_bitmap - Tells if two bitmaps describe the same drawing surface.
         * is_planar_bitmap - Tells if a bitmap is a planar screen bitmap.
         * is_linear_bitmap - Tells if a bitmap is linear.
         * is_memory_bitmap - Tells if a bitmap is a memory bitmap.
         * is_screen_bitmap - Tells if a bitmap is the screen bitmap or sub bitmap.
         * is_video_bitmap - Tells if a bitmap is a screen bitmap, video memory or sub bitmap.
         * is_system_bitmap - Tells if a bitmap is a system bitmap or sub bitmap.
         * is_sub_bitmap - Tells if a bitmap is a sub bitmap.
         * acquire_bitmap - Locks the bitmap before drawing onto it.
         * release_bitmap - Releases a previously locked bitmap.
         * acquire_screen - Shortcut of acquire_bitmap(screen);
         * release_screen - Shortcut of release_bitmap(screen);
         * set_clip_rect - Sets the clipping rectangle of a bitmap.
         * get_clip_rect - Returns the clipping rectangle of a bitmap.
         * add_clip_rect - Intersects a bitmap's clipping rectangle with the given area.
         * set_clip_state - Turns on or off the clipping of a bitmap.
         * get_clip_state - Tells if clipping is on for a bitmap.
         * is_inside_bitmap - Tells if a point is inside a bitmap.
         */

        /// <summary>
        /// Global pointer to the screen hardware video memory.
        /// </summary>
        public static IntPtr screen
        {
            get
            {
                return Marshal.ReadIntPtr(GetAddress("screen"));
            }
        }

        public static int SCREEN_W
        {
            get
            {
                return (gfx_driver ? gfx_driver.w : 0);
            }
        }

        public static int SCREEN_H
        {
            get
            {
                return (gfx_driver ? gfx_driver.h : 0);
            }
        }

        public static int VIRTUAL_W
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("VIRTUAL_W"));
            }
        }

        public static int VIRTUAL_H
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("VIRTUAL_H"));
            }
        }

        /// <summary>
        /// Creates a memory bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_bitmap(int width, int height);

        /// <summary>
        /// Creates a memory bitmap specifying color depth.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_bitmap_ex(int color_depth, int width, int height);

        /// <summary>
        /// Creates a memory sub bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_sub_bitmap(IntPtr parent, int x, int y, int width, int height);

        /// <summary>
        /// Creates a video memory bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_video_bitmap(int width, int height);

        /// <summary>
        /// Creates a system memory bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_system_bitmap(int width, int height);

        /// <summary>
        /// Destroys any type of created bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_bitmap(IntPtr bitmap);

        /// <summary>
        /// Locks the memory used by a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock_bitmap(IntPtr bitmap);

        /// <summary>
        /// Returns the color depth of the specified bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bitmap_color_depth(IntPtr bmp);

        /// <summary>
        /// Returns the mask color of the specified bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bitmap_mask_color(IntPtr bmp);

        /// <summary>
        /// Tells if two bitmaps describe the same drawing surface.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_same_bitmap(IntPtr bmp1, IntPtr bmp2);

        /// <summary>
        /// Tells if a bitmap is a planar screen bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_planar_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is linear.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_linear_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is a memory bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_memory_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is the screen bitmap or sub bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_screen_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is a screen bitmap, video memory or sub bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_video_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is a system bitmap or sub bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_system_bitmap(IntPtr bmp);

        /// <summary>
        /// Tells if a bitmap is a sub bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_sub_bitmap(IntPtr bmp);

        /// <summary>
        /// Locks the bitmap before drawing onto it.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void acquire_bitmap(IntPtr bmp);

        /// <summary>
        /// Releases a previously locked bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void release_bitmap(IntPtr bmp);

        /// <summary>
        /// Shortcut of acquire_bitmap(screen);
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void acquire_screen();

        /// <summary>
        /// Shortcut of release_bitmap(screen);
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void release_screen();

        /// <summary>
        /// Sets the clipping rectangle of a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_clip_rect(IntPtr bitmap, int x1, int y1, int x2, int y2);

        /// <summary>
        /// Returns the clipping rectangle of a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_clip_rect(IntPtr bitmap, ref int x1, ref int y1, ref int x2, ref int y2);

        /// <summary>
        /// Intersects a bitmap's clipping rectangle with the given area.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void add_clip_rect(IntPtr bitmap, int x1, int y1, int x2, int y2);

        /// <summary>
        /// Turns on or off the clipping of a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_clip_state(IntPtr bitmap, int state);

        /// <summary>
        /// Tells if clipping is on for a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_clip_state(IntPtr bitmap);

        /// <summary>
        /// Tells if a point is inside a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_inside_bitmap(IntPtr bmp, int x, int y, int clip);

        #endregion Bitmap objects

        #region Loading image files

        /* load_bitmap - Loads any supported bitmap from a file.
         * load_bmp - Loads a BMP bitmap from a file.
         * load_bmp_pf - Packfile version of load_bmp.
         * load_lbm - Loads an LBM bitmap from a file.
         * load_pcx - Loads a PCX bitmap from a file.
         * load_pcx_pf - Packfile version of load_pcx.
         * load_tga - Loads a TGA bitmap from a file.
         * load_tga_pf - Packfile version of load_tga.
         * save_bitmap - Saves a bitmap into any supported file format.
         * save_bmp - Saves a bitmap into a BMP file.
         * save_bmp_pf - Packfile version of save_bmp.
         * save_pcx - Saves a bitmap into a PCX file.
         * save_pcx_pf - Packfile version of save_pcx.
         * save_tga - Saves a bitmap into a TGA file.
         * save_tga_pf - Packfile version of save_tga.
         * register_bitmap_file_type - Registers custom bitmap loading/saving functions.
         * set_color_conversion - Tells Allegro how to convert images during loading time.
         * get_color_conversion - Returns the current color conversion mode.
         */

        /// <summary>
        /// Loads any supported bitmap from a file.
        /// </summary>
        // TODO: check why loading palette doesn't work
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_bitmap(string filename, IntPtr pal);

        /// <summary>
        /// Loads a BMP bitmap from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_bmp(string filename, IntPtr pal);

        /// <summary>
        /// Packfile version of load_bmp.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern BITMAP load_bmp_pf(IntPtr f, IntPtr pal);

        /// <summary>
        /// Loads an LBM bitmap from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_lbm(string filename, IntPtr pal);

        /// <summary>
        /// Loads a PCX bitmap from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_pcx(string filename, IntPtr pal);

        /// <summary>
        /// Packfile version of load_pcx.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_pcx_pf(IntPtr f, IntPtr pal);

        /// <summary>
        /// Loads a TGA bitmap from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_tga(string filename, IntPtr pal);

        /// <summary>
        /// Packfile version of load_tga.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_tga_pf(PACKFILE f, IntPtr pal);

        /// <summary>
        /// Saves a bitmap into any supported file format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_bitmap(string filename, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Saves a bitmap into a BMP file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_bmp(string filename, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Packfile version of save_bmp.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_bmp_pf(IntPtr f, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Saves a bitmap into a PCX file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_pcx(string filename, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Packfile version of save_pcx.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_pcx_pf(IntPtr f, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Saves a bitmap into a TGA file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_tga(string filename, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Packfile version of save_tga.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_tga_pf(IntPtr f, IntPtr bmp, IntPtr pal);

        /// <summary>
        /// Registers custom bitmap loading/saving functions.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void register_bitmap_file_type(string ext, Load load, Save save);

        /// <summary>
        /// Tells Allegro how to convert images during loading time.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_color_conversion(int mode);

        /// <summary>
        /// Returns the current color conversion mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_color_conversion();

        #endregion Loading image files

        #region Palette routines

        /* set_color - Sets the specified palette entry to the specified RGB triplet.
         * _set_color - Inline version of set_color().
         * set_palette - Sets the entire palette of 256 colors.
         * set_palette_range - Sets a specific range of the palette.
         * get_color - Retrieves the specified palette entry.
         * get_palette - Retrieves the entire palette of 256 colors.
         * get_palette_range - Retrieves a specific palette range.
         * fade_interpolate - Calculates a new palette interpolated between two others.
         * fade_from_range - Gradually fades a part of the palette between two others.
         * fade_in_range - Gradually fades a part of the palette from black.
         * fade_out_range - Gradually fades a part of the palette to black.
         * fade_from - Gradually fades the palette between two others.
         * fade_in - Gradually fades the palette from black.
         * fade_out - Gradually fades the palette to black.
         * select_palette - Sets the internal palette for color conversion.
         * unselect_palette - Restores the palette before last call to select_palette().
         * generate_332_palette - Constructs a fake truecolor palette.
         * generate_optimized_palette - Generates an optimized palette for a bitmap.
         * default_palette - The default IBM BIOS palette.
         * black_palette - A palette containing solid black colors.
         * desktop_palette - The palette used by the Atari ST low resolution desktop.
         */

        /// <summary>
        /// Sets the specified palette entry to the specified RGB triplet.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_color(int index, IntPtr p);

        /// <summary>
        /// Inline version of set_color().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_color(int index, IntPtr p);

        /// <summary>
        /// Sets the entire palette of 256 colors.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_palette(IntPtr p);

        /// <summary>
        /// Sets a specific range of the palette.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_palette_range(IntPtr p, int from, int to, int vsync);

        /// <summary>
        /// Retrieves the specified palette entry.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_color(int index, ref RGB p);

        /// <summary>
        /// Retrieves the entire palette of 256 colors.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_palette(PALETTE p);

        /// <summary>
        /// Retrieves a specific palette range.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_palette_range(PALETTE p, int from, int to);

        /// <summary>
        /// Calculates a new palette interpolated between two others.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_interpolate(PALETTE source, PALETTE dest, PALETTE output, int pos, int from, int to);

        /// <summary>
        /// Gradually fades a part of the palette between two others.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_from_range(PALETTE source, PALETTE dest, int speed, int from, int to);

        /// <summary>
        /// Gradually fades a part of the palette from black.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_in_range(PALETTE p, int speed, int from, int to);

        /// <summary>
        /// Gradually fades a part of the palette to black.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_out_range(int speed, int from, int to);

        /// <summary>
        /// Gradually fades the palette between two others.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_from(PALETTE source, PALETTE dest, int speed);

        /// <summary>
        /// Gradually fades the palette from black.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_in(PALETTE p, int speed);

        /// <summary>
        /// Gradually fades the palette to black.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fade_out(int speed);

        /// <summary>
        /// Sets the internal palette for color conversion.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void select_palette(PALETTE p);

        /// <summary>
        /// Restores the palette before last call to select_palette().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unselect_palette();

        /// <summary>
        /// Constructs a fake truecolor palette.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void generate_332_palette(IntPtr pal);

        /// <summary>
        /// Generates an optimized palette for a bitmap.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int generate_optimized_palette(BITMAP bmp, PALETTE pal, char rsvd[PAL_SIZE]);

        /// <summary>
        /// The default IBM BIOS palette.
        /// </summary>
        public static PALETTE default_palette
        {
            get
            {
                return GetAddress("default_palette");
            }
        }

        /// <summary>
        /// A palette containing solid black colors.
        /// </summary>
        public static PALETTE black_palette
        {
            get
            {
                return GetAddress("black_palette");
            }
        }

        /// <summary>
        /// The palette used by the Atari ST low resolution desktop.
        /// </summary>
        public static PALETTE desktop_palette
        {
            get
            {
                return GetAddress("desktop_palette");
            }
        }

        #endregion Palette routines

        #region Truecolor pixel formats

        /* makecol8
         * makecol15
         * makecol16
         * makecol24
         * makecol32 - Converts an RGB value into a display dependent pixel format.
         * makeacol32 - Converts an RGBA color into a 32-bit display pixel format.
         * makecol - Converts an RGB value into the current pixel format.
         * makecol_depth - Converts an RGB value into the specified pixel format.
         * makeacol
         * makeacol_depth - Converts RGBA colors into display dependent pixel formats.
         * makecol15_dither
         * makecol16_dither - Calculates a dithered 15 or 16-bit RGB value.
         * getr8
         * getg8
         * getb8
         * getr15
         * getg15
         * getb15
         * getr16
         * getg16
         * getb16
         * getr24
         * getg24
         * getb24
         * getr32
         * getg32
         * getb32 - Extract a color component from the specified pixel format.
         * geta32 - Extract the alpha component form a 32-bit pixel format color.
         * getr
         * getg
         * getb
         * geta - Extract a color component from the current pixel format.
         * getr_depth
         * getg_depth
         * getb_depth
         * geta_depth - Extract a color component from a color in a specified pixel format.
         * palette_color - Maps palette indexes into the current pixel format colors.
         * MASK_COLOR_8
         * MASK_COLOR_15
         * MASK_COLOR_16
         * MASK_COLOR_24
         * MASK_COLOR_32 - Constant representing the mask value in sprites.
         */

        /// <summary>
        /// Converts an RGB value into a display dependent pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int makecol32(int r, int g, int b);

        /// <summary>
        /// Converts an RGBA color into a 32-bit display pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int makeacol32(int r, int g, int b, int a);

        /// <summary>
        /// Converts an RGB value into the current pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int makecol(int r, int g, int b);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int makeacol(int r, int g, int b, int a);

        /// <summary>
        /// Extract a color component from the specified pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getr32(int c);

        /// <summary>
        /// Extract a color component from the specified pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getg32(int c);

        /// <summary>
        /// Extract a color component from the specified pixel format.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getb32(int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getr(int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getg(int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getb(int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getr_depth(int color_depth, int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getg_depth(int color_depth, int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getb_depth(int color_depth, int c);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int geta_depth(int color_depth, int c);

        #endregion Truecolor pixel formats

        #region Drawing primitives

        /* clear_bitmap - Clears the bitmap to color 0.
         * clear_to_color - Clears the bitmap to the specified color.
         * putpixel - Writes a pixel into a bitmap.
         * _putpixel
         * _putpixel15
         * _putpixel16
         * _putpixel24
         * _putpixel32 - Faster specific version of putpixel().
         * getpixel - Reads a pixel from a bitmap.
         * _getpixel
         * _getpixel15
         * _getpixel16
         * _getpixel24
         * _getpixel32 - Faster specific version of getpixel().
         * vline - Draws a vertical line onto the bitmap.
         * hline - Draws a horizontal line onto the bitmap.
         * do_line - Calculates all the points along a line.
         * line - Draws a line onto the bitmap.
         * fastline - Faster version of line().
         * triangle - Draws a filled triangle.
         * polygon - Draws a filled polygon.
         * rect - Draws an outline rectangle.
         * rectfill - Draws a solid filled rectangle.
         * do_circle - Calculates all the points in a circle.
         * circle - Draws a circle.
         * circlefill - Draws a filled circle.
         * do_ellipse - Calculates all the points in an ellipse.
         * ellipse - Draws an ellipse.
         * ellipsefill - Draws a filled ellipse.
         * do_arc - Calculates all the points in a circular arc.
         * arc - Draws a circular arc.
         * calc_spline - Calculates a series of values along a bezier spline.
         * spline - Draws a bezier spline using four control points.
         * floodfill - Floodfills an enclosed area.
         */

        /// <summary>
        /// Clears the bitmap to color 0.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clear_bitmap(IntPtr bitmap);

        /// <summary>
        /// Clears the bitmap to the specified color.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clear_to_color(IntPtr bitmap, int color);

        /// <summary>
        /// Writes a pixel into a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void putpixel(IntPtr bmp, int x, int y, int color);

        /// <summary>
        /// Faster specific version of putpixel().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void _putpixel32(IntPtr bmp, int x, int y, int color);

        /// <summary>
        /// Reads a pixel from a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getpixel(IntPtr bmp, int x, int y);

        /// <summary>
        /// Faster specific version of getpixel().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _getpixel32(IntPtr bmp, int x, int y);

        /// <summary>
        /// Draws a vertical line onto the bitmap.
        /// </summary>
        // TODO: check if it's possible to directly access alias function
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void vline(IntPtr bmp, int x, int y1, int y2, int color);
        public static void vline(IntPtr bmp, int x, int y1, int y2, int color)
        {
            line(bmp, x, y1, x, y2, color);
        }

        /// <summary>
        /// Draws a horizontal line onto the bitmap.
        /// </summary>
        // TODO: check if it's possible to directly access alias function
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void hline(IntPtr bmp, int x1, int y, int x2, int color);
        public static void hline(IntPtr bmp, int x1, int y, int x2, int color)
        {
            line(bmp, x1, y, x2, y, color);
        }

        /// <summary>
        /// Calculates all the points along a line.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void do_line(IntPtr bmp, int x1, int y1, int x2, int y2, int d, LineCallback proc);

        /// <summary>
        /// Draws a line onto the bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void line(IntPtr bmp, int x1, int y1, int x2, int y2, int color);

        /// <summary>
        /// Faster version of line().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fastline(IntPtr bmp, int x1, int y1, int x2, int y2, int color);

        /// <summary>
        /// Draws a filled triangle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void triangle(IntPtr bmp, int x1, int y1, int x2, int y2, int x3, int y3, int color);

        /// <summary>
        /// Draws a filled polygon.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void polygon(IntPtr bmp, int vertices, int[] points, int color);

        /// <summary>
        /// Draws an outline rectangle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rect(IntPtr bmp, int x1, int y1, int x2, int y2, int color);

        /// <summary>
        /// Draws a solid filled rectangle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rectfill(IntPtr bmp, int x1, int y1, int x2, int y2, int color);

        /// <summary>
        /// Calculates all the points in a circle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void do_circle(IntPtr bmp, int x, int y, int radius, int d, CircleCallback proc);

        /// <summary>
        /// Draws a circle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void circle(IntPtr bmp, int x, int y, int radius, int color);

        /// <summary>
        /// Draws a filled circle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void circlefill(IntPtr bmp, int x, int y, int radius, int color);

        /// <summary>
        /// Calculates all the points in an ellipse.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void do_ellipse(IntPtr bmp, int x, int y, int rx, int ry, int d, EllipseCallback proc);

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ellipse(IntPtr bmp, int x, int y, int rx, int ry, int color);

        /// <summary>
        /// Draws a filled ellipse.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ellipsefill(IntPtr bmp, int x, int y, int rx, int ry, int color);

        /// <summary>
        /// Calculates all the points in a circular arc.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void do_arc(BITMAP *bmp, int x, int y, fixed a1, fixed a2, int r, int d, ArcCallback proc);

        /// <summary>
        /// Draws a circular arc.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void arc(IntPtr bmp, int x, int y, long ang1, long ang2, int r, int color);

        /// <summary>
        /// Calculates a series of values along a bezier spline.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void calc_spline(int[] points, int npts, int[] x, int[] y);

        /// <summary>
        /// Draws a bezier spline using four control points.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void spline(IntPtr bmp, int[] points, int color);

        /// <summary>
        /// Floodfills an enclosed area.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void floodfill(IntPtr bmp, int x, int y, int color);

        #endregion Drawing primitives

        #region Blitting and sprites

        /* blit - Copies a rectangular area from one bitmap to another.
         * stretch_blit - Scales a rectangular area from one bitmap to another.
         * masked_blit - Copies a rectangle skipping pixels with the mask color.
         * masked_stretch_blit - Scales a rectangular area skipping pixels with the mask color.
         * draw_sprite - Draws a copy of the sprite onto the destination bitmap.
         * stretch_sprite - Stretches a sprite to the destination bitmap.
         * draw_sprite_v_flip
         * draw_sprite_h_flip
         * draw_sprite_vh_flip - Draws the sprite transformed to the destination bitmap.
         * draw_trans_sprite - Draws a sprite blending it with the destination.
         * draw_lit_sprite - Draws a sprite tinted with a specific color.
         * draw_gouraud_sprite - Draws a sprite with gouraud shading.
         * draw_character_ex - Draws non transparent pixels of the sprite with a color.
         * rotate_sprite - Rotates a sprite.
         * rotate_sprite_v_flip - Rotates and flips a sprite.
         * rotate_scaled_sprite - Rotates and stretches a sprite.
         * rotate_scaled_sprite_v_flip - Rotates, stretches and flips a sprite.
         * pivot_sprite - Rotates a sprite around a specified point.
         * pivot_sprite_v_flip - Rotates and flips a sprite around a specified point.
         * pivot_scaled_sprite - Rotates and stretches a sprite around a specified point.
         * pivot_scaled_sprite_v_flip - Rotates, stretches and flips a sprite around a specified point.
         */

        /// <summary>
        /// Copies a rectangular area from one bitmap to another.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void blit(IntPtr source, IntPtr dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height);

        /// <summary>
        /// Scales a rectangular area from one bitmap to another.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stretch_blit(IntPtr source, IntPtr dest, int source_x, int source_y, int source_width, int source_height, int dest_x, int dest_y, int dest_width, int dest_height);

        /// <summary>
        /// Copies a rectangle skipping pixels with the mask color.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void masked_blit(IntPtr source, IntPtr dest, int source_x, int source_y, int dest_x, int dest_y, int width, int height);

        /// <summary>
        /// Scales a rectangular area skipping pixels with the mask color.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void masked_stretch_blit(IntPtr source, IntPtr dest, int source_x, int source_y, int source_w, int source_h, int dest_x, int dest_y, int dest_w, int dest_h);

        /// <summary>
        /// Draws a copy of the sprite onto the destination bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_sprite(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Stretches a sprite to the destination bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stretch_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int w, int h);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_sprite_v_flip(IntPtr bmp, IntPtr sprite, int x, int y);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_sprite_h_flip(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Draws the sprite transformed to the destination bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_sprite_vh_flip(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Draws a sprite blending it with the destination.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_trans_sprite(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Draws a sprite tinted with a specific color.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_lit_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int color);

        /// <summary>
        /// Draws a sprite with gouraud shading.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_gouraud_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int c1, int c2, int c3, int c4);

        /// <summary>
        /// Draws non transparent pixels of the sprite with a color.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_character_ex(IntPtr bmp, IntPtr sprite, int x, int y, int color, int bg);

        /// <summary>
        /// Rotates a sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotate_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int angle);

        /// <summary>
        /// Rotates and flips a sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotate_sprite_v_flip(IntPtr bmp, IntPtr sprite, int x, int y, int angle);

        /// <summary>
        /// Rotates and stretches a sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotate_scaled_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int angle, int scale);

        /// <summary>
        /// Rotates, stretches and flips a sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotate_scaled_sprite_v_flip(IntPtr bmp, IntPtr sprite, int x, int y, int angle, int scale);

        /// <summary>
        /// Rotates a sprite around a specified point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pivot_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int cx, int cy, int angle);

        /// <summary>
        /// Rotates and flips a sprite around a specified point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pivot_sprite_v_flip(IntPtr bmp, IntPtr sprite, int x, int y, int cx, int cy, int angle);

        /// <summary>
        /// Rotates and stretches a sprite around a specified point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pivot_scaled_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int cx, int cy, int angle, int scale);

        /// <summary>
        /// Rotates, stretches and flips a sprite around a specified point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void pivot_scaled_sprite_v_flip(IntPtr bmp, IntPtr sprite, int x, int y, int cx, int cy, int angle, int scale);

        #endregion Blitting and sprites

        #region RLE sprites

        /* get_rle_sprite - Creates an RLE sprite using a bitmap as source.
         * destroy_rle_sprite - Destroys an RLE sprite.
         * draw_rle_sprite - Draws an RLE sprite.
         * draw_trans_rle_sprite - Draws a translucent RLE sprite.
         * draw_lit_rle_sprite - Draws a tinted RLE sprite.
         */

        /// <summary>
        /// Creates an RLE sprite using a bitmap as source.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr get_rle_sprite(IntPtr bitmap);

        /// <summary>
        /// Destroys an RLE sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_rle_sprite(IntPtr sprite);

        /// <summary>
        /// Draws an RLE sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_rle_sprite(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Draws a translucent RLE sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_trans_rle_sprite(IntPtr bmp, IntPtr sprite, int x, int y);

        /// <summary>
        /// Draws a tinted RLE sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_lit_rle_sprite(IntPtr bmp, IntPtr sprite, int x, int y, int color);

        #endregion RLE sprites

        #region Compiled sprites

        /* get_compiled_sprite - Creates a compiled sprite using a bitmap as source.
         * destroy_compiled_sprite - Destroys a compiled sprite.
         * draw_compiled_sprite - Draws a compiled sprite.
         */

        /// <summary>
        /// Creates a compiled sprite using a bitmap as source.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr get_compiled_sprite(IntPtr bitmap, int planar);

        /// <summary>
        /// Destroys a compiled sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_compiled_sprite(IntPtr sprite);

        /// <summary>
        /// Draws a compiled sprite.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void draw_compiled_sprite(IntPtr bmp, IntPtr sprite, int x, int y);

        #endregion Compiled sprites

        #region Fonts

        /* register_font_file_type - Register a new font loading function.
         * load_font - Loads a font from a file.
         * destroy_font - Frees the memory being used by a font structure.
         * make_trans_font - Makes a font use transparency.
         * is_color_font - Returns TRUE if a font is a color font.
         * is_mono_font - Returns TRUE if a font is a monochrome font.
         * is_compatible_font - Check if two fonts are of the same type.
         * get_font_ranges - Returns the number of character ranges in a font.
         * get_font_range_begin - Returns the start of a character range in a font.
         * get_font_range_end - Returns the last character of a character range in a font.
         * extract_font_range - Extracts a range of characters from a font.
         * transpose_font - Transposes all characters in a font.
         * merge_fonts - Merges two fonts into one font.
         * load_dat_font - Loads a FONT from an Allegro datafile.
         * load_bios_font - Loads a 8x8 or 8x16 BIOS format font.
         * load_grx_font - Loads a GRX format font.
         * load_grx_or_bios_font - Loads either a BIOS or GRX format font.
         * load_bitmap_font - Grabs a font from a bitmap file.
         * grab_font_from_bitmap - Grabs a font from a bitmap
         * load_txt_font - Loads a font script.
         */

        /// <summary>
        /// Loads a font from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_font(string filename, IntPtr pal, IntPtr param);

        /// <summary>
        /// Frees the memory being used by a font structure.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_font(IntPtr f);

        /// <summary>
        /// Makes a font use transparency.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void make_trans_font(IntPtr f);

        /// <summary>
        /// Returns TRUE if a font is a color font.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_color_font(IntPtr f);

        /// <summary>
        /// Returns TRUE if a font is a monochrome font.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int is_mono_font(IntPtr f);

        /// <summary>
        /// Extracts a range of characters from a font.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr extract_font_range(IntPtr f, int begin, int end);

        /// <summary>
        /// Transposes all characters in a font.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int transpose_font(IntPtr f, int drange);

        /// <summary>
        /// Merges two fonts into one font.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr merge_fonts(IntPtr f1, IntPtr f2);

        #endregion Fonts

        #region Text output

        /* font - A simple 8x8 fixed size font.
         * allegro_404_char - Character used when Allegro cannot find a glyph.
         * text_length - Returns the length of a string in pixels.
         * text_height - Returns the height of a font in pixels.
         * textout_ex - Writes a string on a bitmap.
         * textout_centre_ex - Writes a centered string on a bitmap.
         * textout_right_ex - Writes a right aligned string on a bitmap.
         * textout_justify_ex - Draws justified text within a region.
         * textprintf_ex - Formatted output of a string.
         * textprintf_centre_ex - Formatted centered output of a string.
         * textprintf_right_ex - Formatted right aligned output of a string.
         * textprintf_justify_ex - Formatted justified output of a string.
         */

        /// <summary>
        /// A simple 8x8 fixed size font.
        /// </summary>
        public static IntPtr font
        {
            get
            {
                return Marshal.ReadIntPtr(GetAddress("font"));
            }
            set
            {
                Marshal.WriteIntPtr(GetAddress("font"), value);
            }
        }

        /// <summary>
        /// Character used when Allegro cannot find a glyph.
        /// </summary>
        public static int allegro_404_char
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("allegro_404_char"));
            }
        }

        /// <summary>
        /// Returns the length of a string in pixels.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int text_length(IntPtr f, string str);

        /// <summary>
        /// Returns the height of a font in pixels.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int text_height(IntPtr f);

        /// <summary>
        /// Writes a string on a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textout_ex(IntPtr bmp, IntPtr f, string s, int x, int y, int color, int bg);

        /// <summary>
        /// Writes a centered string on a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textout_centre_ex(IntPtr bmp, IntPtr f, string s, int x, int y, int color, int bg);

        /// <summary>
        /// Writes a right aligned string on a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textout_right_ex(IntPtr bmp, IntPtr f, string s, int x, int y, int color, int bg);

        /// <summary>
        /// Draws justified text within a region.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textout_justify_ex(IntPtr bmp, IntPtr f, string s, int x1, int x2, int y, int diff, int color, int bg);

        /// <summary>
        /// Formatted output of a string.
        /// </summary>
        // TODO: research varargs
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textprintf_ex(IntPtr bmp, IntPtr f, int x, int y, int color, int bg, string fmt);

        /// <summary>
        /// Formatted centered output of a string.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textprintf_centre_ex(IntPtr bmp, IntPtr f, int x, int y, int color, int bg, string fmt);

        /// <summary>
        /// Formatted right aligned output of a string.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textprintf_right_ex(IntPtr bmp, IntPtr f, int x, int y, int color, int bg, string fmt);

        /// <summary>
        /// Formatted justified output of a string.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void textprintf_justify_ex(IntPtr bmp, IntPtr f, int x1, int x2, int y, int diff, int color, int bg, string fmt);

        #endregion Text output

        #region Polygon rendering

        /* POLYTYPE_FLAT - Polygon rendering mode type
         * POLYTYPE_GCOL - Polygon rendering mode type
         * POLYTYPE_GRGB - Polygon rendering mode type
         * POLYTYPE_ATEX - Polygon rendering mode type
         * POLYTYPE_PTEX - Polygon rendering mode type
         * POLYTYPE_ATEX_MASK
         * POLYTYPE_PTEX_MASK - Polygon rendering mode type
         * POLYTYPE_ATEX_LIT
         * POLYTYPE_PTEX_LIT - Polygon rendering mode type
         * POLYTYPE_ATEX_MASK_LIT
         * POLYTYPE_PTEX_MASK_LIT - Polygon rendering mode type
         * POLYTYPE_ATEX_TRANS
         * POLYTYPE_PTEX_TRANS - Polygon rendering mode type
         * POLYTYPE_ATEX_MASK_TRANS
         * POLYTYPE_PTEX_MASK_TRANS - Polygon rendering mode type
         * polygon3d
         * polygon3d_f - Draws a 3d polygon onto the specified bitmap.
         * triangle3d
         * triangle3d_f - Draws a 3d triangle onto the specified bitmap.
         * quad3d
         * quad3d_f - Draws a 3d quad onto the specified bitmap.
         * clip3d_f - Clips the polygon given in vtx using floating point math,
         * clip3d - Clips the polygon given in vtx using fixed point math.
         */

        /// <summary>
        /// Draws a 3d polygon onto the specified bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void polygon3d_f(IntPtr bmp, int type, IntPtr texture, int vc, IntPtr vtx);
        public static extern void polygon3d_f(IntPtr bmp, int type, IntPtr texture, int vc, IntPtr vtx);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quad3d(IntPtr bmp, int type, IntPtr tex, ref V3D v1, ref V3D v2, ref V3D v3, ref V3D v4);

        /// <summary>
        /// Draws a 3d quad onto the specified bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quad3d_f(IntPtr bmp, int type, IntPtr tex, ref V3D_f v1, ref V3D_f v2, ref V3D_f v3, ref V3D_f v4);

        /// <summary>
        /// Clips the polygon given in vtx using fixed point math.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int clip3d_f(int type, float min_z, float max_z, int vc, IntPtr[] vtx, IntPtr[] vout, IntPtr[] vtmp, int[] _out);
        public static extern int clip3d_f(int type, float min_z, float max_z, int vc, IntPtr vtx, IntPtr vout, IntPtr vtmp, IntPtr _out);

        /* create_zbuffer - Creates a Z-buffer for a bitmap.
         * create_sub_zbuffer - Creates a sub-z-buffer.
         * set_zbuffer - Makes the given Z-buffer the active one.
         * clear_zbuffer - Writes a depth value into the given Z-buffer.
         * destroy_zbuffer - Destroys a Z-buffer.
         */

        /// <summary>
        /// Creates a Z-buffer for a bitmap.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_zbuffer(IntPtr bmp);

        /// <summary>
        /// Creates a sub-z-buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_sub_zbuffer(IntPtr parent, int x, int y, int width, int height);

        /// <summary>
        /// Makes the given Z-buffer the active one.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_zbuffer(IntPtr zbuf);

        /// <summary>
        /// Writes a depth value into the given Z-buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clear_zbuffer(IntPtr zbuf, float z);

        /// <summary>
        /// Destroys a Z-buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_zbuffer(IntPtr zbuf);

        /* create_scene - Allocates memory for a 3d scene.
         * clear_scene - Initializes a scene.
         * destroy_scene - Deallocates the memory used by a scene.
         * scene_polygon3d
         * scene_polygon3d_f - Puts a polygon in the scene rendering list.
         * render_scene - Renders all the queued scene polygons.
         * scene_gap - Number controlling the scene z-sorting algorithm behaviour.
         */

        /// <summary>
        /// Allocates memory for a 3d scene.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int create_scene(int nedge, int npoly);

        /// <summary>
        /// Initializes a scene.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void clear_scene(IntPtr bmp);

        /// <summary>
        /// Deallocates the memory used by a scene.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_scene();

        /// <summary>
        /// Puts a polygon in the scene rendering list.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int scene_polygon3d_f(int type, IntPtr texture, int vc, IntPtr vtx);

        /// <summary>
        /// Renders all the queued scene polygons.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void render_scene();

        #endregion Polygon rendering

        #region Transparency and patterned drawing

        /* drawing_mode - Sets the graphics drawing mode.
         * xor_mode - Shortcut for toggling xor drawing mode on and off.
         * solid_mode - Shortcut for selecting solid drawing mode.
         */

        /// <summary>
        /// Sets the graphics drawing mode
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void drawing_mode(int mode, IntPtr pattern, int x_anchor, int y_anchor);

        /// <summary>
        /// Shortcut for toggling xor drawing mode on and off.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void xor_mode(int on);

        /// <summary>
        /// Shortcut for selecting solid drawing mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void solid_mode();

        /* color_map - Global pointer to the color mapping table.
         * create_trans_table - Fills a color mapping table for translucency effects.
         * create_light_table - Fills a color mapping table for lighting effects.
         * create_color_table - Fills a color mapping table for customised effects.
         * create_blender_table - Emulates truecolor blender effects in palettised modes.
         */

        /* set_trans_blender - Enables a truecolor blender.
         * set_alpha_blender - Enables a special alpha-channel blending mode.
         * set_write_alpha_blender - Enables the special alpha-channel editing mode.
         * set_add_blender - Enables an additive blender mode.
         * set_burn_blender - Enables a burn blender mode.
         * set_color_blender - Enables a color blender mode.
         * set_difference_blender - Enables a difference blender mode.
         * set_dissolve_blender - Enables a dissolve blender mode.
         * set_dodge_blender - Enables a dodge blender mode.
         * set_hue_blender - Enables a hue blender mode.
         * set_invert_blender - Enables an invert blender mode.
         * set_luminance_blender - Enables a luminance blender mode.
         * set_multiply_blender - Enables a multiply blender mode.
         * set_saturation_blender - Enables a saturation blender mode.
         * set_screen_blender - Enables a screen blender mode.
         * set_blender_mode - Specifies a custom set of truecolor blender routines.
         * set_blender_mode_ex - An even more complex version of set_blender_mode().
         */

        /// <summary>
        /// Enables a truecolor blender.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_trans_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a special alpha-channel blending mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_alpha_blender();

        /// <summary>
        /// Enables the special alpha-channel editing mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_write_alpha_blender();

        /// <summary>
        /// Enables an additive blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_add_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a burn blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_burn_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a color blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_color_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a difference blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_difference_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a dissolve blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_dissolve_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a dodge blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_dodge_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a hue blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_hue_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables an invert blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_invert_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a luminance blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_luminance_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a multiply blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_multiply_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a saturation blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_saturation_blender(int r, int g, int b, int a);

        /// <summary>
        /// Enables a screen blender mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_screen_blender(int r, int g, int b, int a);

        /// <summary>
        /// Specifies a custom set of truecolor blender routines
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void set_blender_mode(BLENDER_FUNC b15, BLENDER_FUNC b16, BLENDER_FUNC b24, int r, int g, int b, int a);

        /// <summary>
        /// An even more complex version of set_blender_mode().
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void set_blender_mode_ex(BLENDER_FUNC b15, BLENDER_FUNC b16, BLENDER_FUNC b24, BLENDER_FUNC b32, BLENDER_FUNC b15x, BLENDER_FUNC b16x, BLENDER_FUNC b24x, int r, int g, int b, int a);

        #endregion Transparency and patterned drawing

        #region Convert between color formats

        /* bestfit_color - Finds a palette color fitting the requested RGB values.
         * rgb_map - Look up table to speed up reducing RGB values to palette colors.
         * create_rgb_table - Generates an RGB mapping table with lookup data for a palette.
         * hsv_to_rgb
         * rgb_to_hsv - Converts color values between the HSV and RGB colorspaces.
         */

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void hsv_to_rgb(float h, float s, float v, out int r, out int g, out int b);

        /// <summary>
        /// Converts color values between the HSV and RGB colorspaces.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rgb_to_hsv(int r, int g, int b, ref float h, ref float s, ref float v);

        #endregion Convert between color formats

        #region Direct access to video memory

        /* bmp_write_line - Direct access bank switching line selection for writing.
         * bmp_read_line - Direct access bank switching line selection for reading.
         * bmp_unwrite_line - Direct access bank switching line release.
         */

        /// <summary>
        /// Direct access bank switching line selection for writing.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint bmp_write_line(IntPtr bmp, int line);

        /// <summary>
        /// Direct access bank switching line selection for reading.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong bmp_read_line(IntPtr bmp, int line);

        /// <summary>
        /// Direct access bank switching line release.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bmp_unwrite_line(IntPtr bmp);

        #endregion Direct access to video memory

        #region FLIC routines

        /* play_fli - Plays a FLI or FLC animation from disk.
         * play_memory_fli - Plays a FLI or FLC animation from memory.
         * open_fli
         * open_memory_fli - Makes a FLI file open and ready for playing.
         * close_fli - Closes a FLI file previously opened.
         * next_fli_frame - Reads the next frame of the current animation file.
         * fli_bitmap - Contains the current frame of the animation.
         * fli_palette - Contains the current palette of the animation.
         * fli_bmp_dirty_from
         * fli_bmp_dirty_to - Indicate which parts of the image have changed.
         * fli_pal_dirty_from
         * fli_pal_dirty_to - Indicate which parts of the palette have changed.
         * reset_fli_variables - Resets the bitmap and palette dirty global variables.
         * fli_frame - Stores the current frame number of the animation.
         * fli_timer - Global variable for timing FLI playback.
         */

        /// <summary>
        /// Plays a FLI or FLC animation from disk.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int play_fli(string filename, BITMAP bmp, int loop, IntPtr callback);

        #endregion FLIC routines

        #region Sound init routines

        /* detect_digi_driver - Detects whether the specified digital sound device is available.
         * detect_midi_driver - Detects whether the specified MIDI sound device is available.
         * reserve_voices - Reserve a number of voices for the digital and MIDI drivers.
         * set_volume_per_voice - Sets the volume of a voice.
         * install_sound - Initialises the sound module.
         * remove_sound - Cleans up after you are finished with the sound routines.
         * set_volume - Alters the global sound output volume.
         * set_hardware_volume - Alters the hardware sound output volume.
         * get_volume - Retrieves the global sound output volume.
         * get_hardware_volume - Retrieves the hardware sound output volume.
         */

        /// <summary>
        /// Detects whether the specified digital sound device is available.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int detect_digi_driver(int driver_id);

        /// <summary>
        /// Detects whether the specified MIDI sound device is available.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int detect_midi_driver(int driver_id);

        /// <summary>
        /// Reserve a number of voices for the digital and MIDI drivers.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void reserve_voices(int digi_voices, int midi_voices);

        /// <summary>
        /// Sets the volume of a voice.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_volume_per_voice(int scale);

        /// <summary>
        /// Initialises the sound module.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int install_sound(int digi, int midi, string cfg_path);

        /// <summary>
        /// Cleans up after you are finished with the sound routines.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void remove_sound();

        /// <summary>
        /// Alters the global sound output volume.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_volume(int digi_volume, int midi_volume);

        /// <summary>
        /// Alters the hardware sound output volume.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_hardware_volume(int digi_volume, int midi_volume);

        /// <summary>
        /// Retrieves the global sound output volume.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_volume(out int digi_volume, out int midi_volume);

        /// <summary>
        /// Retrieves the hardware sound output volume.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_hardware_volume(out int digi_volume, out int midi_volume);

        #endregion Sound init routines

        #region Mixer routines

        /* set_mixer_quality - Sets the resampling quality of the mixer.
         * get_mixer_quality - Returns the current mixing quality.
         * get_mixer_frequency - Returns the mixer frequency, in Hz.
         * get_mixer_bits - Returns the mixer bitdepth (8 or 16).
         * get_mixer_channels - Returns the number of output channels.
         * get_mixer_voices - Returns the number of voices allocated to the mixer.
         * get_mixer_buffer_length - Returns the number of samples per channel in the mixer buffer.
         */

        /// <summary>
        /// Sets the resampling quality of the mixer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mixer_quality(int quality);

        /// <summary>
        /// Returns the current mixing quality.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_quality();

        /// <summary>
        /// Returns the mixer frequency, in Hz.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_frequency();

        /// <summary>
        /// Returns the mixer bitdepth (8 or 16).
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_bits();

        /// <summary>
        /// Returns the number of output channels.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_channels();

        /// <summary>
        /// Returns the number of voices allocated to the mixer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_voices();

        /// <summary>
        /// Returns the number of samples per channel in the mixer buffer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_mixer_buffer_length();

        #endregion Mixer routines

        #region Digital sample routines

        /* load_sample - Loads a sample from a file.
         * load_wav - Loads a sample from a RIFF WAV file.
         * load_wav_pf - Packfile version of load_wav.
         * load_voc - Loads a sample from a Creative Labs VOC file.
         * load_voc_pf - Packfile version of load_voc.
         * save_sample - Writes a sample into a file.
         * create_sample - Constructs a new sample structure of the specified type.
         * destroy_sample - Destroys a sample structure when you are done with it.
         * lock_sample - Locks all the memory used by a sample.
         * register_sample_file_type - Registers custom loading/saving sample routines.
         * play_sample - Plays a sample.
         * adjust_sample - Alters the parameters of a sample while it is playing.
         * stop_sample - Kills off a sample.
         */

        /// <summary>
        /// Loads a sample from a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_sample(string filename);

        /// <summary>
        /// Loads a sample from a RIFF WAV file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_wav(string filename);

        /// <summary>
        /// Packfile version of load_wav.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_wav_pf(IntPtr f);

        /// <summary>
        /// Loads a sample from a Creative Labs VOC file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_voc(string filename);

        /// <summary>
        /// Packfile version of load_voc.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_voc_pf(IntPtr f);

        /// <summary>
        /// Writes a sample into a file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int save_sample(string filename, IntPtr spl);

        /// <summary>
        /// Constructs a new sample structure of the specified type.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_sample(int bits, int stereo, int freq, int len);

        /// <summary>
        /// Destroys a sample structure when you are done with it.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_sample(IntPtr spl);

        /// <summary>
        /// Locks all the memory used by a sample.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock_sample(IntPtr spl);

        /// <summary>
        /// Registers custom loading/saving sample routines.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void register_sample_file_type(string ext, SampleLoadCallback load, SampleSaveCallback save);

        /// <summary>
        /// Plays a sample.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int play_sample(IntPtr spl, int vol, int pan, int freq, int loop);

        /// <summary>
        /// Alters the parameters of a sample while it is playing.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void adjust_sample(IntPtr spl, int vol, int pan, int freq, int loop);

        /// <summary>
        /// Kills off a sample.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stop_sample(IntPtr spl);

        /* allocate_voice - Allocates a soundcard voice for a sample.
         * deallocate_voice - Frees a soundcard voice.
         * reallocate_voice - Switches the sample of an already-allocated voice.
         * release_voice - Releases a soundcard voice.
         * voice_start - Activates a voice.
         * voice_stop - Stops a voice.
         * voice_set_priority - Sets the priority of a voice.
         * voice_check - Checks whether a voice is currently allocated.
         * voice_get_position - Returns the current position of a voice.
         * voice_set_position - Sets the position of a voice.
         * voice_set_playmode - Adjusts the loop status of the specified voice.
         * voice_get_volume - Returns the current volume of the voice.
         * voice_set_volume - Sets the volume of the voice.
         * voice_ramp_volume - Starts a volume ramp for a voice.
         * voice_stop_volumeramp - Interrupts a volume ramp operation.
         * voice_get_frequency - Returns the current pitch of the voice.
         * voice_set_frequency - Sets the pitch of the voice.
         * voice_sweep_frequency - Starts a frequency sweep for a voice.
         * voice_stop_frequency_sweep - Interrupts a frequency sweep operation.
         * voice_get_pan - Returns the current pan position.
         * voice_set_pan - Sets the pan position.
         * voice_sweep_pan - Starts a pan sweep for a voice.
         * voice_stop_pan_sweep - Interrupts a pan sweep operation.
         * voice_set_echo - Sets the echo parameters for a voice.
         * voice_set_tremolo - Sets the tremolo parameters for a voice.
         * voice_set_vibrato - Sets the vibrato parameters for a voice.
         */

        /// <summary>
        /// Activates a voice.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void voice_start(int voice);

        /// <summary>
        /// Stops a voice.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void voice_stop(int voice);

        #endregion Digital sample routines

        #region Music routines (MIDI)

        /* load_midi - Loads a MIDI file.
         * destroy_midi - Destroys a MIDI structure when you are done with it.
         * lock_midi - Locks all the memory used by a MIDI file.
         * play_midi - Starts playing the specified MIDI file.
         * play_looped_midi - Starts playing a MIDI file with a user-defined loop position.
         * stop_midi - Stops whatever music is currently playing.
         * midi_pause - Pauses the MIDI player.
         * midi_resume - Resumes playback of a paused MIDI file.
         * midi_seek - Seeks to the given midi_pos in the current MIDI file.
         * get_midi_length - Determines the total playing time of a midi, in seconds.
         * midi_out - Streams a block of MIDI commands into the player.
         * load_midi_patches - Forces the MIDI driver to load a set of patches.
         * midi_pos - Stores the current position in the MIDI file.
         * midi_time - The current position in the MIDI file, in seconds.
         * midi_loop_start
         * midi_loop_end - Loop start and end points, set by play_looped_midi().
         * midi_msg_callback
         * midi_meta_callback
         * midi_sysex_callback - Hook functions allowing you to intercept MIDI player events.
         * load_ibk - Reads in a .IBK patch definition file for the Adlib driver.
         */

        /// <summary>
        /// Loads a MIDI file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_midi(string filename);

        /// <summary>
        /// Destroys a MIDI structure when you are done with it.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_midi(IntPtr midi);

        /// <summary>
        /// Locks all the memory used by a MIDI file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock_midi(IntPtr midi);

        /// <summary>
        /// Starts playing the specified MIDI file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int play_midi(IntPtr midi, int loop);

        /// <summary>
        /// Starts playing a MIDI file with a user-defined loop position.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int play_looped_midi(IntPtr midi, int loop_start, int loop_end);

        /// <summary>
        /// Stops whatever music is currently playing.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stop_midi();

        /// <summary>
        /// Pauses the MIDI player.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void midi_pause();

        /// <summary>
        /// Resumes playback of a paused MIDI file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void midi_resume();

        /// <summary>
        /// Seeks to the given midi_pos in the current MIDI file.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int midi_seek(int target);

        /// <summary>
        /// Determines the total playing time of a midi, in seconds.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_midi_length(IntPtr midi);

        /// <summary>
        /// Streams a block of MIDI commands into the player.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void midi_out(char[] data, int length);

        /// <summary>
        /// Forces the MIDI driver to load a set of patches.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int load_midi_patches();

        /// <summary>
        /// Stores the current position in the MIDI file.
        /// </summary>
        public static long midi_pos
        {
            get
            {
                return Marshal.ReadInt64(GetAddress("midi_pos"));
            }
        }

        /// <summary>
        /// The current position in the MIDI file, in seconds.
        /// </summary>
        public static long midi_time
        {
            get
            {
                return Marshal.ReadInt64(GetAddress("midi_time"));
            }
        }

        public static long midi_loop_start
        {
            get
            {
                return Marshal.ReadInt64(GetAddress("midi_loop_start"));
            }
        }

        /// <summary>
        /// Loop start and end points, set by play_looped_midi().
        /// </summary>
        public static long midi_loop_end
        {
            get
            {
                return Marshal.ReadInt64(GetAddress("midi_loop_end"));
            }
        }

        public static MidiMsgCallback midi_msg_callback
        {
            get
            {
                // TODO: to be implemented
                return null;
            }
        }

        public static MidiMetaCallback midi_meta_callback
        {
            get
            {
                // TODO: to be implemented
                return null;
            }
        }

        /// <summary>
        /// Hook functions allowing you to intercept MIDI player events.
        /// </summary>
        public static MidiSysexCallback midi_sysex_callback
        {
            get
            {
                // TODO: to be implemented
                return null;
            }
        }

        /// <summary>
        /// Reads in a .IBK patch definition file for the Adlib driver.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int load_ibk(string filename, int drums);

        #endregion Music routines (MIDI)

        #region Audio stream routines

        /* play_audio_stream - Creates a new audio stream and starts playing it.
         * stop_audio_stream - Destroys an audio stream when it is no longer required.
         * get_audio_stream_buffer - Tells you if you need to fill the audiostream or not.
         * free_audio_stream_buffer - Tells the audio stream player new data can be played.
         */

        /// <summary>
        /// Creates a new audio stream and starts playing it.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr play_audio_stream(int len, int bits, int stereo, int freq, int vol, int pan);

        /// <summary>
        /// Destroys an audio stream when it is no longer required.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void stop_audio_stream(IntPtr stream);

        /// <summary>
        /// Tells you if you need to fill the audiostream or not.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern byte* get_audio_stream_buffer(IntPtr stream);

        /// <summary>
        /// Tells the audio stream player new data can be played.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_audio_stream_buffer(IntPtr stream);

        #endregion Audio stream routines

        #region Recording routines

        /* install_sound_input - Initialises the sound recorder module.
         * remove_sound_input - Cleans up after you are finished with the sound input routines.
         * get_sound_input_cap_bits - Checks which audio input sample formats are supported.
         * get_sound_input_cap_stereo - Tells if the input driver is capable of stereo recording.
         * get_sound_input_cap_rate - Returns the maximum sample frequency for recording.
         * get_sound_input_cap_parm - Detects if the specified recording parameters are supported.
         * set_sound_input_source - Selects the audio input source.
         * start_sound_input - Starts recording in the specified format.
         * stop_sound_input - Stops audio recording.
         * read_sound_input - Retrieves the last recorded audio buffer.
         * digi_recorder - Hook notifying you when a new sample buffer becomes available.
         * midi_recorder - Hook notifying you when new MIDI data becomes available.
         */

        #endregion Recording routines

        #region File and compression routines

        /* get_executable_name - Obtains the full path to the current executable.
         * fix_filename_case - Converts a filename to a standardised case.
         * fix_filename_slashes - Converts all the directory separators to a standard character.
         * canonicalize_filename - Converts any filename into its canonical form.
         * make_absolute_filename - Makes an absolute filename from a path and relative filename.
         * make_relative_filename - Tries to make a relative filename from absolute path and filename.
         * is_relative_filename - Returns TRUE if the filename is relative.
         * replace_filename - Replaces path+filename with a new filename tail.
         * replace_extension - Replaces filename+extension with a new extension tail.
         * append_filename - Concatenates a filename to a path.
         * get_filename - Returns a pointer to the filename portion of a path.
         * get_extension - Returns a pointer to the extension of a filename.
         * put_backslash - Puts a path separator at the end of a path if needed.
         * file_exists - Tells if a file exists.
         * exists - Shortcut version of file_exists() for normal files.
         * file_size_ex - Returns the size of a file in bytes.
         * file_time - Returns the modification time of a file.
         * delete_file - Removes a file from the disk.
         * for_each_file_ex - Executes callback() for each file matching a wildcard.
         * al_findfirst - Low-level function for searching files.
         * al_findnext - Finds the next file in a search started by al_findfirst().
         * al_findclose - Closes a previously opened search with al_findfirst().
         * al_ffblk_get_size - Get size of file returned by al_findfirst/al_findnext.
         * find_allegro_resource - Searches for a support file in many places.
         * set_allegro_resource_path - Sets a specific resource search path.
         * packfile_password - Sets the global I/O encryption password.
         * pack_fopen - Opens a file according to mode.
         * pack_fopen_vtable
         * pack_fclose - Closes a stream previously opened.
         * pack_fseek - Seeks inside a stream.
         * pack_feof - Returns nonzero as soon as you reach the end of the file.
         * pack_ferror - Tells if an error occurred during an operation on the stream.
         * pack_getc - Returns the next character from a stream.
         * pack_putc - Puts a character in the stream.
         * pack_igetw - Like pack_getc(), but using 16-bit Intel byte ordering words.
         * pack_iputw - Like pack_putc(), but using 16-bit Intel byte ordering words.
         * pack_igetl - Like pack_getc(), but using 32-bit Intel byte ordering words.
         * pack_iputl - Like pack_putc(), but using 32-bit Intel byte ordering words.
         * pack_mgetw - Like pack_getc(), but using 16-bit Motorola byte ordering words.
         * pack_mputw - Like pack_putc(), but using 16-bit Motorola byte ordering words.
         * pack_mgetl - Like pack_getc(), but using 32-bit Motorola byte ordering words.
         * pack_mputl - Like pack_putc(), but using 32-bit Motorola byte ordering words.
         * pack_fread - Reads n bytes from the stream.
         * pack_fwrite - Writes n bytes to the stream.
         * pack_fgets - Reads a line from the stream.
         * pack_fputs - Writes a string to the stream.
         * pack_fopen_chunk - Opens a sub-chunk of a file.
         * pack_fclose_chunk - Closes a previously opened sub-chunk.
         * create_lzss_pack_data - Creates an LZSS structure for compression.
         * free_lzss_pack_data - Frees an LZSS structure.
         * lzss_write - Compresses data using LZSS.
         * create_lzss_unpack_data - Creates an LZSS structure for decompression.
         * free_lzss_unpack_data - Frees an LZSS structure.
         * lzss_read - Decompresses data using LZSS.
         */

        /// <summary>
        /// Replaces path+filename with a new filename tail.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string replace_filename(byte[] dest, string path, string filename, int size);

        /// <summary>
        /// Returns a pointer to the filename portion of a path.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder get_filename(string path);

        /// <summary>
        /// Returns a pointer to the extension of a filename.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder get_extension(string filename);

        /// <summary>
        /// Returns the size of a file in bytes.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int file_size_ex(string filename);

        /// <summary>
        /// Opens a file according to mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pack_fopen(string filename, string mode);

        /// <summary>
        /// Opens a file according to mode.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr pack_fopen_vtable(IntPtr vtable, IntPtr userdata);

        /// <summary>
        /// Closes a stream previously opened.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pack_fclose(IntPtr f);

        /// <summary>
        /// Seeks inside a stream.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pack_fseek(IntPtr f, int offset);

        /// <summary>
        /// Reads n bytes from the stream.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int pack_fread(IntPtr p, int n, IntPtr f);

        #endregion File and compression routines

        #region Datafile routines

        /* load_datafile - Loads a datafile into memory.
         * load_datafile_callback - Loads a datafile into memory, calling a hook per object.
         * unload_datafile - Frees all the objects in a datafile.
         * load_datafile_object - Loads a specific object from a datafile.
         * unload_datafile_object - Frees an object previously loaded by load_datafile_object().
         * find_datafile_object - Searches a datafile for an object with a name.
         * create_datafile_index - Creates an index for a datafile.
         * load_datafile_object_indexed - Loads a single object from a datafile index.
         * destroy_datafile_index - Destroys a datafile index.
         * get_datafile_property - Returns the property string for the object.
         * register_datafile_object - Registers load/destroy functions for custom object types.
         * fixup_datafile - Fixes truecolor images in compiled datafiles.
         * DAT_ID - Makes an ID value from four letters.
         */

        /// <summary>
        /// Loads a datafile into memory.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_datafile(string filename);

        /// <summary>
        /// Loads a datafile into memory, calling a hook per object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_datafile_callback(string filename, DatafileCallback callback);

        /// <summary>
        /// Frees all the objects in a datafile.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unload_datafile(IntPtr dat);

        /// <summary>
        /// Loads a specific object from a datafile.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_datafile_object(string filename, string objectname);

        /// <summary>
        /// Frees an object previously loaded by load_datafile_object().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unload_datafile_object(IntPtr dat);

        /// <summary>
        /// Searches a datafile for an object with a name.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr find_datafile_object(IntPtr dat, string objectname);

        /// <summary>
        /// Creates an index for a datafile.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr create_datafile_index(string filename);

        /// <summary>
        /// Loads a single object from a datafile index.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr load_datafile_object_indexed(IntPtr index, int item);

        /// <summary>
        /// Destroys a datafile index.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void destroy_datafile_index(IntPtr index);

        /// <summary>
        /// Returns the property string for the object.
        /// </summary>
        // TODO: replace with StringBuilder and test
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string get_datafile_property(IntPtr dat, int type);

        /// <summary>
        /// Registers load/destroy functions for custom object types.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void register_datafile_object(int id, DatafileLoadCallback load, DatafileLoadCallback destroy);

        /// <summary>
        /// Fixes truecolor images in compiled datafiles.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void fixup_datafile(IntPtr data);

        /// <summary>
        /// Makes an ID value from four letters.
        /// </summary>
        public static int DAT_ID(char a, char b, char c, char d)
        {
            return AL_ID(a, b, c, d);
        }

        #endregion Datafile routines

        #region Fixed point math routines

        /* itofix - Converts an integer to fixed point.
         * fixtoi - Converts a fixed point to integer with rounding.
         * fixfloor - Returns the greatest integer not greater than x.
         * fixceil - Returns the smallest integer not less than x.
         * ftofix - Converts a floating point value to fixed point.
         * fixtof - Converts a fixed point to floating point.
         * fixmul - Multiplies two fixed point values together.
         * fixdiv - Fixed point division.
         * fixadd - Safe function to add fixed point numbers clamping overflow.
         * fixsub - Safe function to subtract fixed point numbers clamping underflow.
         */

        /// <summary>
        /// Converts an integer to fixed point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int itofix(int x);

        /// <summary>
        /// Converts a fixed point to integer with rounding.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixtoi(int x);

        /// <summary>
        /// Returns the greatest integer not greater than x.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixfloor(int x);

        /// <summary>
        /// Returns the smallest integer not less than x.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixceil(int x);

        /// <summary>
        /// Converts a floating point value to fixed point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ftofix(double x);

        /// <summary>
        /// Converts a fixed point to floating point.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern double fixtof(int x);

        /// <summary>
        /// Multiplies two fixed point values together.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixmul(int x, int y);

        /// <summary>
        /// Fixed point division.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixdiv(int x, int y);

        /// <summary>
        /// Safe function to add fixed point numbers clamping overflow.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixadd(int x, int y);

        /// <summary>
        /// Safe function to subtract fixed point numbers clamping underflow.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixsub(int x, int y);

        /* fixtorad_r - Constant to convert angles in fixed point format to radians.
         * radtofix_r - Constant to convert radians to fixed point angles.
         * fixsin - Fixed point sine of binary angles.
         * fixcos - Fixed point cosine of binary angles.
         * fixtan - Fixed point tangent of binary angles.
         * fixasin - Fixed point inverse sine lookup table.
         * fixacos - Fixed point inverse cosine lookup table.
         * fixatan - Fixed point inverse tangent lookup table.
         * fixatan2 - Fixed point version of the libc atan2() routine.
         * fixsqrt - Fixed point square root.
         * fixhypot - Fixed point hypotenuse.
         */

        /// <summary>
        /// Constant to convert angles in fixed point format to radians.
        /// </summary>
        public static int fixtorad_r
        {
            get
            {
                return (int)Marshal.ReadInt32(GetAddress("fixtorad_r"));
            }
        }

        /// <summary>
        /// Constant to convert radians to fixed point angles.
        /// </summary>
        public static int radtofix_r
        {
            get
            {
                return (int)Marshal.ReadInt32(GetAddress("radtofix_r"));
            }
        }

        /// <summary>
        /// Fixed point sine of binary angles.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixsin(int x);

        /// <summary>
        /// Fixed point cosine of binary angles.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixcos(int x);

        /// <summary>
        /// Fixed point tangent of binary angles.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixtan(int x);

        /// <summary>
        /// Fixed point inverse sine lookup table.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixasin(int x);

        /// <summary>
        /// Fixed point inverse cosine lookup table.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixacos(int x);

        /// <summary>
        /// Fixed point inverse tangent lookup table.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixatan(int x);

        /// <summary>
        /// Fixed point version of the libc atan2() routine.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixatan2(int y, int x);

        /// <summary>
        /// Fixed point square root.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixsqrt(int x);

        /// <summary>
        /// Fixed point hypotenuse.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int fixhypot(int x, int y);

        #endregion Fixed point math routines

        #region 3D math routines

        /* identity_matrix
         * identity_matrix_f - Global containing the identity matrix.
         * get_translation_matrix
         * get_translation_matrix_f - Constructs a translation matrix.
         * get_scaling_matrix
         * get_scaling_matrix_f - Constructs a scaling matrix.
         * get_x_rotate_matrix
         * get_x_rotate_matrix_f - Construct X axis rotation matrices.
         * get_y_rotate_matrix
         * get_y_rotate_matrix_f - Construct Y axis rotation matrices.
         * get_z_rotate_matrix
         * get_z_rotate_matrix_f - Construct Z axis rotation matrices.
         * get_rotation_matrix
         * get_rotation_matrix_f - Constructs X, Y, Z rotation matrices.
         * get_align_matrix - Rotates a matrix to align it along specified coordinate vectors.
         * get_align_matrix_f - Floating point version of get_align_matrix().
         * get_vector_rotation_matrix
         * get_vector_rotation_matrix_f - Constructs X, Y, Z rotation matrices with an angle.
         * get_transformation_matrix - Constructs X, Y, Z rotation matrices with an angle and scaling.
         * get_transformation_matrix_f - Floating point version of get_transformation_matrix().
         * get_camera_matrix - Constructs a camera matrix for perspective projection.
         * get_camera_matrix_f - Floating point version of get_camera_matrix().
         * qtranslate_matrix
         * qtranslate_matrix_f - Optimised routine for translating an already generated matrix.
         * qscale_matrix
         * qscale_matrix_f - Optimised routine for scaling an already generated matrix.
         * matrix_mul
         * matrix_mul_f - Multiplies two matrices.
         * vector_length
         * vector_length_f - Calculates the length of a vector.
         * normalize_vector
         * normalize_vector_f - Converts the vector to a unit vector.
         * dot_product
         * dot_product_f - Calculates the dot product.
         * cross_product
         * cross_product_f - Calculates the cross product.
         * polygon_z_normal
         * polygon_z_normal_f - Finds the Z component of the normal vector to three vertices.
         * apply_matrix
         * apply_matrix_f - Multiplies a point by a transformation matrix.
         * set_projection_viewport - Sets the viewport used to scale the output of persp_project().
         * persp_project
         * persp_project_f - Projects a 3d point into 2d screen space.
         */

        public static MATRIX identity_matrix
        {
            get
            {
                return (MATRIX)Marshal.PtrToStructure(GetAddress("identity_matrix"), typeof(MATRIX));
            }
        }

        public static MATRIX identity_matrix_f
        {
            get
            {
                return (MATRIX)Marshal.PtrToStructure(GetAddress("identity_matrix_f"), typeof(MATRIX));
            }
        }

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_translation_matrix(ref MATRIX m, int x, int y, int z);

        /// <summary>
        /// Constructs a translation matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_translation_matrix_f(ref MATRIX_f m, float x, float y, float z);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_scaling_matrix(ref MATRIX m, int x, int y, int z);

        /// <summary>
        /// Constructs a scaling matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_scaling_matrix_f(ref MATRIX_f m, float x, float y, float z);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_x_rotate_matrix(ref MATRIX m, int r);

        /// <summary>
        /// Construct X axis rotation matrices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_x_rotate_matrix_f(ref MATRIX_f m, float r);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_y_rotate_matrix(ref MATRIX m, int r);

        /// <summary>
        /// Construct Y axis rotation matrices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_y_rotate_matrix_f(ref MATRIX_f m, float r);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_z_rotate_matrix(ref MATRIX m, int r);

        /// <summary>
        /// Construct Z axis rotation matrices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_z_rotate_matrix_f(ref MATRIX_f m, float r);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_rotation_matrix(ref MATRIX m, int x, int y, int z);

        /// <summary>
        /// Constructs X, Y, Z rotation matrices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_rotation_matrix_f(ref MATRIX_f m, float x, float y, float z);

        /// <summary>
        /// Rotates a matrix to align it along specified coordinate vectors.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_align_matrix(ref MATRIX m, int xfront, int yfront, int zfront, int xup, int yup, int zup);

        /// <summary>
        /// Floating point version of get_align_matrix().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_align_matrix_f(ref MATRIX m, float xfront, float yfront, float zfront, float xup, float yup, float zup);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_vector_rotation_matrix(ref MATRIX m, int x, int y, int z, int a);

        /// <summary>
        /// Constructs X, Y, Z rotation matrices with an angle.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_vector_rotation_matrix_f(ref MATRIX_f m, float x, float y, float z, float a);

        /// <summary>
        /// Constructs X, Y, Z rotation matrices with an angle and scaling.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_transformation_matrix(ref MATRIX m, int scale, int xrot, int yrot, int zrot, int x, int y, int z);

        /// <summary>
        /// Floating point version of get_transformation_matrix().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_transformation_matrix_f(ref MATRIX_f m, float scale, float xrot, float yrot, float zrot, float x, float y, float z);

        /// <summary>
        /// Constructs a camera matrix for perspective projection.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_camera_matrix(ref MATRIX m, int x, int y, int z, int xfront, int yfront, int zfront, int xup, int yup, int zup, int fov, int aspect);

        /// <summary>
        /// Floating point version of get_camera_matrix().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_camera_matrix_f(ref MATRIX_f m, float x, float y, float z, float xfront, float yfront, float zfront, float xup, float yup, float zup, float fov, float aspect);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void qtranslate_matrix(ref MATRIX m, int x, int y, int z);

        /// <summary>
        /// Optimised routine for translating an already generated matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void qtranslate_matrix_f(ref MATRIX_f m, float x, float y, float z);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void qscale_matrix(ref MATRIX m, int scale);

        /// <summary>
        /// Optimised routine for scaling an already generated matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void qscale_matrix_f(ref MATRIX_f m, float scale);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void matrix_mul(ref MATRIX m1, ref MATRIX m2, out MATRIX m3);

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void matrix_mul_f(ref MATRIX_f m1, ref MATRIX_f m2, out MATRIX_f m3);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vector_length(int x, int y, int z);

        /// <summary>
        /// Calculates the length of a vector.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern float vector_length_f(float x, float y, float z);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void normalize_vector(ref int x, ref int y, ref int z);

        /// <summary>
        /// Converts the vector to a unit vector.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void normalize_vector_f(ref float x, ref float y, ref float z);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int dot_product(int x1, int y1, int z1, int x2, int y2, int z2);

        /// <summary>
        /// Calculates the dot product.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern float dot_product_f(float x1, float y1, float z1, float x2, float y2, float z2);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cross_product(int x1, int y1, int z1, int x2, int y2, int z2, ref int xout, ref int yout, ref int zout);

        /// <summary>
        /// Calculates the cross product.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void cross_product_f(float x1, float y1, float z1, float x2, float y2, float z2, ref float xout, ref float yout, ref float zout);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int polygon_z_normal(ref V3D v1, ref V3D v2, ref V3D v3);

        /// <summary>
        /// Finds the Z component of the normal vector to three vertices.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern float polygon_z_normal_f(ref V3D_f v1, ref V3D_f v2, ref V3D_f v3);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void apply_matrix(ref MATRIX m, int x, int y, int z, ref int xout, ref int yout, ref int zout);

        /// <summary>
        /// Multiplies a point by a transformation matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void apply_matrix_f(ref MATRIX_f m, float x, float y, float z, ref float xout, ref float yout, ref float zout);

        /// <summary>
        /// Sets the viewport used to scale the output of persp_project().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_projection_viewport(int x, int y, int w, int h);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void persp_project(int x, int y, int z, ref int xout, ref int yout);

        /// <summary>
        /// Projects a 3d point into 2d screen space.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void persp_project_f(float x, float y, float z, ref float xout, ref float yout);

        #endregion 3D math routines

        #region Quaternion math routines

        /* identity_quat - Global variable containing the identity quaternion.
     * get_x_rotate_quat
     * get_y_rotate_quat
     * get_z_rotate_quat - Construct axis rotation quaternions.
     * get_rotation_quat - Constructs a quaternion to rotate points around all three axes.
     * get_vector_rotation_quat - Constructs a quaternion to rotate points around a vector.
     * quat_to_matrix - Constructs a rotation matrix from a quaternion.
     * matrix_to_quat - Constructs a quaternion from a rotation matrix.
     * quat_mul - Multiplies two quaternions.
     * apply_quat - Multiplies a point by a quaternion.
     * quat_interpolate - Constructs a quaternion representing a rotation between from and to.
     * quat_slerp - Version of quat_interpolate() allowing control over the rotation.
     */

        /// <summary>
        /// Global variable containing the identity quaternion.
        /// </summary>
        public static QUAT identity_quat
        {
            get
            {
                return (QUAT)Marshal.PtrToStructure(GetAddress("identity_quat"), typeof(QUAT));
            }
        }

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_x_rotate_quat(ref QUAT q, float r);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_y_rotate_quat(ref QUAT q, float r);

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_z_rotate_quat(ref QUAT q, float r);

        /// <summary>
        /// Constructs a quaternion to rotate points around all three axes.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_rotation_quat(ref QUAT q, float x, float y, float z);

        /// <summary>
        /// Constructs a quaternion to rotate points around a vector.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_vector_rotation_quat(ref QUAT q, float x, float y, float z, float a);

        /// <summary>
        /// Constructs a rotation matrix from a quaternion.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quat_to_matrix(ref QUAT q, ref MATRIX_f m);

        /// <summary>
        /// Constructs a quaternion from a rotation matrix.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void matrix_to_quat(ref MATRIX_f m, ref QUAT q);

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quat_mul(ref QUAT p, ref QUAT q, out QUAT r);

        /// <summary>
        /// Multiplies a point by a quaternion.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void apply_quat(ref QUAT q, float x, float y, float z, ref float xout, ref float yout, ref float zout);

        /// <summary>
        /// Constructs a quaternion representing a rotation between from and to.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void quat_interpolate(ref QUAT from, ref QUAT to, float t, out QUAT q);
        public static void quat_interpolate(ref QUAT from, ref QUAT to, float t, out QUAT _out)
        {
            quat_slerp(ref (from), ref (to), (t), out (_out), QUAT_SHORT);
        }

        /// <summary>
        /// PVersion of quat_interpolate() allowing control over the rotation.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void quat_slerp(ref QUAT from, ref QUAT to, float t, out QUAT q, int how);

        #endregion Quaternion math routines

        #region GUI routines

        /* d_clear_proc - Dialog procedure to clear the screen.
         * d_box_proc
         * d_shadow_box_proc - Dialog procedure drawing boxes onto the screen.
         * d_bitmap_proc - Dialog procedure drawing a bitmap.
         * d_text_proc
         * d_ctext_proc
         * d_rtext_proc - Dialogs procedure drawing text onto the screen.
         * d_button_proc - Dialog procedure implementing a button object.
         * d_check_proc - Dialog procedure implementing a check box object.
         * d_radio_proc - Dialog procedure implementing a radio button object.
         * d_icon_proc - Dialog procedure implementing a bitmap button.
         * d_keyboard_proc - Invisible dialog procedure for implementing keyboard shortcuts.
         * d_edit_proc - Dialog procedure implementing an editable text object.
         * d_list_proc - Dialog procedure implementing a list box object.
         * d_text_list_proc - Dialog procedure implementing a list box object with type ahead.
         * d_textbox_proc - Dialog procedure implementing a text box object.
         * d_slider_proc - Dialog procedure implementing a slider control object.
         * d_menu_proc - Dialog procedure implementing a menu bar object.
         * d_yield_proc - Invisible dialog procedure that yields CPU timeslices.
         */

        /// <summary>
        /// Dialog procedure to clear the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_clear_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure drawing boxes onto the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_box_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialogs procedure drawing text onto the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_text_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialogs procedure drawing text onto the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_ctext_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialogs procedure drawing text onto the screen.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_rtext_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure implementing a button object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_button_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure implementing a check box object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_check_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure implementing a radio button object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_radio_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure implementing an editable text object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_edit_proc(int msg, IntPtr d, int c);

        /// <summary>
        /// Dialog procedure implementing a slider control object.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int d_slider_proc(int msg, IntPtr d, int c);

        /* gui_mouse_focus - Tells if the input focus follows the mouse pointer.
         * gui_fg_color
         * gui_bg_color - The foreground and background colors for the standard dialogs.
         * gui_mg_color - The color used for displaying greyed-out dialog objects.
         * gui_font_baseline - Adjusts the keyboard shortcut underscores height.
         * gui_mouse_x
         * gui_mouse_y
         * gui_mouse_z
         * gui_mouse_b - Hook functions used by the GUI routines to access the mouse state.
         */

        /// <summary>
        /// Tells if the input focus follows the mouse pointer.
        /// </summary>
        public static int gui_mouse_focus
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gui_mouse_focus"));
            }
            set
            {
                Marshal.WriteInt32(GetAddress("gui_mouse_focus"), value);
            }
        }

        /// <summary>
        /// The foreground and background colors for the standard dialogs.
        /// </summary>
        public static int gui_fg_color
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gui_fg_color"));
            }
            set
            {
                Marshal.WriteInt32(GetAddress("gui_fg_color"), value);
            }
        }

        /// <summary>
        /// The foreground and background colors for the standard dialogs.
        /// </summary>
        public static int gui_bg_color
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gui_bg_color"));
            }
            set
            {
                Marshal.WriteInt32(GetAddress("gui_bg_color"), value);
            }
        }

        /// <summary>
        /// The color used for displaying greyed-out dialog objects.
        /// </summary>
        public static int gui_mg_color
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gui_mg_color"));
            }
            set
            {
                Marshal.WriteInt32(GetAddress("gui_mg_color"), value);
            }
        }

        /// <summary>
        /// Adjusts the keyboard shortcut underscores height.
        /// </summary>
        public static int gui_font_baseline
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("gui_font_baseline"));
            }
            set
            {
                Marshal.WriteInt32(GetAddress("gui_font_baseline"), value);
            }
        }

        /// <summary>
        /// Hook functions used by the GUI routines to access the mouse state.
        /// </summary>
        //[DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        //public static extern GuiMouseCallback gui_mouse_x();
        //public static extern int gui_mouse_x();

        // TODO: get not working
        public static GuiMouseCallback gui_mouse_x
        {
            get
            {
                IntPtr callback = GetAddress("gui_mouse_x");
                return (GuiMouseCallback)Marshal.GetDelegateForFunctionPointer(callback, typeof(GuiMouseCallback));
            }
            set
            {
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(value);
                Marshal.WriteInt32(GetAddress("gui_mouse_x"), callback.ToInt32());
            }
        }

        /// <summary>
        /// Hook functions used by the GUI routines to access the mouse state.
        /// </summary>
        /// // TODO: get not working
        public static GuiMouseCallback gui_mouse_b
        {
            get
            {
                return (GuiMouseCallback)Marshal.GetDelegateForFunctionPointer(GetAddress("gui_mouse_b"), typeof(GuiMouseCallback));
            }

            set
            {
                IntPtr callback = Marshal.GetFunctionPointerForDelegate(value);
                Marshal.WriteInt32(GetAddress("gui_mouse_b"), callback.ToInt32());
            }
        }

        /* gui_textout_ex - Draws a text string onto the screen with keyboard shortcut underbars.
         * gui_strlen - Returns the length of a string in pixels.
         * gui_set_screen - Changes the bitmap surface GUI routines draw to.
         * gui_get_screen - Returns the bitmap surface GUI routines draw to.
         * position_dialog - Moves an array of dialog objects to the specified position.
         * centre_dialog - Centers an array of dialog objects.
         * set_dialog_color - Sets the colors of an array of dialog objects.
         * find_dialog_focus - Searches the dialog for the object which has the input focus.
         * offer_focus - Offers the input focus to a particular object.
         * object_message - Sends a message to an object and returns the answer.
         * dialog_message - Sends a message to all the objects in an array.
         * broadcast_dialog_message - Broadcasts a message to all the objects in the active dialog.
         * do_dialog - Basic dialog manager function.
         * popup_dialog - do_dialog() used for popup dialogs.
         * init_dialog - Low level initialisation of a dialog.
         * update_dialog - Low level function to update a dialog player.
         * shutdown_dialog - Destroys a dialog player returned by init_dialog().
         * active_dialog - Global pointer to the most recent activated dialog.
         */

        /// <summary>
        /// Draws a text string onto the screen with keyboard shortcut underbars.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gui_textout_ex(IntPtr bmp, string s, int x, int y, int color, int bg, int centre);

        /// <summary>
        /// Moves an array of dialog objects to the specified position.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gui_set_screen(IntPtr bmp);

        /// <summary>
        /// Moves an array of dialog objects to the specified position.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gui_get_screen();

        /// <summary>
        /// Moves an array of dialog objects to the specified position.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void position_dialog(IntPtr dialog, int x, int y);

        /// <summary>
        /// Centers an array of dialog objects.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void centre_dialog(IntPtr dialog);

        /// <summary>
        /// Sets the colors of an array of dialog objects.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_dialog_color(IntPtr dialog, int fg, int bg);

        /// <summary>
        /// Searches the dialog for the object which has the input focus.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int find_dialog_focus(IntPtr dialog);

        /// <summary>
        /// Sends a message to an object and returns the answer.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int object_message(IntPtr dialog, int msg, int c);

        /// <summary>
        /// Sends a message to all the objects in an array.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int dialog_message(IntPtr dialog, int msg, int c, out int obj);

        /// <summary>
        /// Broadcasts a message to all the objects in the active dialog.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int broadcast_dialog_message(int msg, int c);

        /// <summary>
        /// Basic dialog manager function.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int do_dialog(IntPtr dialog, int focus_obj);

        /// <summary>
        /// do_dialog() used for popup dialogs.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int popup_dialog(IntPtr dialog, int focus_obj);

        /// <summary>
        /// Low level initialisation of a dialog.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr init_dialog(IntPtr dialog, int focus_obj);

        /// <summary>
        /// Low level function to update a dialog player.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int update_dialog(IntPtr player);

        /// <summary>
        /// Destroys a dialog player returned by init_dialog().
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int shutdown_dialog(IntPtr player);

        /* do_menu - Displays an animates a popup menu.
         * init_menu - Low level initialisation of a menu.
         * update_menu - Low level function to update a menu player.
         * shutdown_menu - Destroys a menu player object returned by init_menu().
         * active_menu - Global pointer to the most recent activated menu.
         * gui_menu_draw_menu
         * gui_menu_draw_menu_item - Hooks to modify the appearance of menus.
         * alert - Displays a popup alert box.
         * alert3 - Like alert(), but with three buttons.
         * file_select_ex - Displays the Allegro file selector with a caption.
         * gfx_mode_select - Displays the Allegro graphics mode selection dialog.
         * gfx_mode_select_ex - Extended version of the graphics mode selection dialog.
         * gfx_mode_select_filter - Even more extended version of the graphics mode selection dialog.
         * gui_shadow_box_proc
         * gui_ctext_proc
         * gui_button_proc
         * gui_edit_proc
         * gui_list_proc
         * gui_text_list_proc - Hooks to customise the look and feel of Allegro dialogs.
         */

        /// <summary>
        /// Displays an animates a popup menu.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int do_menu(IntPtr menu, int x, int y);

        /// <summary>
        /// Global pointer to the most recent activated menu.
        /// </summary>
        public static MENU active_menu
        {
            get
            {
                return Marshal.ReadIntPtr(GetAddress("active_menu"));
            }
            set
            {
                Marshal.WriteIntPtr(GetAddress("active_menu"), value);
            }
        }

        /// <summary>
        /// Displays a popup alert box.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int alert(string s1, string s2, string s3, string b1, string b2, int c1, int c2);

        /// <summary>
        /// Extended version of the graphics mode selection dialog.
        /// </summary>
        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gfx_mode_select_ex(ref int card, ref int w, ref int h, ref int color_depth);

        #endregion GUI routines

        #region Windows specific

        [DllImport(ALLEG_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void win_set_window(IntPtr wnd);

        #endregion Windows specific
    }
}