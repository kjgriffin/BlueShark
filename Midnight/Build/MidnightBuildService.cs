using Midnight.Compiler.AST;
using Midnight.Compiling;
using Midnight.Compiling.AST;
using Midnight.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.Build
{
    public class MidnightBuildService
    {

        Dictionary<string, object> RenderProps = new Dictionary<string, object>()
        {
            ["slide.width"] = 1920,
            ["slide.height"] = 1080,
            ["liturgy.keyfillcolor"] = Color.White,
            ["liturgy.keyrect"] = new Rectangle(0, 866, 1920, 216),
        };

        public List<IRenderedSlide> renderedSlides = new List<IRenderedSlide>();

        public void BuildProject(string sourcecode)
        {

            MidnightCompiler compiler = new MidnightCompiler();
            ASTProgram program = compiler.Compile(sourcecode, verboseDebug: true);

            if (program == null)
            {
                // failed to compiler or empty program

            }

            // build slides
            var slides = ((IGenerateSlides)program).GenerateSlides();


            renderedSlides.Clear();
            // render slides
            foreach (var slide in slides)
            {
                renderedSlides.Add(slide.Render(RenderProps));
            }


        }




    }
}
