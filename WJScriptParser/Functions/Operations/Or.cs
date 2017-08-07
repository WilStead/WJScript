namespace WScriptParser.Functions.Operations
{
    public class Or : IOperation
    {
        public string Name => Constants.OR;

        public uint Priority => 30;

        public Variable Combine(Variable left, Variable right)
            => new Variable(left.ToBoolean() || right.ToBoolean());
    }
}
