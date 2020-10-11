using Midnight.Compiling.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler.AST
{
    interface IASTElementGeneratesDebugXML
    {
        void GenerateDebugXMLTree(IASTElement parent, StringBuilder output);
    }
}
