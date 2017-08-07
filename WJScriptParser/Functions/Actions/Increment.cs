using System.Linq;

namespace WScriptParser.Functions.Actions
{
    public class Increment : ActionFunction, IActionFunction
    {
        public override string Name => Constants.INCREMENT;

        public override uint Priority => 100;

        public override Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
            switch (left.Type)
            {
                // Incrementing an array is treated as adding an empty item to the end.
                case Variable.VariableType.Array:
                    left.Children.Add(left.Children.Count.ToString(), new Variable());
                    break;
                // Incrementing a boolean is treated as a toggle.
                case Variable.VariableType.Boolean:
                    left.Boolean = !left.Boolean;
                    break;
                case Variable.VariableType.Number:
                    left.Value++;
                    break;
                case Variable.VariableType.String:
                    if (double.TryParse(left.String, out var value))
                    {
                        left.Type = Variable.VariableType.Number;
                        left.Value = value + 1;
                    }
                    else
                    {
                        // Incrementing a non-numeric string adds a space to the end.
                        left.String += " ";
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public override Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            bool isPrefix = string.IsNullOrWhiteSpace(Token) || Constants.CONTROL_STATEMENTS.Contains(Token);
            if (isPrefix)
            {
                Token = Utilities.GetNextToken(script, ref index);
            }

            Variable current = GetCurrent(script, ref index, stack, context);

            var newValue = Combine(current, Variable.Empty);

            if (isPrefix)
            {
                newValue.Copy(current);
            }

            stack.PushLocal(new Get(Token, current));

            return newValue;
        }
    }
}
