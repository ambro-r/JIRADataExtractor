using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace JIRADataExtractor
{
    [JsonObject]
    class Issue
    {
        [JsonProperty("fields/created")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("fields/parent/key")]
        public string ParentKey { get; set; }
        [JsonProperty("fields/parent/fields/summary")]
        public string ParentName { get; set; }
        [JsonProperty("fields/parent/fields/issuetype/name")]
        public string ParentType { get; set; }
        [JsonProperty("fields/priority/name")]
        public string Priority { get; set; }
        [JsonProperty("fields/project/key")]
        public string ProjectKey { get; set; }
        [JsonProperty("fields/project/name")]
        public string ProjectName { get; set; }
        [JsonProperty("fields/project/projectCategory/name")]
        public string ProjectCategory { get; set; }
        [JsonProperty("fields/summary")]   
        public string Summary { get; set; }
        [JsonProperty("fields/status/name")]
        public string Status { get; set; }
        [JsonProperty("fields/issuetype/name")]
        public string Type { get; set; }
        [JsonProperty("fields/custom.element.sprint")]
        public string[] Sprint { get; set; }

        public Issue()
        {
            Sprint = new string[] { };
        }        

        private string toCSV()
        {
            return "";
        }

        public override string ToString() {
            return new StringBuilder("[Issue:")
                .Append(", id=").Append(Id)
                .Append(", Key=").Append(Key)
                .Append(", CreatedDate=").Append(CreatedDate)
                .Append(", Priority=").Append(Priority)
                .Append(", Status=").Append(Status)
                .Append(", Type=").Append(Type)
                .Append(string.Format(", Parent(key={0};type={1};name:{2})", ParentKey, ParentType, ParentName))
                .Append(string.Format(", Project(key={0};name:{1};category:{2})", ProjectKey, ProjectName, ProjectCategory))
                .Append(", Summary=").Append(Summary)
                .Append(string.Format(", Sprint=({0})", string.Join(";", Sprint.Select(a => a))))
                .Append("]").ToString();
        }
    }
}
