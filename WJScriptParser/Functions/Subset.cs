using System.Collections.Generic;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Subset : IFunction
    {
        public string Name => "Subset";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            var args = Utilities.GetArgs(script, ref index, stack);

            if (context.Equals(Variable.Empty) || args.Count == 0)
            {
                return new Variable(context);
            }

            var startIndex = args[0].ToInteger();
            switch (context.Type)
            {
                case Variable.VariableType.Array:
                    var newItems = new List<KeyValuePair<string, Variable>>();
                    foreach (var key in context.Children.Keys)
                    {
                        var endIndex = args.Count > 1 ? startIndex + args[1].ToInteger() - 1 : context.Children.Count - startIndex;
                        if (int.TryParse(key, out var intKey))
                        {
                            if (intKey >= startIndex && intKey <= endIndex)
                            {
                                intKey -= startIndex;
                                newItems.Add(new KeyValuePair<string, Variable>(intKey.ToString(), context.Children[key]));
                            }
                        }
                        else
                        {
                            newItems.Add(new KeyValuePair<string, Variable>(key, context.Children[key]));
                        }
                    }
                    return new Variable(newItems);
                case Variable.VariableType.String:
                    if (startIndex > context.String.Length)
                    {
                        return new Variable(Variable.VariableType.String);
                    }
                    if (args.Count == 1)
                    {
                        return new Variable(context.String.Substring(startIndex));
                    }
                    else
                    {
                        var length = args[1].ToInteger();
                        if (startIndex + length >= context.String.Length)
                        {
                            return new Variable(context.String.Substring(startIndex));
                        }
                        else
                        {
                            return new Variable(context.String.Substring(startIndex, length));
                        }
                    }
                default:
                    return new Variable(context);
            }
        }
    }
}
