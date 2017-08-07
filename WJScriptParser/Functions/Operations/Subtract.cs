using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class Subtract : IOperation
    {
        public string Name => Constants.SUB;

        public uint Priority => 70;

        public Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
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
                            result.Children.Clear();
                            foreach (var item in diff)
                            {
                                if (int.TryParse(item.Key, out var intKey))
                                {
                                    result.Children.Add(c.ToString(), item.Value);
                                    c++;
                                }
                                else
                                {
                                    result.Children.Add(item.Key, item.Value);
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
                                result.Children.Remove(key);
                                if (int.TryParse(key, out var intKey))
                                {
                                    var higher = result.Children
                                        .Where(kvp => int.TryParse(kvp.Key, out int cKey) && cKey > intKey)
                                        .ToList();
                                    foreach (var item in higher)
                                    {
                                        result.Children.Remove(item.Key);
                                    }
                                    foreach (var item in higher)
                                    {
                                        var cKey = int.Parse(item.Key) + 1;
                                        result.Children.Add(cKey.ToString(), item.Value);
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
                            result.Boolean = false;
                            break;
                        // boolean - boolean is true only if left is true and right is false
                        case Variable.VariableType.Boolean:
                            result.Boolean &= !right.Boolean;
                            break;
                        // boolean - number converts the number to a boolean
                        case Variable.VariableType.Number:
                            result.Boolean &= !Convert.ToBoolean(right.Value);
                            break;
                        // boolean - string converts the string to a boolean
                        case Variable.VariableType.String:
                            result.Boolean &= !right.String.ToBoolean();
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
                            result.Copy(right);
                            if (key != null)
                            {
                                result.Children.Remove(key);
                                if (int.TryParse(key, out var intKey))
                                {
                                    var higher = result.Children
                                        .Where(kvp => int.TryParse(kvp.Key, out int cKey) && cKey > intKey)
                                        .ToList();
                                    foreach (var item in higher)
                                    {
                                        result.Children.Remove(item.Key);
                                    }
                                    foreach (var item in higher)
                                    {
                                        var cKey = int.Parse(item.Key) + 1;
                                        result.Children.Add(cKey.ToString(), item.Value);
                                    }
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = Convert.ToBoolean(left.Value) && !right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            result.Value -= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                result.Value -= value;
                            }
                            // number - non-numeric string is treated as string concatenation with the minus sign included.
                            else
                            {
                                result.Type = Variable.VariableType.String;
                                result.String = $"{left.Value}-{right.String}";
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
                            result.Copy(right);
                            if (key != null)
                            {
                                result.Children.Remove(key);
                                if (int.TryParse(key, out var intKey))
                                {
                                    var higher = result.Children
                                        .Where(kvp => int.TryParse(kvp.Key, out int cKey) && cKey > intKey)
                                        .ToList();
                                    foreach (var item in higher)
                                    {
                                        result.Children.Remove(item.Key);
                                    }
                                    foreach (var item in higher)
                                    {
                                        var cKey = int.Parse(item.Key) + 1;
                                        result.Children.Add(cKey.ToString(), item.Value);
                                    }
                                }
                            }
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = left.String.ToBoolean() && !right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = value - right.Value;
                            }
                            // non-numeric string - number is treated as removing that number from the string.
                            else
                            {
                                result.String = result.String.Replace(right.Value.ToString(), string.Empty);
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var value1)
                                && double.TryParse(right.String, out var value2))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = value1 - value2;
                            }
                            else
                            {
                                result.String = result.String.Replace(right.String, string.Empty);
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
