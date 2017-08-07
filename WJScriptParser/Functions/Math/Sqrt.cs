using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Sqrt : IFunction
    {
        public string Name => "Sqrt";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Sqrt(a.ToDouble())).Sum());
    }
}
