using PricingBasket.API.Discounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for DiscountResultTest and is intended
    ///to contain all DiscountResultTest Unit Tests
    ///</summary>
  [TestClass()]
  public class DiscountResultTest
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
    ///A test for Discount
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void DiscountTest()
    {
      DiscountResult_Accessor target = new DiscountResult_Accessor(); // TODO: Initialize to an appropriate value
      double expected = 20.23F; // TODO: Initialize to an appropriate value
      double actual;
      target.Discount = expected;
      actual = target.Discount;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for Description
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void DescriptionTest()
    {
      DiscountResult_Accessor target = new DiscountResult_Accessor(); // TODO: Initialize to an appropriate value
      string expected = "Test"; // TODO: Initialize to an appropriate value
      string actual;
      target.Description = expected;
      actual = target.Description;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for Applied
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void AppliedTest()
    {
      DiscountResult_Accessor target = new DiscountResult_Accessor(); // TODO: Initialize to an appropriate value
      bool expected = true; // TODO: Initialize to an appropriate value
      bool actual;
      target.Applied = expected;
      actual = target.Applied;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for DiscountResult Constructor
    ///</summary>
    [TestMethod()]
    public void DiscountResultConstructorTest()
    {
      string description = "test"; // TODO: Initialize to an appropriate value
      double discount = 20.5F; // TODO: Initialize to an appropriate value
      bool applied = true; // TODO: Initialize to an appropriate value
      DiscountResult target = new DiscountResult(description, discount, applied);
      Assert.IsNotNull(target);
      Assert.AreEqual(description, target.Description);
      Assert.AreEqual(discount, target.Discount);
      Assert.AreEqual(applied, target.Applied);
    }
  }
}
