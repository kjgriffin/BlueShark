using Midnight.Generator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.Liturgy.Rendering
{
    class LiturgySlideRenderer : ISlideRenderer
    {
        void ISlideRenderer.Visit(Slide slide)
        {
            if (slide.Type != SlideType.Liturgy)
                return;

            // generate blank slide for now

            int renderwidth = Convert.ToInt32(slide.Data["width"]);
            int renderheight = Convert.ToInt32(slide.Data["height"]);

            Bitmap output = new Bitmap(renderwidth, renderheight);
            Graphics gfx = Graphics.FromImage(output);



        }
    }
}
