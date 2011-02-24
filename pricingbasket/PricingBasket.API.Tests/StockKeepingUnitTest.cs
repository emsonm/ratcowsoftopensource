using PricingBasket.API.SKU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for StockKeepingUnitTest and is intended
    ///to contain all StockKeepingUnitTest Unit Tests
    ///</summary>
  [TestClass()]
  public class StockKeepingUnitTest
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
    ///A test for Price
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void PriceTest()
    {
      StockKeepingUnit_Accessor target = new StockKeepingUnit_Accessor(); // TODO: Initialize to an appropriate value
      double expected = 123.45F; // TODO: Initialize to an appropriate value
      double actual;
      target.Price = expected;
      actual = target.Price;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for Name
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void NameTest()
    {
      StockKeepingUnit_Accessor target = new StockKeepingUnit_Accessor(); // TODO: Initialize to an appropriate value
      string expected = "Hasbro"; // TODO: Initialize to an appropriate value
      string actual;
      target.Name = expected;
      actual = target.Name;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for Description
    ///</summary>
    [TestMethod()]
    [DeploymentItem("PricingBasket.API.dll")]
    public void DescriptionTest()
    {
      StockKeepingUnit_Accessor target = new StockKeepingUnit_Accessor(); // TODO: Initialize to an appropriate value
      string expected = "This is a test string\r\nWith a line break."; // TODO: Initialize to an appropriate value
      string actual;
      target.Description = expected;
      actual = target.Description;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for StockKeepingUnit Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitConstructorTest3()
    {
      string name = "Crunchy"; // TODO: Initialize to an appropriate value
      double price = 345.67F; // TODO: Initialize to an appropriate value
      StockKeepingUnit target = new StockKeepingUnit(name, price);

      Assert.AreEqual<string>(name, target.Name, "Names did not match");
      Assert.AreEqual<double>(price, target.Price, "Price did not match");
      Assert.AreEqual<string>(target.Name, target.Description, "Description did not match name for 2 param constructor.");
    }

    /// <summary>
    ///A test for StockKeepingUnit Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitConstructorTest2()
    {
      StockKeepingUnit target = new StockKeepingUnit();
      Assert.IsNotNull(target);
    }

    /// <summary>
    ///A test for StockKeepingUnit Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitConstructorTest1()
    {
      string name = "Crunchy"; // TODO: Initialize to an appropriate value
      double price = 345.67F; // TODO: Initialize to an appropriate value
      StockKeepingUnit clone = new StockKeepingUnit(name, price);

      StockKeepingUnit target = new StockKeepingUnit(clone);

      Assert.AreEqual<string>(clone.Name, target.Name, "Names did not match");
      Assert.AreEqual<double>(clone.Price, target.Price, "Price did not match");
      Assert.AreEqual<string>(clone.Description, target.Description, "Description did not match.");
      
    }

    /// <summary>
    ///A test for StockKeepingUnit Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitConstructorTest()
    {
      string name = "Crunchy"; // TODO: Initialize to an appropriate value
      string description = "Case of Cadbury Crunchy chocolate bars"; // TODO: Initialize to an appropriate value
      double price = 345.67F; // TODO: Initialize to an appropriate value
      StockKeepingUnit target = new StockKeepingUnit(name, description, price);

      Assert.AreEqual<string>(name, target.Name, "Names did not match");
      Assert.AreEqual<double>(price, target.Price, "Price did not match");
      Assert.AreEqual<string>(description, target.Description, "Description did not match");

    }
  }
}
