namespace WScriptParser.Functions.Actions
{
    public abstract class ActionFunction : IActionFunction
    {
        public virtual string Name { get; }

        public virtual uint Priority => 2;

        protected string _token;
        public string Token
        {
            get => _token;
            set => _token = value;
        }

        protected Variable GetCurrent(string script, ref int index, Stack stack, Variable context)
        {
            var indexer = Utilities.GetIndexer(ref _token, stack);

            Variable current;
            var f = stack.GetFunction(Token);
            if (f != null)
            {
                current = f.Evaluate(script, ref index, stack, context);
            }
            else
            {
                Stack.ValueFunction.Token = Token;
                current = Stack.ValueFunction.Evaluate(script, ref index, stack, context);
            }

            // If the variable is being indexed, work with the underlying child instead.
            if (indexer != Variable.Empty)
            {
                // If the index doesn't exist, automatically create a new empty child at that index.
                if (!current.Children.TryGetValue(indexer.ToString(), out var child))
                {
                    child = new Variable();
                    current.Children.Add(indexer.ToString(), child);
                }
                current = child;
            }

            return current;
        }

        public virtual Variable Combine(Variable left, Variable right) => new Variable();

        public virtual Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            Variable current = GetCurrent(script, ref index, stack, context);

            Variable right = Utilities.GetItem(script, ref index, stack, context);

            Combine(current, right);

            stack.PushLocal(new Get(Token, current));

            return new Variable(current);
        }
    }
}
