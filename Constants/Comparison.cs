using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Constants
{
    class Comparison
    {
        private Comparison(string value) { Value = value; }
        public string Value { get; set; }
        public static Comparison LESS_THAN { get { return new Comparison("<"); } }
        public static Comparison LESS_THAN_EQUAL_TO { get { return new Comparison("<="); } }
        public static Comparison GREATER_THAN { get { return new Comparison(">"); } }
        public static Comparison GREATER_THAN_EQUAL_TO { get { return new Comparison(">="); } }
        public static Comparison EQUAL_TO { get { return new Comparison("="); } }
    }
}
