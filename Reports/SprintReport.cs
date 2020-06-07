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
                Log.Debug("Last close sprint: {lastClosedSprint}", lastClosedSprint.ToString());
            } else
            {
                Log.Information("No closed sprints found for board \"{name}\".", board.Name);
            }
        }

    }
}
