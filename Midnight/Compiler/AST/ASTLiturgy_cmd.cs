using Midnight.Compiling.AST;
using Midnight.DataTypes;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Midnight.Compiler.AST
{
    class ASTLiturgy_cmd : IASTElement, IParsable
    {

        private List<Token> Content = new List<Token>();
        private List<ASTLiturgyLine> Lines = new List<ASTLiturgyLine>();


        void IASTElementGeneratesDebugXML.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append("<Liturgy>");
            foreach (var line in Lines)
            {
                ((IASTElementGeneratesDebugXML)line).GenerateDebugXMLTree(this, output);
            }
            output.Append("</Liturgy>");
        }

        IASTElement IParsable.Parse(Lexer lexer, IASTElement parent)
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

            LiturgySubCompiler liturgySubCompiler = new LiturgySubCompiler();
            Lines = liturgySubCompiler.ParseLiturgyLines(Content);


            if (!lexer.Consume("}"))
            {
                throw new MidnightCompilerParseError(lexer.CurrenToken, string.Join(Environment.NewLine, lexer.Messages));
            }

            return this;

        }


    }
}
