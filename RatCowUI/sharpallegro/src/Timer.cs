using System;
using System.Collections.Generic;
using System.Text;

namespace sharpallegro
{
  public class TIMER_DRIVER : ManagedPointer
  {
    public TIMER_DRIVER(IntPtr pointer)
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
  }
}
