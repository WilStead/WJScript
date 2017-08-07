using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Log10 : IFunction
    {
        public string Name => "Log10";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Log10(a.ToDouble())).Sum());
    }
}
