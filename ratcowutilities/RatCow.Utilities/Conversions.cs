using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Utilities
{

  /// <summary>
  /// This call is a copy of the class in RatCow.MVCFramework.Mapping. The plan is to
  /// replace that instance with this one when I next do a release of the tools.
  /// 
  /// Also note the name change - *Helper has a special meaning in .Net, so renamed
  /// to avoid confusion...
  /// </summary>
  public sealed class Conversions
  {
    public static bool IsNumeric( object Expression )
    {
      double num;
      IConvertible convertible = Expression as IConvertible;
      if ( convertible == null )
      {
        char[] chArray = Expression as char[];
        if ( chArray == null )
        {
          return false;
        }
        Expression = new string( chArray );
      }

      TypeCode typeCode = convertible.GetTypeCode();
      if ( ( typeCode != TypeCode.String ) && ( typeCode != TypeCode.Char ) )
      {
        return IsOldNumericTypeCode( typeCode );
      }

      string str = convertible.ToString( null );
      return Double.TryParse( str, out num ); //simplified - NB, original code did more with HEX/OCTAL checking here.
    }

    internal static bool IsOldNumericTypeCode( TypeCode TypCode )
    {
      switch ( TypCode )
      {
        case TypeCode.Boolean:
        case TypeCode.SByte:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Byte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return true;
      }
      return false;
    }

    public static bool IsNumericType( Type type )
    {
      return (
        ( type == typeof( Boolean ) ) ||
        ( type == typeof( Byte ) ) ||
        ( type == typeof( UInt16 ) ) ||
        ( type == typeof( UInt32 ) ) ||
        ( type == typeof( UInt64 ) ) ||
        ( type == typeof( SByte ) ) ||
        ( type == typeof( Int16 ) ) ||
        ( type == typeof( Int32 ) ) ||
        ( type == typeof( Int64 ) ) ||
        ( type == typeof( Single ) ) ||
        ( type == typeof( Double ) ) ||
        ( type == typeof( Decimal ) ) );
    }

    public static bool IsIntegralType( Type type )
    {
      return (
        ( type == typeof( Boolean ) ) ||
        ( type == typeof( Byte ) ) ||
        ( type == typeof( UInt16 ) ) ||
        ( type == typeof( UInt32 ) ) ||
        ( type == typeof( UInt64 ) ) ||
        ( type == typeof( SByte ) ) ||
        ( type == typeof( Int16 ) ) ||
        ( type == typeof( Int32 ) ) ||
        ( type == typeof( Int64 ) ) );
    }

    public static bool IsRealType( Type type )
    {
      if ( ( type == typeof( Single ) ) || ( type == typeof( Double ) ) || ( type == typeof( Decimal ) ) )
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public static string ToString( object o, string def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else
          return o.ToString();
      }
      catch
      {
        return def;
      }
    }

    #region Unsigned integer

    public static UInt64 ToUInt64( string s, UInt64 def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return UInt64.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static UInt64 ToUInt64( object o, UInt64 def )
    {
      try
      {
        //this is winding me up, so I'm forced to fix it...
        //this will make it marginally slower I'm guessing...
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToUInt64( o );
      }
      catch
      {
        return def;
      }
    }

    public static UInt32 ToUInt32( string s, UInt32 def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return UInt32.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static UInt32 ToUInt32( object o, UInt32 def )
    {
      try
      {
        //this is winding me up, so I'm forced to fix it...
        //this will make it marginally slower I'm guessing...
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToUInt32( o );
      }
      catch
      {
        return def;
      }
    }

    public static UInt32 ToUInt16( string s, UInt16 def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return UInt16.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static UInt16 ToUInt16( object o, UInt16 def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToUInt16( o );
      }
      catch
      {
        return def;
      }
    }

    #endregion Unsigned integer

    #region Signed integer

    public static long ToInt64( string s, long def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return Int64.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static long ToInt64( object o, long def )
    {
      try
      {
        //this is winding me up, so I'm forced to fix it...
        //this will make it marginally slower I'm guessing...
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToInt64( o );
      }
      catch
      {
        return def;
      }
    }

    public static Int32 ToInt32( string s, int def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return Int32.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static Int32 ToInt32( object o, int def )
    {
      try
      {
        //this is winding me up, so I'm forced to fix it...
        //this will make it marginally slower I'm guessing...
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToInt32( o );
      }
      catch
      {
        return def;
      }
    }

    public static Int32 ToInt16( string s, Int16 def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return Int16.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    public static Int16 ToInt16( object o, Int16 def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToInt16( o );
      }
      catch
      {
        return def;
      }
    }

    #endregion Signed integer

    public static bool ToBoolean( object o, bool def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToBoolean( o );
      }
      catch
      {
        return def;
      }
    }

    #region DateTime

    public static DateTime ToDateTime( object o, DateTime def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToDateTime( o );
      }
      catch
      {
        return def;
      }
    }

    public static DateTime ToDateTime( string s, DateTime def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return DateTime.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    #endregion DateTime

    #region Double

    public static double ToDouble( object o, double def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToDouble( o );
      }
      catch
      {
        return def;
      }
    }

    public static double ToDouble( string s, double def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return Double.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    #endregion Double

    #region Decimal

    public static decimal ToDecimal( object o, decimal def )
    {
      try
      {
        if ( o == null || o == DBNull.Value )
          return def;
        else if ( o.GetType() == typeof( String ) && (string)o == String.Empty )
          return def;
        else
          return System.Convert.ToDecimal( o );
      }
      catch
      {
        return def;
      }
    }

    public static decimal ToDecimal( string s, decimal def )
    {
      try
      {
        if ( s == null || s == String.Empty )
          return def;
        else
          return Decimal.Parse( s );
      }
      catch
      {
        return def;
      }
    }

    #endregion Decimal
  }
}
