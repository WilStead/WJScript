namespace WScriptParser.Functions
{
    public interface IAction
    {
        uint Priority { get; }

        Variable Combine(Variable left, Variable right);
    }
}
