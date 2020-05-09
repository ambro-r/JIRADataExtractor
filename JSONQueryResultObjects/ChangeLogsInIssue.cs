using JIRADataExtractor.JSONObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONQueryResultsObjects
{
    class ChangeLogsInIssue
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("maxResults")]
        public long MaxResults { get; set; }

        [JsonProperty("startAt")]
        public long StartAt { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("isLast")]
        public bool IsLast { get; set; }

        [JsonProperty("values")]
        public ChangeLog[] ChangeLogs { get; set; }
    }
}
