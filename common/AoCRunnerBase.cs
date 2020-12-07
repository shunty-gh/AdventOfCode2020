using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public abstract class AoCRunnerBase : IAoCRunner
    {
        public int Day { get; }

        public AoCRunnerBase(int day = 0)
        {
            Day = day;
            if (day == 0)
            {
                // Try and work out the day number from the class name
                var name = this.GetType().Name;
                if (name.StartsWith("Day"))
                    Day = int.Parse(name.Substring(3));
            }
            if (Day == 0)
                throw new Exception($"Day number not set for class {this.GetType().Name}");
        }

        public abstract Task Execute(IConfiguration config, ILogger logger, bool useTestData);

        protected async Task<IList<string>> GetInputLines(bool useTestData = false)
        {
            var result = await (useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day));
            if (!result.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }
            return result;
        }

        protected void ShowResult<T>(int part, T result)
        {
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]{part}[/]:[/] [yellow]{result}[/]");
        }
    }
}
