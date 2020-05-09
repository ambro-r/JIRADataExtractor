using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.JSONObjects
{
    class Issuetype
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subtask")]
        public bool Subtask { get; set; }

        [JsonProperty("avatarId")]
        public long AvatarId { get; set; }

        [JsonProperty("entityId")]
        public Guid EntityId { get; set; }
    }
}
