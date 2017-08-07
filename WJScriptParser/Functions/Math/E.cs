using System;

namespace WScriptParser.Functions
{
    public class E : IFunction
    {
        public string Name => "E";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Math.E);
    }
}
