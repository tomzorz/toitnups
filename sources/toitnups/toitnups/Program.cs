using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace toitnups
{
    internal static class Program
    {
        private const string InitArg = "init";
        private const string PushArg = "push";
        private const string AddArg = "add";
        private const string RemoveArg = "remove";

        private const string TnFolder = ".tn";

        static void Main(string[] args)
        {
            // debug
            //args = new[] {"init"};
            args = new[] {"add", "signalr", "Plugins\\Signalr"};

            switch (args.Length)
            {
                case 1:
                    switch (args[0])
                    {
                        case InitArg:
                            Initialize();
                            return;
                        case PushArg:
                            Push();
                            return;
                        default:
                            DisplayHelp();
                            return;
                    }
                case 2:
                    switch (args[0])
                    {
                        case RemoveArg:
                            Remove(args[1]);
                            return;
                        default:
                            DisplayHelp();
                            return;
                    }
                case 3:
                    switch (args[0])
                    {
                        case AddArg:
                            Add(args[1], args[2]);
                            return;
                        default:
                            DisplayHelp();
                            return;
                    }
                default:
                    DisplayHelp();
                    return;
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine(" - USAGE -");
            Console.WriteLine();
            Console.WriteLine($"> {nameof(toitnups)} {InitArg}");
            Console.WriteLine("Checks for a valid Unity project and creates folder for integrations");
            Console.WriteLine();
            Console.WriteLine($"> {nameof(toitnups)} {AddArg} [integration name] [path to dlls inside Unity assets folder]");
            Console.WriteLine("Adds a new integration project");
            Console.WriteLine();
            Console.WriteLine($"> {nameof(toitnups)} {RemoveArg} [integration name]");
            Console.WriteLine("Removes an integration project (although you can just delete the folder...)");
            Console.WriteLine();
            Console.WriteLine($"> {nameof(toitnups)} {PushArg}");
            Console.WriteLine("Publishes the NuGet dlls to the target folder");
            Console.WriteLine();
        }

        private static void Initialize()
        {
            if (!Directory.Exists("Assets"))
            {
                Console.WriteLine("Couldn't find Assets folder, are you sure this is a Unity project's folder?");
                return;
            }

            if (!Directory.Exists("ProjectSettings"))
            {
                Console.WriteLine("Couldn't find ProjectSettings folder, are you sure this is a Unity project's folder?");
                return;
            }

            if (!File.Exists("ProjectSettings\\ProjectVersion.txt"))
            {
                Console.WriteLine("Couldn't find ProjectVersion file, are you sure this is a Unity project's folder?");
                return;
            }

            try
            {
                var version = int.Parse(File.ReadAllLines("ProjectSettings\\ProjectVersion.txt")[0].Split(':')[1].Trim().Split('.')[0]);
                if (version < 2018)
                {
                    Console.WriteLine("Unity version seems to be below 2018.1 which is not supported!");
                }
                else
                {
                    if (!Directory.Exists(TnFolder)) Directory.CreateDirectory(TnFolder);
                    Console.WriteLine("Initialization done.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong... details: " + e);
            }
        }

        private static bool CheckInit()
        {
            if (Directory.Exists(TnFolder)) return true;
            Console.WriteLine($"{nameof(toitnups)} folder missing, make sure you're running the command in the right folder and you've called \"{nameof(toitnups)} {InitArg}\"");
            return false;
        }

        private static bool ValidatePath(string path) => !Path.GetInvalidPathChars().Any(path.Contains);

        private static void Add(string s, string unityPath)
        {
            if (!CheckInit()) return;

            if (!ValidatePath(s))
            {
                Console.WriteLine("The supplied integration name is invalid");
                return;
            }

            if (!ValidatePath(unityPath))
            {
                Console.WriteLine("The supplied Unity path is invalid");
                return;
            }

            var dir = $"{TnFolder}\\integration.{s}";

            if (Directory.Exists(dir))
            {
                Console.WriteLine("Integration already exists with that name");
                return;
            }

            Directory.CreateDirectory(dir);

            var p = new Process
            {
                StartInfo = new ProcessStartInfo("dotnet")
                {
                    WorkingDirectory = dir,
                    Arguments = "new classlib"
                }
            };
            p.Start();
            p.WaitForExit();

            // todo clean unnecessary stuff

            var configPath = $"{dir}\\integration.{s}.config.json";
            var cfg = new Config
            {
                unityPath = unityPath
            };
            File.WriteAllText(configPath, JsonConvert.SerializeObject(cfg));

            Console.WriteLine("Created integration. Now you can open the .csproj file add your NuGet packages.");
        }

        private static void Remove(string s)
        {
            if (!CheckInit()) return;

            var dir = $"{TnFolder}\\integration.{s}";

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Couldn't find integration with the supplied name");
                return;
            }

            Directory.Delete(dir, true);
        }

        private static void Push()
        {
            if (!CheckInit()) return;

            // todo
        }
    }
}
