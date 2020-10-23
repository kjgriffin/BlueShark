using Midnight.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler.AST
{
    interface IGenerateSlides
    {

        public List<ISlide> GenerateSlides(); 

    }
}
