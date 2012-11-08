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
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework.Tools  // <--- corrected namespace capitalisation and revised location
{
  /// <summary>
  /// This is an extremely basic and pretty much a "giant hack". It is meant to
  /// get us to the point that we could create the desired XxxxController initially
  /// without any donkey work. It obviouusly does *not* account for future changes
  /// to the form... this will come later on.
  /// </summary>
  internal class Program
  {
    private static void Main( string[] args )
    {
      //System.Diagnostics.Debugger.Break(); //Use this for force debugging

      CompilerFlags flags = new CompilerFlags();

      //generate a list of assemblies in the associated mvcmap
      var assemblies = new List<String>();

      string className = null;
      //This is new - we try to interpret the compiler params
      if ( args.Length == 0 )
      {
        Console.WriteLine( "USAGE - mvctool [options] classname" );
        Console.WriteLine( "\r\nOPTIONS:" );
        Console.WriteLine( " --abstract / -a : prefix controllers with \"Abstract\" prefix" );
        Console.WriteLine( " --partial-methods / -p : use partial methods (.Net 3.5+)" );
        Console.WriteLine( " -e : send the controller to event handlers" );
        Console.WriteLine( " -v : add pad code to protect list views" );
        Console.WriteLine( " -c : create a new action config file called \"default.mvcmap\" (this can't be used with any other params)" );
        Console.WriteLine( " -C : create a new action config file with view name passed (this can't be used with any other params)" );
        Console.WriteLine( " -r : use the default mvcmap when creating actions" );
        Console.WriteLine( " -R : use the mvcmap with the same name as the form passed when creating actions" );
        Console.WriteLine( " -d : append the .Designer tag to the file name (e.g. Form1.Designer.cs)" );
        Console.WriteLine( " -D : same as -d, but also creates a stub file if one doesn't exist." );
        //Console.WriteLine( " -i : ignore any RESX file in the same scope (do not link resources)" );
        Console.WriteLine();
        return;
      }
      else if ( args.Length > 1 )
      {
        bool foundFormName = false;

        if ( args[ 0 ].Contains( "-C" ) )
        {
          CreateNewActionConfig( String.Format( "{0}.mvcmap", args[ 1 ] ) );
          return;
        }



        foreach ( string arg in args )
        {
          Console.WriteLine( arg );

          if ( arg.StartsWith( "-" ) )
          {
            //assume it's a param
            if ( arg.Contains( "-a" ) )
              flags.IsAbstract = true; //okay, this is a bit of a cheat
            if ( arg.Contains( "-p" ) )
              flags.UsePartialMethods = true;
            if ( arg.Contains( "-e" ) )
              flags.PassControllerToEvents = true;
            if ( arg.Contains( "-v" ) )
              flags.ProtectListViews = true;
            if ( arg.Contains( "-r" ) )
            {
              flags.RestrictActions = true;
              flags.UseDefaultActionsFile = true;
            }
            if ( arg.Contains( "-d" ) )
            {
              flags.AppendDesignedToFilename = true;
            }
            if ( arg.Contains( "-D" ) )
            {
              flags.AppendDesignedToFilename = true;
              flags.CreateEmptyNonDesignedFile = true;
            }
            if ( arg.Contains( "-R" ) )
            {
              flags.RestrictActions = true;
              //flags.UseDefaultActionsFile = false; //this should default to false...
            }
            if ( arg.StartsWith( "-i=" ) )
            {
              string[] list = new StringBuilder( arg ).Replace( "-i=", String.Empty ).Replace( "\"", String.Empty ).ToString().Split( ',' );
              assemblies.AddRange( list );
            }
          }
          else
          {
            //assume it's the form name
            if ( foundFormName )
            {
              Console.WriteLine( String.Format( "Did not understand \"{0}\", already found \"{1}\", ignoring.", className, arg ) );
            }
            else
            {
              className = arg;
              foundFormName = true;
            }
          }
        }

        Console.WriteLine( flags );

        if ( !foundFormName )
        {
          Console.WriteLine( "Could not find the form name in the params! Aborted" );
          return;
        }
      }
      else if ( args.Length == 1 && args[ 0 ].Contains( "-c" ) )
      {
        CreateNewActionConfig( "default.mvcmap" );
        return;
      }
      else //assume one param is form name
      {
        //this fixes a bug where user was unable to create a non abstract controller
        flags.IsAbstract = false;
        flags.UsePartialMethods = false;
        flags.PassControllerToEvents = false;
        flags.ProtectListViews = false;
        className = args[ 0 ];
      }

      string outputAssemblyName = String.Format( "{0}_{1}.dll", className, DateTime.Now.Ticks );




      //we currently assume this is one param and that is the name of the class
      //we also assume the files will be named in a standard C# naming convention.
      //i.e. MainForm -> MainForm.Designer.cs
      if ( ControllerCreationEngine.Compile( className, outputAssemblyName, flags, assemblies ) )
      {
        //if we get here, we created the desired assembly above
        ControllerCreationEngine.Generate( className, outputAssemblyName, flags, assemblies );
      }
      else
      {
        Console.WriteLine( "Error! The file could not be generated." );
        return;
      }
    }

    /// <summary>
    /// Exprimental
    /// </summary>
    private static void CreateNewActionConfig( string name )
    {
      var path = System.IO.Path.Combine( System.Environment.CurrentDirectory, name );

      var config = new ViewActionMap( true );

      //save the config
      ViewActionMap.Save( config, path, true );
    }
  }
}