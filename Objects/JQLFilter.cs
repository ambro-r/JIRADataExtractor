using JIRADataExtractor.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Objects
{
    class JQLFilter
    {
        // example: AND project = PHX 
        public Gate gate { get; set; } // AND
        public string field { get; set; } // Project
        public Comparison comparison { get; set; } // =
        public string value { get; set; } // PHX
    }
}
