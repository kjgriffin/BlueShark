using Midnight.Compiler.AST;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiling.AST
{
    class ASTProgram : IASTElement
    {

        List<IASTElement> Children = new List<IASTElement>();

        void IASTElement.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append("<ASTProgram>");
            foreach (var child in Children)
            {
                child.GenerateDebugXMLTree(this, output);
            }
            output.Append("</ASTProgram>");
        }

        IASTElement IASTElement.Parse(Lexer lexer, IASTElement parent)
        {
            while (!lexer.InspectEOF())
            {
                lexer.GobbleWhitespace();
                // find a valid command
                IASTElement cmd = new ASTCommand();
                Children.Add(cmd.Parse(lexer, this));
                lexer.GobbleWhitespace();
            }
            return this;
        }
    }
}
