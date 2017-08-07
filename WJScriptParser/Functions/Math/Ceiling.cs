using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Ceiling : IFunction
    {
        public string Name => "Ceiling";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Ceiling(a.ToDouble())).Sum());
    }
}
