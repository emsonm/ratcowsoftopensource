using System;

namespace sharpallegro
{
    public class DATAFILE_PROPERTY : ManagedPointer
    {
        public DATAFILE_PROPERTY(IntPtr pointer)
            : base(pointer)
        {
        }

        public IntPtr dat
        {
            get
            {
                return ReadPointer(0);
            }
        }

        public int type
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }
    }

    public class DATAFILE : ManagedPointer
    {
        public DATAFILE(IntPtr pointer)
            : base(pointer)
        {
        }

        public DATAFILE this[int index]
        {
            get
            {
                return new DATAFILE(Offset(pointer, 4 * index * sizeof(Int32)));
            }
        }

        public IntPtr dat
        {
            get
            {
                return ReadPointer(0);
            }
        }

        public int type
        {
            get
            {
                return ReadInt(sizeof(Int32));
            }
        }

        public int size
        {
            get
            {
                return ReadInt(2 * sizeof(Int32));
            }
        }

        public DATAFILE_PROPERTY prop
        {
            get
            {
                return new DATAFILE_PROPERTY(ReadPointer(3 * sizeof(Int32)));
            }
        }

        public static implicit operator DATAFILE(IntPtr pointer)
        {
            return new DATAFILE(pointer);
        }
    }
}