using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class Item
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("fieldtype")]
        public string Fieldtype { get; set; }

        [JsonProperty("fieldId", NullValueHandling = NullValueHandling.Ignore)]
        public string FieldId { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("fromString")]
        public string ItemFromString { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("toString")]
        public string ItemToString { get; set; }

        [JsonProperty("tmpFromAccountId")]
        public string TmpFromAccountId { get; set; }

        [JsonProperty("tmpToAccountId")]
        public string TmpToAccountId { get; set; }
    }
}
