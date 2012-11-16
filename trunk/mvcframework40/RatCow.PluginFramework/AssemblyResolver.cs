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

using System.Reflection;

namespace RatCow.PluginFramework
{
  public class AssemblyResolver
  {
    public List<String> AssemblyList { get; set; }
    List<Assembly> externals = null;

    public AssemblyResolver( IEnumerable<String> assemblyList )
    {
      AssemblyList = new List<string>();

      externals = new List<Assembly>();

      foreach ( var assembly in assemblyList )
      {
        AddAssembly( assembly );
      }

      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler( CurrentDomain_AssemblyResolve );
    }

    public Assembly AddAssembly( string assembly )
    {
      Assembly result = null;

      if ( AssemblyList.IndexOf( assembly ) <= 0 )
      {

        AssemblyList.Add( assembly );

        try
        {
          result = Assembly.LoadFrom( assembly );
          if ( result != null )
            externals.Add( result );
        }
        catch //this is a quick hack
        {
          result = Assembly.LoadWithPartialName( assembly );
          if ( result != null )
            externals.Add( result );
        }
      }

      return result;
    }

    public Assembly GetAssembly( string assemblyName )
    {
      Assembly result = externals.Where( x => x.FullName.Contains( assemblyName ) || x.FullName == assemblyName ).SingleOrDefault();
      if ( result != null )
        return result;
      else
        return null;
    }

    private Assembly CurrentDomain_AssemblyResolve( object sender, ResolveEventArgs args )
    {
      var result = GetAssembly( args.Name );
      if ( result == null )
      {
        AddAssembly( args.Name );
      }

      return result;
    }
  }

}
