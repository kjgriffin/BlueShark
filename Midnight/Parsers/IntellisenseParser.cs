using Midnight.DataTypes;
using Midnight.LanguageDefinitions;
using Midnight.Lexing;
using Midnight.PreProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;

namespace Midnight.Parsers
{

    public class IllegalParseException : Exception
    {
        public Token EToken { get; set; }
        public string EMessage { get; set; }

        public IllegalParseException(Token eToken, string message)
        {
            EToken = eToken;
            EMessage = message;
        }
    }

    delegate bool ParserCommand();

    public class IntellisenseParser
    {


        
        Lexer lexer = new Lexer();
        List<string> suggestions;
        List<(string cmdname, ParserCommand func)> Commands;

        public IntellisenseParser()
        {
            Commands = new List<(string, ParserCommand)>()
            {
                ("Liturgy", Cmd_Liturgy),
            };
        }


        public (bool success, List<string> suggestions) GetSuggestions(string input)
        {
            suggestions = new List<string>();
            lexer.Tokenize(input.StripSingleLineComments("//").StripMultiLineComments("/*", "*/"), LanguageDefinition.Seperators);

            try
            {
                StartParsing();
            }
            catch (IllegalParseException ex)
            {
                return (false, new List<string>() { ex.EMessage });
            }
            return (true, suggestions);
        }

        private void StartParsing()
        {
            // Grammer expects commands
            bool lastcommandhassuggestion = false;
            while (!lexer.InspectEOF())
            {
                lastcommandhassuggestion = ParseCommand();
            }

            if (!lastcommandhassuggestion)
            {
                suggestions = Commands.Select(c => c.cmdname).ToList();
            }
        }

        private bool TryParseNextToken_ThrowIfFail(string expected)
        {
            if (lexer.InspectEOF())
            {
                suggestions.Add(expected);
                return true;
            }
            if (!lexer.Consume(expected))
            {
                throw new IllegalParseException(lexer.CurrenToken, $"Unexpected Token: {lexer.CurrenToken} [expected:'{expected}']");
            }
            return false;
        }

        private bool ParseCommand()
        {
            lexer.GobbleWhitespace();
            // check if command is recognized
            var t = lexer.Consume();

            if (t.Equivalent(lexer.EOF))
            {
                suggestions = Commands.Select(c => c.cmdname).ToList();
                return true;
            }

            foreach (var cmd in Commands)
            {
                if (t.Value == cmd.cmdname)
                {
                    return cmd.func();
                }
            }

            // check if partially started
            // this only valid if last token
            if (!lexer.Peek1().Equivalent(lexer.EOF))
            {
                return false;
            }
            var potentials = Commands.Select(c => c.cmdname).Where(c => c.Contains(t.Value)).ToList();

            if (potentials.Count > 0)
            {
                suggestions = potentials;
                return true;
            }

            // error - unrecognized command
            throw new IllegalParseException(t, $"Unknown Command: {t}");

        }

        private bool Cmd_Liturgy()
        {

            lexer.GobbleWhitespace();
            if (TryParseNextToken_ThrowIfFail("(")) return true;
            lexer.GobbleWhitespace();
            if (TryParseNextToken_ThrowIfFail(")")) return true;
            lexer.GobbleWhitespace();
            if (TryParseNextToken_ThrowIfFail("{")) return true;

            lexer.ConsumeUntil("}");

            if (lexer.InspectEOF())
            {
                // help for content
                suggestions.Add("}");
                suggestions.Add("[Note] Text here will be formated as liturgy");
                return true;
            }

            TryParseNextToken_ThrowIfFail("}");
            lexer.GobbleWhitespace();

            return false;

        }






    }
}
