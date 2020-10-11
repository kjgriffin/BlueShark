using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using System.Text;

namespace Midnight.Layout
{
    class LiturgySlideLayout
    {
        public Size Size { get; set; } = new Size(1920, 1080);
        public Rectangle Key { get; set; } = new Rectangle(0, 864, 1920, 216);
        public Rectangle SpeakerBox { get; set; } = new Rectangle(50, 3, 60, 210);
        public Rectangle TextBox { get; set; } = new Rectangle(120, 3, 1750, 210);
        public int InterLineSpace { get; set; } = 5;
        public string FontName { get; set; } = "Arial";
        public int FontSize { get; set; } = 36;

    }
}
