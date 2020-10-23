using Midnight.Compiler.AST;
using Midnight.Generator;
using Midnight.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiling.AST
{
    class ASTProgram : IASTElement, IParsable, IGenerateSlides
    {

        List<IASTElement> Children = new List<IASTElement>();

        void IASTElementGeneratesDebugXML.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append("<ASTProgram>");
            foreach (var child in Children)
            {
                child.GenerateDebugXMLTree(this, output);
            }
            output.Append("</ASTProgram>");
        }

        List<ISlide> IGenerateSlides.GenerateSlides()
        {
            List<ISlide> projectSlides = new List<ISlide>();
            foreach (var child in Children)
            {
                if (child is IGenerateSlides)
                {
                    projectSlides.AddRange(((IGenerateSlides)child).GenerateSlides());
                }
            }
            return projectSlides;
        }

        IASTElement IParsable.Parse(Lexer lexer, IASTElement parent)
        {
            while (!lexer.InspectEOF())
            {
                lexer.GobbleWhitespace();
                // find a valid command
                IASTElement cmd = new ASTCommand();
                Children.Add(cmd.Parse(lexer, this));
                lexer.GobbleWhitespace();
            }
            return this;
        }
    }
}
