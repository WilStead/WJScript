using System;
using System.Linq;
using System.Text;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class Multiply : IOperation
    {
        public string Name => Constants.MULT;

        public uint Priority => 80;

        public Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
            int index;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        // array * array is treated as union
                        case Variable.VariableType.Array:
                            index = result.Children.Keys
                                .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                                .Max();
                            index++;
                            var matches = right.Children
                                .Where(r => !left.Children.Any(l =>
                                    (int.TryParse(r.Key, out var rKey) || int.TryParse(l.Key, out var lKey) || r.Key == l.Key)
                                    && r.Value.Equals(l.Value)))
                                .ToList();
                            foreach (var item in matches)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    result.Children.Add(index.ToString(), item.Value);
                                    index++;
                                }
                                else
                                {
                                    result.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        // array * other is applied distributively
                        case Variable.VariableType.Boolean:
                        case Variable.VariableType.Number:
                        case Variable.VariableType.String:
                            foreach (var key in left.Children.Keys)
                            {
                                result.Children[key] = Combine(left.Children[key], right);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                // * functions as an XNOR for booleans.
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            result.Type = Variable.VariableType.Array;
                            foreach (var item in right.Children)
                            {
                                result.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            result.Boolean = left.Boolean == right.Boolean;
                            break;
                        // boolean * number converts the number as a boolean value and performs an XNOR
                        case Variable.VariableType.Number:
                            result.Boolean = left.Boolean == Convert.ToBoolean(right.Value);
                            break;
                        // boolean * string converts the string to a boolean and performs an XNOR
                        case Variable.VariableType.String:
                            result.Boolean = left.Boolean == right.String.ToBoolean();
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            result.Type = Variable.VariableType.Array;
                            foreach (var item in right.Children)
                            {
                                result.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = Convert.ToBoolean(left.Value) == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            result.Value *= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                result.Value *= value;
                            }
                            else
                            {
                                // A multiplied string is repeated.
                                result.Type = Variable.VariableType.String;
                                var sb = new StringBuilder();
                                for (int i = 0; i < left.Value; i++)
                                {
                                    sb.Append(right.String);
                                }
                                result.String = sb.ToString();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.String:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            result.Type = Variable.VariableType.Array;
                            foreach (var item in right.Children)
                            {
                                result.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = left.String.ToBoolean() == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = value * right.Value;
                            }
                            else
                            {
                                var sb = new StringBuilder();
                                for (int i = 0; i < right.Value; i++)
                                {
                                    sb.Append(left.String);
                                }
                                result.String = sb.ToString();
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var l))
                            {
                                if (double.TryParse(right.String, out var r))
                                {
                                    result.Type = Variable.VariableType.Number;
                                    result.Value = l * r;
                                }
                                else
                                {
                                    var sb = new StringBuilder();
                                    for (int i = 0; i < l; i++)
                                    {
                                        sb.Append(right.String);
                                    }
                                    result.String = sb.ToString();
                                }
                            }
                            else if (double.TryParse(right.String, out var r))
                            {
                                var sb = new StringBuilder();
                                for (int i = 0; i < r; i++)
                                {
                                    sb.Append(left.String);
                                }
                                result.String = sb.ToString();
                            }
                            else
                            {
                                result.String = $"{left.String}*{right.String}";
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
