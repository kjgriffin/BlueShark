using Midnight.Compiler.AST;
using Midnight.Compiling;
using Midnight.Compiling.AST;
using Midnight.Liturgy.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Build
{
    public class MidnightBuildService
    {


        public void BuildProject(string sourcecode)
        {

            MidnightCompiler compiler = new MidnightCompiler();
            ASTProgram program = compiler.Compile(sourcecode, verboseDebug:true);

            if (program == null)
            {
                // failed to compiler or empty program

            }

            // build slides
            var slides = ((IGenerateSlides)program).GenerateSlides();

            // render slides

            LiturgySlideRenderer r1 = new LiturgySlideRenderer();
            foreach (var slide in slides)
            {
                slide.AcceptRenderer(r1);
            }



        }




    }
}
