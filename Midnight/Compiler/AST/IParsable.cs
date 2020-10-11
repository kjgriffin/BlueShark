using Midnight.Compiling.AST;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler.AST
{
    interface IParsable : IASTElementGeneratesDebugXML
    {
        IASTElement Parse(Lexer lexer, IASTElement parent);
    }
}
