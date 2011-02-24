using PricingBasket.API.Discounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PricingBasket.API.Tests
{


  /// <summary>
  ///This is a test class for DiscountResultsTest and is intended
  ///to contain all DiscountResultsTest Unit Tests
  ///</summary>
  [TestClass()]
  public class DiscountResultsTest
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
    ///A test for Sum
    ///</summary>
    [TestMethod()]
    public void SumTest()
    {
      DiscountResults target = new DiscountResults(); // TODO: Initialize to an appropriate value

      target.AddRange(new DiscountResult[] {
        new DiscountResult("test1", 50.0, true),
        new DiscountResult("test2", 50.0, false), //this one should be ignored
        new DiscountResult("test3", 100.0, true),
        new DiscountResult("test4", 25.0, true),
        new DiscountResult("test5", 75.0, true)
      });

      double expected = 250.00F; // TODO: Initialize to an appropriate value
      double actual;
      actual = target.Sum();
      Assert.AreEqual<double>(expected, actual);
    }

    /// <summary>
    ///A test for DiscountResults Constructor
    ///</summary>
    [TestMethod()]
    public void DiscountResultsConstructorTest1()
    {
      DiscountResults target = new DiscountResults();
      Assert.IsNotNull(target);
    }

    /// <summary>
    ///A test for DiscountResults Constructor
    ///</summary>
    [TestMethod()]
    public void DiscountResultsConstructorTest()
    {
      IEnumerable<DiscountResult> collection = new DiscountResult[] {
        new DiscountResult("test1", 50.0, true),
        new DiscountResult("test2", 50.0, false), //this one should be ignored
        new DiscountResult("test3", 100.0, true),
        new DiscountResult("test4", 25.0, true),
        new DiscountResult("test5", 75.0, true)
      }; // TODO: Initialize to an appropriate value
      
      DiscountResults target = new DiscountResults(collection);

      Assert.IsNotNull(target);
      Assert.AreEqual(target.Count, 5);
    }
  }
}
