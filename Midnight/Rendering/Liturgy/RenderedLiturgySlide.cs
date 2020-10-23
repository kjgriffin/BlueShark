using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.Rendering.Liturgy
{
    class RenderedLiturgySlide : IRenderedSlide
    {
        object IRenderedSlide.SlideContent { get => img; }
        string IRenderedSlide.Name { get => name; }

        Bitmap img;
        string name;

        Dictionary<string, string> metadata = new Dictionary<string, string>() { };


        public RenderedLiturgySlide(string name, Bitmap img)
        {
            this.img = img;
            this.name = name;
        }

        (bool found, object value) IRenderedSlide.GetMetadata(string property)
        {
            if (metadata.ContainsKey(property))
            {
                return (true, metadata[property]);
            }
            return (false, null);
        }
    }
}
