/*
 * Copyright 2010 Rat Cow Software and Matt Emson. All rights reserved.
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
#if !CF_20
using System.Linq;
#endif
using System.Text;

using System.Reflection;

namespace RatCow.MvcFramework
{

  /// <summary>
  /// At some point i want to expand on this and cache the types maybe? Or at least, cache the property lookups.
  /// </summary>
  public class OutletAccessor
  {
    /// <summary>
    /// This is not strictly necessary - but it cuts down on typing and adds generic sugar.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetPropertyValue<T>(object instance, string name)
    {
      T result = default(T);

      try
      {
        Type targetType = instance.GetType();
        PropertyInfo pi = targetType.GetProperty(name);
        //run the property getter
        object o = pi.GetValue(instance, null);
#if !USE_COMPACTFRAMEWORK
        result = (T)Convert.ChangeType(o, result.GetType()); //could just cast here
#else
        result = (T)o; //CF - just do CAST here as it's a lot simpler
#endif
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message); //as I need "somewhere" to log this
        result = default(T); //just to be sure we didn't alter above - this is more "sanity" checking and is likely not required.
      }

      return result;
    }

    /// <summary>
    /// This is not strictly necessary - but it cuts down on typing and adds generic sugar.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool SetPropertyValue<T>(object instance, string name, T value)
    {
      bool result = false;

      try
      {
        Type targetType = instance.GetType();
        PropertyInfo pi = targetType.GetProperty(name);
        //run the property getter
        pi.SetValue(instance, value, null);
        result = true;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message); //as I need "somewhere" to log this
        result = false; //just to be sure we didn't alter above - this is more "sanity" checking and is likely not required.
      }

      return result;
    }
  }
}
