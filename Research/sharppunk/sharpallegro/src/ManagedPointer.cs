using System;
using System.Runtime.InteropServices;

namespace sharpallegro
{
    [ComVisible(true)]
    public class ManagedPointer
    {
        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(@"kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpszLib);

        public ManagedPointer()
        {
            pointer = IntPtr.Zero;
        }

        public ManagedPointer(int size)
            : this(Alloc(size))
        {
        }

        public static IntPtr GetAddress(string lib, string name)
        {
            IntPtr handle = LoadLibrary(lib);
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

        public static IntPtr Offset(IntPtr src, int offset)
        {
            return new IntPtr(src.ToInt32() + offset);
        }

        public IntPtr Offset(int offset)
        {
            return Offset(pointer, offset);
        }

        public static IntPtr Alloc(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        public IntPtr ReadPointer(int position)
        {
            return Marshal.ReadIntPtr(Offset(pointer, position));
        }

        public string ReadString(int position)
        {
            return Marshal.PtrToStringAnsi(ReadPointer(position));
        }

        public byte ReadByte(int position)
        {
            return Marshal.ReadByte(Offset(pointer, position));
        }

        public Int16 ReadShort(int position)
        {
            return Marshal.ReadInt16(Offset(pointer, position));
        }

        public int ReadInt(int position)
        {
            return Marshal.ReadInt32(Offset(pointer, position));
        }

        //public T Read<T>(int position)
        //{
        //  return (T)Marshal.ReadByte(IntPtr.Zero);
        //}

        public void WritePointer(int position, IntPtr value)
        {
            Marshal.WriteIntPtr(Offset(pointer, position), value);
        }

        public void WriteString(int position, string value)
        {
            WritePointer(position, Marshal.StringToCoTaskMemAnsi(value));
        }

        public void WriteByte(int position, byte value)
        {
            Marshal.WriteByte(Offset(pointer, position), value);
        }

        public void WriteShort(int position, short value)
        {
            Marshal.WriteInt16(Offset(pointer, position), value);
        }

        public void WriteInt(int position, int value)
        {
            Marshal.WriteInt32(Offset(pointer, position), value);
        }

        public ManagedPointer(IntPtr pointer)
        {
            this.pointer = pointer;
        }

        public static bool operator true(ManagedPointer managed)
        {
            return managed.pointer != IntPtr.Zero;
        }

        public static bool operator false(ManagedPointer managed)
        {
            return managed.pointer == IntPtr.Zero;
        }

        public static bool operator !(ManagedPointer managed)
        {
            return managed.pointer == IntPtr.Zero;
        }

        public static implicit operator IntPtr(ManagedPointer managed)
        {
            return managed.pointer;
        }

        public static implicit operator ManagedPointer(IntPtr pointer)
        {
            return new ManagedPointer(pointer);
        }

        public ManagedPointer this[int index]
        {
            get
            {
                return new ManagedPointer(Offset(index * sizeof(Int32)));
            }
        }

        public IntPtr pointer;
    }

    public class UnmanagedArray : ManagedPointer
    {
        public UnmanagedArray(IntPtr pointer)
            : base(pointer)
        {
        }
    }

    public class UnmanagedByteArray : ManagedPointer
    {
        public UnmanagedByteArray(IntPtr pointer)
            : base(pointer)
        {
        }
    }

    // TODO: see wether to use a template
    public class ManagedPointerArray : ManagedPointer
    {
        public ManagedPointerArray(IntPtr pointer)
            : base(pointer)
        {
        }

        public ManagedPointerArray(int size)
            : this(Alloc(size * sizeof(Int32)))
        {
        }

        public int this[int index]
        {
            get
            {
                return ReadInt(index * sizeof(Int32));
            }

            set
            {
                WriteInt(index * sizeof(Int32), value);
            }
        }

        public static implicit operator ManagedPointerArray(IntPtr pointer)
        {
            return new ManagedPointerArray(pointer);
        }
    }

    public class ManagedBytePointerArray : ManagedPointer
    {
        public ManagedBytePointerArray(IntPtr pointer)
            : base(pointer)
        {
        }

        public ManagedBytePointerArray(int size)
            : this(Alloc(size * sizeof(byte)))
        {
        }

        public byte this[int index]
        {
            get
            {
                return ReadByte(index * sizeof(byte));
            }

            set
            {
                WriteByte(index * sizeof(byte), value);
            }
        }

        public static implicit operator ManagedBytePointerArray(IntPtr pointer)
        {
            return new ManagedBytePointerArray(pointer);
        }
    }

    // TODO: see wether to use a template
    public class ManagedPointerBidimensionalArray : ManagedPointerArray
    {
        private int w, h;

        public ManagedPointerBidimensionalArray(IntPtr pointer, int w, int h)
            : base(pointer)
        {
            this.w = w;
            this.h = h;
        }

        public byte this[int x, int y]
        {
            get
            {
                return ReadByte((y * w + x) * sizeof(byte));
            }

            set
            {
                WriteByte((y * w + x) * sizeof(byte), value);
            }
        }

        public IntPtr this[int index]
        {
            get
            {
                return Offset(index * w * sizeof(byte));
            }
        }
    }
}