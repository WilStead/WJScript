using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Cos : IFunction
    {
        public string Name => "Cos";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Cos(a.ToDouble())).Sum());
    }
}
