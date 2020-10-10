using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.LanguageDefinitions
{
    static class LanguageDefinition
    {

        public static List<string> Seperators = new List<string>()
        {
            Environment.NewLine,
            " ",
            ",",
            ".",
            "?",
            ":",
            ";",
            "-",
            "(",
            ")",
            "{",
            "}",
            "\"",
            "'"
        };

    }
}
