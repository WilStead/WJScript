using System;
using System.Collections.Generic;
using System.Linq;
using WScriptParser.Extensions;
using WScriptParser.Functions;
using WScriptParser.Functions.Operations;

namespace WScriptParser
{
    public class Variable
    {
        public enum VariableType { None, Array, Boolean, Break, Continue, Number, String }

        public static Variable Empty = new Variable();

        private bool _boolean;
        public bool Boolean
        {
            get => _boolean;
            set
            {
                Type = VariableType.Boolean;
                _boolean = value;
            }
        }

        public Dictionary<string, Variable> Children { get; set; } = new Dictionary<string, Variable>();

        private string _string;
        public string String
        {
            get => _string;
            set
            {
                Type = VariableType.String;
                _string = value;
            }
        }

        public VariableType Type { get; set; }

        private double _value = double.NaN;
        public double Value
        {
            get => _value;
            set
            {
                Type = VariableType.Number;
                _value = value;
            }
        }

        public Variable() { }

        public Variable(VariableType type)
        {
            Type = type;

            switch (type)
            {
                case VariableType.Number:
                    Value = 0;
                    break;
                case VariableType.String:
                    String = string.Empty;
                    break;
                default:
                    break;
            }
        }

        public Variable(double value)
        {
            Type = VariableType.Number;
            Value = value;
        }

        public Variable(string value)
        {
            Type = VariableType.String;
            String = value;
        }

        public Variable(bool value)
        {
            Type = VariableType.Boolean;
            Boolean = value;
        }

        public Variable(IList<Variable> values)
        {
            Type = VariableType.Array;
            for (int i = 0; i < values.Count; i++)
            {
                Children[i.ToString()] = new Variable(values[i]);
            }
        }

        public Variable(IEnumerable<KeyValuePair<string, Variable>> values)
        {
            Type = VariableType.Array;
            foreach (var item in values)
            {
                Children[item.Key] = new Variable(item.Value);
            }
        }

        public Variable(Variable other) => Copy(other);

        public Variable Copy(Variable other)
        {
            Boolean = other.Boolean;
            Children.Clear();
            foreach (var item in other.Children)
            {
                Children.Add(item.Key, item.Value);
            }
            String = other.String;
            Value = other.Value;
            Type = other.Type;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Variable))
            {
                return false;
            }
            var other = obj as Variable;
            if (Type != other.Type
                || Boolean != other.Boolean
                || !Children.SequenceEqual(other.Children))
            {
                return false;
            }
            switch (Type)
            {
                case VariableType.Number:
                    if (double.IsNaN(Value))
                    {
                        return double.IsNaN(other.Value);
                    }
                    return Value == other.Value;
                case VariableType.String:
                    return String.Equals(other.String, StringComparison.Ordinal);
                default:
                    return true;
            }
        }

        public override int GetHashCode() => base.GetHashCode();

        public bool ToBoolean()
        {
            switch (Type)
            {
                // Arrays are truthy if they are non-empty
                case VariableType.Array:
                    return Children.Count > 0;
                case VariableType.Boolean:
                    return Boolean;
                case VariableType.Number:
                    return Convert.ToBoolean(Value);
                case VariableType.String:
                    return String.ToBoolean();
                default:
                    return false;
            }
        }

        public double ToDouble()
        {
            switch (Type)
            {
                case VariableType.Array:
                    return Children.Count;
                case VariableType.Boolean:
                    return Boolean ? 1 : 0;
                case VariableType.Number:
                    return Value;
                case VariableType.String:
                    if (double.TryParse(String, out var value))
                    {
                        return value;
                    }
                    else
                    {
                        return String.Length;
                    }
                default:
                    return 0;
            }
        }

        public int ToInteger()
        {
            switch (Type)
            {
                case VariableType.Array:
                    return Children.Count;
                case VariableType.Boolean:
                    return Boolean ? 1 : 0;
                case VariableType.Number:
                    return (int)Math.Round(Value);
                case VariableType.String:
                    if (double.TryParse(String, out var value))
                    {
                        return (int)Math.Round(value);
                    }
                    else
                    {
                        return String.Length;
                    }
                default:
                    return 0;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case VariableType.Array:
                    return $"{Constants.START_INDEX}{Children.Aggregate(string.Empty, (s, v) => $"{s}{(s.Length > 0 ? "," : "")}{v.ToString()}")}{Constants.END_INDEX}";
                case VariableType.Boolean:
                    return Boolean.ToString();
                case VariableType.Number:
                    return Value.ToString();
                case VariableType.String:
                    return String;
                default:
                    return string.Empty;
            }
        }

        public object ToValue()
        {
            switch (Type)
            {
                case VariableType.Array:
                    // Add any integer-indexed items in order.
                    var list = new List<object>();
                    var intIndexed = Children.Where(kvp => int.TryParse(kvp.Key, out var intKey));
                    // If any integer-indexed items are present, but the index doesn't start at 0,
                    // pad the list with nulls so that the indexes will be consistent.
                    if (intIndexed.Count() > 0)
                    {
                        var lowest = intIndexed.Min(kvp => int.Parse(kvp.Key));
                        for (int i = 0; i < lowest; i++)
                        {
                            list.Add(null);
                        }
                    }
                    foreach (var child in intIndexed.OrderBy(kvp => int.Parse(kvp.Key)))
                    {
                        list.Add(child.Value.ToValue());
                    }
                    // Add any non-int-indexed items.
                    foreach (var child in Children.Except(intIndexed))
                    {
                        list.Add(child.Value.ToValue());
                    }
                    return list;
                case VariableType.Boolean:
                    return Boolean;
                case VariableType.Number:
                    return Value;
                case VariableType.String:
                    return String;
                default:
                    return null;
            }
        }
    }
}
