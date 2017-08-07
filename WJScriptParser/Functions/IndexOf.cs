using System.Linq;

namespace WScriptParser.Functions
{
    public class IndexOf : IFunction
    {
        public string Name => "IndexOf";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var args = Utilities.GetArgs(script, ref index, stack);

            if (context.Equals(Variable.Empty) || args.Count == 0)
            {
                return new Variable(-1);
            }

            switch (context.Type)
            {
                case Variable.VariableType.Array:
                    var match = context.Children.Where(kvp => args.Any(a => kvp.Value.Equals(a)))
                        .Select(kvp => kvp.Key)
                        .FirstOrDefault();
                    if (string.IsNullOrEmpty(match))
                    {
                        return new Variable(-1);
                    }
                    if (int.TryParse(match, out var intKey))
                    {
                        return new Variable(intKey);
                    }
                    return new Variable(match);
                case Variable.VariableType.String:
                    var result = -1;
                    for (int i = 0; i < args.Count; i++)
                    {
                        result = context.String.IndexOf(args[i].ToString());
                        if (result != -1)
                        {
                            break;
                        }
                    }
                    return new Variable(result);
                default:
                    return new Variable(-1);
            }
        }
    }
}
