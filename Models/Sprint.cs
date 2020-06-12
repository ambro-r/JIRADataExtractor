using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Models
{
    class Sprint
    {

       [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("completeDate")]
        public DateTime CompleteDate { get; set; }

        public override string ToString()
        {
            return new StringBuilder("[Sprint:")
                .Append("id=").Append(Id)
                .Append(", State=").Append(State)
                .Append(", Name=").Append(Name)
                .Append(", StartDate=").Append(StartDate)
                .Append(", EndDate=").Append(EndDate)
                .Append(", CompleteDate=").Append(CompleteDate)
                .Append("]").ToString();
        }
    }
}
