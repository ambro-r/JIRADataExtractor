using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JIRADataExtractor
{
    class Issue
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public long Id { get; set; }
        public string Key { get; set; }
        public string Parent { get; set; }
        public string Priority { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Dictionary<String, String> CustomFields { get; set; } 

        public Issue()
        {
            CustomFields = new Dictionary<string, string>();
            Parent = "";
        }

        public override string ToString() {
            return new StringBuilder("[Issue:")
                .Append(", id=").Append(Id)
                .Append(", Key=").Append(Key)
                .Append(", CreatedDate=").Append(CreatedDate)
                .Append(", Priority=").Append(Priority)
                .Append(", Status=").Append(Status)
                .Append(", Type=").Append(Type)
                .Append(", Parent=").Append(Parent)
                .Append(", Summary=").Append(Summary)
                .Append(", CustomFields=(").Append(string.Join("'", CustomFields.Select(a => $"{a.Key}:{a.Value}"))).Append(")")
                .Append("]").ToString();
        }
    }
}
