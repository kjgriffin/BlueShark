using Midnight.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Midnight.Generator.Components
{
    class TextBlock
    {

        public Word Text { get; set; }

        /// <summary>
        ///  Bounding box of the text. Relative position.
        /// </summary>
        public Rectangle TextBoundingBox { get; set; }

        /// <summary>
        /// Boundingbox of textbox (includes padding around text). 
        /// </summary>
        public Rectangle BoundingBox { get; set; }

        /// <summary>
        /// Color of text.
        /// </summary>
        public Color Foreground { get; set; }
       
        /// <summary>
        /// Background color of text.
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Color of BoundingBox.
        /// </summary>
        public Color BoundingBackground { get; set; }

    }
}
