/*
 * Copyright 2010 - 2012 Rat Cow Software and Matt Emson. All rights reserved.
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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace RatCow.MvcFramework.Tools
{
  public class ViewActionMap
  {
    public ViewActionMap()
    {
      GlobalMap = new ViewActions();
      ControlActionMap = new ViewControlActions();
      AssemblyDependencies = new ViewAssemblies();
    }

    /// <summary>
    /// Simplify getting the defaults
    /// </summary>
    public ViewActionMap( bool createDefaults )
      : this()
    {
      if ( createDefaults ) InitDefaults();
    }

    //global action list
    [XmlArray( "GlobalMap" )]
    [XmlArrayItem( "ViewAction" )]
    public ViewActions GlobalMap { get; set; }

    //specific controls (this augments, never overrides)
    [XmlArray( "ControlActionMap" )]
    [XmlArrayItem( "ViewControlAction" )]
    public ViewControlActions ControlActionMap { get; set; }

    [XmlArray( "AssemblyDependencies" )]
    [XmlArrayItem( "ViewAssembly" )]
    public ViewAssemblies AssemblyDependencies { get; set; }

    /// <summary>
    /// This is called to create a default file
    /// </summary>
    public virtual void InitDefaults()
    {
      GlobalMap.Add( new ViewAction() { EventName = "Click", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" } );

      var textBoxAction = new ViewActions();
      textBoxAction.Add( new ViewAction() { EventName = "TextChanged", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" } );
      ControlActionMap.Add( new ViewControlAction() { ControlType = "TextBox", ControlActions = textBoxAction } );

      var comboBoxAction = new ViewActions();
      comboBoxAction.Add( new ViewAction() { EventName = "CheckedChanged", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" } );
      ControlActionMap.Add( new ViewControlAction() { ControlType = "CheckBox", ControlActions = comboBoxAction } );
    }

    /// <summary>
    /// Utility function - this should be moved to a factory
    /// </summary>
    public static ViewActionMap Load( string filename )
    {
      XmlSerializer xmlSerializer = new XmlSerializer( typeof( ViewActionMap ) );

      ViewActionMap data = null;

      try
      {
        using ( var reader = new StreamReader( new FileStream( filename, FileMode.Open, FileAccess.Read ) ) )
        {
          data = (ViewActionMap)xmlSerializer.Deserialize( reader );
        }
      }
      catch ( Exception ex ) //should specialise this.
      {
        System.Diagnostics.Debug.WriteLine( ex.Message );
        data = null; //just to be sure
      }

      return data;
    }

    /// <summary>
    /// Utility function - this should be moved to a factory
    /// </summary>
    public static bool Save( ViewActionMap data, string filename, bool overwrite )
    {
      var path = Path.GetFullPath( filename ); //this *should* convert to a long path, except the file acces methods mess this up still...

      if ( File.Exists( path ) )
      {
        //we bail if we have the "don't overwrite" bit set
        if ( !overwrite ) return false;
        else
        {
          using ( var file = System.IO.File.OpenWrite( path ) )
          {
            file.SetLength( 0 ); //wipe out the file contents

            var xmlSerializer = new XmlSerializer( typeof( ViewActionMap ) );

            using ( StreamWriter writer = new StreamWriter( file ) )
            {
              xmlSerializer.Serialize( writer, data );
            }

            file.Close(); //avoid locking etc
          }
        }
      }
      else
      {
        var xmlSerializer = new XmlSerializer( typeof( ViewActionMap ) );

        using ( StreamWriter writer = System.IO.File.CreateText( filename ) )
        {
          xmlSerializer.Serialize( writer, data );
        }
      }
      return true;
    }
  }
}