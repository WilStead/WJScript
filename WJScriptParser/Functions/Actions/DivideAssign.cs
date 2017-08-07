using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Actions
{
    public class DivideAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.DIV_ASSIGN;

        public override uint Priority => 8;

        public override Variable Combine(Variable left, Variable right)
        {
            var c = 0;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        // array / array is treated as an except
                        case Variable.VariableType.Array:
                            var unique = left.Children
                                .Where(l => !right.Children.Any(r =>
                                    (int.TryParse(r.Key, out var rKey) || int.TryParse(l.Key, out var lKey) || r.Key == l.Key)
                                    && r.Value.Equals(l.Value)))
                                .ToList();
                            left.Children.Clear();
                            foreach (var item in unique)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    left.Children.Add(c.ToString(), item.Value);
                                    c++;
                                }
                                else
                                {
                                    left.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                        case Variable.VariableType.Number:
                        case Variable.VariableType.String:
                            var matches = left.Children
                                .Where(l => l.Value.Equals(right))
                                .Select(l => l.Key)
                                .ToList();
                            foreach (var key in matches)
                            {
                                left.Children.Remove(key);
                                if (int.TryParse(key, out var intKey))
                                {
                                    var higher = left.Children
                                        .Where(kvp => int.TryParse(kvp.Key, out int cKey) && cKey > intKey)
                                        .ToList();
                                    foreach (var item in higher)
                                    {
                                        left.Children.Remove(item.Key);
                                    }
                                    foreach (var item in higher)
                                    {
                                        var cKey = int.Parse(item.Key) + 1;
                                        left.Children.Add(cKey.ToString(), item.Value);
                                    }
                                }
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
                            var nonMatches = right.Children
                                .Where(r => !r.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in nonMatches)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    left.Children.Add(c.ToString(), item.Value);
                                    c++;
                                }
                                else
                                {
                                    left.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            left.Boolean = left.Boolean == right.Boolean;
                            break;
                        // boolean / number converts the number as a boolean value and performs an XNOR
                        case Variable.VariableType.Number:
                            left.Boolean = left.Boolean == Convert.ToBoolean(right.Value);
                            break;
                        // boolean / string converts the string to a boolean and performs an XNOR
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
                            var nonMatches = right.Children
                                .Where(r => !r.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in nonMatches)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    left.Children.Add(c.ToString(), item.Value);
                                    c++;
                                }
                                else
                                {
                                    left.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = Convert.ToBoolean(left.Value) == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            left.Value /= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                left.Value /= value;
                            }
                            else
                            {
                                left.Type = Variable.VariableType.String;
                                left.String = $"{left.Value}/{right.String}";
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
                            var nonMatches = right.Children
                                .Where(kvp => !kvp.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in nonMatches)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    left.Children.Add(c.ToString(), item.Value);
                                    c++;
                                }
                                else
                                {
                                    left.Children.Add(item.Key, item.Value);
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = left.String.ToBoolean() == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = value / right.Value;
                            }
                            else
                            {
                                // string / number divides the string's length and returns the resulting substring.
                                left.String = left.String.Substring(0,
                                    (int)Math.Max(0, Math.Round(left.String.Length / right.Value)));
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var r))
                            {
                                if (double.TryParse(left.String, out var l))
                                {
                                    left.Type = Variable.VariableType.Number;
                                    left.Value = l / r;
                                }
                                else
                                {
                                    left.String = left.String.Substring(0,
                                        (int)Math.Max(0, Math.Round(left.String.Length / r)));
                                }
                            }
                            else
                            {
                                left.String = $"{left.String}/{right.String}";
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
