using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.UKBankAccValidator
{
  public enum ValidationResult { Invalid, Valid, Notfound };

  public class Core
  {
    /// <summary>
    ///
    /// </summary>
    public static ValidationResult ValidateAccountNumber( int sortCode, int accountNumber )
    {
      //get the matching account numbers
      var rules = Rules.rules.Where( x => x.Start <= sortCode && x.End >= sortCode );

      if ( rules.Count() > 0 )
      {
        return ProcessRules( rules, sortCode, accountNumber );
      }

      return ValidationResult.Notfound; //we treat thes is "valid"
    }

    /// <summary>
    ///
    /// </summary>
    private static ValidationResult ProcessRules( IEnumerable<Rule> rules, int sortCode, int accountNumber )
    {
      ValidationResult result = ValidationResult.Invalid;

      var rulesa = rules.ToArray();

      string value;

      //sort out any exceptions

      //we use this to work out if we need to do the next rule
      bool exceptionrule0 = (
               rulesa[ 0 ].Exception == 02 ||
               rulesa[ 0 ].Exception == 09 ||
               rulesa[ 0 ].Exception == 10 ||
               rulesa[ 0 ].Exception == 11 ||
               rulesa[ 0 ].Exception == 12 ||
               rulesa[ 0 ].Exception == 13 ||
               rulesa[ 0 ].Exception == 14
      );

      Rule rule = rulesa[ 0 ];

      value = ApplyExceptionsToAccount( sortCode, accountNumber, rule );

      //we need to check for all zeros here
      if ( value == "00000000000000" )
      {
        return ValidationResult.Invalid;
      }

      Rule erule;

      #region Validation 1

      switch ( rule.Exception )
      {
        case 1:
          erule = Exception1Rule( rule );
          result = GetRuleResult( value, erule );
          break;

        case 2:
          if ( rulesa.Length == 2 && rulesa[ 1 ].Exception == 9 )
          {
            if ( GetAccountDigit( 'A', value ) != 0 && GetAccountDigit( 'G', value ) != 9 )
            {
              erule = Exception2aRule( rule );
              result = GetRuleResult( value, erule );
            }
            else if ( GetAccountDigit( 'A', value ) != 0 && GetAccountDigit( 'G', value ) == 9 )
            {
              erule = Exception2bRule( rule );
              result = GetRuleResult( value, erule );
            }
            else
              result = GetRuleResult( value, rule );
          }
          else
            result = GetRuleResult( value, rule );

          //no need for extra checks if it passes here
          if ( result == ValidationResult.Valid )
            return ValidationResult.Valid;

          break;

        case 6:
          var digit1 = GetAccountDigit( 'A', value );
          var digit2 = GetAccountDigit( 'G', value );
          var digit3 = GetAccountDigit( 'H', value );
          if ( ( digit1 >= 4 && digit1 <= 8 ) && ( digit2 == digit3 ) )
            return ValidationResult.Valid;
          else
            result = GetRuleResult( value, rule );
          break;

        case 7:
          if ( GetAccountDigit( 'G', value ) == 9 )
          {
            erule = Exception7Rule( rule );
            result = GetRuleResult( value, erule );
          }
          else
            result = GetRuleResult( value, rule );
          break;

        case 10:
          if ( rulesa.Length == 2 && rulesa[ 1 ].Exception == 11 )
          {
            var digit4 = ( GetAccountDigit( 'A', value ) * 10 ) + GetAccountDigit( 'B', value ); ;
            var digit5 = GetAccountDigit( 'G', value );
            if ( ( digit4 == 09 || digit4 == 99 ) && digit5 == 9 )
            {
              erule = Exception10Rule( rule );
              result = GetRuleResult( value, erule );
            }
            else
              result = GetRuleResult( value, rule );

            if ( result == ValidationResult.Valid )
              return ValidationResult.Valid;
          }
          else
            result = GetRuleResult( value, rule );
          break;

        case 12:
          if ( rulesa.Length == 2 && rulesa[ 1 ].Exception == 13 )
          {
            result = GetRuleResult( value, rule );

            if ( result == ValidationResult.Valid )
              return ValidationResult.Valid;
          }
          else
            result = GetRuleResult( value, rule );
          break;

        case 14:
          result = GetRuleResult( value, rule );

          if ( result == ValidationResult.Valid )
            return ValidationResult.Valid;
          break;

        default:
          result = GetRuleResult( value, rule );
          break;
      }

      #endregion

      if ( result == ValidationResult.Invalid )
      {
        //if we have one rule, we must return the result now
        if ( rulesa.Length == 1 )
          return ValidationResult.Invalid; //because it is
        else if ( exceptionrule0 )
        {
          //we run the second rule
          rule = rulesa[ 1 ];

          #region Validation 2

          switch ( rule.Exception )
          {
            case 1:
              erule = Exception1Rule( rule );
              result = GetRuleResult( value, erule );
              break;

            case 3:
              var digit0 = GetAccountDigit( 'C', value );
              if ( digit0 == 6 || digit0 == 9 )
                return result; //we don't need to run test 2
              else
                result = GetRuleResult( value, rule );
              break;

            case 6:
              var digit1 = GetAccountDigit( 'A', value );
              var digit2 = GetAccountDigit( 'G', value );
              var digit3 = GetAccountDigit( 'H', value );
              if ( ( digit1 >= 4 && digit1 <= 8 ) && ( digit2 == digit3 ) )
                return ValidationResult.Valid;
              else
                result = GetRuleResult( value, rule );
              break;

            case 9:
              //the instructions imply this is correct
              var newvalue = "309634" + accountNumber.ToString( "00000000" );
              if ( rulesa[ 0 ].Exception == 2 )
              {
                //if ( GetAccountDigit( 'A', value ) != 0 && GetAccountDigit( 'A', value ) != 9 )
                //{
                //  erule = Exception2aRule( rule );
                //  result = GetRuleResult( newvalue, erule );
                //}
                //else if ( GetAccountDigit( 'A', value ) != 0 && GetAccountDigit( 'A', value ) == 9 )
                //{
                //  erule = Exception2bRule( rule );
                //  result = GetRuleResult( newvalue, erule );
                //}
                //else
                result = GetRuleResult( newvalue, rule );
              }
              else
                result = GetRuleResult( newvalue, rule );
              break;

            case 11:
              if ( rulesa[ 0 ].Exception == 10 )
              {
                var digit4 = ( GetAccountDigit( 'A', value ) * 10 ) + GetAccountDigit( 'B', value ); ;
                var digit5 = GetAccountDigit( 'G', value );
                if ( ( digit4 == 09 || digit4 == 99 ) && digit5 == 9 )
                {
                  erule = Exception10Rule( rule );
                  result = GetRuleResult( value, erule );
                }
                else
                  result = GetRuleResult( value, rule );

                if ( result == ValidationResult.Valid )
                  return ValidationResult.Valid;
              }
              else
                result = GetRuleResult( value, rule );
              break;

            case 13:
              if ( rulesa[ 0 ].Exception == 12 )
              {
                result = GetRuleResult( value, rule );

                if ( result == ValidationResult.Valid )
                  return ValidationResult.Valid;
              }
              else
                result = GetRuleResult( value, rule );
              break;

            case 14:
              var accnum = accountNumber.ToString();
              var digit8 = Convert.ToInt32( accnum.Substring( 7, 1 ) ); //8th digit from left
              var iscouts = ( digit8 == 0 || digit8 == 1 || digit8 == 9 );
              if ( !iscouts )
                result = GetRuleResult( value, rule );
              else
              {
                //here's were it get's weird
                var coutsaccnum = sortCode.ToString( "000000" ) + "0" + accountNumber.ToString().Remove( 7, 1 );
                result = GetRuleResult( value, rule );
              }

              break;

            default:
              result = GetRuleResult( value, rule );
              break;
          }

          #endregion

          return result;
        }
        else
          return ValidationResult.Invalid;
      }
      else if ( result == ValidationResult.Valid )
      {
        if ( rulesa.Length == 1 )
          return ValidationResult.Valid;
        else if ( exceptionrule0 )
        {
          return ValidationResult.Valid;
        }
        else
        {
          //we run the second rule
          rule = rulesa[ 1 ];

          #region Validation 2

          switch ( rule.Exception )
          {
            case 1:
              erule = Exception1Rule( rule );
              result = GetRuleResult( value, erule );
              break;

            case 3:
              var digit0 = GetAccountDigit( 'C', value );
              if ( digit0 == 6 || digit0 == 9 )
                return result; //we don't need to run test 2
              else
                result = GetRuleResult( value, rule );
              break;

            case 6:
              var digit1 = GetAccountDigit( 'A', value );
              var digit2 = GetAccountDigit( 'G', value );
              var digit3 = GetAccountDigit( 'H', value );
              if ( ( digit1 >= 4 && digit1 <= 8 ) && ( digit2 == digit3 ) )
                return ValidationResult.Valid;
              else
                result = GetRuleResult( value, rule );
              break;

            case 9:
              //the instructions imply this is correct
              var newvalue = "309634" + accountNumber.ToString( "00000000" );
              //if ( rulesa[ 0 ].Exception == 2 )
              //{
              //  if ( rule.A != 0 && rule.G != 9 )
              //  {
              //    erule = Exception2aRule( rule );
              //    result = GetRuleResult( newvalue, erule );
              //  }
              //  else if ( GetAccountDigit( 'A', value ) != 0 && GetAccountDigit( 'A', value ) == 9 )
              //  {
              //    erule = Exception2bRule( rule );
              //    result = GetRuleResult( newvalue, erule );
              //  }
              //  else
              //    result = GetRuleResult( newvalue, rule );
              //}
              //else
              result = GetRuleResult( newvalue, rule );
              break;

            case 11:
              if ( rulesa[ 0 ].Exception == 10 )
              {
                var digit4 = ( GetAccountDigit( 'A', value ) * 10 ) + GetAccountDigit( 'B', value ); ;
                var digit5 = GetAccountDigit( 'G', value );
                if ( ( digit4 == 09 || digit4 == 99 ) && digit5 == 9 )
                {
                  erule = Exception10Rule( rule );
                  result = GetRuleResult( value, erule );
                }
                else
                  result = GetRuleResult( value, rule );

                if ( result == ValidationResult.Valid )
                  return ValidationResult.Valid;
              }
              else
                result = GetRuleResult( value, rule );
              break;

            case 13:
              if ( rulesa[ 0 ].Exception == 12 )
              {
                result = GetRuleResult( value, rule );

                if ( result == ValidationResult.Valid )
                  return ValidationResult.Valid;
              }
              else
                result = GetRuleResult( value, rule );
              break;

            case 14:
              var accnum = accountNumber.ToString();
              var digit8 = Convert.ToInt32( accnum.Substring( 7, 1 ) ); //8th digit from left
              var iscouts = ( digit8 == 0 || digit8 == 1 || digit8 == 9 );
              if ( !iscouts )
                result = GetRuleResult( value, rule );
              else
              {
                //here's were it get's weird
                var coutsaccnum = sortCode.ToString( "000000" ) + "0" + accountNumber.ToString().Remove( 7, 1 );
                result = GetRuleResult( value, rule );
              }

              break;

            default:
              result = GetRuleResult( value, rule );
              break;
          }

          #endregion

          return result;
        }
      }

      return result;
    }

    private static string ApplyExceptionsToAccount( int sortCode, int accountNumber, Rule rule )
    {
      string value;
      switch ( rule.Exception )
      {
        case 5:
          value = AccountNumberUtils.ConvertAccount( sortCode.ToString( "000000" ), accountNumber.ToString( "00000000" ) );
          break;
        case 8:
          value = "090126" + accountNumber.ToString( "00000000" );
          break;
        default:
          value = sortCode.ToString( "000000" ) + accountNumber.ToString( "00000000" );
          break;
      }
      return value;
    }

    private static Rule Exception10Rule( Rule rule )
    {
      var erule = new Rule()
      {
        Exception = rule.Exception,
        Start = rule.Start,
        End = rule.End,
        Modulus = rule.Modulus,
        U = 0,
        V = 0,
        W = 0,
        X = 0,
        Y = 0,
        Z = 0,
        A = 0,
        B = 0,
        C = rule.C,
        D = rule.D,
        E = rule.E,
        F = rule.F,
        G = rule.G,
        H = rule.H
      };
      return erule;
    }

    private static Rule Exception7Rule( Rule rule )
    {
      var erule = new Rule()
      {
        Exception = rule.Exception,
        Start = rule.Start,
        End = rule.End,
        Modulus = rule.Modulus,
        U = 0,
        V = 0,
        W = 0,
        X = 0,
        Y = 0,
        Z = 0,
        A = 0,
        B = 0,
        C = rule.C,
        D = rule.D,
        E = rule.E,
        F = rule.F,
        G = rule.G,
        H = rule.H
      };
      return erule;
    }

    public static int GetAccountDigit( char digit, string value )
    {
      var s = new StringBuilder( value );
      switch ( digit )
      {
        case 'U': return Convert.ToInt32( s[ 00 ].ToString() );
        case 'V': return Convert.ToInt32( s[ 01 ].ToString() );
        case 'W': return Convert.ToInt32( s[ 02 ].ToString() );
        case 'X': return Convert.ToInt32( s[ 03 ].ToString() );
        case 'Y': return Convert.ToInt32( s[ 04 ].ToString() );
        case 'Z': return Convert.ToInt32( s[ 05 ].ToString() );
        case 'A': return Convert.ToInt32( s[ 06 ].ToString() );
        case 'B': return Convert.ToInt32( s[ 07 ].ToString() );
        case 'C': return Convert.ToInt32( s[ 08 ].ToString() );
        case 'D': return Convert.ToInt32( s[ 09 ].ToString() );
        case 'E': return Convert.ToInt32( s[ 10 ].ToString() );
        case 'F': return Convert.ToInt32( s[ 11 ].ToString() );
        case 'G': return Convert.ToInt32( s[ 12 ].ToString() );
        case 'H': return Convert.ToInt32( s[ 13 ].ToString() );
        default: return 0;
      }
    }

    private static Rule Exception2bRule( Rule rule )
    {
      var erule = new Rule()
      {
        Exception = rule.Exception,
        Start = rule.Start,
        End = rule.End,
        Modulus = rule.Modulus,
        U = 0,
        V = 0,
        W = 0,
        X = 0,
        Y = 0,
        Z = 0,
        A = 0,
        B = 0,
        C = 8,
        D = 7,
        E = 10,
        F = 9,
        G = 3,
        H = 1
      };
      return erule;
    }

    private static Rule Exception2aRule( Rule rule )
    {
      var erule = new Rule()
      {
        Exception = rule.Exception,
        Start = rule.Start,
        End = rule.End,
        Modulus = rule.Modulus,
        U = 0,
        V = 0,
        W = 1,
        X = 2,
        Y = 5,
        Z = 3,
        A = 6,
        B = 4,
        C = 8,
        D = 7,
        E = 10,
        F = 9,
        G = 3,
        H = 1
      };
      return erule;
    }

    private static Rule Exception1Rule( Rule rule )
    {
      var erule = new Rule()
      {
        Exception = rule.Exception,
        Start = rule.Start,
        End = rule.End,
        Modulus = ModulusType.DBLAL,
        U = rule.U,
        V = rule.V,
        W = rule.W,
        X = rule.X,
        Y = rule.Y,
        Z = rule.Z,
        A = rule.A,
        B = rule.B,
        C = rule.C,
        D = rule.D,
        E = rule.E,
        F = rule.F,
        G = rule.G,
        H = rule.H
      };
      return erule;
    }

    private static ValidationResult GetRuleResult( string value, Rule rule )
    {
      ValidationResult result = ValidationResult.Invalid;

      //try the rule
      switch ( rule.Modulus )
      {
        case ModulusType.MOD10:
          result = CalcMod10( rule, value );
          break;
        case ModulusType.MOD11:
          result = CalcMod11( rule, value );
          break;
        case ModulusType.DBLAL:
          result = CalcModDBLAL( rule, value );
          break;
      }
      return result;
    }

    private static int DBALSplit( int value )
    {
      var vu = value % 10; //get units
      var result = ( ( value - vu ) / 10 ) + vu;
      return (int)result;
    }

    private static ValidationResult CalcModDBLAL( Rule rule, string value )
    {
      var s = new StringBuilder( value );
      int tally = 0;
      tally += DBALSplit( Convert.ToInt32( s[ 00 ].ToString() ) * rule.U );
      tally += DBALSplit( Convert.ToInt32( s[ 01 ].ToString() ) * rule.V );
      tally += DBALSplit( Convert.ToInt32( s[ 02 ].ToString() ) * rule.W );
      tally += DBALSplit( Convert.ToInt32( s[ 03 ].ToString() ) * rule.X );
      tally += DBALSplit( Convert.ToInt32( s[ 04 ].ToString() ) * rule.Y );
      tally += DBALSplit( Convert.ToInt32( s[ 05 ].ToString() ) * rule.Z );
      tally += DBALSplit( Convert.ToInt32( s[ 06 ].ToString() ) * rule.A );
      tally += DBALSplit( Convert.ToInt32( s[ 07 ].ToString() ) * rule.B );
      tally += DBALSplit( Convert.ToInt32( s[ 08 ].ToString() ) * rule.C );
      tally += DBALSplit( Convert.ToInt32( s[ 09 ].ToString() ) * rule.D );
      tally += DBALSplit( Convert.ToInt32( s[ 10 ].ToString() ) * rule.E );
      tally += DBALSplit( Convert.ToInt32( s[ 11 ].ToString() ) * rule.F );
      tally += DBALSplit( Convert.ToInt32( s[ 12 ].ToString() ) * rule.G );
      tally += DBALSplit( Convert.ToInt32( s[ 13 ].ToString() ) * rule.H );

      if ( rule.Exception == 1 )
        tally += 27;

      var modulus = tally % 10;

      if ( rule.Exception == 5 )
      {
        var checkdigit = Convert.ToInt32( s[ 13 ].ToString() );

        if ( checkdigit == 0 && modulus == 0 )
          return ValidationResult.Valid;
        else if ( ( 10 - modulus ) == checkdigit )
          return ValidationResult.Valid;
        else
          return ValidationResult.Invalid;
      }

      if ( modulus == 0 )
        return ValidationResult.Valid;
      else
        return ValidationResult.Invalid;
    }

    private static ValidationResult CalcMod11( Rule rule, string value )
    {
      var s = new StringBuilder( value );
      int tally = 0;
      tally += Convert.ToInt32( s[ 00 ].ToString() ) * rule.U;
      tally += Convert.ToInt32( s[ 01 ].ToString() ) * rule.V;
      tally += Convert.ToInt32( s[ 02 ].ToString() ) * rule.W;
      tally += Convert.ToInt32( s[ 03 ].ToString() ) * rule.X;
      tally += Convert.ToInt32( s[ 04 ].ToString() ) * rule.Y;
      tally += Convert.ToInt32( s[ 05 ].ToString() ) * rule.Z;
      tally += Convert.ToInt32( s[ 06 ].ToString() ) * rule.A;
      tally += Convert.ToInt32( s[ 07 ].ToString() ) * rule.B;
      tally += Convert.ToInt32( s[ 08 ].ToString() ) * rule.C;
      tally += Convert.ToInt32( s[ 09 ].ToString() ) * rule.D;
      tally += Convert.ToInt32( s[ 10 ].ToString() ) * rule.E;
      tally += Convert.ToInt32( s[ 11 ].ToString() ) * rule.F;
      tally += Convert.ToInt32( s[ 12 ].ToString() ) * rule.G;
      tally += Convert.ToInt32( s[ 13 ].ToString() ) * rule.H;

      var modulus = tally % 11;

      if ( rule.Exception == 4 )
      {
        var checkdigit = Convert.ToInt32( s[ 12 ].ToString() ) * 10 + Convert.ToInt32( s[ 13 ].ToString() );

        if ( modulus == checkdigit )
          return ValidationResult.Valid;
        else
          return ValidationResult.Invalid;
      }
      else if ( rule.Exception == 5 )
      {
        var checkdigit = Convert.ToInt32( s[ 12 ].ToString() );

        if ( checkdigit == 0 && modulus == 0 )
          return ValidationResult.Valid;
        else if ( checkdigit == 1 )
          return ValidationResult.Invalid;
        else if ( ( 11 - modulus ) == checkdigit )
          return ValidationResult.Valid;
        else
          return ValidationResult.Invalid;
      }

      if ( modulus == 0 )
        return ValidationResult.Valid;
      else
        return ValidationResult.Invalid;
    }

    private static ValidationResult CalcMod10( Rule rule, string value )
    {
      var s = new StringBuilder( value );
      int tally = 0;
      tally += Convert.ToInt32( s[ 00 ].ToString() ) * rule.U;
      tally += Convert.ToInt32( s[ 01 ].ToString() ) * rule.V;
      tally += Convert.ToInt32( s[ 02 ].ToString() ) * rule.W;
      tally += Convert.ToInt32( s[ 03 ].ToString() ) * rule.X;
      tally += Convert.ToInt32( s[ 04 ].ToString() ) * rule.Y;
      tally += Convert.ToInt32( s[ 05 ].ToString() ) * rule.Z;
      tally += Convert.ToInt32( s[ 06 ].ToString() ) * rule.A;
      tally += Convert.ToInt32( s[ 07 ].ToString() ) * rule.B;
      tally += Convert.ToInt32( s[ 08 ].ToString() ) * rule.C;
      tally += Convert.ToInt32( s[ 09 ].ToString() ) * rule.D;
      tally += Convert.ToInt32( s[ 10 ].ToString() ) * rule.E;
      tally += Convert.ToInt32( s[ 11 ].ToString() ) * rule.F;
      tally += Convert.ToInt32( s[ 12 ].ToString() ) * rule.G;
      tally += Convert.ToInt32( s[ 13 ].ToString() ) * rule.H;

      var modulus = tally % 10;

      if ( modulus == 0 )
        return ValidationResult.Valid;
      else
        return ValidationResult.Invalid;
    }
  }
}