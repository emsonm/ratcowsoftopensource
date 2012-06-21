using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Pocket.API
{
  public class Credentials
  {
    /// <summary>
    /// user name used to log in to Pocket
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Password used with supplied username
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The API Key applied for with developer centre
    /// </summary>
    public string APIKey { get; set; }
  }
}