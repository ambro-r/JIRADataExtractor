using JIRADataExtractor.JSONObjects;
using JIRADataExtractor.JSONQueryResultsObjects;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor
{
    class SprintIssues
    {
        private JIRAConnectionHandler JIRAConnectionHandler;
        private SprintIssues() { }

        public SprintIssues(String userName, String password, String baseURL)
        {
            JIRAConnectionHandler = new JIRAConnectionHandler(userName, password, baseURL);
        }

        public List<Issue> getSprintIssues(long sprintID)
        {
            return getSprintIssues(sprintID, new string[] { });
        }

        public List<Issue> getSprintIssues(long sprintID, String[] customFields)
        {
            Log.Information("Getting issues for sprint with ID {sprintID}", sprintID);
            return parseJSON(execute("/rest/agile/1.0/sprint/" + sprintID, customFields), customFields);
        }

        public List<Issue> getSprintIssues(String sprintName)
        {
            return getSprintIssues(sprintName, new string[] { });
        }

        public List<Issue> getSprintIssues(String sprintName, String[] customFields)
        {
            Log.Information("Getting issues for sprint with name {sprintName}", sprintName);
            return parseJSON(execute("/rest/api/3/search?jql=Sprint=\"" + sprintName + "\"", customFields), customFields);
        }

        private string execute(String unfilteredURI, String[] customFields)
        {
            string fields = "&fields=issuetype,parent,summary,created,priority,status";
            foreach (string field in customFields)
            {
                fields += "," + field;
            }
            Log.Debug("Field filter applied is {fields}", fields);
            return JIRAConnectionHandler.execute(unfilteredURI + fields);
        }

        private List<Issue> parseJSON(String jSONResponse, String[] customFields)
        {
            List<Issue> issues = new List<Issue>();
            var jsonData = JObject.Parse(jSONResponse);
            foreach (JObject issueJSON in jsonData["issues"])
            {
                Issue issue = new Issue();
                issue.Id = (long)issueJSON["id"];
                issue.Key = (string)issueJSON["key"];

                JObject fieldJSON = (JObject) issueJSON["fields"];
                issue.Summary = (string)fieldJSON["summary"];
                issue.CreatedDate = (DateTimeOffset)fieldJSON["created"];
                issue.Priority = (string)fieldJSON["priority"]["name"];
                issue.Status = (string)fieldJSON["status"]["name"];
                issue.Type = (string)fieldJSON["issuetype"]["name"];
                

                if (fieldJSON.ContainsKey("parent"))
                {
                    issue.Parent = (string)fieldJSON["parent"]["key"];
                }

                Dictionary<string, string> custom = new Dictionary<string, string>();
                foreach (string field in customFields)
                {
                    custom.Add(field, (string)fieldJSON[field]);
                }
                issue.CustomFields = custom;
                issues.Add(issue);
                Console.WriteLine(issue.ToString());
            }
            Log.Information("{issueCount} issues found in sprint.", issues.Count);
            return issues;
        }

    }
}
