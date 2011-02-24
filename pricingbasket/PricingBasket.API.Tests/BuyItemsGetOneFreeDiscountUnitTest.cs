using PricingBasket.API.Discounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingBasket.API.SKU;

namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for BuyItemsGetOneFreeDiscountUnitTest and is intended
    ///to contain all BuyItemsGetOneFreeDiscountUnitTest Unit Tests
    ///</summary>
  [TestClass()]
  public class BuyItemsGetOneFreeDiscountUnitTest
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
    ///A test for CalculatedDiscount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void ApplyDiscountTest()
    {
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5);
      BuyItemsGetOneFreeDiscountUnit target = new BuyItemsGetOneFreeDiscountUnit("Test 1 discount", temp, 1); // TODO: Initialize to an appropriate value
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp }); // TODO: Initialize to an appropriate value
      
      double expected = 1.5F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.AreEqual<double>(expected, actual.Discount);
    }

    /// <summary>
    ///A test for CalculatedDiscount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void ApplyDiscountTest2()
    {
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5);
      BuyItemsGetOneFreeDiscountUnit target = new BuyItemsGetOneFreeDiscountUnit("Test 1 discount", temp, 2); // TODO: Initialize to an appropriate value
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp, temp }); // TODO: Initialize to an appropriate value

      double expected = 1.5F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.AreEqual<double>(expected, actual.Discount);
    }

    /// <summary>
    ///A test for CalculatedDiscount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void ApplyDiscountTest3()
    {
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5);
      BuyItemsGetOneFreeDiscountUnit target = new BuyItemsGetOneFreeDiscountUnit("Test 1 discount", temp, 2); // TODO: Initialize to an appropriate value
      
      //this should match 2 offers, should ignore the extra item
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp, temp, temp, temp, temp, temp}); // TODO: Initialize to an appropriate value

      double expected = 3.0F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.AreEqual<double>(expected, actual.Discount);
    }

    /// <summary>
    ///A test for CalculatedDiscount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void ApplyDiscountTest4()
    {
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5);
      StockKeepingUnit temp2 = new StockKeepingUnit("Test2", 5.0);
      BuyItemsGetOneFreeDiscountUnit target = new BuyItemsGetOneFreeDiscountUnit("Test 1 discount", temp, 2); // TODO: Initialize to an appropriate value

      //this should match 2 offers, should ignore the extra items
      //The clone should make no difference to the results
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp2, new StockKeepingUnit(temp), temp, temp, temp, temp2, temp, new StockKeepingUnit(temp) }); // TODO: Initialize to an appropriate value

      double expected = 3.0F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.AreEqual<double>(expected, actual.Discount);
    }

    /// <summary>
    ///A test for BuyItemsGetOneFreeDiscountUnit Constructor
    ///</summary>
    [TestMethod()]
    public void BuyItemsGetOneFreeDiscountUnitConstructorTest()
    {
      string name = "Test1"; // TODO: Initialize to an appropriate value
      StockKeepingUnit target1 = new StockKeepingUnit("Test1", 1.5); ; // TODO: Initialize to an appropriate value
      int targetCount = 1; // TODO: Initialize to an appropriate value
      BuyItemsGetOneFreeDiscountUnit target = new BuyItemsGetOneFreeDiscountUnit(name, target1, targetCount);

      Assert.IsNotNull(target, "Valid instance");

      double expected = 1.5F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(new StockKeepingUnits() {target1, target1});

      Assert.AreEqual<double>(expected, actual.Discount);

      Assert.IsNotNull(target);
      Assert.AreEqual(name, target.Name);
      Assert.AreEqual(name, target.Name);
      Assert.AreEqual(targetCount, target.TargetLevel);
    }
  }
}
