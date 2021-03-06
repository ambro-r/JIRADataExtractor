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
using System.Text;

namespace JIRADataExtractor.Parsers
{
    class Parser
    {
        protected ConnectionHandler ConnectionHandler;
        protected Parser() { }
        protected Parser(ConnectionHandler connectionHandler)
        {
            ConnectionHandler = connectionHandler;
        }
        public Parser(Connection connection) 
        {
            ConnectionHandler = new ConnectionHandler(connection.UserName, connection.Password, connection.BaseURL);
        }
        protected Parser(String userName, String password, String baseURL)
        {
            ConnectionHandler = new ConnectionHandler(userName, password, baseURL);
        }

        protected string GetJQLFilter(JQLFilter jQLFilter)
        {
            List<JQLFilter> jQLFilters = new List<JQLFilter>(1);
            jQLFilters.Add(jQLFilter);
            return GetJQLFilter(jQLFilters);
        }
        protected string GetJQLFilter(List<JQLFilter> jQLFilters)
        {
            StringBuilder jql = new StringBuilder();
            foreach (JQLFilter jQLFilter in jQLFilters)
            {
                if (jql.Length > 0 && !string.IsNullOrEmpty(jQLFilter.Gate.Value))
                {
                    jql.Append(" ").Append(jQLFilter.Gate.Value).Append(" ");
                }
                jql.Append(jQLFilter.Field).Append(jQLFilter.Comparison.Value)
                    .Append("\"").Append(jQLFilter.Value).Append("\"");
            }
            if (jql.Length > 0)
            {
                jql.Insert(0, "jql=");
            }
            return jql.ToString();
        }

        protected T ParseJSON<T>(string jSONResponse)
        {
            return ParseJSON<T>(jSONResponse, new Dictionary<string, string>(0));
        }

        protected T ParseJSON<T>(string jSONResponse, Dictionary<string, string> customElements)
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
            else if (objectType.Equals(typeof(Issue)))
            {
                settings.Converters.Add(new NestedJSONConverter<Issue>(customElements));
            }
            return JsonConvert.DeserializeObject<T>(jSONResponse, settings);
        }

    }
}
