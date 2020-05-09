using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class Fields
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("issuetype")]
        public Issuetype Issuetype { get; set; }

        [JsonProperty("epic")]
        public object Epic { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
