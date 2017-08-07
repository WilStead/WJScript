using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Sign : IFunction
    {
        public string Name => "Sign";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var sign = 1d;
            var args = Utilities.GetArgs(script, ref index, stack)
                  .Select(a => a.ToDouble());
            foreach (var arg in args)
            {
                sign *= arg;
            }
            return new Variable(Math.Sign(sign));
        }
    }
}
