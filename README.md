# WJScript
A general-purpose scripting language implemented in C#.

## What is WJScript?
WJScript is a [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) library which makes a general-purpose text script interpreter available in your .NET projects.

One key goal of WJScript is that it is intended to be error-proof. In other words: a script should always run without throwing any exceptions that could interrupt the calling code.

Of course, a script may not do what it was intended to do, if there are syntax or logic errors in the source. But no matter what happens, the goal is that running the script should not cause problems for the code that calls it.

In order to help implement this desired behavior, WJScript is an unusual language in some respects. For instance, all operations are legal on and between all types, and have well-defined results.

Does your script add a boolean and a number, and divide the result by a string? Well, that isn't likely to generate very helpful results ... but it *will* compile, and run without errors!

The current version of WJScript is run under the hood by the Roslyn script engine, and as such is actually C#, but by making use of my [UniversalVariable library](https://github.com/WilStead/UniversalVariable) the syntax and behavior can seem quite different. Take this example:
```c#
let x = 42;
x["newProp"] = "Hello, ";
let y = x["newProp"] + "World!";
x = false || y;
return x; // "Hello, World!"
```

## How do I get this?
Available as a [NuGet package](https://www.nuget.org/packages/WJScriptParser).

## Can I contribute?
Yes! [Pull requests](https://help.github.com/articles/about-pull-requests/) are welcome for bug fixes and/or new features.

Please add unit tests for any new feature.

## Help! It's not working!
Feel free to submit an [Issue](https://help.github.com/articles/about-issues/) if something isn't working right. Please refer to the [project wiki](https://github.com/WilStead/WJScript/wiki), as well as the [wiki for UniversalVariable](https://github.com/WilStead/UniversalVariable/wiki) to verify that your trouble is due to an actual bug, and not simply a design limitation of the libraries.
