using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.UKBankAccValidator
{

  public class AccountNumberUtils
  {
    //    public String[] Problems = {
    //      "01", // nat west
    //      "0900", // santander (090931 - 090136 ali, 090151 - 090156 migrated)
    //      "0919",
    //72 - Santander
    //08
    //0860 0861
    //0831 0821 co-operative

    //82 clydesdale
    //89-00 89-29 santander

    public static string ConvertAccount( string sortcode, string accountNumber )
    {
      if ( sortcode.StartsWith( "01" ) )
      {
        //apply NatWest rule
        if ( accountNumber.Length == 10 )
        {
          return sortcode + accountNumber.Substring( 2 );
        }
      }
      else if ( sortcode.StartsWith( "08" ) )
      {
        //cooperative bank rules? These are tricky, as the sort code 08 is used by oads of banks
        if ( accountNumber.Length == 10 )
        {
          return sortcode + accountNumber.Substring( 0, 8 );
        }
      }
      else if ( sortcode.StartsWith( "09" ) )
      {
        //santander rules?
        int code = Convert.ToInt32( sortcode.Substring( 0, 4 ) );

        if ( code >= 0900 && code <= 0919 )
        {
          if ( accountNumber.Length == 9 )
          {
            return sortcode.Substring( 0, 5 ) + accountNumber;
          }
        }
      }
      else if ( sortcode.StartsWith( "89" ) )
      {
        //santander
        int code = Convert.ToInt32( sortcode.Substring( 0, 4 ) );

        if ( code >= 8900 && code <= 8929 )
        {
          if ( accountNumber.Length == 9 )
          {
            return sortcode.Substring( 0, 5 ) + accountNumber;
          }
        }
      }

      return ReplaceSortCode(sortcode) + accountNumber;
    }

    internal class SortCodeSwap
    {
      public int Current { get; set; }
      public int Replacement { get; set; }
    }

    static SortCodeSwap[] swapList = new SortCodeSwap[]
    {
      new SortCodeSwap() {Current=938173, Replacement=938017 },
      new SortCodeSwap() {Current=938289, Replacement=938068},
      new SortCodeSwap() {Current=938297, Replacement=938076 },
      new SortCodeSwap() {Current=938600, Replacement=938611 },
      new SortCodeSwap() {Current=938602, Replacement=938343 },
      new SortCodeSwap() {Current=938604, Replacement=938603 },
      new SortCodeSwap() {Current=938608, Replacement=938408 },
      new SortCodeSwap() {Current=938609, Replacement=938424 },
      new SortCodeSwap() {Current=938613, Replacement=938017 },
      new SortCodeSwap() {Current=938616, Replacement=938068 },
      new SortCodeSwap() {Current=938618, Replacement=938657 },
      new SortCodeSwap() {Current=938620, Replacement=938343 },
      new SortCodeSwap() {Current=938622, Replacement=938130 },
      new SortCodeSwap() {Current=938628, Replacement=938181 },
      new SortCodeSwap() {Current=938643, Replacement=938246 },
      new SortCodeSwap() {Current=938647, Replacement=938611 },
      new SortCodeSwap() {Current=938648, Replacement=938246 },
      new SortCodeSwap() {Current=938649, Replacement=938394 },
      new SortCodeSwap() {Current=938651, Replacement=938335 },
      new SortCodeSwap() {Current=938653, Replacement=938424 },
      new SortCodeSwap() {Current=938654, Replacement=938621 }
    };

    public static string ReplaceSortCode( string sortcode )
    {
      var found = swapList.Where( x => x.Current == Convert.ToInt32( sortcode ) ).SingleOrDefault();
      if ( found == null )
        return sortcode;
      else return found.Replacement.ToString();
    }
  }

  public enum ModulusType { MOD10, MOD11, DBLAL };

  public class Rule
  {


    public Rule()
    {

    }

    public int Start { get; set; }
    public int End { get; set; }
    public ModulusType Modulus { get; set; }
    public int U { get; set; }
    public int V { get; set; }
    public int W { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
    public int E { get; set; }
    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int Exception { get; set; }
  }
}
