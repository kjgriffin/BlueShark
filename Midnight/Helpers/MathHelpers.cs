using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.Helpers
{
    static class MathHelpers
    {

        public static Rectangle Move(this Rectangle rect, Point displacement)
        {
            return new Rectangle(rect.X + displacement.X, rect.Y + displacement.Y, rect.Width, rect.Height);
        }
    }
}
