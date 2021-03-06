﻿using Midnight.Compiling.AST;
using Midnight.DataTypes;
using Midnight.Generator;
using Midnight.Generator.Slides;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Midnight.Compiler.AST
{
    class ASTLiturgy_cmd : IASTElement, IParsable, IGenerateSlides
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

        List<ISlide> IGenerateSlides.GenerateSlides()
        {
            List<ISlide> slides = new List<ISlide>();


            // for now just put each line onto a slide
            LiturgySlide s = new LiturgySlide();
            s.Name = "liturgy";
            s.Number = 1;
            s.Data["width"] = 1920;
            s.Data["height"] = 1080;

            s.Data["lines"] = Lines;

            foreach (var line in Lines)
            {
                s.Text.Add(new Generator.Components.TextBlock()
                {
                    Background = Color.Black,
                    BoundingBackground = Color.Black,
                    Foreground = Color.White,
                    Text = new Word() { Value = line.SpeakerText, Attributes = new Dictionary<string, string>()
                    {
                        ["fontname"] = "Arial",
                        ["fontsize"] = "36"
                    }, Format = TextFormat.Bold },
                    BoundingBox = new Rectangle(0, 0, 100, 100),
                    TextBoundingBox = new Rectangle(0, 0, 100, 100)
                });
            }

            slides.Add(s);

            return slides;

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
