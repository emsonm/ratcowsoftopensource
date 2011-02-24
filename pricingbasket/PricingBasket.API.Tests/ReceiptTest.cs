/*
 * Copyright 2011 Rat Cow Software and Matt Emson. All rights reserved.
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

using PricingBasket.API.Receipts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for ReceiptTest and is intended
    ///to contain all ReceiptTest Unit Tests
    ///</summary>
  [TestClass()]
  public class ReceiptTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for Total
    ///</summary>
    [TestMethod()]
    public void TotalTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      double expected = 2.65F; // TODO: Initialize to an appropriate value
      double actual;
      target.Total = expected;
      actual = target.Total;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for Subtotal
    ///</summary>
    [TestMethod()]
    public void SubtotalTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      double expected = 2.65F; // TODO: Initialize to an appropriate value
      double actual;
      target.Subtotal = expected;
      actual = target.Subtotal;
      Assert.AreEqual(expected, actual);
    }

    const string TEST1 = "A test string."; //Basics
    const string TEST2 = "Another test string? Yes!"; //more punctuation
    const string TEST3 = "This contains 123456 and $£€."; //utf8/unicode characters
    const string TEST4 = "Yet another test string, with more characters."; //Length

    /// <summary>
    ///A test for Remarks
    ///</summary>
    [TestMethod()]
    public void RemarksTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      StringBuilder expected = new StringBuilder();

      //not in order on purpose.
      expected.AppendLine(TEST1);
      expected.AppendLine(TEST3);
      expected.AppendLine(TEST2);
      expected.AppendLine(TEST4);

      //configure remarks
      target.AddRemark(TEST1);
      target.AddRemark(TEST3);
      target.AddRemark(TEST2);
      target.AddRemark(TEST4);
 
      string actual;
      actual = target.Remarks;

      Assert.AreEqual<string>(expected.ToString(), actual);
    }

    /// <summary>
    ///A test for Remarks
    ///</summary>
    [TestMethod()]
    public void RemarksTest2()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      StringBuilder expected = new StringBuilder();

      //not in order on purpose.
      expected.AppendLine(TEST1);
      expected.AppendLine(TEST2);
      expected.AppendLine(TEST3);
      expected.AppendLine(TEST4);

      //configure remarks
      target.AddRemark(TEST1);
      target.AddRemark(TEST3);
      target.AddRemark(TEST2);
      target.AddRemark(TEST4);

      string actual;
      actual = target.Remarks;

      Assert.AreNotEqual<string>(expected.ToString(), actual);
    }

    /// <summary>
    ///A test for Discount
    ///</summary>
    [TestMethod()]
    public void DiscountTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      StringBuilder expected = new StringBuilder();

      //not in order on purpose.
      expected.Append(TEST1);
      expected.AppendFormat("\r\n{0}", TEST3);
      expected.AppendFormat("\r\n{0}", TEST2);
      expected.AppendFormat("\r\n{0}", TEST4);

      //configure remarks
      target.AddDiscount(TEST1);
      target.AddDiscount(TEST3);
      target.AddDiscount(TEST2);
      target.AddDiscount(TEST4);

      string actual;
      actual = target.Discount;

      string temp = expected.ToString();

      Assert.AreEqual(temp.Length, actual.Length);
      Assert.AreEqual(temp.CompareTo(actual), 0);
      Assert.AreEqual<string>(temp, actual);
    }

    /// <summary>
    ///A test for Discount
    ///</summary>
    [TestMethod()]
    public void DiscountTest2()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      StringBuilder expected = new StringBuilder();

      //not in order on purpose.
      expected.AppendLine(TEST1);
      expected.AppendLine(TEST2);
      expected.AppendLine(TEST3);
      expected.AppendLine(TEST4);

      //configure remarks
      target.AddDiscount(TEST1);
      target.AddDiscount(TEST3);
      target.AddDiscount(TEST2);
      target.AddDiscount(TEST4);

      string actual;
      actual = target.Discount;

      Assert.AreNotEqual<string>(expected.ToString(), actual);
    }

    /// <summary>
    ///A test for AddRemark
    ///</summary>
    [TestMethod()]
    public void AddRemarkTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      string remark = string.Format("{0}\r\n", TEST1); ; // TODO: Initialize to an appropriate value
      target.AddRemark(TEST1);

      Assert.AreEqual<string>(target.Remarks, remark);
    }

    /// <summary>
    ///A test for AddDiscount
    ///</summary>
    [TestMethod()]
    public void AddDiscountTest()
    {
      Receipt target = new Receipt(); // TODO: Initialize to an appropriate value
      string discount = TEST1; // TODO: Initialize to an appropriate value
      target.AddDiscount(TEST1);

      Assert.AreEqual<string>(target.Discount, discount);
    }

    /// <summary>
    ///A test for Receipt Constructor
    ///</summary>
    [TestMethod()]
    public void ReceiptConstructorTest()
    {
      Receipt target = new Receipt();
      Assert.IsNotNull(target);
    }

    /// <summary>
    ///A test for Receipt Constructor
    ///</summary>
    [TestMethod()]
    public void ReceiptConstructorTest2()
    {
      Receipt target = new Receipt();
      Assert.IsInstanceOfType(target, typeof(Receipt));
    }
  }
}
