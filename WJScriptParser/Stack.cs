using System.Collections.Generic;
using WScriptParser.Functions;
using WScriptParser.Functions.Actions;
using WScriptParser.Functions.Operations;

namespace WScriptParser
{
    /// <summary>
    /// The global context for an instance of the parser.
    /// </summary>
    public class Stack
    {
        internal static Dictionary<string, IActionFunction> Actions { get; } = new Dictionary<string, IActionFunction>();

        internal Stack<StackLevel> ExecutionStack { get; } = new Stack<StackLevel>();

        internal static Dictionary<string, IFunction> Functions { get; } = new Dictionary<string, IFunction>();

        internal static Identity IdentityFunction { get; } = new Identity();

        internal static IOperation NoOp { get; } = new NoOp();

        internal static Dictionary<string, IOperation> Operations { get; } = new Dictionary<string, IOperation>();

        internal static Value ValueFunction { get; } = new Value();

        static Stack()
        {
            AddOperation(new Add());
            AddOperation(new And());
            AddOperation(new Divide());
            AddOperation(new Equal());
            AddOperation(new GreaterThan());
            AddOperation(new GreaterThanEqual());
            AddOperation(new LessThan());
            AddOperation(new LessThanEqual());
            AddOperation(new Modulus());
            AddOperation(new Multiply());
            AddOperation(new NotEqual());
            AddOperation(new Or());
            AddOperation(new Pow());
            AddOperation(new Subtract());

            AddAction(new AddAssign());
            AddAction(new AndAssign());
            AddAction(new Assign());
            AddAction(new Decrement());
            AddAction(new DivideAssign());
            AddAction(new Increment());
            AddAction(new ModulusAssign());
            AddAction(new MultiplyAssign());
            AddAction(new OrAssign());
            AddAction(new SubtractAssign());
            AddAction(new XorAssign());

            AddAction(new Child());

            AddGlobal(new Break());
            AddGlobal(new Continue());
            AddGlobal(new FunctionDeclaration());
            AddGlobal(new If());
            AddGlobal(new Return());
            AddGlobal(new While());

            AddGlobal(new IndexOf());
            AddGlobal(new ToLower());
            AddGlobal(new ToUpper());
            AddGlobal(new Size());
            AddGlobal(new Subset());

            AddGlobal(new Abs());
            AddGlobal(new Acos());
            AddGlobal(new Asin());
            AddGlobal(new Atan());
            AddGlobal(new Ceiling());
            AddGlobal(new Cos());
            AddGlobal(new Cosh());
            AddGlobal(new E());
            AddGlobal(new Exp());
            AddGlobal(new Floor());
            AddGlobal(new Log());
            AddGlobal(new Log10());
            AddGlobal(new Max());
            AddGlobal(new Pi());
            AddGlobal(new Round());
            AddGlobal(new Sign());
            AddGlobal(new Sin());
            AddGlobal(new Sinh());
            AddGlobal(new Sqrt());
            AddGlobal(new Tan());
            AddGlobal(new Tanh());
            AddGlobal(new Truncate());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Stack"/>.
        /// </summary>
        public Stack() { }

        /// <summary>
        /// Add a new <see cref="IActionFunction"/> to the global context.
        /// </summary>
        public static void AddAction(IActionFunction action) => Actions.Add(action.Name, action);

        /// <summary>
        /// Add a new <see cref="IFunction"/> to the global context.
        /// </summary>
        public static void AddGlobal(IFunction function) => Functions.Add(function.Name, function);

        /// <summary>
        /// Add a new <see cref="IOperation"/> to the global context.
        /// </summary>
        public static void AddOperation(IOperation operation) => Operations.Add(operation.Name, operation);

        internal static IActionFunction GetActionFunction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            Actions.TryGetValue(name, out var action);
            return action;
        }

        internal IFunction GetArrayFunction(string token, ref int index, string action)
        {
            if (!string.IsNullOrWhiteSpace(action))
            {
                return null;
            }

            var startIndex = token.IndexOf(Constants.START_INDEX);
            if (startIndex <= 0)
            {
                return null;
            }

            var length = token.Length;
            var indexer = Utilities.GetIndexer(ref token, this);
            if (indexer == Variable.Empty)
            {
                return null;
            }

            var f = GetFunction(token);
            if (f == null)
            {
                return null;
            }

            index -= (length - startIndex - 1);
            return f;
        }

        internal IFunction GetFunction(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            IFunction function;
            if (ExecutionStack.Count > 0)
            {
                var locals = ExecutionStack.Peek().Variables;
                if (locals.TryGetValue(token, out function))
                {
                    return function;
                }
            }

            Functions.TryGetValue(token, out function);
            return function;
        }

        internal IFunction GetFunctionOrAction(string token, ref string action)
        {
            // Recognized literals are returned as-is, without checking to see if there is anything
            // in scope with a matching name. This avoids things like "6=2;a=6+1;" returning 3
            // instead of 7.
            if (Value.GetLiteral(token, this) != null)
            {
                ValueFunction.Token = token;
                return ValueFunction;
            }

            var a = GetActionFunction(action);
            if (a != null)
            {
                a.Token = token;
                action = null;
                return a;
            }

            var f = GetFunction(token);
            if (f != null)
            {
                return f;
            }

            ValueFunction.Token = token;
            return ValueFunction;
        }

        internal static IAction GetAction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NoOp;
            }

            if (Operations.TryGetValue(name, out var operation))
            {
                return operation;
            }

            if (Actions.TryGetValue(name, out var action))
            {
                return action;
            }

            return NoOp;
        }

        internal IFunction ParseFunction(string script, ref int index, string token, ref string action)
        {
            // Parenthetical expression or array literal.
            if ((token.Length == 1
                && (token[0] == Constants.START_ARG || token[0] == Constants.START_INDEX))
                || index >= script.Length)
            {
                return IdentityFunction;
            }

            var f = GetArrayFunction(token, ref index, action);
            if (f != null)
            {
                return f;
            }

            return GetFunctionOrAction(token, ref action);
        }

        internal void PopLocals()
        {
            if (ExecutionStack.Count > 0)
            {
                ExecutionStack.Pop();
            }
        }

        internal void PushLocal(IFunction local)
        {
            if (string.IsNullOrEmpty(local.Name))
            {
                return;
            }

            StackLevel locals = null;

            if (ExecutionStack.Count == 0)
            {
                locals = new StackLevel();
                ExecutionStack.Push(locals);
            }
            else
            {
                locals = ExecutionStack.Peek();
            }

            locals.Variables[local.Name] = local;
        }

        internal void PushLocals(StackLevel locals) => ExecutionStack.Push(locals);
    }
}
