using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiling.AST
{
    interface IASTElement
    {

        public IASTElement Parse(Lexer lexer, IASTElement parent);
        public void GenerateDebugXMLTree(IASTElement parent, StringBuilder output);

    }
}
