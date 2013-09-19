using System;

namespace sharpallegro
{
    public class FONT : ManagedPointer
    {
        private FONT(IntPtr pointer)
            : base(pointer)
        {
        }

        public IntPtr data
        {
            get
            {
                return ReadPointer(0);
            }
        }

        public int height
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }

        public IntPtr vtable
        {
            get
            {
                return ReadPointer(sizeof(Int32));
            }
        }

        public static implicit operator FONT(IntPtr pointer)
        {
            return new FONT(pointer);
        }
    }
}