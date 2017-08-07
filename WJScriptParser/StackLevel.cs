using System.Collections.Generic;
using WScriptParser.Functions;

namespace WScriptParser
{
    public class StackLevel
    {
        public string Name { get; set; }

        public Dictionary<string, IFunction> Variables { get; set; } = new Dictionary<string, IFunction>();

        public StackLevel(string name = null)
        {
            Name = name;
        }
    }
}
