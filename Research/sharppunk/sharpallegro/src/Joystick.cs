using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    ///* information about a single joystick axis */
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct JOYSTICK_AXIS_INFO
    {
        public int pos;
        public int d1, d2;
        public string name;
    }

    ///* information about one or more axis (a slider or directional control) */
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct JOYSTICK_STICK_INFO
    {
        public int flags;
        public int num_axis;

        //JOYSTICK_AXIS_INFO axis[MAX_JOYSTICK_AXIS];
        public JOYSTICK_AXIS_INFO[] axis
        {
            get
            {
                return new JOYSTICK_AXIS_INFO[0];
            }
        }

        public string name;
    }

    ///* information about a joystick button */
    [StructLayout(LayoutKind.Sequential)]
    public struct JOYSTICK_BUTTON_INFO
    {
        public int b;
        public string name;
    }

    /* information about an entire joystick */

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct JOYSTICK_INFO
    {
        public int flags;
        public int num_sticks;
        public int num_buttons;

        //JOYSTICK_STICK_INFO stick[MAX_JOYSTICK_STICKS];
        public JOYSTICK_STICK_INFO[] stick
        {
            get
            {
                return new JOYSTICK_STICK_INFO[0];
            }
        }

        //JOYSTICK_BUTTON_INFO button[MAX_JOYSTICK_BUTTONS];
        public JOYSTICK_BUTTON_INFO[] button
        {
            get
            {
                return new JOYSTICK_BUTTON_INFO[0];
            }
        }
    }

    public class JOYSTICK_DRIVER : ManagedPointer
    {
        public JOYSTICK_DRIVER(IntPtr pointer)
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

        //AL_METHOD(int, init, (void));
        //AL_METHOD(void, exit, (void));
        //AL_METHOD(int, poll, (void));
        //AL_METHOD(int, save_data, (void));
        //AL_METHOD(int, load_data, (void));
        //AL_METHOD(AL_CONST char *, calibrate_name, (int n));
        //AL_METHOD(int, calibrate, (int n));
    }
}