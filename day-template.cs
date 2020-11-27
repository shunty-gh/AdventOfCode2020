using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day00 : IAoCRunner
    {
        public int Day => 0;

        public async Task Execute(ILogger logger)
        {
            var input = AoCUtils.GetDayLines(Day);
            var part1 = 0;
            var part2 = 0;

            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");

            await Task.CompletedTask;
        }
    }
}
