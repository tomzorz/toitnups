# toitnups
**T**ool f**O**r un**IT**y **NU**get **P**ackage**S** - use a "dummy" .NET Standard 2.0 library to pull the required NuGet libraries and their dependencies together and move them over to your Unity project.

![platform-any](https://img.shields.io/badge/platform-any-green.svg?longCache=true&style=flat-square) ![nuget-yes](https://img.shields.io/badge/nuget-yes-green.svg?longCache=true&style=flat-square) ![license-MIT](https://img.shields.io/badge/license-MIT-blue.svg?longCache=true&style=flat-square)

You must have [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/windows) or higher installed.

⚠ 👉 Yes, the **SDK**, not just the runtime, as toitnups relies on the `dotnet ...` commands.

## Use the pre-built `toitnups`

You can quickly install and try [toitnups from nuget.org](https://www.nuget.org/packages/toitnups/) using the following commands:

```console
dotnet tool install -g toitnups
    [navigate to your Unity project directory]
toitnups init
toitnups add sample Plugins\Sample
    [add your nuget packages]
toitnups push
```

> Note: You may need to open a new command/terminal window the first time you install the tool.

## How does it work?

### 1️⃣ `init` makes sure everything is in the right place

This wouldn't be strictly necessary, but I think it's best to validate the project version and folder structure. If everything is in order the `.tn` folder is created.

### 2️⃣ `add` adds a "dummy" project and some congfiguration

The dotnet toolchain runs to create a .NET Standard 2.0 project and a config file is created that stores the location of the plugins inside the `Assets` folder.

### 3️⃣ you open the `integration.NAME.csproj` file that was just created

... and add the needed NuGet references via Visual Studio as you normally would.

### 4️⃣ `push` migrates the required files to the Unity project

The dotnet toolchain runs again, publishing the project in release mode. This copies all the necessary files to one folder. Then these files are gathered, copied over to the target folder, and the `link.xml` file is created/updated, ensuring that none of the code is removed by the sometimes too eager IL2CPP backend.

## Anything else?

### There's also a `remove` command...

It removes an integration, essentially the opposite of `add`.

⚠ 👉 Important to note, that this does not remove any files from the Unity project.

## Why not use the NuGet package manager from the Asset Store or other method XYZ?

There are multiple factors here.

1. The asset store NuGet package manager is separately maintained/implemented, therefore it's always going to lag behind any official implementation and have more bugs/issues.
2. I strongly believe that Unity's way to extend the editor is wrong. I shouldn't have to add assets/packages to my *project* to extend the *editor*. 
3. This tool automates the hurdles with `link.xml`. 

## Future features

See [issues tagged with enhancements](https://github.com/tomzorz/toitnups/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement).
