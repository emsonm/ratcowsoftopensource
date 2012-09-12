using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.UKBankAccValidator
{

  public enum ModulusType { MOD10, MOD11, DBLAL };

  public class VocaRule
  {

    public VocaRule()
    {
    }

    public string GetCode()
    {
      return String.Format( "Start={0}, End={1}, Modulus=ModulusType.{2}, Exception={3}, U={4},V={5},W={6},X={7},Y={8},Z={9},A={10},B={11},C={12},D={13},E={14},F={15},G={16},H={17}",
        this.Start,
        this.End,
        this.Modulus.ToString(),
        this.Exception,
        this.U,
        this.V,
        this.W,
        this.X,
        this.Y,
        this.Z,
        this.A,
        this.B,
        this.C,
        this.D,
        this.E,
        this.F,
        this.G,
        this.H
      ); 
    }

    public VocaRule( string rule )
    {
      //rules contain extra spaces, and I'm lazy
      while ( rule.Contains( "  " ) )
        rule = rule.Replace( "  ", " " );

      //should now contain only one space between each element
      var data = rule.Split( ' ' );

      Start = Convert.ToInt32( data[ 0 ] );
      End = Convert.ToInt32( data[ 1 ] );
      switch ( data[ 2 ] )
      {
        case "MOD10":
          Modulus = ModulusType.MOD10;
          break;
        case "MOD11":
          Modulus = ModulusType.MOD11;
          break;
        case "DBLAL":
          Modulus = ModulusType.DBLAL;
          break;
      }

      U = Convert.ToInt32( data[ 03 ].Trim() );
      V = Convert.ToInt32( data[ 04 ].Trim() );
      W = Convert.ToInt32( data[ 05 ].Trim() );
      X = Convert.ToInt32( data[ 06 ].Trim() );
      Y = Convert.ToInt32( data[ 07 ].Trim() );
      Z = Convert.ToInt32( data[ 08 ].Trim() );
      A = Convert.ToInt32( data[ 09 ].Trim() );
      B = Convert.ToInt32( data[ 10 ].Trim() );
      C = Convert.ToInt32( data[ 11 ].Trim() );
      D = Convert.ToInt32( data[ 12 ].Trim() );
      E = Convert.ToInt32( data[ 13 ].Trim() );
      F = Convert.ToInt32( data[ 14 ].Trim() );
      G = Convert.ToInt32( data[ 15 ].Trim() );
      H = Convert.ToInt32( data[ 16 ].Trim() );

      if ( data.Length == 18 )
        Exception = Convert.ToInt32( data[ 17 ].Trim() );
    }

    public int Start { get; internal set; }
    public int End { get; internal set; }
    public ModulusType Modulus { get; internal set; }
    public int U { get; internal set; }
    public int V { get; internal set; }
    public int W { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public int Z { get; internal set; }
    public int A { get; internal set; }
    public int B { get; internal set; }
    public int C { get; internal set; }
    public int D { get; internal set; }
    public int E { get; internal set; }
    public int F { get; internal set; }
    public int G { get; internal set; }
    public int H { get; internal set; }
    public int Exception { get; internal set; }
  }
}
