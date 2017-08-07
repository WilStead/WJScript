using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Floor : IFunction
    {
        public string Name => "Floor";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Floor(a.ToDouble())).Sum());
    }
}
