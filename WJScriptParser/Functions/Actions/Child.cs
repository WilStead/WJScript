namespace WScriptParser.Functions.Actions
{
    public class Child : ActionFunction, IActionFunction
    {
        public override string Name => Constants.CHILD_SEPARATOR;

        public override uint Priority => 1000;

        public override Variable Combine(Variable left, Variable right)
        {
            if (string.IsNullOrEmpty(right.String))
            {
                return new Variable();
            }

            // Accessing a child of an empty variable turns it into an array (still empty).
            if (left.Equals(Variable.Empty))
            {
                left.Type = Variable.VariableType.Array;
            }

            // If the child is an existing index, return the value.
            if (left.Children.TryGetValue(right.String, out var value))
            {
                return value;
            }

            // If the child is a valid index to a string value, return the char.
            if (left.Type == Variable.VariableType.String
                && int.TryParse(right.String, out var index)
                && index >= 0
                && index < left.String.Length)
            {
                return new Variable(left.String[index].ToString());
            }

            // Accessing a child that doesn't currently exist creates it.
            var v = new Variable();
            left.Children.Add(right.String, v);
            return v;
        }

        public override Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            Variable current = GetCurrent(script, ref index, stack, context);

            var endIndex = index;
            var token = Utilities.GetNextToken(script, ref endIndex);

            // Child function.
            if (script[endIndex] == Constants.START_ARG)
            {
                return Utilities.GetItem(script, ref index, stack, current);
            }

            index = endIndex;

            var result = Combine(current, new Variable(token));

            // Re-pushed in case a null changed to an array.
            stack.PushLocal(new Get(Token, current));

            return result;
        }
    }
}
