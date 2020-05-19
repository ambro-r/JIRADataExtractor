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

        protected string GetJQLFilter(List<JQLFilter> jQLFilters)
        {
            StringBuilder jql = new StringBuilder();
            foreach (JQLFilter jQLFilter in jQLFilters)
            {
                if (jql.Length > 0)
                {
                    jql.Append(" ").Append(jQLFilter.gate.Value).Append(" ");
                }
                jql.Append(jQLFilter.field).Append(jQLFilter.comparison.Value).Append(jQLFilter.value);
            }
            if (jql.Length > 0)
            {
                jql.Insert(0, "jql=");
            }
            Log.Debug("JQL filter: {jql}", jql.ToString());
            return jql.ToString();
        }
    }
}
