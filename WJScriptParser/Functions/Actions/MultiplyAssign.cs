using System;
using System.Linq;
using System.Text;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Actions
{
    public class MultiplyAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.MULT_ASSIGN;

        public override uint Priority => 8;

        public override Variable Combine(Variable left, Variable right)
        {
            int index;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        // array * array is treated as union
                        case Variable.VariableType.Array:
                            index = left.Children.Keys
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
                                    left.Children.Add(index.ToString(), item.Value);
                                    index++;
                                }
                                else
                                {
                                    left.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        // array * other is applied distributively
                        case Variable.VariableType.Boolean:
                        case Variable.VariableType.Number:
                        case Variable.VariableType.String:
                            foreach (var key in left.Children.Keys.ToList())
                            {
                                left.Children[key] = Combine(left.Children[key], right);
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
                            foreach (var item in right.Children)
                            {
                                left.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            left.Type = Variable.VariableType.Array;
                            break;
                        case Variable.VariableType.Boolean:
                            left.Boolean = left.Boolean == right.Boolean;
                            break;
                        // boolean * number converts the number as a boolean value and performs an XNOR
                        case Variable.VariableType.Number:
                            left.Boolean = left.Boolean == Convert.ToBoolean(right.Value);
                            break;
                        // boolean * string converts the string to a boolean and performs an XNOR
                        case Variable.VariableType.String:
                            left.Boolean = left.Boolean == right.String.ToBoolean();
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            foreach (var item in right.Children)
                            {
                                left.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            left.Type = Variable.VariableType.Array;
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = Convert.ToBoolean(left.Value) == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            left.Value *= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                left.Value *= value;
                            }
                            else
                            {
                                // A multiplied string is repeated.
                                left.Type = Variable.VariableType.String;
                                var sb = new StringBuilder();
                                for (int i = 0; i < left.Value; i++)
                                {
                                    sb.Append(right.String);
                                }
                                left.String = sb.ToString();
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
                            foreach (var item in right.Children)
                            {
                                left.Children.Add(item.Key, Combine(item.Value, left));
                            }
                            left.Type = Variable.VariableType.Array;
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = left.String.ToBoolean() == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = value * right.Value;
                            }
                            else
                            {
                                var sb = new StringBuilder();
                                for (int i = 0; i < right.Value; i++)
                                {
                                    sb.Append(left.String);
                                }
                                left.String = sb.ToString();
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var l))
                            {
                                if (double.TryParse(right.String, out var r))
                                {
                                    left.Type = Variable.VariableType.Number;
                                    left.Value = l * r;
                                }
                                else
                                {
                                    var sb = new StringBuilder();
                                    for (int i = 0; i < l; i++)
                                    {
                                        sb.Append(right.String);
                                    }
                                    left.String = sb.ToString();
                                }
                            }
                            else if (double.TryParse(right.String, out var r))
                            {
                                var sb = new StringBuilder();
                                for (int i = 0; i < r; i++)
                                {
                                    sb.Append(left.String);
                                }
                                left.String = sb.ToString();
                            }
                            else
                            {
                                left.String = $"{left.String}*{right.String}";
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return left;
        }
    }
}
