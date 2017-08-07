using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Sinh : IFunction
    {
        public string Name => "Sinh";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Sinh(a.ToDouble())).Sum());
    }
}
