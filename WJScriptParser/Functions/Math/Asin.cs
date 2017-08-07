using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Asin : IFunction
    {
        public string Name => "Asin";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Asin(a.ToDouble())).Sum());
    }
}
