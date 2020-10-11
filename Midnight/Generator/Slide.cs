using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Generator
{
    class Slide
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public SlideType Type { get; set; }

        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();


        public void AcceptRenderer(ISlideRenderer renderer)
        {
            renderer.Visit(this);
        }

    }
}
