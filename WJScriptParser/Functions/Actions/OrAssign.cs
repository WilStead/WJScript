namespace WScriptParser.Functions.Actions
{
    public class OrAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.OR_ASSIGN;

        public override uint Priority => 3;

        public override Variable Combine(Variable left, Variable right)
        {
            left.Boolean = left.ToBoolean() || right.ToBoolean();
            left.Type = Variable.VariableType.Boolean;
            return left;
        }
    }
}
