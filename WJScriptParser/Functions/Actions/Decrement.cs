using System.Linq;

namespace WScriptParser.Functions.Actions
{
    public class Decrement : ActionFunction, IActionFunction
    {
        public override string Name => Constants.DECREMENT;

        public override uint Priority => 100;

        public override Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
            switch (left.Type)
            {
                // Decrementing an array is treated as removing the last numerically-indexed item, if one exists.
                case Variable.VariableType.Array:
                    var lastIndex = left.Children.Keys.Select(k => int.TryParse(k, out var key) ? key : -1).Max();
                    if (lastIndex != -1)
                    {
                        left.Children.Remove(lastIndex.ToString());
                    }
                    break;
                // Decrementing a boolean is treated as a toggle.
                case Variable.VariableType.Boolean:
                    left.Boolean = !left.Boolean;
                    break;
                case Variable.VariableType.Number:
                    left.Value--;
                    break;
                case Variable.VariableType.String:
                    if (double.TryParse(left.String, out var value))
                    {
                        left.Type = Variable.VariableType.Number;
                        left.Value = value - 1;
                    }
                    else
                    {
                        // Decrementing a non-numeric string removes the last character.
                        left.String = left.String.Substring(0, left.String.Length - 1);
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
