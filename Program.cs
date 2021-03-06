﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    class Program
    {
        static IEnumerable<IAoCRunner> AvailableDays;
        static ILogger logger;

        private static IConfiguration _config;
        public static IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.private.json", optional: true)
                        .Build();
                }
                return _config;
            }
        }

        static async Task Main(string[] args)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                logger = InitialiseLogging();
                try
                {
                    AvailableDays = LoadDays().ToList();

                    await BuildCommandLine()
                        .UseDefaults()
                        .Build()
                        .InvokeAsync(args);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error running AoC with args: {@Args}", args);
                    AnsiConsole.WriteException(ex);
                }
                finally
                {
                    Serilog.Log.CloseAndFlush();
                }
            }
            finally
            {
                timer.Stop();
                AnsiConsole.MarkupLine($"[blue]All completed in [yellow]{timer.ElapsedMilliseconds}[/]ms[/]");
            }
        }

        /// <summary>
        /// The handler for the main RootCommand object
        /// </summary>
        private static async Task RunSelectedDays(IEnumerable<int> days, bool test, bool all)
        {
            // If "all" is true then create a list of all days up to the current day
            var dd = all
                ? Enumerable.Range(1, DateTime.Today.Month == 12 ? Math.Min(25, DateTime.Today.Day) : 25)
                : NormaliseDays(days);

            AnsiConsole.WriteLine();
            SpanglyTitle("Advent Of Code 2020");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold invert] Attempting to {(test ? "TEST" : "run")} day{(dd.Count() > 1 ? "s" : "")} {string.Join(",", dd)} [/]");
            AnsiConsole.WriteLine();
            foreach (var day in dd)
            {
                try
                {
                    // Find the appropriate IDayRunner, if one exists, and run it
                    var dr = AvailableDays?.FirstOrDefault(d => d.Day == day);
                    if (dr == null)
                    {
                        AnsiConsole.MarkupLine($"[red] Unable to find a class to execute for day [yellow]{day}[/][/]");
                        continue;
                    }

                    AnsiConsole.MarkupLine($"[white]-> Day [yellow]{day}[/][/]");
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        await dr.Execute(Configuration, logger, test);
                    }
                    finally
                    {
                        sw.Stop();
                        AnsiConsole.MarkupLine($" [lightsteelblue]Day [blue]{day}[/] completed in [yellow]{sw.ElapsedMilliseconds}[/]ms[/]");
                    }
                    AnsiConsole.WriteLine();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error running AoC day(s) {@DayNumbers}", dd);
                    AnsiConsole.MarkupLine($"[red] Error running day {day}[/]");
                    AnsiConsole.MarkupLine($"[red] {ex.Message}[/]");
                    //AnsiConsole.WriteException(ex);
                    AnsiConsole.WriteLine();
                }
            }
        }

        /// <summary>
        /// Return a list of created instances of all classes in this assembly that
        /// implement the IAoCRunner interface
        /// </summary>
        private static IEnumerable<IAoCRunner> LoadDays()
        {
            var result = new List<IAoCRunner>();
            // Get all types that support the IDayRunner interface except for the interface itself)
            var assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IAoCRunner).IsAssignableFrom(type) && type != typeof(IAoCRunner) && !type.IsAbstract && type != typeof(Day00))
                {
                    if (Activator.CreateInstance(type) is IAoCRunner dr)
                    {
                        result.Add(dr);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Remove duplicates and/or provide sensible defaults for the list
        /// of day numbers
        /// </summary>
        private static IEnumerable<int> NormaliseDays(IEnumerable<int> source)
        {
            // Make sure we have a valid list of days. Default to the current day
            // if we are in December or 25th if not.

            if (source == null || !source.Any())
                return new List<int> { DateTime.Today.Month == 12 ? Math.Min(25, DateTime.Today.Day) : 25 };

            return source
                .Where(d => d <= 25)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        /// <summary>
        /// Create the basic structure of command line arguments
        /// </summary>
        private static CommandLineBuilder BuildCommandLine() =>
            new CommandLineBuilder(CreateRootCommand());

        /// <summary>
        /// Create the main RootCommand command line argument class
        /// </summary>
        private static RootCommand CreateRootCommand()
        {
            var rootcmd = new RootCommand("Run solutions to the AdventOfCode problems for selected days.\r\neg dotnet run -- --days 1 2 3\r\nor dotnet run -- -a")
            {
                new Option<bool>(
                    new string[] { "--all", "-a" },
                    "Try and run all days up to and including the current day"),
                new Option<IEnumerable<int>>(
                    new string[] { "--days", "-d" },
                    "Specify which days to run (from 1 to 25) separated by space(s)"),
                new Option<bool>(
                    new string[] { "--test", "-t" },
                    "Run with test data, if available"),
            };
            rootcmd.Handler = CommandHandler.Create<List<int>, bool, bool>(RunSelectedDays);
            return rootcmd;
        }

        /// <summary>
        /// Output the given text to the console in random ANSI colouring
        /// </summary>
        private static void SpanglyTitle(string title)
        {
            //var colours = new string[] { "red", "green", "yellow", "blue", "purple" };
            var colours = new string[] { "red", "orange1", "yellow", "green", "blue", "purple", "violet" };
            var rnd = new Random();
            foreach(var c in $" * * * {title} * * * ")
            {
                var colour = colours[rnd.Next(colours.Length)];
                AnsiConsole.Markup($"[bold {colour}]{c}[/]");
            }
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// Set up logging to Serilog
        /// </summary>
        private static Serilog.ILogger InitialiseLogging()
        {
            const string SeqLocal = "http://localhost:5341";

            var sconfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                //.WriteTo.Console()
                .WriteTo.Udp("localhost", 7071, System.Net.Sockets.AddressFamily.InterNetwork, outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
                .WriteTo.Seq(SeqLocal);

            Serilog.Log.Logger = sconfig.CreateLogger();
            return Serilog.Log.Logger;
        }

    }
}
