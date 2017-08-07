using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Actions
{
    public class XorAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.XOR_ASSIGN;

        public override uint Priority => 9;

        public override Variable Combine(Variable left, Variable right)
        {
            var c = 0;
            switch (left.Type)
            {
                // ^ is treated as an intersect for arrays
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var unique = left.Children
                                .Where(l => right.Children.Any(r =>
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
                                .ToList();
                            left.Children.Clear();
                            foreach (var item in matches)
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
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var matches = right.Children
                                .Where(r => r.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in matches)
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
                            left.Boolean ^= right.Boolean;
                            break;
                        // boolean ^ number treats the number as a boolean value and performs an XOR
                        case Variable.VariableType.Number:
                            left.Boolean ^= Convert.ToBoolean(right.Value);
                            break;
                        // boolean ^ string converts the string to a boolean and performs an XOR
                        case Variable.VariableType.String:
                            left.Boolean ^= right.String.ToBoolean();
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var matches = right.Children
                                .Where(r => r.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in matches)
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
                            left.Boolean = Convert.ToBoolean(left.Value) ^ right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            left.Value = Math.Pow(left.Value, right.Value);
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                left.Value = Math.Pow(left.Value, value);
                            }
                            else
                            {
                                left.Type = Variable.VariableType.String;
                                left.String = $"{left.Value}^{right.String}";
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
                            var matches = right.Children
                                .Where(kvp => kvp.Value.Equals(left))
                                .ToList();
                            left.Type = Variable.VariableType.Array;
                            left.Children.Clear();
                            foreach (var item in matches)
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
                            left.Boolean = left.String.ToBoolean() ^ right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = Math.Pow(value, right.Value);
                            }
                            else
                            {
                                left.String = $"{left.String}^{right.Value}";
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var r) && double.TryParse(left.String, out var l))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = Math.Pow(l, r);
                            }
                            else
                            {
                                left.String = $"{left.String}^{right.String}";
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
