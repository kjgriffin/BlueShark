using Midnight.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Generator
{
    interface ISlide
    {
        string Name { get; }
        int Number { get; }

        SlideType Type { get; }

        Dictionary<string, object> Data { get; }


        IRenderedSlide Render(Dictionary<string, object> props);

    }
}
