using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Sin : IFunction
    {
        public string Name => "Sin";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Sin(a.ToDouble())).Sum());
    }
}
