using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Rendering
{
    public interface IRenderedSlide
    {

        /// <summary>
        /// The rendered content of the slide. Could be a bitmap or path to media file.
        /// </summary>
        public object SlideContent { get; }

        /// <summary>
        /// True if slide content is a path to the actual source. False if (bitmatp).
        /// </summary>
        public bool FillContentFromPath { get; }

        /// <summary>
        /// Filename for exporting.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets metadata about the slide. 
        /// </summary>
        /// <param name="property">The name of the metadata property to get.</param>
        /// <returns>True if found property, False if property doesn't exist. Returns value of property or null if doesn't exist.</returns>
        public (bool found, object value) GetMetadata(string property);


    }


}
