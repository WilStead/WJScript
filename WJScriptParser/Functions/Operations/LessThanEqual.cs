using System;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class LessThanEqual : IOperation
    {
        public string Name => Constants.LESS_THAN_EQUAL;

        public uint Priority => 60;

        public Variable Combine(Variable left, Variable right)
        {
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    switch (right.Type)
                    {
                        // array <= array compares lengths
                        case Variable.VariableType.Array:
                            return new Variable(left.Children.Count <= right.Children.Count);
                        // array <= boolean is true if the array is empty, or the boolean is true
                        case Variable.VariableType.Boolean:
                            return new Variable(right.Boolean || left.Children.Count == 0);
                        // array <= number compares the length to the number
                        case Variable.VariableType.Number:
                            return new Variable(left.Children.Count <= right.Value);
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                return new Variable(left.Children.Count <= value);
                            }
                            else
                            {
                                // array <= string compares lengths
                                return new Variable(left.Children.Count <= right.String.Length);
                            }
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        // boolean <= array is true if the boolean is false, or array is non-empty
                        case Variable.VariableType.Array:
                            return new Variable(!left.Boolean || right.Children.Count > 0);
                        // boolean <= boolean is true if the right is true or they are both false
                        case Variable.VariableType.Boolean:
                            return new Variable(right.Boolean || !left.Boolean);
                        // boolean <= number converts the boolean to a number and performs a numeric comparison
                        case Variable.VariableType.Number:
                            return new Variable(Convert.ToDouble(left.Boolean) <= right.Value);
                        // boolean <= string converts the string to a boolean and performs a comparison
                        case Variable.VariableType.String:
                            return new Variable(right.String.ToBoolean() || !left.Boolean);
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        // number <= array compares the number to the length
                        case Variable.VariableType.Array:
                            return new Variable(left.Value <= right.Children.Count);
                        case Variable.VariableType.Boolean:
                            return new Variable(left.Value <= Convert.ToDouble(right.Boolean));
                        case Variable.VariableType.Number:
                            return new Variable(left.Value <= right.Value);
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                return new Variable(left.Value <= value);
                            }
                            else
                            {
                                // number <= string compares the number to the length
                                return new Variable(left.Value <= right.String.Length);
                            }
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.String:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            if (double.TryParse(left.String, out var lv))
                            {
                                return new Variable(lv <= right.Children.Count);
                            }
                            else
                            {
                                // string <= array compares lengths
                                return new Variable(left.String.Length <= right.Children.Count);
                            }
                        // string <= boolean converts the string to a boolean and performs a comparison
                        case Variable.VariableType.Boolean:
                            return new Variable(right.Boolean || !left.String.ToBoolean());
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                return new Variable(value <= right.Value);
                            }
                            else
                            {
                                // string <= number compares the number to the length
                                return new Variable(left.String.Length <= right.Value);
                            }
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var r) && double.TryParse(left.String, out var l))
                            {
                                return new Variable(l <= r);
                            }
                            else
                            {
                                return new Variable(left.String.CompareTo(right.String) <= 0);
                            }
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return new Variable(false);
        }
    }
}
