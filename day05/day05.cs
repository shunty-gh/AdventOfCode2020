using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day05 : IAoCRunner
    {
        public int Day => 5;

        public async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            await AoCUtils.GetDayInput(config, Day, "");
            var input = useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day);
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }
            var part1 = 0;
            var part2 = 0;

            part1 = input.Max(l =>
                Convert.ToInt32(l.Replace('F', '0')
                    .Replace('B', '1')
                    .Replace('L', '0')
                    .Replace('R', '1'), 2)
            );
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");

            await Task.CompletedTask;
        }
    }
}
