using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day01 : IAoCRunner
    {
        public int Day => 1;

        public async Task Execute(ILogger logger)
        {
            var input = AoCUtils.GetDayLines(Day).Select(i => Int64.Parse(i)).ToList();
            Int64 part1 = 0, part2 = 0;
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (part1 == 0)
                    {
                        if (input[i] + input[j] == 2020)
                        {
                            part1 = input[i] * input[j];
                        }
                    }

                    if (part2 == 0)
                    {
                        for (var k = 0; k < input.Count; k++)
                        {
                            if (input[i] + input[j] + input[k] == 2020)
                            {
                                part2 = input[i] * input[j] * input[k];
                            }
                        }
                    }

                    if (part1 > 0 && part2 > 0)
                        break;
                }
            }

            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");

            await Task.CompletedTask;
        }
    }
}
