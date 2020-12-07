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
            var input = await (useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day));
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }

            var seats = input.Select(l =>
                Convert.ToInt32(l.Replace('F', '0')
                    .Replace('B', '1')
                    .Replace('L', '0')
                    .Replace('R', '1'), 2)
                )
                .OrderBy(i => i)
                .ToList();
            var part1 = seats.Max();
            int part2 = 0, lastseat = 0;
            foreach (var seat in seats)
            {
                if (lastseat + 2 == seat)
                {
                    part2 = seat - 1;
                    break;
                }
                lastseat = seat;
            }
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");
        }
    }
}
