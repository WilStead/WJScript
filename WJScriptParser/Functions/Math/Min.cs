using System.Linq;

namespace WScriptParser.Functions
{
    public class Min : IFunction
    {
        public string Name => "Min";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => a.ToDouble()).Min());
    }
}
