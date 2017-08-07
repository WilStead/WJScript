namespace WScriptParser.Functions
{
    public class Value : IFunction
    {
        public string Name { get; set; }

        public string Token { get; set; }

        /// <summary>
        /// If the given token is recognized as a literal array, boolean, string, or number, it is returned
        /// as a <see cref="Variable"/>. Otherwise, null is returned.
        /// </summary>
        public static Variable GetLiteral(string token, Stack stack)
        {
            if (token == "true")
            {
                return new Variable(true);
            }
            if (token == "false")
            {
                return new Variable(false);
            }

            if (token.Length > 1 &&
                token[0] == Constants.START_INDEX &&
                token[token.Length - 1] == Constants.END_INDEX)
            {
                int index = 0;
                return new Variable(Utilities.GetArgs(token, ref index, stack, Constants.START_INDEX, Constants.END_INDEX));
            }

            if (token.Length > 1 &&
                token[0] == Constants.STRING_DELIMITER &&
                token[token.Length - 1] == Constants.STRING_DELIMITER)
            {
                return new Variable(token.Substring(1, token.Length - 2));
            }

            if (double.TryParse(token, out var value))
            {
                return new Variable(value);
            }

            return null;
        }

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => GetLiteral(Token, stack) ?? new Variable();
    }
}
