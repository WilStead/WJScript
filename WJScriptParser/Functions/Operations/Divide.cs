﻿using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class Divide : IOperation
    {
        public string Name => Constants.DIV;

        public uint Priority => 80;

        public Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
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
                            result.Children.Clear();
                            foreach (var item in unique)
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
                // / functions as an XNOR for booleans.
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var nonMatches = right.Children
                                .Where(r => !r.Value.Equals(left))
                                .ToList();
                            result.Type = Variable.VariableType.Array;
                            result.Children.Clear();
                            foreach (var item in nonMatches)
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
                            result.Boolean = left.Boolean == right.Boolean;
                            break;
                        // boolean / number converts the number as a boolean value and performs an XNOR
                        case Variable.VariableType.Number:
                            result.Boolean = left.Boolean == Convert.ToBoolean(right.Value);
                            break;
                        // boolean / string converts the string to a boolean and performs an XNOR
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
                            var nonMatches = right.Children
                                .Where(r => !r.Value.Equals(left))
                                .ToList();
                            result.Type = Variable.VariableType.Array;
                            result.Children.Clear();
                            foreach (var item in nonMatches)
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
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = Convert.ToBoolean(left.Value) == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            result.Value /= right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                result.Value /= value;
                            }
                            else
                            {
                                result.Type = Variable.VariableType.String;
                                result.String = $"{left.Value}/{right.String}";
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
                            result.Type = Variable.VariableType.Array;
                            result.Children.Clear();
                            foreach (var item in nonMatches)
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
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = left.String.ToBoolean() == right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = value / right.Value;
                            }
                            else
                            {
                                // string / number divides the string's length and returns the resulting substring.
                                result.String = left.String.Substring(0,
                                    (int)Math.Max(0, Math.Round(left.String.Length / right.Value)));
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var r))
                            {
                                if (double.TryParse(left.String, out var l))
                                {
                                    result.Type = Variable.VariableType.Number;
                                    result.Value = l / r;
                                }
                                else
                                {
                                    result.String = left.String.Substring(0,
                                        (int)Math.Max(0, Math.Round(left.String.Length / r)));
                                }
                            }
                            else
                            {
                                result.String = $"{left.String}/{right.String}";
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