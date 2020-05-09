using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class ChangeLog
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CreatedDate { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("historyMetadata", NullValueHandling = NullValueHandling.Ignore)]
        public HistoryMetadata HistoryMetadata { get; set; }
    }
}
