using Midnight.Compiler.AST;
using Midnight.DataTypes;
using Midnight.LanguageDefinitions;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler
{
    class LiturgySubCompiler
    {

        public List<ASTLiturgyLine> ParseLiturgyLines(List<Token> tokens)
        {
            Lexer lexer = new Lexer();
            lexer.StartLexer(tokens);

            List<ASTLiturgyLine> lines = new List<ASTLiturgyLine>();

            while (!lexer.InspectEOF())
            {
                lines.Add(ParseLiturgyLine(lexer));
            }

            return lines;

        }

        private ASTLiturgyLine ParseLiturgyLine(Lexer lexer)
        {
            lexer.GobbleWhitespace();

            LiturgySpeakers speaker = LiturgySpeakers.None;
            string speakertext = "";

            // look for speaker
            if (lexer.Inspect("P"))
            {
                speaker = LiturgySpeakers.Pastor;
            }
            else if (lexer.Inspect("A"))
            {
                speaker = LiturgySpeakers.Assistant;
            }
            else if (lexer.Inspect("L"))
            {
                speaker = LiturgySpeakers.Leader;
            }
            else if (lexer.Inspect("C"))
            {
                speaker = LiturgySpeakers.Congregation;
            }
            speakertext = lexer.Consume().Value;

            lexer.GobbleWhitespace();


            List<Word> linewords = new List<Word>();

            // until we find next speaker or EOF add to line
            while (!lexer.InspectEOF() && !lexer.Inspect("^[PALC]$", isRegex: true))
            {
                var t = lexer.Consume();

                Word w;

                // for now only special thing is a token 'T' should be a special char (escape it to get a normal 'T')    
                if (t.Value == "T")
                {
                    if (t.IsEscaped)
                    {
                        w = new Word() { Value = t.Value, Format = TextFormat.None };
                    }
                    else
                    {
                        w = new Word() { Value = t.Value, Format = TextFormat.None, Attributes = new Dictionary<string, string>() { ["LSBSymbol"] = "T"} };
                    }
                }
                else
                {
                    w = new Word() { Value = t.AsText, Format = TextFormat.None };
                }

                linewords.Add(w);


            }

            return new ASTLiturgyLine(speakertext, speaker, linewords);
        }

    }
}
