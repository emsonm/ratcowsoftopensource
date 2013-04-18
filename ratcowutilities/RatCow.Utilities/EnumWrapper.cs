using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Utilities
{
  /// <summary>
  /// EnumWrapper is a class that wraps the enum definition used to save an enum value to EF4.3.x
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class EnumWrapper<T>
  {
    private T fValue;
    public T Value
    {
      get { return fValue; }
      set { fValue = value; }
    }

    public string StringValue
    {
      get { return fValue.ToString(); }
      set { fValue = (T)Enum.Parse( typeof( T ), value, true ); }
    }

    public byte ByteValue
    {
      get { return (byte)(object)fValue; }
      set { fValue = (T)(object)value; }
    }

    public static implicit operator EnumWrapper<T>( T p )
    {
      return new EnumWrapper<T> { Value = p };
    }

    public static implicit operator T( EnumWrapper<T> pw )
    {
      if ( pw == null ) return default(T);
      else return pw.fValue;
    }
  }
 
}
