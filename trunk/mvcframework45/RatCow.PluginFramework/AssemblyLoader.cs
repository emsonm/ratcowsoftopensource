/*
 * Copyright 2012 Rat Cow Software and Matt Emson. All rights reserved.
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
using System.IO;

using System.Reflection;

namespace RatCow.PluginFramework
{
  public class AssemblyLoader
  {
    /// <summary>
    /// 
    /// </summary>
    public static Assembly LoadAssemblyWithDependencies( string assemblyName, string libPath )
    {
      Assembly result = null;

      //first we load the assembly for reflection
      var assembly = Assembly.ReflectionOnlyLoadFrom( assemblyName );

      var dependencies = new List<string>();

      foreach (var dependency in GetDependencies(assembly))
      {
        //we look in current directory and lib directory, otherwise assume GAC and don't add
        if (File.Exists(dependency))
          dependencies.Add(dependency);
        else if (File.Exists(Path.Combine(libPath, dependency)))
          dependencies.Add( Path.Combine( libPath, dependency ) ); 
        else
          {}  //assume GAC   
      }

      var resolver = new AssemblyResolver(dependencies);

      result = Assembly.LoadFrom(assemblyName);

      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Assembly LoadAssemblyWithDependencies( string assemblyName )
    {
      return LoadAssemblyWithDependencies( assemblyName, System.Environment.CurrentDirectory );
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerable<String> GetDependencies( string assemblyName )
    {
      //get the dependencies for a specific assembly
      var names = Assembly.ReflectionOnlyLoadFrom( assemblyName ).GetReferencedAssemblies();
      if ( names != null )
      {
        foreach ( var name in names )
        {
          yield return name.Name;
        }
      }
      else yield return String.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerable<String> GetDependencies( Assembly assembly )
    {
      //get the dependencies for a specific assembly
      var names = assembly.GetReferencedAssemblies();
      if ( names != null )
      {
        foreach ( var name in names )
        {
          yield return name.Name;
        }
      }
      else yield return String.Empty;
    }
  }
}
