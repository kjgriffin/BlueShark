using Midnight.Compiling.AST;
using Midnight.DataTypes;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Midnight.Compiler.AST
{
    class ASTLiturgy_cmd : IASTElement
    {

        private List<Token> Content = new List<Token>();


        void IASTElement.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append("<Liturgy>");
            foreach (var item in Content)
            {
                output.Append(item.AsText);
            }
            output.Append("</Liturgy>");
        }

        IASTElement IASTElement.Parse(Lexer lexer, IASTElement parent)
        {
            if (!lexer.Consume("Liturgy"))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }
            lexer.GobbleWhitespace();
            if (!lexer.Consume("("))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }
            lexer.GobbleWhitespace();
            if (!lexer.Consume(")"))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }
            lexer.GobbleWhitespace();
            if (!lexer.Consume("{"))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }

            Content = lexer.ConsumeUntil("}");


            if (!lexer.Consume("}"))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }

            return this;

        }


    }
}
