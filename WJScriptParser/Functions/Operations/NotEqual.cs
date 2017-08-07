using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class NotEqual : IOperation
    {
        public string Name => Constants.NOT_EQUAL;

        public uint Priority => 50;

        public Variable Combine(Variable left, Variable right)
        {
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        // array != array compares indexes and values, not reference
                        case Variable.VariableType.Array:
                            return new Variable(left.Children.Any(l => !right.Children.Any(rc => rc.Key == l.Key && rc.Value.Equals(l.Value)))
                                || right.Children.Any(rc => !left.Children.Any(l => l.Key == rc.Key && l.Value.Equals(rc.Value))));
                        // array != boolean is true if the boolean is true and the array is empty, or the boolean is false and the array is non-empty
                        case Variable.VariableType.Boolean:
                            return new Variable((right.Boolean && left.Children.Count == 0) || (!right.Boolean && left.Children.Count > 0));
                        // array != number compares the length to the number
                        case Variable.VariableType.Number:
                            return new Variable(left.Children.Count != right.Value);
                        // array != string compares length if the string is a number, and is true otherwise
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var r))
                            {
                                return new Variable(left.Children.Count != r);
                            }
                            else
                            {
                                return new Variable(true);
                            }
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            return new Variable((left.Boolean && right.Children.Count == 0) || (!left.Boolean && right.Children.Count > 0));
                        case Variable.VariableType.Boolean:
                            return new Variable(left.Boolean != right.Boolean);
                        // boolean != number converts the boolean to a number and performs a numeric comparison
                        case Variable.VariableType.Number:
                            return new Variable(Convert.ToDouble(left.Boolean) != right.Value);
                        // boolean != string converts the string to a boolean and performs a string comparison
                        case Variable.VariableType.String:
                            return new Variable(left.Boolean != right.String.ToBoolean());
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        // number != array compares the number to the length
                        case Variable.VariableType.Array:
                            return new Variable(left.Value != right.Children.Count);
                        case Variable.VariableType.Boolean:
                            return new Variable(left.Value != Convert.ToDouble(right.Boolean));
                        case Variable.VariableType.Number:
                            return new Variable(left.Value != right.Value);
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                return new Variable(left.Value != value);
                            }
                            else
                            {
                                // number != string compares the number to the length
                                return new Variable(left.Value != right.String.Length);
                            }
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.String:
                    switch (right.Type)
                    {
                        // string != array compares length if the string is a number, and is true otherwise
                        case Variable.VariableType.Array:
                            if (double.TryParse(left.String, out var l))
                            {
                                return new Variable(right.Children.Count != l);
                            }
                            else
                            {
                                return new Variable(true);
                            }
                        // string != boolean converts the string to a boolean and performs a string comparison
                        case Variable.VariableType.Boolean:
                            return new Variable(left.String.ToBoolean() != right.Boolean);
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                return new Variable(value != right.Value);
                            }
                            else
                            {
                                // string != number compares the number to the length
                                return new Variable(left.String.Length != right.Value);
                            }
                        case Variable.VariableType.String:
                            return new Variable(left.String.CompareTo(right.String) != 0);
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return new Variable(true);
        }
    }
}
