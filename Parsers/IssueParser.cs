using JIRADataExtractor.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Parsers
{
    class IssueParser
    {
        private JIRAConnectionHandler JIRAConnectionHandler;
        private IssueParser() { }
        public IssueParser(JIRAConnectionHandler jIRAConnectionHandler)
        {
            JIRAConnectionHandler = jIRAConnectionHandler;
        }
        public IssueParser(String userName, String password, String baseURL)
        {
            JIRAConnectionHandler = new JIRAConnectionHandler(userName, password, baseURL);
        }

        public Issue GetIssue(long issueID)
        {
            return GetIssue(issueID, new Dictionary<string, string>());
        }

        public Issue GetIssue(long issueID, Dictionary<string, string> customElements)
        {
            Log.Information("Getting issue with id {issueID}", issueID);
            return ParseJSON(Execute(issueID.ToString()), customElements);
        }

        public Issue GetIssue(string issueKey)
        {
            return GetIssue(issueKey, new Dictionary<string, string>());
        }

        public Issue GetIssue(string issueKey, Dictionary<string, string> customElements)
        {
            Log.Information("Getting issue with key {issueKey}", issueKey);
            return ParseJSON(Execute(issueKey), customElements);
        }

        private string Execute(string issueIdOrKey)
        {
            return JIRAConnectionHandler.execute("/rest/api/3/issue/" + issueIdOrKey);
        }

        private Issue ParseJSON(String jSONResponse, Dictionary<string, string> customElements)
        { 
            if(Log.IsEnabled(LogEventLevel.Debug))
            {
                Log.Debug("{jsonData}", JObject.Parse(jSONResponse).ToString());
            }
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NestedJSONConverter<Issue>(customElements));
            return JsonConvert.DeserializeObject<Issue>(jSONResponse, settings);
        }

    }
}
