namespace WScriptParser.Functions
{
    public class Break : IFunction
    {
        public string Name => Constants.BREAK;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Variable.VariableType.Break);
    }
}
