using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Models
{
    class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("custom")]
        public bool Custom { get; set; }

        public override string ToString()
        {
            return new StringBuilder("[Field:")
                .Append("id=").Append(Id)
                .Append(", Key=").Append(Key)
                .Append(", name=").Append(Name)
                .Append(", custom=").Append(Custom)
                .Append("]").ToString();
        }
    }
}
