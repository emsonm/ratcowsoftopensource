using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace RatCow.Pocket.API.DataObjects
{
  public class StatsResponse
  {
    [JsonProperty("user_since")]
    public int UserSince { get; set; }

    [JsonProperty("count_list")]
    public int CountList { get; set; }

    [JsonProperty("count_unread")]
    public int CountUnread { get; set; }

    [JsonProperty("count_read")]
    public int CountRead { get; set; }
  }
}