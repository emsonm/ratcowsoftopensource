using PricingBasket.API.SKU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PricingBasket.API.Tests
{
    
    
    /// <summary>
    ///This is a test class for StockKeepingUnitsTest and is intended
    ///to contain all StockKeepingUnitsTest Unit Tests
    ///</summary>
  [TestClass()]
  public class StockKeepingUnitsTest
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
    public void PriceTest()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      target.Add( new StockKeepingUnit("Wheel", 88.50));
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));

      double expected = 150.00F; // TODO: Initialize to an appropriate value
      double actual;
      actual = target.Price();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for FindSKUs
    ///</summary>
    [TestMethod()]
    public void FindSKUsTest_VerifyWheelsCanBeFound()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(wheel);
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));
      target.Add(wheel);

      string skuname = wheel.Name; // TODO: Initialize to an appropriate value
      StockKeepingUnits expected = new StockKeepingUnits(new StockKeepingUnit[] { new StockKeepingUnit(wheel), wheel, wheel, new StockKeepingUnit(wheel) }); // TODO: Initialize to an appropriate value
      
      StockKeepingUnits actual;
      actual = target.FindSKUs(skuname);
      Assert.AreEqual(expected.Count, actual.Count);
    }

    /// <summary>
    ///A test for FindSKUs
    ///</summary>
    [TestMethod()]
    public void FindSKUsTest_VerifyWheelsCount()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(wheel);
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));
      target.Add(wheel);

      int expected = 4; //we have 4 wheels

      string skuname = wheel.Name; // TODO: Initialize to an appropriate value
 
      StockKeepingUnits actual;
      actual = target.FindSKUs(skuname);
      Assert.AreEqual(expected, actual.Count);
    }

    /// <summary>
    ///A test for FindSingletonSKU
    ///</summary>
    [TestMethod()]
    public void FindSingletonSKU_VerifyByName()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));


      string skuname = wheel.Name; // TODO: Initialize to an appropriate value

      //as we are not always returning the SAME instance, testing that would be pointless.
      StockKeepingUnit expected = new StockKeepingUnit(wheel); // TODO: Initialize to an appropriate value

      StockKeepingUnit actual;
      actual = target.FindSingletonSKU(skuname);

      Assert.AreEqual<string>(expected.Name, actual.Name);
    }

    /// <summary>
    ///A test for FindSKU
    ///</summary>
    [TestMethod()]
    public void FindSKUTest_VerifyByName()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(wheel);
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));
      target.Add(wheel);


      string skuname = wheel.Name; // TODO: Initialize to an appropriate value

      //as we are not always returning the SAME instance, testing that would be pointless.
      StockKeepingUnit expected = new StockKeepingUnit(wheel); // TODO: Initialize to an appropriate value
      
      StockKeepingUnit actual;
      actual = target.FindSKU(skuname);

      Assert.AreEqual<string>(expected.Name, actual.Name);
    }

    /// <summary>
    ///A test for FindSingletonSKU
    ///</summary>
    [TestMethod()]
    public void FindSingletonSKUTest_VerifyByPrice()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(wheel);
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));

      string skuname = wheel.Name; // TODO: Initialize to an appropriate value

      //as we are not always returning the SAME instance, testing that would be pointless.
      StockKeepingUnit expected = new StockKeepingUnit(wheel); // TODO: Initialize to an appropriate value

      //we have enfourced a limit that the FindSKU will return null when there is 0 or > 1 SKU
      //in the list.
      StockKeepingUnit actual;
      actual = target.FindSingletonSKU(skuname);

      Assert.IsNotNull(actual, "There was no 'actual' item instance found");
      Assert.AreEqual<double>(expected.Price, actual.Price, string.Format("L: {0} - R: {1}", expected.Price, actual.Price));
    }

    /// <summary>
    ///A test for FindSKU
    ///</summary>
    [TestMethod()]
    public void FindSKUTest_VerifyByPrice()
    {
      StockKeepingUnits target = new StockKeepingUnits(); // TODO: Initialize to an appropriate value

      StockKeepingUnit wheel = new StockKeepingUnit("Wheel", 88.50);
      target.Add(wheel);
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyre", 50.00));
      target.Add(new StockKeepingUnit(wheel)); //because, we don't just use the same instance
      target.Add(new StockKeepingUnit("Tyrehorn", 11.50));
      target.Add(wheel);

      string skuname = wheel.Name; // TODO: Initialize to an appropriate value

      //as we are not always returning the SAME instance, testing that would be pointless.
      StockKeepingUnit expected = new StockKeepingUnit(wheel); // TODO: Initialize to an appropriate value

      //we have enfourced a limit that the FindSKU will return null when there is 0 or > 1 SKU
      //in the list.
      StockKeepingUnit actual;
      actual = target.FindSKU(skuname);

      Assert.IsNotNull(actual, "There was no 'actual' item instance found");
      Assert.AreEqual<double>(expected.Price, actual.Price, string.Format("L: {0} - R: {1}", expected.Price, actual.Price));
    }

    /// <summary>
    ///A test for StockKeepingUnits Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitsConstructorTest1()
    {
      StockKeepingUnits target = new StockKeepingUnits();
      Assert.IsNotNull(target);
    }

    /// <summary>
    ///A test for StockKeepingUnits Constructor
    ///</summary>
    [TestMethod()]
    public void StockKeepingUnitsConstructorTest()
    {
      IEnumerable<StockKeepingUnit> collection = new List<StockKeepingUnit> {new StockKeepingUnit("A", 1.0), new StockKeepingUnit("B", 2.0), new StockKeepingUnit("C", 3.0)}; // TODO: Initialize to an appropriate value
      StockKeepingUnits target = new StockKeepingUnits(collection);
      Assert.IsNotNull(target);
      Assert.AreEqual<int>(target.Count, 3);
    }
  }
}
