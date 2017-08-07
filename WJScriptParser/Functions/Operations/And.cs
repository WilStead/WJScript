namespace WScriptParser.Functions.Operations
{
    public class And : IOperation
    {
        public string Name => Constants.AND;

        public uint Priority => 40;

        public Variable Combine(Variable left, Variable right)
            => new Variable(left.ToBoolean() && right.ToBoolean());
    }
}
