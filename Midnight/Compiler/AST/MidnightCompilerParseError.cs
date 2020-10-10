using Midnight.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler.AST
{
    class MidnightCompilerParseError : Exception
    {
        public Token EToken { get; set; }
        public string EMessage { get; set; }

        public MidnightCompilerParseError(Token t, string emessage)
        {
            EToken = t;
            EMessage = emessage;
        }
    }
}
