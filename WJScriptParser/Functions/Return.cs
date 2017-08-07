namespace WScriptParser.Functions
{
    public class Return : IFunction
    {
        public string Name => Constants.RETURN;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            Utilities.TryParseChars(script, ref index, ' ');

            var result = Parser.Parse(script, ref index, stack, context);

            index = script.Length;

            return result;
        }
    }
}
