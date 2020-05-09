using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class Sprint
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? StartDate { get; set; }

        [JsonProperty("endDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? EndDate { get; set; }

        [JsonProperty("completeDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CompleteDate { get; set; }

        [JsonProperty("originBoardId")]
        public long OriginBoardId { get; set; }

        [JsonProperty("goal")]
        public string Goal { get; set; }
    }
}
