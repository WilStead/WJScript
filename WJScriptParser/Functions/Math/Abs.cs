using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Abs : IFunction
    {
        public string Name => "Abs";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Abs(a.ToDouble())).Sum());
    }
}
