using JIRADataExtractor.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Reports
{

    class Report
    {

        protected ConnectionHandler JIRAConnectionHandler;
        protected Report() { }
        protected Report(ConnectionHandler jIRAConnectionHandler)
        {
            JIRAConnectionHandler = jIRAConnectionHandler;
        }

    }
}
