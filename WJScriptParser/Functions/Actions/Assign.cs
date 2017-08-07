namespace WScriptParser.Functions.Actions
{
    public class Assign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.ASSIGNMENT;

        public override uint Priority => 5;

        public override Variable Combine(Variable left, Variable right)
            => left.Copy(right);
    }
}
