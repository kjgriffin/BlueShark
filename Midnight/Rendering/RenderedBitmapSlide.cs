using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Midnight.Rendering
{
    class RenderedBitmapSlide : IRenderedSlide
    {

        Bitmap img;
        string name;
        Dictionary<string, string> metadata = new Dictionary<string, string>();

        object IRenderedSlide.SlideContent => img;

        bool IRenderedSlide.FillContentFromPath => false;

        string IRenderedSlide.Name => name;

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
