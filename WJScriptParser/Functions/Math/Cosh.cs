using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Cosh : IFunction
    {
        public string Name => "Cosh";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Cosh(a.ToDouble())).Sum());
    }
}
