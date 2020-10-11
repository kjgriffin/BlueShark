using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Generator
{
    interface ISlideRenderer
    {
        public void Visit(Slide slide);
    }
}
