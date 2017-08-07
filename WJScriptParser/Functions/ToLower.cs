namespace WScriptParser.Functions
{
    public class ToLower : IFunction
    {
        public string Name => "ToLower";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var args = Utilities.GetArgs(script, ref index, stack);

            if (context.Type == Variable.VariableType.String)
            {
                return new Variable(context.String.ToLower());
            }
            return new Variable(context);
        }
    }
}
