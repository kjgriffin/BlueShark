using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midnight.DataTypes;
using Midnight.Lexer;
using MidnightTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midnight.Lexer.Tests
{
    [TestClass()]
    public class LexerTests
    {
        [TestMethod()]
        public void TokenizeRepeatedTest()
        {
            const string input = "tttt";
            Lexer l = new Lexer();
            var res = l.Tokenize(input, new List<string>() { "t" });
            List<Token> golden = new List<Token>()
            {
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 0, Value = "t"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 1, Value = "t"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 2, Value = "t"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 3, Value = "t"},
            };
            Assert.IsTrue(golden.AreEqualTokenContent(res));
        }

        [TestMethod()]
        public void TokenizeTest()
        {
            const string input = "This is, a test of #break stuff { and (";
            Lexer l = new Lexer();
            var res = l.Tokenize(input, new List<string>() { "is", "#", " " });
            List<Token> golden = new List<Token>()
            {
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 0, Value = "Th"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 2, Value = "is"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 4, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 5, Value = "is"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 7, Value = ","} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 8, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 9, Value = "a"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 10, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 11, Value = "test"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 15, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 16, Value = "of"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 18, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 19, Value = "#"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 20, Value = "break"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 25, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 26, Value = "stuff"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 31, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 32, Value = "{"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 33, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 34, Value = "and"} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 37, Value = " "} ,
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 38, Value = "("} ,
            };

            Assert.IsTrue(golden.AreEqualTokenContent(res));
        }

        [TestMethod()]
        public void TokenizeLineColNumsTest()
        {
            const string input = "Line 1.\r\nLine 2.";
            Lexer l = new Lexer();
            var res = l.Tokenize(input, new List<string>() { " ", "\r\n" });
            List<Token> golden = new List<Token>() {
                new Token() {IsEscaped = false, LineNumber= 0, SColNumber = 0, Value= "Line"},
                new Token() {IsEscaped = false, LineNumber= 0, SColNumber = 4, Value= " "},
                new Token() {IsEscaped = false, LineNumber= 0, SColNumber = 5, Value= "1."},
                new Token() {IsEscaped = false, LineNumber= 0, SColNumber = 7, Value= "\r\n"},
                new Token() {IsEscaped = false, LineNumber= 1, SColNumber = 0, Value= "Line"},
                new Token() {IsEscaped = false, LineNumber= 1, SColNumber = 4, Value= " "},
                new Token() {IsEscaped = false, LineNumber= 1, SColNumber = 5, Value= "2."},
            };

            Assert.IsTrue(golden.AreEqualTokenContent(res));


        }

        [TestMethod()]
        public void TokenizeEscapeSeqTest()
        {

            const string input = @"this is\tab for \\stuff";
            Lexer l = new Lexer();
            var res = l.Tokenize(input, new List<string>() { " " });
            List<Token> golden = new List<Token>()
            {
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 0, Value = "this"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 4, Value = " "},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 5, Value = "is"},
                new Token() {IsEscaped = true, LineNumber = 0, SColNumber = 8, Value = "tab"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 11, Value = " "},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 12, Value = "for"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 15, Value = " "},
                new Token() {IsEscaped = true, LineNumber = 0, SColNumber = 17, Value = @"\"},
                new Token() {IsEscaped = false, LineNumber = 0, SColNumber = 18, Value = "stuff"},
            };

            Assert.IsTrue(golden.AreEqualTokenContent(res));

        }

        [TestMethod()]
        public void InspectRegexTest()
        {
            const string input = @"This is a \test.";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " ", "." });

            Assert.IsTrue(l.Inspect("This"));
            Assert.IsTrue(l.Inspect(".*"));
            Assert.IsTrue(l.Inspect(@"\w"));
            l.Consume();
            l.Consume();
            l.Consume();
            l.Consume();
            l.Consume();
            l.Consume();
            Assert.IsTrue(l.Inspect("test", escaped: true));
        }

        [TestMethod()]
        public void InspectEOFTest()
        {
            Lexer l = new Lexer();
            Assert.IsTrue(l.InspectEOF());
        }

        [TestMethod()]
        public void PeekTest()
        {
            const string input = @"Testing 123.";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " " });

            Assert.IsTrue(l.Peek().Equivalent(new Token() { IsEscaped = false, LineNumber = 0, SColNumber = 0, Value = "Testing" }));
        }

        [TestMethod()]
        public void Peek1Test()
        {
            const string input = @"Testing 123.";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " " });

            Assert.IsTrue(l.Peek1().Equivalent(new Token() { IsEscaped = false, LineNumber = 0, SColNumber = 7, Value = " " }));
        }

        [TestMethod()]
        public void ConsumeTest()
        {
            const string input = @"Testing 123.";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " " });

            Assert.IsTrue(l.Consume("T.*ing"));
            Assert.IsFalse(l.Consume("wrong"));
        }

        [TestMethod()]
        public void ConsumeUntilTest()
        {
            const string input = @"Testing 123. This too shall pass.";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " ", "." });

            var capture = l.ConsumeUntil(".", true);
            Assert.AreEqual("Testing 123", string.Join("", capture.Select(p => p.Value)));
            Assert.AreEqual(l.CurrenToken.Value, ".");
        }

        [TestMethod()]
        public void GobbleWhitespaceTest()
        {
            const string input = "Line 1:   \t.\r\n and stuff";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " ", "." });

            l.Consume();
            l.GobbleWhitespace();
            Assert.IsTrue(l.Consume("1:"));
            l.GobbleWhitespace();
            Assert.IsTrue(l.Consume("."));
            l.GobbleWhitespace();
            Assert.IsTrue(l.Consume("and"));
        }

        [TestMethod()]
        public void ConsumeArgListTest()
        {
            const string input = "Method(p1, true, 1234)";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " ", ",", "'", "(", ")" });
            l.Consume("Method");
            var args = l.ConsumeArgList("param1", "param2", "param3");
            Assert.AreEqual("p1", args["param1"]);
            Assert.AreEqual("true", args["param2"]);
            Assert.AreEqual("1234", args["param3"]);
        }
        
        [TestMethod()]
        public void ConsumeEnclosedArgListTest()
        {
            const string input = "Method('p1', 'true', '1234')";
            Lexer l = new Lexer();
            l.Tokenize(input, new List<string>() { " ", ",", "'", "(", ")" });
            l.Consume("Method");
            var args = l.ConsumeEnclosedArgList("param1", "param2", "param3");
            Assert.AreEqual("p1", args["param1"]);
            Assert.AreEqual("true", args["param2"]);
            Assert.AreEqual("1234", args["param3"]);
        }
    }
}