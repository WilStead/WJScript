﻿using Microsoft.CodeAnalysis;
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
        public class DynamicGlobals { public object[] args; }

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
                var addedImports = args.Select(a => a.GetType().Namespace).Distinct();
                var addedAssemblies = args.Select(a => a.GetType().GetTypeInfo().Assembly).Distinct();
                var globals = new DynamicGlobals { args = args };

                result = await EvaluateAsync(source, globals, addedImports, addedAssemblies);
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

        private static async Task<object> EvaluateAsync(
            string source,
            object globals = null,
            IEnumerable<string> addedImports = null,
            IEnumerable<Assembly> addedAssemblies = null)
        {
            var imports = new List<string> { "System.Collections.Generic", "System.Linq", "System.Text", "UniversalVariable", "UniversalVariable.Math" };
            var assemblies = new List<Assembly>
            {
                typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).GetTypeInfo().Assembly,
                typeof(let).GetTypeInfo().Assembly
            };
            try
            {
                if (globals == null)
                {
                    return await CSharpScript.EvaluateAsync(
                        source,
                        ScriptOptions.Default.WithImports(imports).WithReferences(assemblies.ToArray()));
                }
                else
                {
                    imports = imports.Concat(addedImports).ToList();
                    assemblies = assemblies.Concat(addedAssemblies).ToList();
                    return await CSharpScript.EvaluateAsync(
                        source,
                        ScriptOptions.Default.WithImports(imports).WithReferences(assemblies.ToArray()),
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
