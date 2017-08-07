namespace WScriptParser.Functions
{
    public class Get : IFunction
    {
        private string _name;

        public string Name => _name;

        private Variable _value;

        public Get(string name, Variable value)
        {
            _name = name;
            _value = value;
        }

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            if (index < script.Length && script[index] == Constants.START_INDEX)
            {
                Utilities.TryParseChars(script, ref index, Constants.START_INDEX);

                var indexer = Parser.Parse(script, ref index, stack, context, Constants.END_INDEX);

                // Retrieving a non-existent index gets an empty variable, not an error.
                if (!_value.Children.TryGetValue(indexer.ToString(), out var value))
                {
                    value = new Variable();
                }

                Utilities.TryParseChars(script, ref index, Constants.END_INDEX);

                return value;
            }

            return _value;
        }
    }
}
