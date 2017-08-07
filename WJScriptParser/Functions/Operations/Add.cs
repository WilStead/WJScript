using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Operations
{
    public class Add : IOperation
    {
        public string Name => Constants.ADD;

        public uint Priority => 70;

        public Variable Combine(Variable left, Variable right)
        {
            var result = new Variable(left);
            int index;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    index = result.Children.Keys
                        .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                        .Max();
                    index++;
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            foreach (var item in right.Children)
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
                        case Variable.VariableType.Boolean:
                        case Variable.VariableType.Number:
                        case Variable.VariableType.String:
                            result.Children.Add(index.ToString(), right);
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Boolean:
                    switch (right.Type)
                    {
                        // boolean + array = the array
                        case Variable.VariableType.Array:
                            result.Copy(right);
                            break;
                        // boolean + boolean is treated as an AND
                        case Variable.VariableType.Boolean:
                            result.Boolean &= right.Boolean;
                            break;
                        // boolean + number treats the number as a boolean value and performs an AND
                        case Variable.VariableType.Number:
                            result.Boolean &= Convert.ToBoolean(right.Value);
                            break;
                        // boolean + string converts the string to a boolean and performs an AND
                        case Variable.VariableType.String:
                            result.Boolean &= right.String.ToBoolean();
                            break;
                        default:
                            break;
                    }
                    break;
                case Variable.VariableType.Number:
                    switch (right.Type)
                    {
                        case Variable.VariableType.Array:
                            var v = new Variable(left.Value);
                            result.Copy(right);
                            index = result.Children.Keys
                                .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                                .Max();
                            index++;
                            result.Children.Add(index.ToString(), v);
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = Convert.ToBoolean(left.Value) && right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            result.Value += right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                result.Value += value;
                            }
                            else
                            {
                                result.Type = Variable.VariableType.String;
                                result.String = left.Value.ToString() + right.String;
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
                            var s = new Variable(left.String);
                            result.Copy(right);
                            index = result.Children.Keys
                                .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                                .Max();
                            index++;
                            result.Children.Add(index.ToString(), s);
                            break;
                        case Variable.VariableType.Boolean:
                            result.Type = Variable.VariableType.Boolean;
                            result.Boolean = left.String.ToBoolean() && right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = value + right.Value;
                            }
                            else
                            {
                                result.String += right.Value.ToString();
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var lv)
                                && double.TryParse(right.String, out var rv))
                            {
                                result.Type = Variable.VariableType.Number;
                                result.Value = lv + rv;
                            }
                            else
                            {
                                result.String += right.String;
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
