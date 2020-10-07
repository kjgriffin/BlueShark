using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midnight.DataTypes;
using Midnight.Lexer;
using MidnightTests;
using System;
using System.Collections.Generic;
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
            Assert.IsTrue(golden.AreEqualContent(res));
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
         
            Assert.IsTrue(golden.AreEqualContent(res));
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

            Assert.IsTrue(golden.AreEqualContent(res));

            
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

            Assert.IsTrue(golden.AreEqualContent(res));

        }
    }
}