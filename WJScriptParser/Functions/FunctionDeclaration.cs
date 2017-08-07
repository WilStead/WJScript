using System;
using System.Collections.Generic;

namespace WScriptParser.Functions
{
    public class FunctionDeclaration : IFunction
    {
        public string Name => Constants.FUNCTION;

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
        {
            string functionName = Utilities.GetNextToken(script, ref index);

            string[] args = new string[0];
            if (Utilities.TryParseChars(script, ref index, Constants.START_ARG, ' '))
            {
                int endArgIndex = script.IndexOf(Constants.END_ARG, index);
                if (endArgIndex >= 0)
                {
                    string[] rawArgs = script.Substring(index, endArgIndex - index)
                        .Split(Constants.ARG_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                    var tmp = new List<string>();
                    for (int i = 0; i < rawArgs.Length; i++)
                    {
                        var a = rawArgs[i].Trim();
                        if (!string.IsNullOrEmpty(a))
                        {
                            tmp.Add(a);
                        }
                    }
                    if (tmp.Count > 0)
                    {
                        args = tmp.ToArray();
                    }

                    index = endArgIndex + 1;
                }
            }

            Utilities.TryParseChars(script, ref index, Constants.START_BLOCK, ' ');

            var body = Utilities.GetBody(script, ref index, Constants.START_BLOCK, Constants.END_BLOCK);

            stack.PushLocal(new CustomFunction(functionName, body, args));

            return new Variable(functionName);
        }
    }
}
