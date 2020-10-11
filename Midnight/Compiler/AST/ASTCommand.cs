using Midnight.Compiling.AST;
using Midnight.DataTypes;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;

namespace Midnight.Compiler.AST
{
    class ASTCommand : IASTElement, IParsable
    {

        private Token CommandName = new Token();
        private IASTElement Command;

        void IASTElementGeneratesDebugXML.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append($"<ASTCommand CommandName=\"{CommandName.AsText}\">");
            Command.GenerateDebugXMLTree(this, output);
            output.Append("</ASTCommand>");
        }



        IASTElement IParsable.Parse(Lexer lexer, IASTElement parent)
        {
            /*
                Figure out what command it is
                Parse the actual command
             */

            lexer.GobbleWhitespace();
            CommandName = lexer.Peek();

            if (CommandName.Value == "Liturgy" && !CommandName.IsEscaped)
            {
                Command = new ASTLiturgy_cmd();
                Command.Parse(lexer, this);
                return this;
            }
            else
            {
                // unrecognized sequence
                throw new MidnightCompilerParseError(lexer.CurrenToken, $"[Compiler Error] Error parsing 'Command'. Unrecognized command {lexer.CurrenToken.Value} at Line: {lexer.CurrenToken.LineNumber} on Col: {lexer.CurrenToken.SColNumber}");
            }
        }




    }
}
