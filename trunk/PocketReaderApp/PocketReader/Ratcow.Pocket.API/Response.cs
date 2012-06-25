using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace RatCow.Pocket.API
{
  public class Response
  {
    public string ReturnedValue { get; set; }

    public bool WasCalled { get; set; }

    public string Status { get; set; }

    public int StatusCode { get; set; }

    public int UserLimit { get; set; }

    public int UserRemaining { get; set; }

    public int UserReset { get; set; }

    public int KeyLimit { get; set; }

    public int KeyRemaining { get; set; }

    public int KeyReset { get; set; }

    /// <summary>
    /// Get all the limits
    /// </summary>
    public void SetLimits(NameValueCollection headers)
    {
      UserLimit = Int32.Parse(headers["X-Limit-User-Limit"] ?? "0");
      UserRemaining = Int32.Parse(headers["X-Limit-User-Remaining"] ?? "0");
      UserReset = Int32.Parse(headers["X-Limit-User-Reset"] ?? "0");

      KeyLimit = Int32.Parse(headers["X-Limit-Key-Limit"] ?? "0");
      KeyRemaining = Int32.Parse(headers["X-Limit-Key-Remaining"] ?? "0");
      KeyReset = Int32.Parse(headers["X-Limit-Key-Reset"] ?? "0");
    }
  }

  public enum AuthenticationResponses : int { OK = 200, InvalidRequest = 400, BadCredentials = 401, RateLimitExceeded = 403, ServerDown = 503, UnknownError = 0 }

  public enum StateRequest : byte { All, Read, Unread }
}