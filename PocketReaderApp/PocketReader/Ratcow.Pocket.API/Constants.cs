using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Pocket.API
{
  internal class Constants
  {
    /// <summary>
    /// Base of the API
    /// </summary>
    public static string APIBase = "https://readitlaterlist.com/v2/";

    /// <summary>
    /// Used whenever a login action is required
    /// </summary>
    public static string LoginCredentials = "username={0}&password={1}&apikey={2}";

    public static string FormatXml = "&format=xml";
    public static string FormatJson = String.Empty; //Json is default

    #region Authenticate

    /// <summary>
    /// Basis for the authenticate call
    /// </summary>
    public static string AuthenticateBase = APIBase + "auth?";

    /// <summary>
    /// Authenticate, including params
    /// </summary>
    public static string Authenticate = AuthenticateBase + LoginCredentials;

    #endregion Authenticate

    public static string StatsBase = APIBase + "stats?";
    public static string Stats = StatsBase + LoginCredentials;
    public static string StatsXml = StatsBase + LoginCredentials + FormatXml;
    public static string StatsJson = Stats; //as this is essentially the same
  }
}