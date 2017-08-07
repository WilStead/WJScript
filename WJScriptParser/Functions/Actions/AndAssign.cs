﻿namespace WScriptParser.Functions.Actions
{
    public class AndAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.AND_ASSIGN;

        public override uint Priority => 4;

        public override Variable Combine(Variable left, Variable right)
        {
            left.Boolean = left.ToBoolean() && right.ToBoolean();
            left.Type = Variable.VariableType.Boolean;
            return left;
        }
    }
}
