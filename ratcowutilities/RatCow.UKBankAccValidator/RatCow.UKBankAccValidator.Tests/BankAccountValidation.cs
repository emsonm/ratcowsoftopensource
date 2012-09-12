using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RatCow.UKBankAccValidator;

namespace RatCow.UKBankAccValidator.Tests
{
  [TestClass]
  public class BankAccountValidation
  {
    [TestMethod]
    public void StandardRulesThatShouldAllBeEqual()
    {
      //pass mod 10
      ValidationResult result = Core.ValidateAccountNumber( 089999, 66374958 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //pass mod 11
      result = Core.ValidateAccountNumber( 107999, 88837491 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //pass mod 11 and dblal
      result = Core.ValidateAccountNumber( 202959, 63748472 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //exception 10 and 11, first check passes, second fails
      result = Core.ValidateAccountNumber( 871427, 46238510 );

      Assert.AreEqual( result, ValidationResult.Valid );


      //exception 10 and 11, first check fails, second passes
      result = Core.ValidateAccountNumber( 872427, 46238510 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //Exception 10 where in the account number ab=99 and the g=9. The first 
      //check passes and the second check fails.
      result = Core.ValidateAccountNumber( 871427, 99123496 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //Exception 3, and the sorting code is the start of a range. As c=6 the 
      //second check should be ignored.
      result = Core.ValidateAccountNumber( 820000, 73688637 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //Exception 3, and the sorting code is the end of a range. As c=9 the 
      //second check should be ignored.

      result = Core.ValidateAccountNumber( 827999, 73988638 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 3. As c6 or 9 perform both checks pass.   Y
      result = Core.ValidateAccountNumber( 827101, 28748352 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 4 where the remainder is equal to the checkdigit. 134020 63849203 Y
      result = Core.ValidateAccountNumber( 827101, 28748352 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 1 – ensures that 27 has been added to the accumulated total 
      //and passes double alternate modulus check. Y
      result = Core.ValidateAccountNumber( 118765, 64371389 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 6 where the account fails standard check but is a foreign 
      //currency account. Y
      result = Core.ValidateAccountNumber( 200915, 41011166 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 5 where the check passes.   Y
      result = Core.ValidateAccountNumber( 938611, 07806039 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 5 where the check passes with substitution.   Y
      result = Core.ValidateAccountNumber( 938600, 42368003 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 5 where both checks produce a remainder of 0 and pass.  
      result = Core.ValidateAccountNumber( 938063, 55065200 );

      Assert.AreEqual( result, ValidationResult.Valid );

      //Exception 7 where passes but would fail the standard check. 772798 99345694 Y
      result = Core.ValidateAccountNumber( 938063, 55065200 );

      Assert.AreEqual( result, ValidationResult.Valid );
      ////Exception 8 where the check passes.   Y
      result = Core.ValidateAccountNumber( 086090, 06774744 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 2 & 9 where the first check passes and second check fails.   Y
      result = Core.ValidateAccountNumber( 309070, 02355688 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 2 & 9 where the first check fails and second check passes with 
      //substitution. Y
      result = Core.ValidateAccountNumber( 309070, 12345668 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 2 & 9 where a0 and g9 and passes.   Y
      result = Core.ValidateAccountNumber( 309070, 12345677 );

      Assert.AreEqual( result, ValidationResult.Valid );
      //Exception 2 & 9 where a0 and g=9 and passes.   Y
      result = Core.ValidateAccountNumber( 309070, 99345694 );

      Assert.AreEqual( result, ValidationResult.Valid );
    }
  }
}
