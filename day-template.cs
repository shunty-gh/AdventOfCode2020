using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day00 : IAoCRunner
    {
        public int Day => 0;

        public async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await (useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day));
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }
            var part1 = 0;
            var part2 = 0;

            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");
        }
    }
}
