using JIRADataExtractor.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Reports
{

    class Report
    {

        protected JIRAConnectionHandler JIRAConnectionHandler;
        protected Report() { }
        protected Report(JIRAConnectionHandler jIRAConnectionHandler)
        {
            JIRAConnectionHandler = jIRAConnectionHandler;
        }

    }
}
