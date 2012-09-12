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

namespace RatCow.Utilities
{
  /// <summary>
  /// This call is a copy of the class in RatCow.MVCFramework.Mapping. The plan is to
  /// replace that instance with this one when I next do a release of the tools.
  /// 
  /// Also note the name change - *Helper has a special meaning in .Net, so renamed
  /// to avoid confusion...
  /// </summary>
  public sealed class GenericsUtils
  {
    /// <summary>
    /// This static method creates a basic generic class type from two inputs
    /// </summary>
    // usage: given a wish to create the type MyType<int> dynamically,
    //        basicGenericType would be : var basicGenericType = typeof(MyType<>);
    //        typeToUse would be        : var typeToUse = typeof(int);
    public static Type GetGenericType( Type basicGenericType, Type typeToUse )
    {
      Type[] typeArgs = { typeToUse }; //type we are mapping
      return GetGenericType( basicGenericType, typeArgs );
    }

    public static Type GetGenericType( Type basicGenericType, Type[] typeArgs )
    {
      Type mor = basicGenericType.MakeGenericType( typeArgs ); //mapping object result - this is a generic type
      return mor;
    }

    // usage: given a wish to create an instance of MyType<int> dynamically,
    //        basicGenericType would be : var basicGenericType = typeof(MyType<>);
    //        typeToUse would be        : var typeToUse = typeof(int);
    private static object InternalCreateGenericInstance( Type basicGenericType, Type typeToUse, object[] args )
    {
      Type got = GetGenericType( basicGenericType, typeToUse );
      object result = Activator.CreateInstance( got, args );
      return result;
    }

    private static object InternalCreateGenericInstance( Type basicGenericType, Type[] typeArgs, object[] args )
    {
      Type got = GetGenericType( basicGenericType, typeArgs );
      object result = Activator.CreateInstance( got, args );
      return result;
    }

    private static object CreateGenericInstance( Type basicGenericType, Type typeToUse, params object[] args )
    {
      return InternalCreateGenericInstance( basicGenericType, typeToUse, args );
    }

    public static object CreateGenericInstance( Type basicGenericType, Type[] typeArgs, params object[] args )
    {
      return InternalCreateGenericInstance( basicGenericType, typeArgs, args );
    }

    //this seems like a contradiction, but if you have an interface defined, this cleans the code up nicely
    public static T CreateGenericInstance<T>( Type basicGenericType, Type typeToUse, params object[] args )
    {
      return (T)InternalCreateGenericInstance( basicGenericType, typeToUse, args );
    }

    public static T CreateGenericInstance<T>( Type basicGenericType, Type[] typeArgs, params object[] args )
    {
      return (T)InternalCreateGenericInstance( basicGenericType, typeArgs, args );
    }
  }
}
