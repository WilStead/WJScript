using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Exp : IFunction
    {
        public string Name => "Exp";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Exp(a.ToDouble())).Sum());
    }
}
