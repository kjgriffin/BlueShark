using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Midnight.DataTypes
{
    public class Token
    {
        /// <summary>
        /// The line number (0 based) the token was in the original source text.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// The column number (0 based) the starting character of the token was in the original source text.
        /// </summary>
        public int SColNumber { get; set; }

        /// <summary>
        /// The content of the token.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Whether the token value was escaped.
        /// </summary>
        public bool IsEscaped { get; set; }

        public override string ToString()
        {
            string escaped = IsEscaped ? "\\" : "";
            return $"{escaped}`{Value}` on {LineNumber} at {SColNumber}";
        }

        public bool Equivalent(object obj)
        {
            Token a = this;
            Token b = obj as Token;

            return a.IsEscaped == b.IsEscaped && a.LineNumber == b.LineNumber && a.SColNumber == b.SColNumber && a.Value == b.Value;
        }

    }
}
