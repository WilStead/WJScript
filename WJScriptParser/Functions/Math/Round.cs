using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Round : IFunction
    {
        public string Name => "Round";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Round(a.ToDouble())).Sum());
    }
}
