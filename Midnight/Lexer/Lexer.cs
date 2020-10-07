using Midnight.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;

namespace Midnight.Lexer
{
    public class Lexer
    {


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

        public List<Token> Tokenize(string source, List<string> seperators, string escapeseq = @"\")
        {

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



            return res;
        }


    }
}
