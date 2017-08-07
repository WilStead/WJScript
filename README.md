# WJScript
A general-purpose scripting language implemented in C#.

## What is WJScript?
WJScript is a [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) library which makes a general-purpose text script interpreter available in your .NET projects.

One key goal of WJScript is that it is intended to be error-proof. In other words: source code should always compiles, and always run without throwing any exceptions that could interrupt calling code.

Of course, a script may not do what it was intended to do, if there are syntax or logic errors in the source. But no matter what happens, the goal is that running the script should not cause problems for the code that calls it.

In order to help implement this desired behavior, WJScript is an unusual language in some respects. For instance, all operations are legal on and between all types, and have well-defined results.

Does your script add a boolean and a number, and divide the result by a string? Well, that isn't likely to generate very helpful results ... but it *will* compile, and run without errors!

## How do I get this?
Available as a [NuGet package](https://www.nuget.org/packages/WJScriptParser).

## Credit
The WJScript parser was based on [this article](http://www.codemag.com/article/1607081) by Vassili Kaplan, with many modifications.