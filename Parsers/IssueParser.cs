using JIRADataExtractor.Converters;
using JIRADataExtractor.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIRADataExtractor.Parsers
{
    class IssueParser : Parser
    {
        public IssueParser(JIRAConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }
        public IssueParser(String userName, String password, String baseURL) : base(userName, password, baseURL)
        {
        }
        public Issue GetIssue(string issueIdOrKey)
        {
            return GetIssue(issueIdOrKey, new Dictionary<string, string>());
        }
        public Issue GetIssue(string issueIdOrKey, Dictionary<string, string> customElements)
        {
            Log.Information("Getting issue with issueIdOrKey {issueIdOrKey}", issueIdOrKey);
            return ParseJSON(JIRAConnectionHandler.execute("/rest/api/3/issue/" + issueIdOrKey), customElements);
        }
        public List<Issue> SearchIssues(List<JQLFilter> jQLFilters)
        {
            return SearchIssues(jQLFilters, new Dictionary<string, string>());
        }
        public List<Issue> SearchIssues(List<JQLFilter> jQLFilters, Dictionary<string, string> customElements)
        {
            string jqlFilter = "&" + GetJQLFilter(jQLFilters);
            int startAt = 15;
            string jSONResponse = JIRAConnectionHandler.execute("/rest/api/3/search?startAt=" + startAt + jqlFilter);
            Console.WriteLine(jSONResponse);
            /*
             {
   "expand":"schema,names",
   "startAt":0,
   "maxResults":50,
   "total":16,
   "issues":[

             */
            return null;
        }

        /*
       * project = PHX AND updated >= 2020-05-11 AND updated <= 2020-05-23 order by created DESC
       *https://bitventuredev.atlassian.net/browse/PHX-123?jql=project%20%3D%20PHX%20AND%20updated%20%3E%3D%202020-05-11%20AND%20updated%20%3C%3D%202020-05-23%20order%20by%20created%20DESC
       */

        private Issue ParseJSON(string jSONResponse, Dictionary<string, string> customElements)
        { 
            if(Log.IsEnabled(LogEventLevel.Verbose))
            {
                Log.Verbose("Parsing JSON Object:\n{jsonData}", JObject.Parse(jSONResponse).ToString());
            }
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NestedJSONConverter<Issue>(customElements));
            return JsonConvert.DeserializeObject<Issue>(jSONResponse, settings);
        }

    }
}
