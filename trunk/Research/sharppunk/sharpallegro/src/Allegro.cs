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
 * $Id: Allegro.cs 92 2009-10-06 16:16:14Z eugeniofavalli $
 * 
 */

using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    public class Allegro : AllegroAPI
    {
#if DEBUG
        new public static int set_gfx_mode(int card, int w, int h, int v_w, int v_h)
        {
            return AllegroAPI.set_gfx_mode(GFX_AUTODETECT_WINDOWED, w, h, v_w, v_h);
        }
#endif

        new public static GFX_MODE_LIST get_gfx_mode_list(int card)
        {
            IntPtr p = AllegroAPI.get_gfx_mode_list(card);
            GFX_MODE_LIST ret = (GFX_MODE_LIST)Marshal.PtrToStructure(p, typeof(GFX_MODE_LIST));
            // Saves the original unmanaged memory address
            ret.p = p;
            return ret;
        }

        public static void destroy_gfx_mode_list(GFX_MODE_LIST mode_list)
        {
            // Frees the memory allocated by allegro, the GC should take care of managed memory
            AllegroAPI.destroy_gfx_mode_list(mode_list.p);
        }

        new public static bool poll_scroll()
        {
            return AllegroAPI.poll_scroll() > 0;
        }

        new public unsafe static int install_keyboard()
        {
            int ret = AllegroAPI.install_keyboard();
            IntPtr hdl = LoadLibrary(ALLEG_DLL);
            if (hdl != IntPtr.Zero)
            {
                IntPtr addr = GetProcAddress(hdl, "key");
                if (addr != IntPtr.Zero)
                {
                    key = new Keys();
                    Keys.key = (byte*)addr.ToPointer();
                }
            }
            return ret;
        }

        /// <summary>
        /// Tells if there are keypresses waiting in the input buffer.
        /// </summary>
        new public static bool keypressed()
        {
            return (AllegroAPI.keypressed() != 0);
        }

        public static void create_color_table(IntPtr table, IntPtr pal, Delegate blend, IntPtr callback)
        {
            AllegroAPI.create_color_table(table, pal, Marshal.GetFunctionPointerForDelegate(blend), callback);
        }

        new public static string[] get_config_argv(string section, string name, ref int argc)
        {
            ManagedPointer mp = new ManagedPointer(AllegroAPI.get_config_argv(section, name, ref argc));
            string[] ret = new string[argc];
            for (int i = 0; i < argc; i++)
            {
                ret[i] = mp.ReadString(i * sizeof(Int32));
            }
            return ret;
        }

        new public static JOYSTICK_INFO[] joy
        {
            get
            {
                IntPtr p = GetAddress("joy");
                //GFX_MODE_LIST ret = ()Marshal.PtrToStructure(p, typeof(GFX_MODE_LIST));
                // Saves the original unmanaged memory address
                //ret.p = p;
                return null;
            }
        }

        new public static void persp_project(int x, int y, int z, out int xout, out int yout)
        {
            xout = fixmul(fixdiv(x, z), _persp_xscale) + _persp_xoffset;
            yout = fixmul(fixdiv(y, z), _persp_yscale) + _persp_yoffset;
        }

        public static int _persp_xscale
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("_persp_xscale"));
            }
        }

        public static int _persp_yscale
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("_persp_yscale"));
            }
        }

        public static int _persp_xoffset
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("_persp_xoffset"));
            }
        }

        public static int _persp_yoffset
        {
            get
            {
                return Marshal.ReadInt32(GetAddress("_persp_yoffset"));
            }
        }
    }
}
