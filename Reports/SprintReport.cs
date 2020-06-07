using JIRADataExtractor.Constants;
using JIRADataExtractor.Handlers;
using JIRADataExtractor.Objects;
using JIRADataExtractor.Parsers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Reports
{
    class SprintReport : Report
    {
        public SprintReport(JIRAConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }

        public void LastCloseSprintReport(long boardID)
        {
            BoardParser boardParser = new BoardParser(JIRAConnectionHandler);
            Board board = boardParser.GetBoard(boardID);
            List<Sprint> sprints = boardParser.GetSprints(board);
            Sprint lastClosedSprint = null;
            foreach (Sprint sprint in sprints)
            {
                if("closed".Equals(sprint.State.ToLower())) {
                    if (lastClosedSprint == null || (lastClosedSprint.CompleteDate < sprint.CompleteDate)) {
                        lastClosedSprint = sprint;
                    }
                }                
            }
            if(lastClosedSprint != null)
            {
                Log.Debug("Last closed sprint: {lastClosedSprint}", lastClosedSprint.ToString());
                List<JQLFilter> jQLFilters = new List<JQLFilter>(3);
                jQLFilters.Add(new JQLFilter(Fields.PROJECT, Comparison.EQUAL_TO, board.ProjectKey, Gate.AND));
                jQLFilters.Add(new JQLFilter(Fields.UPDATED, Comparison.GREATER_THAN_EQUAL_TO, lastClosedSprint.StartDate.ToString("yyyy-MM-dd"), Gate.AND));
                jQLFilters.Add(new JQLFilter(Fields.UPDATED, Comparison.LESS_THAN_EQUAL_TO, lastClosedSprint.CompleteDate.ToString("yyyy-MM-dd"), Gate.AND));
                FieldParser fieldParser = new FieldParser(JIRAConnectionHandler);
                IssueParser issueParser = new IssueParser(JIRAConnectionHandler);
                List<Issue> issues = issueParser.SearchIssues(jQLFilters, fieldParser.GetCustomElements());
                Log.Debug("{issueCount} issues found that were updted between {startDate} and {endDate} for project with key {key}", issues.Count, lastClosedSprint.StartDate.ToString("yyyy-MM-dd"), lastClosedSprint.CompleteDate.ToString("yyyy-MM-dd"), board.ProjectKey);
                foreach(Issue issue in issues)
                {                   
                    string changeLog = issueParser.GetIssueChangeLog(issue.Key);
                    // TO DO : Need to parse through the change log to figure out if the issue had anything to do with the sprint.
                }
            }
            else
            {
                Log.Information("No closed sprints found for board \"{name}\".", board.Name);
            }
        }

    }
}
