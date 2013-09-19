using System;

namespace sharpallegro
{
    public class SYSTEM_DRIVER : ManagedPointer
    {
        public SYSTEM_DRIVER(IntPtr pointer)
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

        public IntPtr gfx_drivers
        {
            get
            {
                return ReadPointer(32 * sizeof(Int32));
            }
            set
            {
                WritePointer(32 * sizeof(Int32), value);
            }
        }

        public IntPtr digi_drivers
        {
            get
            {
                return ReadPointer(33 * sizeof(Int32));
            }
        }

        public IntPtr midi_drivers
        {
            get
            {
                return ReadPointer(34 * sizeof(Int32));
            }
        }

        public AllegroAPI._DRIVER_INFO[] _gfx_drivers()
        {
            return AllegroAPI.get_drivers(gfx_drivers);
        }

        public AllegroAPI._DRIVER_INFO[] _digi_drivers()
        {
            return AllegroAPI.get_drivers(digi_drivers);
        }

        public AllegroAPI._DRIVER_INFO[] _midi_drivers()
        {
            return AllegroAPI.get_drivers(midi_drivers);
        }

        //int  id;
        //AL_CONST char *name;
        //AL_CONST char *desc;
        //AL_CONST char *ascii_name;
        //AL_METHOD(int, init, (void));
        //AL_METHOD(void, exit, (void));
        //AL_METHOD(void, get_executable_name, (char *output, int size));
        //AL_METHOD(int, find_resource, (char *dest, AL_CONST char *resource, int size));
        //AL_METHOD(void, set_window_title, (AL_CONST char *name));
        //AL_METHOD(int, set_close_button_callback, (AL_METHOD(void, proc, (void))));
        //AL_METHOD(void, message, (AL_CONST char *msg));
        //AL_METHOD(void, assert, (AL_CONST char *msg));
        //AL_METHOD(void, save_console_state, (void));
        //AL_METHOD(void, restore_console_state, (void));
        //AL_METHOD(struct BITMAP *, create_bitmap, (int color_depth, int width, int height));
        //AL_METHOD(void, created_bitmap, (struct BITMAP *bmp));
        //AL_METHOD(struct BITMAP *, create_sub_bitmap, (struct BITMAP *parent, int x, int y, int width, int height));
        //AL_METHOD(void, created_sub_bitmap, (struct BITMAP *bmp, struct BITMAP *parent));
        //AL_METHOD(int, destroy_bitmap, (struct BITMAP *bitmap));
        //AL_METHOD(void, read_hardware_palette, (void));
        //AL_METHOD(void, set_palette_range, (AL_CONST struct RGB *p, int from, int to, int retracesync));
        //AL_METHOD(struct GFX_VTABLE *, get_vtable, (int color_depth));
        //AL_METHOD(int, set_display_switch_mode, (int mode));
        //AL_METHOD(void, display_switch_lock, (int lock, int foreground));
        //AL_METHOD(int, desktop_color_depth, (void));
        //AL_METHOD(int, get_desktop_resolution, (int *width, int *height));
        //AL_METHOD(void, get_gfx_safe_mode, (int *driver, struct GFX_MODE *mode));
        //AL_METHOD(void, yield_timeslice, (void));
        //AL_METHOD(void *, create_mutex, (void));
        //AL_METHOD(void, destroy_mutex, (void *handle));
        //AL_METHOD(void, lock_mutex, (void *handle));
        //AL_METHOD(void, unlock_mutex, (void *handle));
        //AL_METHOD(_DRIVER_INFO *, gfx_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, digi_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, midi_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, keyboard_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, mouse_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, joystick_drivers, (void));
        //AL_METHOD(_DRIVER_INFO *, timer_drivers, (void));
    }
}