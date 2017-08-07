using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Tanh : IFunction
    {
        public string Name => "Tanh";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Tanh(a.ToDouble())).Sum());
    }
}
