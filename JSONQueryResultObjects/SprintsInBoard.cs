using JIRADataExtractor.JSONObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONQueryResultsObjects
{
    class SprintsInBoard
    {
        [JsonProperty("maxResults")]
        public long MaxResults { get; set; }

        [JsonProperty("startAt")]
        public long StartAt { get; set; }

        [JsonProperty("isLast")]
        public bool IsLast { get; set; }

        [JsonProperty("values")]
        public Sprint[] Sprints { get; set; }
    }
}
