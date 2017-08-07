using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WScriptParser
{
    internal static class Utilities
    {
        /// <summary>
        /// Parses the next argument list, and advances the cursor.
        /// </summary>
        /// <returns>The list of arguments (including any invalid arguments as <see cref="Variable.Empty"/>.</returns>
        internal static List<Variable> GetArgs(string script, ref int index, Stack stack, char start = Constants.START_ARG, char end = Constants.END_ARG)
        {
            List<Variable> args = new List<Variable>();

            TryParseChars(script, ref index, start);

            if (index >= script.Length || script[index] == Constants.END_STATEMENT)
            {
                return args;
            }

            int endIndex = index;
            GetBody(script, ref endIndex, start, end);

            while (index < endIndex)
            {
                args.Add(GetItem(script, ref index, stack, Variable.Empty));
            }

            TryParseChars(script, ref index, end);

            return args;
        }

        /// <summary>
        /// Consumes and returns the body of a code block.
        /// </summary>
        internal static string GetBody(string script, ref int index, char start, char end)
        {
            StringBuilder sb = new StringBuilder(script.Length);
            int braces = 0;

            for (; index < script.Length; index++)
            {
                char current = script[index];

                if (string.IsNullOrWhiteSpace(current.ToString()) && sb.Length == 0)
                {
                    continue;
                }
                else if (current == start)
                {
                    braces++;
                }
                else if (current == end)
                {
                    braces--;
                }

                sb.Append(current);
                if (braces == -1)
                {
                    if (current == end)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extracts the indexer portion (if any) of the given token.
        /// </summary>
        /// <returns>The indexer, if one is present; <see cref="Variable.Empty"/>, of not.</returns>
        internal static Variable GetIndexer(ref string token, Stack stack)
        {
            var start = token.IndexOf(Constants.START_INDEX);
            if (start <= 0)
            {
                return Variable.Empty;
            }

            var end = token.IndexOf(Constants.END_INDEX, start + 1);
            if (end <= start + 1)
            {
                return Variable.Empty;
            }

            var index = start;
            TryParseChars(token, ref index, Constants.START_ARG, Constants.START_INDEX);

            token = token.Substring(0, start);
            return Parser.Parse(token, ref index, stack, Variable.Empty, Constants.END_INDEX);
        }

        /// <summary>
        /// Parses the next variable, and advances the cursor.
        /// </summary>
        /// <returns>
        /// THe variable if one is successfully parsed, or <see cref="Variable.Empty"/> if none was
        /// found, or a syntax error is present.
        /// </returns>
        internal static Variable GetItem(string script, ref int index, Stack stack, Variable context)
        {
            // Consume the argument separator and/or a space.
            TryParseChars(script, ref index, Constants.ARG_SEPARATOR, ' ');
            TryParseChars(script, ref index, Constants.ARG_SEPARATOR, ' ');

            // Improperly formed list; return an empty argument.
            if (script.Length <= index)
            {
                return new Variable();
            }

            Variable value = null;

            if (script[index] == Constants.STRING_DELIMITER)
            {
                var token = GetNextToken(script, ref index, Constants.STRING_DELIMITER);
                value = new Variable(token.Substring(1, token.Length - 2));
            }
            else if (script[index] == Constants.START_INDEX)
            {
                index++; // Consume the starting delimiter.
                value = new Variable(GetArgs(script, ref index, stack, Constants.START_INDEX, Constants.END_INDEX));
            }
            else
            {
                value = Parser.Parse(script, ref index, stack, context, Constants.NEXT_OR_END_SECTION);
            }

            // Consume any trailing whitespace.
            TryParseChars(script, ref index, Constants.END_ARG, ' ');

            return value;
        }

        /// <summary>
        /// Consumes and returns the next token, up to the given terminator(s).
        /// </summary>
        /// <returns>
        /// The token obtained; or <see cref="string.Empty"/> if none was found or a syntax error is present.
        /// </returns>
        internal static string GetNextToken(string script, ref int index, params char[] ends)
        {
            if (ends.Length == 0)
            {
                ends = Constants.DELIMITERS;
            }

            // Consume any leading space, unless it is a terminator.
            if (!ends.Contains(' '))
            {
                TryParseChars(script, ref index, ' ');
            }

            // Consume any leading array delimiter.
            var isArray = TryParseChars(script, ref index, Constants.START_INDEX);

            // Consume any leading string delimiter.
            var isString = !isArray && TryParseChars(script, ref index, Constants.STRING_DELIMITER);

            char current = index < script.Length
                ? script[index]
                : Constants.EMPTY;
            char prev = index > 0
                ? script[index - 1]
                : Constants.EMPTY;

            int endIndex = script.IndexOfAny(ends, index);

            // Empty token, or invalid syntax.
            if (index >= endIndex)
            {
                // Consume the closing delimiter of an empty array.
                if (isArray)
                {
                    TryParseChars(script, ref index, Constants.END_INDEX);
                }
                // Consume the closing delimiter of an empty string.
                else if (isString)
                {
                    TryParseChars(script, ref index, Constants.STRING_DELIMITER);
                }
                return string.Empty;
            }

            // Skip arg separators in arrays.
            if (isArray)
            {
                while ((endIndex > 0 && script[endIndex] == Constants.ARG_SEPARATOR)
                    && endIndex + 1 < script.Length)
                {
                    endIndex = script.IndexOfAny(ends, endIndex + 1);
                }
            }
            // Skip escaped characters in strings.
            else if (isString)
            {
                while ((endIndex > 0 && script[endIndex - 1] == Constants.STRING_ESCAPE)
                    && endIndex + 1 < script.Length)
                {
                    endIndex = script.IndexOfAny(ends, endIndex + 1);
                }
            }

            // Empty token, or invalid syntax.
            if (endIndex < index)
            {
                return string.Empty;
            }

            // If the last consumed char was the string delimiter, rewind to avoid replacing it.
            if (script[endIndex - 1] == Constants.STRING_DELIMITER)
            {
                endIndex--;
            }

            // Get the token, and fix any double-escaped quotes.
            string token = script.Substring(index, endIndex - index).Replace("\\\"", "\"");

            index = endIndex;

            // Consume the closing delimiter for arrays.
            if (isArray)
            {
                TryParseChars(script, ref index, Constants.END_INDEX);
                token = $"[{token}]";
            }
            // Consume the closing delimiter for strings.
            else if (isString)
            {
                TryParseChars(script, ref index, Constants.STRING_DELIMITER);
                token = $"\"{token}\"";
            }

            // Consume any trailing space.
            TryParseChars(script, ref index, Constants.STRING_DELIMITER, ' ');

            return token;
        }

        /// <summary>
        /// Moves the cursor forward to the next statement.
        /// </summary>
        /// <returns>
        /// True if another statement exists; false if the end of the block has been reached.
        /// </returns>
        internal static bool NextStatement(string script, ref int index)
        {
            while (index < script.Length)
            {
                char currentChar = script[index];
                switch (currentChar)
                {
                    case Constants.END_BLOCK:
                        index++;
                        return false;
                    case Constants.START_BLOCK:
                    case Constants.STRING_DELIMITER:
                    case ' ':
                    case Constants.END_STATEMENT:
                    case Constants.END_ARG:
                        index++;
                        break;
                    default: return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Moves the cursor forward by one if the character under the cursor is one of the indicated expected characters.
        /// </summary>
        /// <returns>true if the cursor has moved forward; false otherwise.</returns>
        internal static bool TryParseChars(string script, ref int index, params char[] expected)
        {
            if (index < script.Length && expected.Contains(script[index]))
            {
                index++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the cursor backward by one if the character under the cursor is one of the indicated unexpected characters.
        /// </summary>
        /// <returns>true if the cursor has moved backward; false otherwise.</returns>
        internal static bool TryUnparseChars(string script, ref int index, params char[] unexpected)
        {
            if (index < script.Length && index > 0 && unexpected.Contains(script[index]))
            {
                index--;
                return true;
            }
            return false;
        }
    }
}
