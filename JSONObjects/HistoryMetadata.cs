using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class HistoryMetadata
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
