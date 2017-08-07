namespace WScriptParser.Functions
{
    public class If : IFunction
    {
        public string Name => Constants.IF;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            int startIndex = index;

            Variable result = Parser.Parse(script, ref index, stack, context, Constants.END_ARG);

            if (result.ToBoolean())
            {
                result = Parser.ProcessBlock(script, ref index, stack, context);

                if (result.Type == Variable.VariableType.Break
                    || result.Type == Variable.VariableType.Continue)
                {
                    index = startIndex;
                    Parser.SkipBlock(script, ref index);
                }
                SkipLogicBlocks(script, ref index);

                return result;
            }

            // If the condition was not true, skip the if block.
            Parser.SkipBlock(script, ref index);

            int endOfToken = index;
            string nextToken = Utilities.GetNextToken(script, ref endOfToken);

            if (nextToken == Constants.ELSE_IF)
            {
                index = endOfToken + 1;
                return Evaluate(script, ref index, stack, context);
            }
            else if (nextToken == Constants.ELSE)
            {
                index = endOfToken + 1;
                return Parser.ProcessBlock(script, ref index, stack, context);
            }
            else
            {
                return new Variable();
            }
        }

        private static void SkipLogicBlocks(string script, ref int index)
        {
            while (index < script.Length)
            {
                int endOfToken = index;
                string nextToken = Utilities.GetNextToken(script, ref endOfToken);
                if (nextToken != Constants.ELSE_IF
                    && nextToken != Constants.ELSE)
                {
                    return;
                }
                index = endOfToken;
                Parser.SkipBlock(script, ref index);
            }
        }
    }
}
