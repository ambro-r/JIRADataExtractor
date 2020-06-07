using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Objects
{
    class Board
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location/projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("location/projectKey")]
        public string ProjectKey { get; set; }

        public override string ToString()
        {
            return new StringBuilder("[Board:")
                .Append("id=").Append(Id)
                .Append(", name=").Append(Name)
                .Append(", projectName=").Append(ProjectName)
                .Append(", projectKey=").Append(ProjectKey)
                .Append("]").ToString();
        }
    }
}
