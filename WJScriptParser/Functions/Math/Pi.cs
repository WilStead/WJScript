using System;

namespace WScriptParser.Functions
{
    public class Pi : IFunction
    {
        public string Name => "Pi";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Math.PI);
    }
}
