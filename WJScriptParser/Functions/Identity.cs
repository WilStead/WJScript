namespace WScriptParser.Functions
{
    public class Identity : IFunction
    {
        public string Name { get; set; }

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => index > 0 && script[index - 1] == Constants.START_INDEX // Array literal.
                ? new Variable(Utilities.GetArgs(script, ref index, stack, Constants.START_INDEX, Constants.END_INDEX))
                : Parser.Parse(script, ref index, stack, context, Constants.END_ARG);
    }
}
