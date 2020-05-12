using JIRADataExtractor.Objects;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIRADataExtractor
{
    class FieldParser
    {
        private JIRAConnectionHandler JIRAConnectionHandler;
        private FieldParser() { }
        public FieldParser(JIRAConnectionHandler jIRAConnectionHandler)
        {
            JIRAConnectionHandler = jIRAConnectionHandler;
        }
        public FieldParser(String userName, String password, String baseURL)
        {
            JIRAConnectionHandler = new JIRAConnectionHandler(userName, password, baseURL);
        }

        public List<Field> GetFields() {
            List<Field> fields = new List<Field>();
            string jSONResponse = JIRAConnectionHandler.execute("/rest/api/3/field");
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
