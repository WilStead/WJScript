using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Actions
{
    public class SubtractAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.SUB_ASSIGN;

        public override uint Priority => 7;

        public override Variable Combine(Variable left, Variable right)
        {
            var c = 0;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var diff = left.Children
                                .Where(l => !right.Children.Any(r =>
                                    (int.TryParse(r.Key, out var rKey) || int.TryParse(l.Key, out var lKey) || r.Key == l.Key)
                                    && r.Value.Equals(l.Value)))
                                .ToList();
                            left.Children.Clear();
                            foreach (var item in diff)
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
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        // boolean - array treats the array as a truthy value
                        case Variable.VariableType.Array:
                            left.Boolean = false;
                            break;
                        // boolean - boolean is true only if left is true and right is false
                        case Variable.VariableType.Boolean:
                            left.Boolean = left.Boolean && !right.Boolean;
                            break;
                        // boolean - number converts the number to a boolean
                        case Variable.VariableType.Number:
                            left.Boolean = left.Boolean && !Convert.ToBoolean(right.Value);
                            break;
                        // boolean - string converts the string to a boolean
                        case Variable.VariableType.String:
                            left.Boolean = left.Boolean && !right.String.ToBoolean();
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var key = right.Children.Where(kvp => kvp.Value.Equals(left))
                                .Select(kvp => kvp.Key)
                                .FirstOrDefault();
                            left.Copy(right);
                            if (key != null)
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
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = Convert.ToBoolean(left.Value) && !right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            left.Value -= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                left.Value -= value;
                            }
                            // number - non-numeric string is treated as string concatenation with the minus sign included.
                            else
                            {
                                left.Type = Variable.VariableType.String;
                                left.String = $"{left.Value}-{right.String}";
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
                            var key = right.Children.Where(kvp => kvp.Value.Equals(left))
                                .Select(kvp => kvp.Key)
                                .FirstOrDefault();
                            left.Copy(right);
                            if (key != null)
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
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = left.String.ToBoolean() && !right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = value - right.Value;
                            }
                            // non-numeric string - number is treated as removing that number from the string.
                            else
                            {
                                left.String = left.String.Replace(right.Value.ToString(), string.Empty);
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var value1)
                                && double.TryParse(right.String, out var value2))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = value1 - value2;
                            }
                            else
                            {
                                left.String = left.String.Replace(right.String, string.Empty);
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
