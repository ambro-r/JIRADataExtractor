
using JIRADataExtractor.JSONObjects;
using JIRADataExtractor.JSONQueryResultsObjects;
using Newtonsoft.Json.Linq;

using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace JIRADataExtractor
{
    class LastCompletedSprint
    {

        private JIRAConnectionHandler JIRAConnectionHandler;
        private LastCompletedSprint() { }

        public LastCompletedSprint(String userName, String password, String baseURL)
        {
            JIRAConnectionHandler = new JIRAConnectionHandler(userName, password, baseURL);
        }

        public void sprints(long boardId)
        {
            long sprintID = getLastClosedSprintID(boardId);
            string jSONResponse = JIRAConnectionHandler.execute("/rest/agile/1.0/sprint/" + sprintID);
            Sprint sprint = JToken.Parse(jSONResponse).ToObject<Sprint>();
            jSONResponse = JIRAConnectionHandler.execute("/rest/agile/1.0/sprint/" + sprintID + "/issue?fields=epic,priority,status,created");
            IssuesInSprint issuesInSprint = JToken.Parse(jSONResponse).ToObject<IssuesInSprint>();
            StringBuilder issues = new StringBuilder();
            foreach (Issue issue in issuesInSprint.Issues)
            {
                string dateAdded = getDateAddedToSprint(sprint.Name, issue.Id);
                issues.Append("\n\t").Append(issue.Key).Append(",").Append(issue.Fields.Status.Name).Append(",").Append(dateAdded);
            }
            Log.Information("{sprintName} ({startDate} to {endate})", sprint.Name, sprint.StartDate.Value.Date.ToString(), sprint.EndDate.Value.Date.ToString());
            Log.Information("\tIssue count: {issueCount}.", issuesInSprint.Issues.Length);
            Log.Information("{issues}", issues.ToString());
        }

        // TO DO : Need to support pagination 
        private string getDateAddedToSprint(string sprintName, long issueID)
        {
            string jSONResponse = JIRAConnectionHandler.execute("/rest/api/3/issue/" + issueID + "/changelog");
            ChangeLogsInIssue changeLogsInIssue = JToken.Parse(jSONResponse).ToObject<ChangeLogsInIssue>();
            foreach(ChangeLog changeLog in changeLogsInIssue.ChangeLogs)
            {
                foreach(Item item in changeLog.Items)
                {
                    if("Sprint".Equals(item.Field) && sprintName.Equals(item.ItemToString))
                    {
                        Log.Debug("Found sprint change log item from:{from} to:{to} on {createdDate}", item.ItemFromString, item.ItemToString, changeLog.CreatedDate.ToString());
                        return changeLog.CreatedDate.Value.Date.ToString();
                    }
                }
                
            }
            return "";
        }


        private long getLastClosedSprintID(long boardID)
        {
            Log.Information("Getting the last closed sprint for board {boardId}", boardID);
            DateTime closed = DateTime.MinValue;
            long sprintID = -1;
            var result = string.Empty;
            try
            {
                Boolean processNextPage = true;
                long position = 0;
                while (processNextPage) {
                    Log.Debug("Quering board {boardID} for closed sprints, starting at {position}", boardID, position);
                    string jSONResponse = JIRAConnectionHandler.execute("/rest/agile/1.0/board/" + boardID + "/sprint?state=closed&startAt=" + position);
                    SprintsInBoard board = JToken.Parse(jSONResponse).ToObject<SprintsInBoard>();
                    Log.Debug("{sprintCount} / {maxResults} sprints returned.", board.Sprints.Length, board.MaxResults);
                    foreach (Sprint sprint in board.Sprints)
                    {
                        Log.Debug("Evaluating sprint {sprintID} closed {completedDate}", sprint.Id, sprint.CompleteDate.ToString());
                        DateTime current = sprint.CompleteDate.HasValue ? sprint.CompleteDate.Value.DateTime : DateTime.MinValue;
                        if (closed < current)
                        {
                            closed = current;
                            sprintID = sprint.Id;
                        }
                    }
                    if (board.Sprints.Length >= board.MaxResults)
                    {
                        position += board.MaxResults;
                    } else
                    {
                        processNextPage = false;
                    }
                }
          
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Log.Information("The last closed sprint for board {boardId} is sprint {sprintID}, closed {closedDate}.", boardID, sprintID, closed);
            return sprintID;
        }
    }
}
