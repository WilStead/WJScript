using System.Linq;

namespace WScriptParser.Functions
{
    public class Size : IFunction
    {
        private const string _name = "Size";
        public string Name => _name;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var size = 0d;
            var args = Utilities.GetArgs(script, ref index, stack);

            if (context.Equals(Variable.Empty))
            {
                return new Variable(size);
            }

            // With 0 arguments, returns the size of the context.
            // With 1 or more arguments, returns true if the size is equal to any of the arguments; false otherwise.
            switch (context.Type)
            {
                case Variable.VariableType.Array:
                    if (args.Count == 0)
                    {
                        size = context.Children.Count;
                    }
                    else
                    {
                        return new Variable(args.Any(a => a.ToInteger() == context.Children.Count));
                    }
                    break;
                case Variable.VariableType.Boolean:
                    if (args.Count == 0)
                    {
                        size = context.Boolean ? 1 : 0;
                    }
                    else
                    {
                        return new Variable(args.Any(a => a.ToInteger() == (context.Boolean ? 1 : 0)));
                    }
                    break;
                case Variable.VariableType.Number:
                    if (args.Count == 0)
                    {
                        size = context.Value;
                    }
                    else
                    {
                        return new Variable(args.Any(a => a.ToDouble() == context.Value));
                    }
                    break;
                case Variable.VariableType.String:
                    if (args.Count == 0)
                    {
                        size = context.String.Length;
                    }
                    else
                    {
                        return new Variable(args.Any(a => a.ToInteger() == context.String.Length));
                    }
                    break;
                default:
                    break;
            }

            return new Variable(size);
        }
    }
}
