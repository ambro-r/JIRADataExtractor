using JIRADataExtractor.Constants;
using JIRADataExtractor.Converters;
using JIRADataExtractor.Objects;
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
        public BoardParser(JIRAConnectionHandler jIRAConnectionHandler) : base(jIRAConnectionHandler)
        {
        }
        public BoardParser(String userName, String password, String baseURL) : base(userName, password, baseURL)
        {
        }

        public List<Sprint> GetSprints(int boardID)
        {
            Log.Information("Getting sprints for board with boardID {boardID}", boardID);
            List<Sprint> sprints = new List<Sprint>();
            int startAt = 0;
            bool isLast = false;
            while (!isLast)
            {
                string jSONResponse = JIRAConnectionHandler.execute("/rest/agile/1.0/board/" + boardID + "/sprint?startAt=" + startAt);
                var jObject = JObject.Parse(jSONResponse);
                int maxResults = Convert.ToInt32(jObject[JQLSearchResult.MAX_RESULTS]);
                isLast = Convert.ToBoolean(jObject[JQLSearchResult.IS_LAST]);
                Log.Debug("Getting up to {maxResults} results per search. Current result set starting a {startAt}, is last: {isLast}.", maxResults, startAt, isLast);
                foreach (JObject sprintJSON in jObject[JQLSearchResult.VALUES])
                {
                    sprints.Add(ParseJSON<Sprint>(sprintJSON.ToString()));
                }
            }
            Log.Information("Board with boardID {boardID} has {sprintCount} issues.", boardID, sprints.Count);
            return sprints;
        }
        public Board GetBoard(int boardID)
        {
            Log.Information("Getting board with boardID {boardID}", boardID);
            return ParseJSON<Board>(JIRAConnectionHandler.execute("/rest/agile/1.0/board/" + boardID));
        }
        private T ParseJSON<T>(string jSONResponse)
        {
            if (Log.IsEnabled(LogEventLevel.Verbose))
            {
                Log.Verbose("Parsing JSON Object:\n{jsonData}", JObject.Parse(jSONResponse).ToString());
            }
            Type objectType = typeof(T);
            var settings = new JsonSerializerSettings();
            if (objectType.Equals(typeof(Board)))
            {
                settings.Converters.Add(new NestedJSONConverter<Board>());
            }
            else if (objectType.Equals(typeof(Sprint)))
            {
                settings.Converters.Add(new NestedJSONConverter<Sprint>());
            }
            return JsonConvert.DeserializeObject<T>(jSONResponse, settings);
        }

    }
}
