using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Newtonsoft.Json;

namespace RatCow.Pocket.API
{
  using DataObjects;

  public class Client
  {
    /// <summary>
    /// helper
    /// </summary>
    private string ApiKeyWithCredentials(string api, Credentials credentials)
    {
      return String.Format(api, credentials.UserName, credentials.Password, credentials.APIKey);
    }

    /// <summary>
    /// Direct API Call
    /// </summary>
    internal Response DirectCall(string api, Credentials credentials)
    {
      var client = new WebClient();

      Response result = new Response() { WasCalled = false };

      try
      {
        result.ReturnedValue = client.DownloadString(ApiKeyWithCredentials(api, credentials));
        result.WasCalled = true;
        result.StatusCode = 200;
        result.Status = client.ResponseHeaders["Status"];
        result.SetLimits(client.ResponseHeaders);
      }
      catch (WebException ex)
      {
        result.ReturnedValue = String.Empty;
        result.WasCalled = true;
        var response = ((HttpWebResponse)(ex.Response));
        result.Status = response.StatusCode.ToString();
        result.StatusCode = (int)response.StatusCode;
      }

      return result;
    }

    /// <summary>
    /// A raw call returning exactly what the server does
    /// </summary>
    public Response AuthenticateRaw(Credentials credentials)
    {
      return DirectCall(Constants.Authenticate, credentials);
    }

    /// <summary>
    /// Gives more specific responses
    /// </summary>
    public AuthenticationResponses Authenticate(Credentials credentials)
    {
      var result = AuthenticateRaw(credentials);

      switch (result.StatusCode)
      {
        case 200:
          return AuthenticationResponses.OK;
        case 400:
          return AuthenticationResponses.InvalidRequest;
        case 401:
          return AuthenticationResponses.BadCredentials;
        case 403:
          return AuthenticationResponses.RateLimitExceeded;
        case 503:
          return AuthenticationResponses.ServerDown;
        default:
          return AuthenticationResponses.UnknownError;
      }
    }

    /// <summary>
    /// Stats raw call
    /// </summary>
    public Response StatsRaw(Credentials credentials, bool asXML)
    {
      return DirectCall(asXML ? Constants.StatsXml : Constants.StatsJson, credentials);
    }

    /// <summary>
    ///
    /// </summary>
    public StatsResponse Stats(Credentials credentials)
    {
      var result = StatsRaw(credentials, false);

      //deserialise the response
      try
      {
        var resultInstance = JsonConvert.DeserializeObject<StatsResponse>(result.ReturnedValue);

        return resultInstance;
      }
      catch //todo: pad this out
      {
        return null;
      }
    }
  }
}