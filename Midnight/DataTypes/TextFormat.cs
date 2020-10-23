using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.DataTypes
{
    enum TextFormat
    {
        None = 0,
        Bold = 1,
        Italics = 2,
        Underlined = 4,
        Superscript = 8,
        Subscript = 16,
    }

    static class TextFormatConverter
    {
        public static FontStyle ToFontStyle(this TextFormat format)
        {
            FontStyle res = FontStyle.Regular;
            if ((format & TextFormat.Bold) == TextFormat.Bold)
            {
                res |= FontStyle.Bold;
            }
            if ((format & TextFormat.Italics) == TextFormat.Italics)
            {
                res |= FontStyle.Italic;
            }
            if ((format & TextFormat.Underlined) == TextFormat.Underlined)
            {
                res |= FontStyle.Underline;
            }
            return res;
        }
    }
}
