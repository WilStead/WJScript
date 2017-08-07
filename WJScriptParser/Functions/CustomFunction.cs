namespace WScriptParser.Functions
{
    public class CustomFunction : IFunction
    {
        private string[] _args;
        private string _body;

        public string Name { get; }

        public CustomFunction(string name, string body, params string[] args)
        {
            _args = args;
            _body = body;
            Name = name;
        }

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var args = Utilities.GetArgs(script, ref index, stack);

            Utilities.TryUnparseChars(script, ref index, Constants.START_BLOCK);

            // Missing arguments get passed an empty variable.
            for (int i = args.Count; i < _args.Length; i++)
            {
                args.Add(new Variable());
            }

            var stackLevel = new StackLevel(Name);
            for (int i = 0; i < _args.Length; i++)
            {
                stackLevel.Variables[_args[i]] = new Get(_args[i], args[i]);
            }
            stack.PushLocals(stackLevel);

            Variable result = null;
            int bodyIndex = 0;
            while (bodyIndex < _body.Length - 1)
            {
                result = Parser.Parse(_body, ref bodyIndex, stack, context);
                Utilities.NextStatement(_body, ref bodyIndex);
            }

            stack.PopLocals();

            return result;
        }
    }
}
