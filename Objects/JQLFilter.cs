using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Objects
{
    class SearchElement
    {
        public enum OPERATORS { OR, AND };

        public OPERATORS Operator { get; set; }

        //   project = PHX AND updated >= 2020-05-11 AND updated <= 2020-05-23 order by created DESC
    }
}
