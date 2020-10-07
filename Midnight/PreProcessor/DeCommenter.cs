using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midnight.PreProcessor
{
    public static class DeCommenter
    {
        /// <summary>
        /// Removes single line comments.
        /// </summary>
        /// <param name="source">Source text to remove comment lines from.</param>
        /// <param name="singlelinecommentseq">Sequence identifying start of single line comment.</param>
        /// <returns>Source text with single line comments removed. Ends in a newline.</returns>        
        public static string StripSingleLineComments(this string source, string singlelinecommentseq)
        {
            var lines = source.Split(System.Environment.NewLine);
            StringBuilder sb = new StringBuilder();
            foreach (var line in lines)
            {
                var lineparts = line.Split(singlelinecommentseq, 2, StringSplitOptions.None);
                sb.Append(lineparts.First());
                sb.Append(System.Environment.NewLine);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Removes multiline comments.
        /// </summary>
        /// <param name="source">Source text to remove comments from.</param>
        /// <param name="startmultiliencommentseq">Sequence identifying start of multiline comment.</param>
        /// <param name="endmultilinecommentseq">Sequence identifying end of multiline comment.</param>
        /// <returns>Source text with multiline comments removed.</returns>
        public static string StripMultiLineComments(this string source, string startmultiliencommentseq, string endmultilinecommentseq)
        {
            string work = source;
            StringBuilder sb = new StringBuilder();
            while (work.Contains(startmultiliencommentseq))
            {
                // find start of comment
                var parts = work.Split(startmultiliencommentseq, 2, StringSplitOptions.None);
                // add stuff before comment
                sb.Append(parts.First());
                if (parts.Length > 1)
                {
                    work = parts.Last();
                }
                // find end of comment
                parts = work.Split(endmultilinecommentseq, 2, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    work = parts.Last();
                }
                else
                {
                    work = "";
                }
            }
            sb.Append(work);
            return sb.ToString();
        }
        
    }
}
