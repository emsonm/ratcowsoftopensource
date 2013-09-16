using System;

namespace sharpallegro
{
  public class MOUSE_DRIVER : ManagedPointer
  {
    public MOUSE_DRIVER(IntPtr pointer)
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
        return ReadString(sizeof(Int32));
      }
    }

    public string ascii_name
    {
      get
      {
        return ReadString(sizeof(Int32));
      }
    }

    public static implicit operator MOUSE_DRIVER(IntPtr pointer)
    {
      return new MOUSE_DRIVER(pointer);
    }
  }
}
