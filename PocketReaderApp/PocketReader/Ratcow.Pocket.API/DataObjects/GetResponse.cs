using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace RatCow.Pocket.API.DataObjects
{
  /*
  {"status":1,
   "list":{"155011097":{"item_id":"155011097","title":"Add api test","url":"http://getpocket.com/api/docs","time_updated":"1340632130","time_added":"1340632130","state":"0"}},
   "since":1340632130,
   "complete":1}
  */

  //{"status":1,
  // "list":{
  //   "84343336":{"item_id":"84343336","title":"Password haystack","url":"https://www.grc.com/haystack.htm","time_updated":"1340634168","time_added":"1340634168","state":"0"},
  //   "88880153":{"item_id":"88880153","title":"NHibernate vs Entity FW","url":"http://ayende.com/blog/4351/nhibernate-vs-entity-framework-4-0","time_updated":"1340634162","time_added":"1340634162","state":"0"},
  //   "183436916":{"item_id":"183436916","title":"OSNews : hands-on/hands-off","url":"http://www.osnews.com/story/26112/Hands-on_or_hands-off_","time_updated":"1340634156","time_added":"1340634156","state":"0"},
  //   "155011097":{"item_id":"155011097","title":"Add api test","url":"http://getpocket.com/api/docs","time_updated":"1340632130","time_added":"1340632130","state":"0"}
  // },
  // "since":1340634168,
  // "complete":1}

  public class GetResponse
  {
    public GetResponse()
    {
    }

    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("since")]
    public int Since { get; set; }

    [JsonProperty("list")]
    public Dictionary<int, GetResponseItem> Items { get; set; }

    [JsonProperty("complete")]
    public int Complete { get; set; }
  }

  public class GetResponseItem
  {
    public GetResponseItem()
    {
    }

    [JsonProperty("item_id")]
    public int ItemId { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("url")]
    public string URL { get; set; }

    [JsonProperty("time_updated")]
    public int TimeUpdated { get; set; }

    [JsonProperty("time_added")]
    public int TimeAdded { get; set; }

    [JsonProperty("state")]
    public int State { get; set; }
  }
}