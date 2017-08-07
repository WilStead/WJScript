namespace WScriptParser.Functions.Operations
{
    public class NoOp : IOperation
    {
        private static string _name = Constants.EMPTY.ToString();
        public string Name => _name;

        public uint Priority => 0;

        public Variable Combine(Variable left, Variable right)
            => new Variable(right);
    }
}
