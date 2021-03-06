﻿using JIRADataExtractor.Constants;
using JIRADataExtractor.Converters;
using JIRADataExtractor.Handlers;
using JIRADataExtractor.Models;
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
        public IssueParser(ConnectionHandler connectionHandler) : base(connectionHandler)
        {
        }
        public IssueParser(Connection connection) : base(connection)
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
            return ParseJSON<Issue>(ConnectionHandler.BasicAuthentication("/rest/api/3/issue/" + issueIdOrKey), customElements);
        }

        // TO DO : Need to build a proper object to represent the ChangeLog
        public string GetIssueChangeLog(string issueIdOrKey)
        {
            Log.Information("Getting issue changelog with issueIdOrKey {issueIdOrKey}", issueIdOrKey);
            return ConnectionHandler.BasicAuthentication("/rest/api/3/issue/" + issueIdOrKey + "?expand=changelog&fields=\"\"");
            
        }
        public List<Issue> SearchIssues(List<JQLFilter> jQLFilters)
        {
            return SearchIssues(jQLFilters, new Dictionary<string, string>());
        }
        // TO DO : When seaching, need to find a way to ony return certain fields (i.e. might only want a list of Issue Keys)
        // -> Need to append &fields= for a field list
        public List<Issue> SearchIssues(List<JQLFilter> jQLFilters, Dictionary<string, string> customElements)
        {
            List<Issue> issues = new List<Issue>();
            int startAt = 0;
            string jqlFilter = GetJQLFilter(jQLFilters);
            Log.Information("Searching issues with filter {jqlFilter}", jqlFilter);
            bool moreResultsAvailable = true;
            while (moreResultsAvailable) {
                string jSONResponse = ConnectionHandler.BasicAuthentication("/rest/api/3/search?startAt=" + startAt + "&" + jqlFilter);
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

    }
}
