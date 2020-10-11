using System;
using System.Collections.Generic;
using System.Text;

namespace Midnight.DataTypes
{
    class Word
    {
        public TextFormat Format { get; set; }
        public string Value { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}
