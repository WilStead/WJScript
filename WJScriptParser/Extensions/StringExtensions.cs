using System;

namespace WScriptParser.Extensions
{
    public static class StringExtensions
    {
        public static bool ToBoolean(this string str)
        {
            try
            {
                // If the string is like "true" or "false", return the converted value.
                return Convert.ToBoolean(str);
            }
            catch (FormatException)
            {
                // If the string can be parsed as a double, the boolean equivalent of the double is returned.
                if (double.TryParse(str, out var value))
                {
                    return Convert.ToBoolean(value);
                }
                // Otherwise, any non-empty string is considered "truthy."
                return !string.IsNullOrWhiteSpace(str);
            }
        }
    }
}
