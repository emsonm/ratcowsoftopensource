using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using RatCow.UKBankAccValidator;

namespace generaterules
{
  class Program
  {
    static void Main( string[] args )
    {
      //assume the fist param in args is the file name
      var filein = File.OpenText( args[ 0 ] );

      StreamWriter fileout;
      //asume the second (if it exists) is where to put the file
      if ( args.Length == 2 )
        fileout = File.CreateText( Path.Combine( args[ 1 ], "Rules.cs" ) );
      else
        fileout = File.CreateText( "Rules.cs" );

      fileout.WriteLine( "using System;" );
      fileout.WriteLine( "namespace RatCow.UKBankAccValidator" );
      fileout.WriteLine( "{" );
      fileout.WriteLine( "  public static class Rules" );
      fileout.WriteLine( "  {" );
      fileout.WriteLine( "    public static Rule[] rules = new Rule[]" );
      fileout.WriteLine( "    {" );

      while ( true )
      {
        var line = filein.ReadLine();
        if ( line == null || line == String.Empty ) break;
        var rule = new VocaRule( line );
        fileout.WriteLine( "      new Rule(){ " + rule.GetCode() + " }," );

      }
      fileout.WriteLine( "      new Rule()" ); //dummy
      fileout.WriteLine( "    };" );
      fileout.WriteLine( "  }" );
      fileout.WriteLine( "}" );

      fileout.Close();

      filein.Close();
    }
  }
}
