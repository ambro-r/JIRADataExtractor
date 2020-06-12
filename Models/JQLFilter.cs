using JIRADataExtractor.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Models
{
    class JQLFilter
    {
        // example: AND project = PHX 
        public Gate Gate { get; set; } // AND
        public string Field { get; set; } // Project
        public Comparison Comparison { get; set; } // =
        public string Value { get; set; } // PHX

        public JQLFilter(string field, Comparison comparison, string value)
        {
            Field = field;
            Comparison = comparison;
            Value = value;
        }

        public JQLFilter(string field, Comparison comparison, string value, Gate gate)
        {
            Field = field;
            Comparison = comparison;
            Value = value;
            Gate = gate;
        }

    }
}
