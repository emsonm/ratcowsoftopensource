/*
 * Copyright 2007 - 2012 Rat Cow Software and Matt Emson. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used
 *    to endorse or promote products derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RatCow.Utilities
{
  public sealed class EnumUtils
  {
    /// <summary>
    /// Gets the max value of the given enum
    /// </summary>
    public static int Max( object enumInstance )
    {
      try
      {
        Type enuType = enumInstance.GetType();

        FieldInfo[] fi = enuType.GetFields();

        Type filedType = fi[ 0 ].GetValue( enumInstance ).GetType();
        object o =
          Convert.ChangeType( fi[ fi.Length - 1 ].GetValue( enumInstance ), filedType );
        return Convert.ToInt32( o );
      }
      catch
      {
        return -1;
      }
    }

    /// <summary>
    /// Gets the min value of the given enum
    /// </summary>
    public static int Min( object enumInstance )
    {
      try
      {
        Type enuType = enumInstance.GetType();

        FieldInfo[] fi = enuType.GetFields();

        Type filedType = fi[ 0 ].GetValue( enumInstance ).GetType();
        object o =
          Convert.ChangeType( fi[ 1 ].GetValue( enumInstance ), filedType );
        return Convert.ToInt32( o );
      }
      catch
      {
        return -1;
      }
    }
  }

  public sealed class EnumUtils<EnumType>
  {
    /// <summary>
    /// Converts an string to the exact matching enum value..
    /// Returns false if fails
    /// </summary>
    public static bool StringToEnum( string invalue, out EnumType outvalue )
    {
      if ( Enum.IsDefined( typeof( EnumType ), invalue ) )
      {
        outvalue = (EnumType)( Enum.Parse( typeof( EnumType ), invalue ) );
        return true;
      }

      outvalue = default( EnumType );
      return false;
    }

    /// <summary>
    /// Attempts to do the same as "StringToEnum", but ignoring case
    /// </summary>
    public static bool StringToEnumI( string invalue, out EnumType outvalue )
    {
      object o = null;
      try
      {
        o = ( Enum.Parse( typeof( EnumType ), invalue, true ) );
      }
      catch
      {
        //this can fail if the value is not found...
        outvalue = default( EnumType );
        return false;
      }

      if ( o != null && o.GetType() == typeof( EnumType ) )
      {
        outvalue = (EnumType)o;
        return true;
      }

      outvalue = default( EnumType );
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool ObjectToEnum( object invalue, out EnumType outvalue )
    {
      object o = null;
      try
      {
        o = ( Enum.ToObject( typeof( EnumType ), invalue ) );
      }
      catch
      {
        //this can fail if the value is not found...
        outvalue = default( EnumType );
        return false;
      }

      if ( o != null && o.GetType() == typeof( EnumType ) )
      {
        outvalue = (EnumType)o;
        return true;
      }

      outvalue = default( EnumType );
      return false;
    }

    /// <summary>
    /// This is dumb, but here for completenaess
    /// </summary>
    public static string EnumToString( EnumType value )
    {
      return value.ToString();
    }

    /// <summary>
    /// Gets the integer value of an enumeration
    /// </summary>
    public static Int32 EnumToInt32( EnumType value )
    {
      Type t = Enum.GetUnderlyingType( typeof( EnumType ) );

      if (
            t == typeof( sbyte ) ||
            t == typeof( short ) ||
            t == typeof( int ) ||
            t == typeof( byte ) ||
            t == typeof( uint ) ||
            t == typeof( long ) ||
            t == typeof( ulong )
          )
      {
        object o = Enum.ToObject( typeof( EnumType ), value );
        return Convert.ToInt32( o );
      }

      return -1;
    }

    /// <summary>
    /// Converts the 32 bit int to an enum.
    /// </summary>
    public static bool Int32ToEnum( Int32 invalue, out EnumType outvalue )
    {
      object o = null;

      try
      {
        o = Enum.ToObject( typeof( EnumType ), invalue );

        if ( o != null && o.GetType() == typeof( EnumType ) )
        {
          outvalue = (EnumType)o;
          return true;
        }
      }
      catch { } // for our purposes, we don't really care...

      outvalue = default( EnumType );
      return false;
    }


  }
}


