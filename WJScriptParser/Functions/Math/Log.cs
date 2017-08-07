using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Log : IFunction
    {
        public string Name => "Log";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Log(a.ToDouble())).Sum());
    }
}
