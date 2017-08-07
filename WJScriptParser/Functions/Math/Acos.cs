using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Acos : IFunction
    {
        public string Name => "Acos";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Acos(a.ToDouble())).Sum());
    }
}
