using System;

namespace sharpallegro
{
    public class KEYBOARD_DRIVER : ManagedPointer
    {
        public KEYBOARD_DRIVER(IntPtr pointer)
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

        public int autorepeat
        {
            get
            {
                return ReadInt(4 * sizeof(Int32));
            }
        }

        //AL_METHOD(int,  init, (void));
        //AL_METHOD(void, exit, (void));
        //AL_METHOD(void, poll, (void));
        //AL_METHOD(void, set_leds, (int leds));
        //AL_METHOD(void, set_rate, (int delay, int rate));
        //AL_METHOD(void, wait_for_input, (void));
        //AL_METHOD(void, stop_waiting_for_input, (void));
        //AL_METHOD(int,  scancode_to_ascii, (int scancode));
        //AL_METHOD(AL_CONST char *, scancode_to_name, (int scancode));
    }
}
