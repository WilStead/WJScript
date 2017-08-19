using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UniversalVariable;

namespace WJScriptParser
{
    public static class Parser
    {
        public class DynamicGlobals { public let[] args; }

        public static object Evaluate(string source, params object[] args)
        {
            var task = EvaluateAsync(source, args);
            task.Wait();
            return task.Result;
        }

        public static async Task<object> EvaluateAsync(string source, params object[] args)
        {
            object result;
            if (args.Length == 0)
            {
                result = await EvaluateAsync(source);
            }
            else
            {
                var globals = new DynamicGlobals { args = args.Select(a => new let(a)).ToArray() };

                result = await EvaluateAsync(source, globals);

                for (int i = 0; i < args.Length; i++)
                {
                    var type = args[i].GetType();
                    if (!type.GetTypeInfo().IsValueType || type.IsByRef)
                    {
                        if (globals.args[i].PropertyCount == 0)
                        {
                            args[i] = globals.args[i].Value;
                        }
                        else
                        {
                            var arg = globals.args[i].ToDynamic();
                            if ((arg as IDictionary<string, object>).TryGetValue("Value", out var value))
                            {
                                args[i] = value;
                            }
                            else
                            {
                                foreach (var prop in arg as IDictionary<string, object>)
                                {
                                    var argField = type.GetRuntimeField(prop.Key);
                                    if (argField == null)
                                    {
                                        type.GetRuntimeProperty(prop.Key).SetValue(args[i], prop.Value);
                                    }
                                    else
                                    {
                                        argField.SetValue(args[i], prop.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (result != null && result.GetType() == typeof(let))
            {
                return (result as let).Value;
            }
            else
            {
                return result;
            }
        }

        private static async Task<object> EvaluateAsync(string source, object globals = null)
        {
            try
            {
                if (globals == null)
                {
                    return await CSharpScript.EvaluateAsync(
                        source,
                        ScriptOptions.Default
                            .WithImports("System.Collections.Generic", "System.Linq", "System.Text", "UniversalVariable", "UniversalVariable.Math")
                            .WithReferences(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).GetTypeInfo().Assembly, typeof(let).GetTypeInfo().Assembly));
                }
                else
                {
                    return await CSharpScript.EvaluateAsync(
                        source,
                        ScriptOptions.Default
                            .WithImports("System.Collections.Generic", "System.Linq", "System.Text", "UniversalVariable", "UniversalVariable.Math")
                            .WithReferences(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).GetTypeInfo().Assembly, typeof(let).GetTypeInfo().Assembly),
                        globals, typeof(DynamicGlobals));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
