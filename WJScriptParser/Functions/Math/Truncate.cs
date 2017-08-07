using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Truncate : IFunction
    {
        public string Name => "Truncate";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Truncate(a.ToDouble())).Sum());
    }
}
