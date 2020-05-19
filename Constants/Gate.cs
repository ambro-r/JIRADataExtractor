using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Constants
{
    public class Gate
    {
        private Gate(string value) { Value = value; }
        public string Value { get; set; } 
        public static Gate AND { get { return new Gate("AND"); } }
        public static Gate OR { get { return new Gate("OR"); } }
    }
}
