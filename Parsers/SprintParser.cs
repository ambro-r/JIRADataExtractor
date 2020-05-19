using JIRADataExtractor.Constants;
using JIRADataExtractor.Objects;
using JIRADataExtractor.Parsers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor
{
    class SprintParser : Parser
    {
        // TO DO: Need to rethink and rework this
        public SprintParser(JIRAConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }
        public SprintParser(String userName, String password, String baseURL) : base(userName, password, baseURL)
        {
        }
        public List<Issue> GetSprintIssues(long sprintID)
        {
            return GetSprintIssues(sprintID, new string[] { });
        }

        public List<Issue> GetSprintIssues(long sprintID, String[] customFields)
        {
            Log.Information("Getting issues for sprint with ID {sprintID}", sprintID);
            return ParseJSON(Execute("/rest/agile/1.0/sprint/" + sprintID, customFields), customFields);
        }

        public List<Issue> GetSprintIssues(String sprintName)
        {
            return GetSprintIssues(sprintName, new string[] { });
        }

        public List<Issue> GetSprintIssues(String sprintName, String[] customFields)
        {
            Log.Information("Getting issues for sprint with name {sprintName}", sprintName);
            string jqlFilter = GetJQLFilter(new JQLFilter("Sprint", Comparison.EQUAL_TO, sprintName));
            return ParseJSON(Execute("/rest/api/3/search?" + jqlFilter, customFields), customFields);
        }

        private string Execute(String unfilteredURI, String[] customFields)
        {
            return JIRAConnectionHandler.execute(unfilteredURI);
        }

        private List<Issue> ParseJSON(String jSONResponse, String[] customFields)
        {

            List<Issue> issues = new List<Issue>();
            Console.WriteLine(jSONResponse);
            /*
            var jsonData = JObject.Parse(jSONResponse);
            Console.WriteLine(jsonData.ToString());
            foreach (JObject issueJSON in jsonData["issues"])
            {
                Issue issue = new Issue();
              

                Dictionary<string, string> custom = new Dictionary<string, string>();
                foreach (string field in customFields)
                {
                    custom.Add(field, (string)fieldJSON[field]);
                }
                issue.CustomFields = custom;

                issues.Add(issue);
            }
            Log.Information("{issueCount} issues found in sprint.", issues.Count);
            */
            return issues;
        }
        /*
             "customfield_10016": [
      "com.atlassian.greenhopper.service.sprint.Sprint@b1fce5f[id=65,rapidViewId=42,state=ACTIVE,name=Phoenix Iteration 02,goal=,startDate=2020-05-11T18:00:00.000Z,endDate=2020-05-22T15:00:00.000Z,completeDate=<null>,sequence=65]",
      "com.atlassian.greenhopper.service.sprint.Sprint@40dc7d41[id=63,rapidViewId=42,state=CLOSED,name=Phoenix Iteration 01,goal=To finish all items committed too.,startDate=2020-04-28T09:00:00.000Z,endDate=2020-05-08T15:00:00.000Z,completeDate=2020-05-08T17:28:29.905Z,sequence=63]"
    ],
    */

    }
}
