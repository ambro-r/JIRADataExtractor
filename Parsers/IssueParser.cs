using JIRADataExtractor.Constants;
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
        public IssueParser(ConnectionDetails connectionDetails) : base(connectionDetails)
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
            List<Issue> issues = new List<Issue>();
            int startAt = 0;
            string jqlFilter = GetJQLFilter(jQLFilters);
            Log.Information("Searching issues with filter {jqlFilter}", jqlFilter);
            bool moreResultsAvailable = true;
            while (moreResultsAvailable) {
                string jSONResponse = JIRAConnectionHandler.execute("/rest/api/3/search?startAt=" + startAt + "&" + jqlFilter);
                var jObject = JObject.Parse(jSONResponse);
                int maxResults = Convert.ToInt32(jObject[JQLSearchResult.MAX_RESULTS]);
                int totalResults = Convert.ToInt32(jObject[JQLSearchResult.TOTAL]);
                Log.Debug("Getting up to {maxResults} results per search. Current result set starting a {startAt}, total results: {totalResults}.", maxResults, startAt, totalResults);
                startAt += maxResults;
                moreResultsAvailable = startAt < totalResults;
                Log.Verbose("More results available: {moreResultsAvailable}.", moreResultsAvailable);
                foreach (JObject issueJSON in jObject["issues"])
                {
                    issues.Add(ParseJSON<Issue>(issueJSON.ToString(), customElements));  
                }
            }
            Log.Information("Issue search has returned {issueCount} issues.", issues.Count);
            return issues;
        }
        /*
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
        */

    }
}
