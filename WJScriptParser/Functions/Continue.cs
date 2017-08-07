namespace WScriptParser.Functions
{
    public class Continue : IFunction
    {
        public string Name => Constants.CONTINUE;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Variable.VariableType.Continue);
    }
}
