using Midnight.DataTypes;
using Midnight.Generator.Components;
using Midnight.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using Midnight.Helpers;
using System.Drawing.Text;
using System.Text;

namespace Midnight.Generator.Slides
{
    class LiturgySlide : ISlide
    {
        public List<TextBlock> Text { get; set; } = new List<TextBlock>();
        public string Name { get; set; }
        public int Number { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        string ISlide.Name => Name;
        int ISlide.Number => Number;
        SlideType ISlide.Type => SlideType.Liturgy;
        Dictionary<string, object> ISlide.Data => Data;

        IRenderedSlide ISlide.Render(Dictionary<string, object> props)
        {

            RenderedBitmapSlide res = new RenderedBitmapSlide();

            res.name = Name;
            // render bitmap
            Bitmap bmp = new Bitmap((int)props["slide.width"], (int)props["slide.height"]);
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.Clear(Color.Gray);

            // draw key
            gfx.FillRectangle(new SolidBrush((Color)props["liturgy.keyfillcolor"]), (Rectangle)props["liturgy.keyrect"]);

            // draw every line
            foreach (var t in Text)
            {
                FontStyle format = t.Text.Format.ToFontStyle();
                Font f = new Font(t.Text.Attributes["fontname"], Convert.ToInt32(t.Text.Attributes["fontsize"]), format);


                SolidBrush b = new SolidBrush(t.Foreground);

                gfx.DrawString(t.Text.Value, f, b, t.TextBoundingBox.Move(((Rectangle)props["liturgy.keyrect"]).Location).Location);
            }



            res.img = bmp;

            


            return res;

        }
    }
}
