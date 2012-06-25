using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RatCow.Pocket.API.Tests
{
  using API;
  using API.DataObjects;

  /// <summary>
  /// Summary description for APITests
  /// </summary>
  [TestClass]
  public class APITests
  {
    public APITests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

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
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //

    #endregion Additional test attributes

    [TestMethod]
    public void TestAuthenticationRaw()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "1d7g8L3dp7afSA2b4cT998eo42A3Npi9" };

      var result = api.AuthenticateRaw(credentials);

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.IsTrue(result.ReturnedValue.Contains("200 OK"));
    }

    [TestMethod]
    public void TestAuthenticationRaw2()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "wrongpassword", APIKey = "1d7g8L3dp7afSA2b4cT998eo42A3Npi9" };

      var result = api.AuthenticateRaw(credentials);

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.IsFalse(result.ReturnedValue.Contains("200 OK"));
    }

    [TestMethod]
    public void TestAuthentication()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.Authenticate(credentials);

      Assert.IsNotNull(result);
      Assert.AreEqual(result, AuthenticationResponses.OK);
    }

    [TestMethod]
    public void TestStatsRawXml()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.StatsRaw(credentials, true); //xml

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);
    }

    [TestMethod]
    public void TestStatsRawJson()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.StatsRaw(credentials, false); //json

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);
    }

    [TestMethod]
    public void TestStatsJson()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.Stats(credentials); //json

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void TestAddRaw()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.AddRaw(credentials, "http://getpocket.com/api/docs", "Add api test");

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);
    }

    [TestMethod]
    public void TestAddRaw2()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.AddRaw(credentials, "http://www.osnews.com/story/26112/Hands-on_or_hands-off_", "OSNews : hands-on/hands-off");

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);

      result = api.AddRaw(credentials, "http://ayende.com/blog/4351/nhibernate-vs-entity-framework-4-0", "NHibernate vs Entity FW");

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);

      result = api.AddRaw(credentials, "https://www.grc.com/haystack.htm", "Password haystack");

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);
    }

    [TestMethod]
    public void TestGetRaw()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.GetRaw(credentials, StateRequest.All, 0, 0, true, false, false);

      Assert.IsNotNull(result);
      Assert.IsTrue(result.WasCalled);
      Assert.AreEqual(result.StatusCode, 200);
    }

    [TestMethod]
    public void TestGetBasic()
    {
      var api = new Client();
      var credentials = new Credentials() { UserName = "mytestaccount", Password = "MySuperPassword123", APIKey = "bz9Tfz7fgc950lfI97d126evp1p1GyP2" };

      var result = api.GetBasic(credentials, 0);

      Assert.IsNotNull(result);
    }
  }
}