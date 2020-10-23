using Midnight.Compiling.AST;
using Midnight.DataTypes;
using Midnight.LanguageDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.Compiler.AST
{
    class ASTLiturgyLine : IASTElementGeneratesDebugXML
    {

        public string SpeakerText = "";
        public LiturgySpeakers Speaker = LiturgySpeakers.None;
        public List<Word> Words = new List<Word>();

        public ASTLiturgyLine(string speakertext, LiturgySpeakers speaker, List<Word> word)
        {
            SpeakerText = speakertext;
            Speaker = speaker;
            Words = word;
        }

        void IASTElementGeneratesDebugXML.GenerateDebugXMLTree(IASTElement parent, StringBuilder output)
        {
            output.Append($"<ASTLiturgyLine Speaker=\"{Speaker}\" SpeakerText=\"{SpeakerText}\">");
            foreach (var word in Words)
            {
                output.Append($"<Word Format=\"{word.Format}\" Value=\"{word.Value}\">");
                foreach (var kvp in word.Attributes)
                {
                    output.Append($"<Attribute Name=\"{kvp.Key}\" Value=\"{kvp.Value}\"/>");
                }
                output.Append($"</Word>");
            }
            output.Append("</ASTLiturgyLine>");
        }
    }
}
