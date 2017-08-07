﻿using System;
using System.Linq;

namespace WScriptParser.Functions
{
    public class Atan : IFunction
    {
        public string Name => "Atan";

        public Variable Evaluate(string script, ref int index, Stack stack, Variable context)
            => new Variable(Utilities.GetArgs(script, ref index, stack)
                .Select(a => Math.Atan(a.ToDouble())).Sum());
    }
}
