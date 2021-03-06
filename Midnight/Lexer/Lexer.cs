﻿using Midnight.DataTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Midnight.Lexing
{
    public class Lexer
    {
        /// <summary>
        /// The token representing the end of the file. Not part of the lexer's token list.
        /// </summary>
        public Token EOF { get; private set; } = new Token() { IsEscaped = false, LineNumber = -1, SColNumber = -1, Value = "$EOF" };


        private List<string> Split(string input, List<string> seperators)
        {
            List<string> tmpparts = new List<string>() { input };

            foreach (var sep in seperators)
            {
                var newparts = new List<string>();
                foreach (var part in tmpparts)
                {
                    var bits = System.Text.RegularExpressions.Regex.Split(part, $"({System.Text.RegularExpressions.Regex.Escape(sep)})");
                    newparts.AddRange(bits.Where(b => b != string.Empty));
                }
                tmpparts = newparts;
            }

            return tmpparts.Where(p => p != string.Empty).ToList();
        }

        /// <summary>
        /// Generates a list of tokens from the input and updates the lexers token list to this.
        /// </summary>
        /// <param name="source">Input source text.</param>
        /// <param name="seperators">List of seperators to split into tokens by. Should be ordered by detection priority.</param>
        /// <param name="escapeseq">Escape sequence to escape mark tokens as escaped. Will have highest tokenization priority.</param>
        /// <returns>List of tokens generated.</returns>
        public List<Token> Tokenize(string source, List<string> seperators, string escapeseq = @"\")
        {
            mTokens = new List<Token>();
            mTokenPos = 0;

            seperators.Insert(0, escapeseq);

            List<Token> tmp = new List<Token>();
            List<string> splitwords = Split(source, seperators);

            int lnum = 0;
            int cnum = 0;

            foreach (var word in splitwords)
            {
                Token t = new Token()
                {
                    IsEscaped = false,
                    LineNumber = lnum,
                    SColNumber = cnum,
                    Value = word
                };
                tmp.Add(t);
                cnum += word.Length;
                if (word.Contains(System.Environment.NewLine))
                {
                    lnum++;
                    cnum = 0;
                }

            }

            List<Token> res = new List<Token>();

            bool inescape = false;
            foreach (var token in tmp)
            {
                if (inescape)
                {
                    token.IsEscaped = true;
                    res.Add(token);
                    inescape = false;
                }
                else if (token.Value == escapeseq)
                {
                    inescape = true;
                }
                else
                {
                    res.Add(token);
                }
            }

            EOF.LineNumber = lnum;
            EOF.SColNumber = cnum;

            mTokens = res;
            return res;
        }

        public void StartLexer(List<Token> tokens)
        {
            mTokenPos = 0;
            mTokens = tokens;
            EOF.LineNumber = LastToken.LineNumber + 1;
            EOF.SColNumber = LastToken.SColNumber + 1;
        }


        private int mTokenPos = 0;
        private List<Token> mTokens = new List<Token>();

        /// <summary>
        /// List of all tokens the lexer has.
        /// </summary>
        public List<Token> Tokens { get => mTokens; }

        /// <summary>
        /// The token at the current position of the lexer.
        /// </summary>
        public Token CurrenToken
        {
            get
            {
                if (mTokenPos < mTokens.Count)
                {
                    return mTokens[mTokenPos];
                }
                else
                {
                    return EOF;
                }
            }
        }

        /// <summary>
        /// Gets the last token before EOF. Returns EOF if no tokens.
        /// </summary>
        public Token LastToken
        {
            get
            {
                if (mTokens.Count > 0)
                {
                    return mTokens.Last();
                }
                else
                {
                    return EOF;
                }
            }
        }

        public List<string> Messages { get; set; } = new List<string>();



        /// <summary>
        /// Check if the lexer is at the end of the file.
        /// </summary>
        /// <returns>True if at end.</returns>
        public bool InspectEOF()
        {
            return CurrenToken == EOF;
        }

        /// <summary>
        /// Check if the current token matches the provided pattern.
        /// </summary>
        /// <param name="expected">Regex pattern</param>
        /// <param name="isRegex">Set true if pattern is regex.</param>
        /// <param name="escaped">If the token should be escaped.</param>
        /// <returns>True if match</returns>
        public bool Inspect(string expected, bool isRegex = false, bool escaped = false)
        {
            if (!InspectEOF())
            {
                string pattern = isRegex ? expected : Regex.Escape(expected);
                return Regex.Match(CurrenToken.Value, pattern).Success && CurrenToken.IsEscaped == escaped;
            }
            return false;
        }

        /// <summary>
        /// Check if the provided token matches the provided pattern.
        /// </summary>
        /// <param name="expected">Regex pattern</param>
        /// <param name="isRegex">Set true if pattern is regex.</param>
        /// <param name="escaped">If the token should be escaped.</param>
        /// <returns>True if match</returns>
        public bool InspectToken(Token t, string expected, bool isRegex = false, bool escaped = false)
        {
            if (!t.Equivalent(EOF))
            {
                string pattern = isRegex ? expected : Regex.Escape(expected);
                return Regex.Match(t.Value, pattern).Success && t.IsEscaped == escaped;
            }
            return false;
        }


        /// <summary>
        /// Returns the current token without consuming it.
        /// </summary>
        /// <returns>The current token.</returns>
        public Token Peek()
        {
            return CurrenToken;
        }

        /// <summary>
        /// Returns the next token the lexer will inspect, or EOF if no next token.
        /// </summary>
        /// <returns>The next token or EOF.</returns>
        public Token Peek1()
        {
            if (mTokenPos + 1 < mTokens.Count)
            {
                return mTokens[mTokenPos + 1];
            }
            return EOF;
        }

        /// <summary>
        /// Advances the lexer to the next token. Returns the current one.
        /// </summary>
        /// <returns>The current token before advancing.</returns>
        public Token Consume()
        {
            Token res = CurrenToken;
            mTokenPos++;
            return res;
        }

        /// <summary>
        /// Checks the current token matches the expected pattern. Advances the lexer to the current one if so and returns true.
        /// </summary>
        /// <param name="expected">The pattern to match the token to.</param>
        /// <param name="isRegex">Set true to escape regex characters in expected.</param>
        /// <param name="escaped">If the token should be escaped.</param>
        /// <returns>True if matched and advanced. False otherwise.</returns>
        public bool Consume(string expected, bool isRegex = false, bool escaped = false)
        {
            if (Inspect(expected, isRegex, escaped))
            {
                Consume();
                return true;
            }
            else
            {
                string esq = escaped ? "\\" : "";
                Messages.Add($"[Lexer.Consume] expected: `{esq}{expected}` found {CurrenToken}");
                return false;
            }
        }

        /// <summary>
        /// Advances the lexer position until current token matches the expected pattern.
        /// </summary>
        /// <param name="expected">Regex pattern to match token to.</param>
        /// <param name="isRegex">Set true to escape regex characters in expected.</param>
        /// <param name="escaped">Is the token escaped.</param>
        /// <returns>List of all tokens consumed</returns>
        public List<Token> ConsumeUntil(string expected, bool isRegex = false, bool escaped = false)
        {
            List<Token> res = new List<Token>();
            while (!Inspect(expected, isRegex, escaped) && !InspectEOF())
            {
                res.Add(CurrenToken);
                mTokenPos++;
            }
            return res;
        }

        /// <summary>
        /// Advances the lexer to the next non-whitespace token.
        /// </summary>
        public void GobbleWhitespace()
        {
            while (Inspect(@"^\s$", isRegex: true) && !InspectEOF())
            {
                Consume();
            }
        }


        /// <summary>
        /// Advances the lexer state to the endseq. Gets the values for all parameters.
        /// </summary>
        /// <param name="startseq">Sequence identifying start of parameter list.</param>
        /// <param name="endseq">Sequence identifying end of parameter list.</param>
        /// <param name="sep">Sequence identifying parameter seperators</param>
        /// <param name="encseq">Sequence identifying parameter value enclosure</param>
        /// <param name="enclosed">Are all parameters enclosed</param>
        /// <param name="paramnames">Names of the parameters</param>
        /// <returns></returns>
        public Dictionary<string, string> ConsumeArgList(string startseq, string endseq, string sep, string encseq, bool enclosed = false, params string[] paramnames)
        {
            Dictionary<string, string> paramvals = new Dictionary<string, string>();


            Consume(startseq);

            int pnum = 0;
            foreach (var pname in paramnames)
            {
                GobbleWhitespace();
                string paramendmatch = pnum < paramnames.Length - 1 ? sep : endseq;
                if (enclosed)
                {
                    Consume(encseq);
                    paramendmatch = encseq;
                }

                var vals = ConsumeUntil(paramendmatch);
                StringBuilder sb = new StringBuilder();
                foreach (var v in vals)
                {
                    sb.Append(v.Value);
                }
                paramvals.Add(paramnames[pnum], sb.ToString().Trim());

                if (enclosed)
                {
                    Consume(encseq);
                }

                GobbleWhitespace();

                if (pnum < paramnames.Length - 1)
                {
                    Consume(sep);
                }

                pnum++;

            }

            Consume(endseq);
            return paramvals;
        }

        /// <summary>
        /// Advances the lexer state to the endseq. Gets the values for all parameters.
        /// </summary>
        /// <param name="paramnames">Names of the parameters</param>
        /// <returns></returns>
        public Dictionary<string, string> ConsumeArgList(params string[] paramnames)
        {
            return ConsumeArgList("(", ")", ",", "\'", false, paramnames);
        }

        /// <summary>
        /// Advances the lexer state to the endseq. Gets the values for all parameters.
        /// </summary>
        /// <param name="paramnames">Names of the parameters</param>
        /// <returns></returns>
        public Dictionary<string, string> ConsumeEnclosedArgList(params string[] paramnames)
        {
            return ConsumeArgList("(", ")", ",", "\'", true, paramnames);
        }




        /// <summary>
        /// Provides advanced options for parsing an argument list.
        /// </summary>
        /// <param name="startseq">Sequence identifying start of arglist.</param>
        /// <param name="sepseq">Sequence identifying seperation of parameters.</param>
        /// <param name="endseq">Sequence identifying end of arglist.</param>
        /// <param name="encseq">Sequence identifying parameter value enclosing sequence.</param>
        /// <param name="paramnames">List of parameters to parse. (bool: True if the parameter is enclosed, string: name of the parameter)</param>
        /// <returns>Dictionary keyed on the parameter names with thier respective values as parsed.</returns>        
        public Dictionary<string, string> ConsumeArgList(string startseq, string sepseq, string endseq, string encseq, params (bool enclosed, string pname)[] paramnames)
        {
            Dictionary<string, string> arglist = new Dictionary<string, string>();

            Consume(startseq);

            int pnum = 0;

            foreach (var p in paramnames)
            {
                GobbleWhitespace();

                string paramendmatch = p.enclosed ? encseq : pnum < paramnames.Length - 1 ? sepseq : endseq;

                if (p.enclosed)
                {
                    Consume(encseq);
                }

                StringBuilder sb = new StringBuilder();
                var ts = ConsumeUntil(paramendmatch);
                foreach (var item in ts)
                {
                    sb.Append(item.Value);
                }

                string val = sb.ToString();
                if (!p.enclosed)
                {
                    val = val.Trim();
                }

                arglist.Add(p.pname, val);

                if (p.enclosed)
                {
                    Consume(encseq);
                }

                GobbleWhitespace();

                Consume(pnum < paramnames.Length - 1 ? sepseq : endseq);

                pnum++;
            }

            return arglist;
        }




    }
}
