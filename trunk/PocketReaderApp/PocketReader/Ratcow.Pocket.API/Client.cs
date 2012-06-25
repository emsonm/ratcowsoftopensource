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

    /// <summary>
    ///
    /// </summary>
    public Response AddRaw(Credentials credentials, string url, string title)
    {
      return DirectCall(Constants.Add + String.Format(Constants.AddInfill, url, title), credentials);
    }

    /// <summary>
    /// This is probably too simplistic?
    /// </summary>
    public bool Add(Credentials credentials, string url, string title)
    {
      var result = AddRaw(credentials, url, title);

      return result.StatusCode == 200;
    }

    /// <summary>
    ///
    /// </summary>
    public Response GetRaw(Credentials credentials, StateRequest state, Int32 since, Int32 count, bool tags, bool thisAppOnly, bool asXml)
    {
      var list = new StringBuilder(Constants.Get);

      //Filter by state
      switch (state)
      {
        case StateRequest.Read:
          list.Append(Constants.GetRead);
          break;

        case StateRequest.Unread:
          list.Append(Constants.GetUnread);
          break;

        case StateRequest.All:
        default:
          break;
      }

      //use the "since" API call
      if (since > 0)
      {
        list.AppendFormat(Constants.GetSinceInfill, since);
      }

      //Limit the results, 0 here means all
      if (count > 0)
      {
        list.AppendFormat(Constants.GetCountInfill, count);
      }

      //Get the tags?
      list.AppendFormat(Constants.GetTagsInfill, tags ? "1" : "0");

      //Just stuff this API key added
      list.AppendFormat(Constants.GetForThisAppInfill, thisAppOnly ? "1" : "0");

      //XML or JSON
      if (asXml) list.Append(Constants.FormatXml);

      return DirectCall(list.ToString(), credentials);
    }

    public GetResponse GetBasic(Credentials credentials, int since)
    {
      var result = GetRaw(credentials, StateRequest.All, since, 0, true, false, false);

      //deserialise the response
      try
      {
        var resultInstance = JsonConvert.DeserializeObject<GetResponse>(result.ReturnedValue);

        return resultInstance;
      }
      catch //todo: pad this out
      {
        return null;
      }
    }
  }
}