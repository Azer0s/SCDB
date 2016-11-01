using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linguistics
{
    /// <summary>
    /// Stores a tagged word.
    /// </summary>
    public class TypeWordCombo
    {
        public TypeWordCombo(string type, string word)
        {
            Type = type;
            Word = word;
        }
        public string Type { get; set; }
        public string Word { get; set; }
    }
}
