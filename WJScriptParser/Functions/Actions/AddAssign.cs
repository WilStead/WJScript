using System;
using System.Linq;
using WScriptParser.Extensions;

namespace WScriptParser.Functions.Actions
{
    public class AddAssign : ActionFunction, IActionFunction
    {
        public override string Name => Constants.ADD_ASSIGN;

        public override uint Priority => 7;

        public override Variable Combine(Variable left, Variable right)
        {
            int index;
            switch (left.Type)
            {
                case Variable.VariableType.Array:
                    index = left.Children.Keys
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
                                    left.Children.Add(index.ToString(), item.Value);
                                    index++;
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
                            left.Children.Add(left.Children.Count.ToString(), right);
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
                            left.Copy(right);
                            break;
                        // boolean + boolean is treated as an AND
                        case Variable.VariableType.Boolean:
                            left.Boolean &= right.Boolean;
                            break;
                        // boolean + number treats the number as a boolean value and performs an AND
                        case Variable.VariableType.Number:
                            left.Boolean &= Convert.ToBoolean(right.Value);
                            break;
                        // boolean + string converts the string to a boolean and performs an AND
                        case Variable.VariableType.String:
                            left.Boolean &= right.String.ToBoolean();
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
                            left.Copy(right);
                            index = left.Children.Keys
                                .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                                .Max();
                            index++;
                            left.Children.Add(index.ToString(), v);
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = Convert.ToBoolean(left.Value) && right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            left.Value += right.Value;
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(right.String, out var value))
                            {
                                left.Value += value;
                            }
                            else
                            {
                                left.Type = Variable.VariableType.String;
                                left.String = left.Value.ToString() + right.String;
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
                            left.Copy(right);
                            index = left.Children.Keys
                                .Select(k => int.TryParse(k, out var intKey) ? intKey : -1)
                                .Max();
                            index++;
                            left.Children.Add(index.ToString(), s);
                            break;
                        case Variable.VariableType.Boolean:
                            left.Type = Variable.VariableType.Boolean;
                            left.Boolean = left.String.ToBoolean() && right.Boolean;
                            break;
                        case Variable.VariableType.Number:
                            if (double.TryParse(left.String, out var value))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = value + right.Value;
                            }
                            else
                            {
                                left.String += right.Value.ToString();
                            }
                            break;
                        case Variable.VariableType.String:
                            if (double.TryParse(left.String, out var lv)
                                && double.TryParse(right.String, out var rv))
                            {
                                left.Type = Variable.VariableType.Number;
                                left.Value = lv + rv;
                            }
                            else
                            {
                                left.String += right.String;
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
