using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Tan : IFunction
    {
        public string Name => "Tan";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Tan(a.ToDouble())).Sum());
    }
}
