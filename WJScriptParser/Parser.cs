using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WScriptParser.Functions;

namespace WScriptParser
{
    public static class Parser
    {
        private static (Variable Variable, IAction Action) CombineTokens(
            (Variable Variable, IAction Action) current,
            ref int index,
            List<(Variable Variable, IAction Action)> tokens,
            bool single = false)
        {
            var result = current;

            while (index < tokens.Count
                && result.Variable.Type != Variable.VariableType.Break
                && result.Variable.Type != Variable.VariableType.Continue)
            {
                (Variable Variable, IAction Action) next = tokens[index++];

                while (result.Action.Priority < next.Action.Priority)
                {
                    next = CombineTokens(next, ref index, tokens, single: true);
                }

                result.Variable = result.Action.Combine(result.Variable, next.Variable);
                result.Action = next.Action;

                if (single
                    || result.Variable.Type == Variable.VariableType.Break
                    || result.Variable.Type == Variable.VariableType.Continue)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a raw string of source code into valid script. Invalid input results in an empty script, not an error.
        /// </summary>
        public static string Compile(string source)
        {
            StringBuilder sb = new StringBuilder(source.Length);

            // Non-space whitespace is converted to spaces.
            // The two-word "else if" is converted to a single token for easier parsing.
            source = source.Replace('\t', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace("else if", "elseif");

            bool comment = false;
            bool quote = false;
            char prev = Constants.EMPTY;
            int parens = 0;
            int blocks = 0;
            int indexes = 0;

            for (int i = 0; i < source.Length; i++)
            {
                char current = source[i];
                char next = i + 1 < source.Length
                    ? source[i + 1]
                    : Constants.EMPTY;

                // Comments are stripped from compiled script.
                if (comment)
                {
                    if (current == '\n')
                    {
                        comment = false;
                    }
                    continue;
                }

                if (Constants.STRING_DELIMITERS.Contains(current))
                {
                    if (prev != Constants.STRING_ESCAPE)
                    {
                        quote = !quote;
                    }

                    // The valid string delimiters are standardized in compiled code.
                    sb.Append(Constants.STRING_DELIMITER);

                    prev = current;
                    continue;
                }

                switch (current)
                {
                    case Constants.COMMENT:
                        if (!quote && next == Constants.COMMENT)
                        {
                            comment = true;
                            continue;
                        }
                        break;
                    case ' ':
                        // Extra spaces, and spaces around delimiters are stripped from compiled code,
                        // except in strings, and when it would concatenate two actions (e.g. a + ++b => a+++b).
                        if (!quote
                            && (next == ' ' || prev == ' '
                            || sb.Length == 0 || i == source.Length - 1
                            || Constants.DELIMITERS.Contains(next)
                            || Constants.DELIMITERS.Contains(prev))
                            && (!Constants.ACTIONS.Any(a => a[0] == next)
                            || !Constants.ACTIONS.Any(a => a[a.Length - 1] == prev)))
                        {
                            continue;
                        }
                        break;
                    case Constants.END_ARG:
                        if (!quote)
                        {
                            parens--;
                        }
                        break;
                    case Constants.START_ARG:
                        if (!quote)
                        {
                            parens++;
                        }
                        break;
                    case Constants.END_BLOCK:
                        if (!quote)
                        {
                            blocks--;
                        }
                        break;
                    case Constants.START_BLOCK:
                        if (!quote)
                        {
                            blocks++;
                        }
                        break;
                    case Constants.START_INDEX:
                        if (!quote)
                        {
                            indexes--;
                        }
                        break;
                    case Constants.END_INDEX:
                        if (!quote)
                        {
                            indexes++;
                        }
                        break;
                    default: break;
                }
                sb.Append(current);
                prev = current;
            }

            // Uneven brackets results in an empty script.
            if (parens != 0 || blocks != 0 || indexes != 0)
            {
                return string.Empty;
            }

            return sb.ToString();
        }

        private static string GetAction(string script, int index)
        {
            if (index < 0 || index >= script.Length)
            {
                return null;
            }

            var search = script.Substring(index);

            // Prefer the longest match, to avoid returning = when == is present.
            var match = Constants.ACTIONS
                .Where(a => search.StartsWith(a))
                .OrderByDescending(a => a.Length)
                .FirstOrDefault();
            if (!string.IsNullOrEmpty(match))
            {
                return match;
            }

            return null;
        }

        private static string GetNextAction(string script, ref int index, params char[] ends)
        {
            if (index >= script.Length
                || script[index] == Constants.END_ARG
                || ends.Contains(script[index]))
            {
                return null;
            }

            int startIndex = index;

            string action = GetAction(script, startIndex);
            while (action == null
                && startIndex < script.Length
                && script[startIndex] == Constants.END_ARG)
            {
                action = GetAction(script, ++startIndex);
            }

            if (action != null)
            {
                index += action.Length + Math.Max(0, startIndex - index);

                Utilities.TryParseChars(script, ref index, ' ');
            }
            return action;
        }

        private static bool IsTokenComplete(string script, int index, string token, ref string action, params char[] ends)
        {
            char current = index > 0
                ? script[index - 1]
                : Constants.EMPTY;
            char next = index < script.Length
                ? script[index]
                : Constants.EMPTY;
            char prev = index > 1
                ? script[index - 2]
                : Constants.EMPTY;

            if (ends.Contains(next)
                || Constants.TOKEN_DELIMITERS.Contains(current)
                || Constants.TOKEN_DELIMITERS.Contains(next))
            {
                return true;
            }

            // Negative number.
            if (token.Length == 1 && current == '-' && next != '-')
            {
                return false;
            }

            // Scientific notation.
            if (token.Length > 1
                && char.ToUpper(prev) == 'E'
                && (current == '-' || current == '+' || char.IsDigit(current))
                && char.IsDigit(token[token.Length - 2]))
            {
                return false;
            }

            // Numeric separator (rather than property separator).
            if (token.Length >= 1
                && next == '.'
                && int.TryParse(token, out var prevInt))
            {
                return false;
            }

            // A space or an action signals the end of the current token.
            if (next == ' ' || (action = GetAction(script, index)) != null)
            {
                return true;
            }

            return false;
        }

        internal static Variable Parse(string script, ref int index, Stack stack, Variable context, params char[] ends)
        {
            if (ends.Length == 0)
            {
                ends = Constants.END_SECTION;
            }

            var tokens = Tokenize(script, ref index, stack, context, ends);

            if (tokens.Count == 0)
            {
                return new Variable();
            }

            if (tokens.Count == 1)
            {
                return tokens[0].Variable;
            }

            int i = 1;
            return CombineTokens(tokens[0], ref i, tokens).Variable;
        }

        /// <summary>
        /// Executes the given script and returns the result.
        /// </summary>
        /// <param name="script">The script to be executed.</param>
        /// <param name="precompiled">
        /// If true, the compilation step is skipped (can produce unexpected results if the script
        /// hasn't been previously compiled).
        /// </param>
        /// <returns>
        /// The return value. May be null, a boolean, a double, or a string, or a dictionary of
        /// string keys with values of any of these types.
        /// </returns>
        public static object Execute(string script, bool precompiled = false)
        {
            var stack = new Stack();

            if (!precompiled && !string.IsNullOrWhiteSpace(script))
            {
                script = Compile(script);
            }
            if (string.IsNullOrWhiteSpace(script))
            {
                return null;
            }

            int index = 0;
            Variable result = null;
            while (index < script.Length)
            {
                result = Parse(script, ref index, stack, Variable.Empty);
                Utilities.NextStatement(script, ref index);
            }

            result = result ?? Variable.Empty;

            return result.ToValue();
        }

        internal static Variable ProcessBlock(string script, ref int index, Stack stack, Variable context)
        {
            Variable result = null;
            while (index < script.Length)
            {
                if (!Utilities.NextStatement(script, ref index)
                    || index >= script.Length)
                {
                    return result ?? new Variable();
                }

                result = Parse(script, ref index, stack, context);

                if (result.Type == Variable.VariableType.Break
                    || result.Type == Variable.VariableType.Continue)
                {
                    return result;
                }
            }
            return result;
        }

        internal static void SkipBlock(string script, ref int index)
        {
            int openBraceCount = 0;
            int closeBraceCount = 0;
            while (openBraceCount == 0 || openBraceCount > closeBraceCount)
            {
                // Mismatched braces; should be caught and omitted during compilation.
                if (index >= script.Length)
                {
                    return;
                }
                char current = script[index++];
                switch (current)
                {
                    case Constants.START_BLOCK:
                        openBraceCount++;
                        break;
                    case Constants.END_BLOCK:
                        closeBraceCount++;
                        break;
                }
            }
        }

        private static List<(Variable Variable, IAction Action)> Tokenize(string script, ref int index, Stack stack, Variable context, params char[] ends)
        {
            var tokens = new List<(Variable Variable, IAction Action)>(16);

            if (index >= script.Length || ends.Contains(script[index]))
            {
                tokens.Add((new Variable(), Stack.NoOp));
                return tokens;
            }

            var tokenBuilder = new StringBuilder();
            var negation = 0;
            var quote = false;

            do
            {
                var not = false;
                if (!quote && script.Substring(index).StartsWith(Constants.NOT))
                {
                    negation++;
                    index += Constants.NOT.Length;
                    if (tokenBuilder.Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        not = true;
                    }
                }

                if (!not)
                {
                    tokenBuilder.Append(script[index++]);
                }

                quote =
                    !not
                    && tokenBuilder[tokenBuilder.Length - 1] == Constants.STRING_DELIMITER
                    && (index == 1 || script[index - 2] != Constants.STRING_ESCAPE)
                    ? !quote
                    : quote;

                string action = null;
                if (!not
                    && (quote
                    || (!IsTokenComplete(script, index, tokenBuilder.ToString(), ref action, ends)
                    && index < script.Length)))
                {
                    continue;
                }

                var token = tokenBuilder.ToString();

                // Probable missing statement end. Rewind.
                if (Constants.CONTROL_STATEMENTS.Contains(token) && tokens.Count > 0)
                {
                    index -= token.Length;
                    break;
                }

                if (action != null)
                {
                    index += action.Length;
                }

                Utilities.TryParseChars(script, ref index, ' ');

                var function = stack.ParseFunction(script, ref index, token, ref action);
                var variable = function.Evaluate(script, ref index, stack, context);

                if (function == Stack.IdentityFunction)
                {
                    Utilities.TryParseChars(script, ref index, Constants.END_ARG);
                }

                // Booleans and numbers can be negated. other types ignore negation.
                if (negation > 0)
                {
                    if (variable.Type == Variable.VariableType.Boolean)
                    {
                        variable.Boolean = !((negation % 2 == 0) ^ variable.Boolean);
                        negation = 0;
                    }
                    else if (variable.Type == Variable.VariableType.Number)
                    {
                        variable = new Variable(!((negation % 2 == 0) ^ Convert.ToBoolean(variable.Value)));
                        negation = 0;
                    }
                }

                if (action == null)
                {
                    action = GetNextAction(script, ref index, ends);
                }

                var next = index < script.Length
                    ? script[index]
                    : Constants.EMPTY;
                if (tokens.Count == 0
                    && (next == Constants.END_STATEMENT
                    || index >= script.Length
                    || (variable.Type != Variable.VariableType.Number
                    && action == null)))
                {
                    tokens.Add((variable, Stack.NoOp));
                    return tokens;
                }

                tokens.Add((variable, Stack.GetAction(action)));
                tokenBuilder.Clear();
            } while (index < script.Length && (quote || !ends.Contains(script[index])));

            return tokens;
        }
    }
}
