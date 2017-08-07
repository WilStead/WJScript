namespace WScriptParser.Functions
{
    public class While : IFunction
    {
        private const int MAX_LOOP = 256000;

        public string Name => Constants.WHILE;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            int startIndex = index;
            int cycles = 0;

            while (true)
            {
                index = startIndex;

                if (!Parser.Parse(script, ref index, stack, context, Constants.END_ARG).ToBoolean())
                {
                    break;
                }

                // Break early if it appears to be an infinite loop.
                if (++cycles >= MAX_LOOP)
                {
                    break;
                }

                if (Parser.ProcessBlock(script, ref index, stack, context).Type == Variable.VariableType.Break)
                {
                    index = startIndex;
                    break;
                }
            }

            // After breaking, skip the while block.
            Parser.SkipBlock(script, ref index);
            return new Variable();
        }
    }
}
