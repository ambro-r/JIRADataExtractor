using JIRADataExtractor.Objects;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Parsers
{
    class Parser
    {
        protected JIRAConnectionHandler JIRAConnectionHandler;
        protected Parser() { }
        protected Parser(JIRAConnectionHandler jIRAConnectionHandler)
        {
            JIRAConnectionHandler = jIRAConnectionHandler;
        }
        protected Parser(String userName, String password, String baseURL)
        {
            JIRAConnectionHandler = new JIRAConnectionHandler(userName, password, baseURL);
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
    }
}
