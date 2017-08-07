namespace WScriptParser.Functions
{
    public class ToUpper : IFunction
    {
        public string Name => "ToUpper";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var args = Utilities.GetArgs(script, ref index, stack);

            if (context.Type == Variable.VariableType.String)
            {
                return new Variable(context.String.ToUpper());
            }
            return new Variable(context);
        }
    }
}
