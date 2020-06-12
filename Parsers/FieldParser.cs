using JIRADataExtractor.Constants;
using JIRADataExtractor.Handlers;
using JIRADataExtractor.Models;
using JIRADataExtractor.Parsers;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIRADataExtractor
{
    class FieldParser : Parser
    {
        public FieldParser(ConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }
        public FieldParser(Connection connection) : base(connection)
        {
        }
        public FieldParser(String userName, String password, String baseURL) : base(userName, password, baseURL)
        {
        }

        public Dictionary<string, string> GetCustomElements()
        {
            List<Field> fields = GetFields();
            Dictionary<string, string> customElements = new Dictionary<string, string>();
            foreach (Field field in fields)
            {
                if(string.Equals(field.Key, Fields.CUSTOM_SPRINT, StringComparison.OrdinalIgnoreCase))
                {
                    customElements.Add(field.Name.ToLower(), Fields.CUSTOM_SPRINT.ToLower());
                }
            }
            return customElements;
        }

        public List<Field> GetFields() {
            List<Field> fields = new List<Field>();
            string jSONResponse = JIRAConnectionHandler.BasicAuthentication("/rest/api/3/field");
            JArray jsonArray = JArray.Parse(jSONResponse);
            Log.Debug("Returned JSON response is an array with {count} elements.", jsonArray.Count);
            foreach (var element in jsonArray) {
                fields.Add(JToken.Parse(element.ToString()).ToObject<Field>());
            }
            Log.Information("Found {fieldCount} fields.", fields.Count);
            return fields;
        }

    }
}
