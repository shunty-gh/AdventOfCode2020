using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day06 : IAoCRunner
    {
        public int Day => 6;

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
            int part1 = 0, part2 = 0, lineindex = 0, maxindex = input.Count;
            var current1 = new List<char>();
            var current2 = new List<char>();
            var newblock = true;
            while (lineindex < maxindex)
            {
                var line = input[lineindex++];
                current1.AddRange(line.ToCharArray());

                if (!string.IsNullOrWhiteSpace(line))
                {
                    current2 = (newblock
                        ? line.ToCharArray()
                        : current2.Intersect(line.ToCharArray()))
                        .ToList();
                    newblock = false;
                }
                if (string.IsNullOrWhiteSpace(line) || lineindex >= maxindex)
                {
                    part1 += current1.Distinct().Count();
                    current1.Clear();

                    part2 += current2.Count;
                    newblock = true;
                }
            }
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");
        }
    }
}
