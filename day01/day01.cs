using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day01 : IAoCRunner
    {
        public int Day => 1;

        public async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = (await (useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day)))
                .Select(i => Int64.Parse(i));
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]: No {(useTestData ? "test " : "")}input available[/]");
                return;
            }

            Int64 part1 = 0, part2 = 0;
            foreach (var i in input)
            {
                foreach (var j in input)
                {
                    if (part1 == 0 && i + j == 2020)
                        part1 = i * j;

                    if (part2 == 0)
                    {
                        foreach (var k in input)
                        {
                            if (i + j + k == 2020)
                            {
                                part2 = i * j * k;
                                break;
                            }
                        }
                    }

                    if (part1 > 0 && part2 > 0)
                        break;
                }
                if (part1 > 0 && part2 > 0)
                    break;
            }

            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");
        }
    }
}
