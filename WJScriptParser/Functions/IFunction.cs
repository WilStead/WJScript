namespace WScriptParser.Functions
{
    public interface IFunction
    {
        string Name { get; }

        Variable Evaluate(string script, ref int index, Stack stack, Variable context);
    }
}
