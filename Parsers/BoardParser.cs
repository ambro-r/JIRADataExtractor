using JIRADataExtractor.Constants;
using JIRADataExtractor.Converters;
using JIRADataExtractor.Handlers;
using JIRADataExtractor.Models;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JIRADataExtractor.Parsers
{
    class BoardParser : Parser
    {
        public BoardParser(ConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }
        public BoardParser(ConnectionDetails connectionDetails) : base(connectionDetails)
        {
        }
        public BoardParser(String userName, String password, String baseURL) : base(userName, password, baseURL)
        {
        }

        public Board GetBoard(long boardID)
        {
            Log.Information("Getting board with boardID {boardID}", boardID);
            return ParseJSON<Board>(JIRAConnectionHandler.BasicAuthentication("/rest/agile/1.0/board/" + boardID));
        }

        public List<Sprint> GetSprints(Board board)
        {
            return GetSprints(board.Id);
        }

        public List<Sprint> GetSprints(long boardID)
        {
            Log.Information("Getting sprints for board with boardID {boardID}", boardID);
            List<Sprint> sprints = new List<Sprint>();
            int startAt = 0;
            bool isLast = false;
            while (!isLast)
            {
                string jSONResponse = JIRAConnectionHandler.BasicAuthentication("/rest/agile/1.0/board/" + boardID + "/sprint?startAt=" + startAt);
                var jObject = JObject.Parse(jSONResponse);
                int maxResults = Convert.ToInt32(jObject[JQLSearchResult.MAX_RESULTS]);
                isLast = Convert.ToBoolean(jObject[JQLSearchResult.IS_LAST]);
                Log.Debug("Getting up to {maxResults} results per search. Current result set starting a {startAt}, is last: {isLast}.", maxResults, startAt, isLast);
                if (jObject[JQLSearchResult.VALUES] != null)
                {
                    foreach (JObject sprintJSON in jObject[JQLSearchResult.VALUES])
                    {
                        sprints.Add(ParseJSON<Sprint>(sprintJSON.ToString()));
                    }
                }
                else
                {                                  
                    Log.Information("No sprints found for board with boardID {boardID}", boardID);
                    break; // Break out the loop
                }
            }
            Log.Information("Board with boardID {boardID} has {sprintCount} issues.", boardID, sprints.Count);
            return sprints;
        }

    }
}
