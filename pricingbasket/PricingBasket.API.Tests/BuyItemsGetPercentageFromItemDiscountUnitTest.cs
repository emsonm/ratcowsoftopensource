using PricingBasket.API.Discounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingBasket.API.SKU;

namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for BuyItemsGetPercentageFromItemDiscountUnitTest and is intended
    ///to contain all BuyItemsGetPercentageFromItemDiscountUnitTest Unit Tests
    ///</summary>
  [TestClass()]
  public class BuyItemsGetPercentageFromItemDiscountUnitTest
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
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5F);
      StockKeepingUnit temp2 = new StockKeepingUnit("Test2", 5.0F);
      BuyItemsGetPercentageFromItemDiscountUnit target = new BuyItemsGetPercentageFromItemDiscountUnit("Test 1 discount", temp, temp2, 2, 50.0); // TODO: Initialize to an appropriate value
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp, temp2 }); // TODO: Initialize to an appropriate value

      double expected = 2.5F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.IsTrue(System.Math.Round(expected, 2) == System.Math.Round(actual.Discount, 2));
    }

    /// <summary>
    ///A test for CalculatedDiscount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void ApplyDiscountTest2()
    {
      StockKeepingUnit temp = new StockKeepingUnit("Test1", 1.5F);
      StockKeepingUnit temp2 = new StockKeepingUnit("Test2", 5.0F);
      BuyItemsGetPercentageFromItemDiscountUnit target = new BuyItemsGetPercentageFromItemDiscountUnit("Test 1 discount", temp, temp2, 2, 50.0); // TODO: Initialize to an appropriate value
      StockKeepingUnits items = new StockKeepingUnits(new StockKeepingUnit[] { temp, temp, temp2, temp, temp, temp2, temp2 }); // TODO: Initialize to an appropriate value

      //there should be 2 discounts on Test 2 (50% or 5, so 2 x 2.5) and the extra Test2 should be ignored.
      double expected = 5.0F; // TODO: Initialize to an appropriate value
      DiscountResult actual = target.ApplyDiscount(items);

      Assert.IsTrue(System.Math.Round(expected, 2) == System.Math.Round(actual.Discount, 2), string.Format("L: {0} R: {1}", expected, actual.Discount));
    }
    

    /// <summary>
    ///A test for BuyItemsGetPercentageFromItemDiscountUnit Constructor
    ///</summary>
    [TestMethod()]
    public void BuyItemsGetPercentageFromItemDiscountUnitConstructorTest()
    {
      string name = "Test1"; // TODO: Initialize to an appropriate value
      StockKeepingUnit target1 = new StockKeepingUnit("Test1", 1.5); // TODO: Initialize to an appropriate value    
      StockKeepingUnit discounted = new StockKeepingUnit("Test1", 1.5); ; // TODO: Initialize to an appropriate value
      int targetCount = 1; // TODO: Initialize to an appropriate value
      double percentage = 50.0F; // TODO: Initialize to an appropriate value
      BuyItemsGetPercentageFromItemDiscountUnit target = new BuyItemsGetPercentageFromItemDiscountUnit(name, target1, discounted, targetCount, percentage);

      Assert.IsNotNull(target);
      Assert.AreEqual(name, target.Name);
      Assert.AreEqual(name, target.Name);
      Assert.AreEqual(targetCount, target.TargetLevel);
      Assert.AreEqual(percentage, target.Discount);
    }
  }
}
