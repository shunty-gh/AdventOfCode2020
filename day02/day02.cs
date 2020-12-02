using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day02 : IAoCRunner
    {
        public int Day => 2;

        public async Task Execute(ILogger logger)
        {
            var input = AoCUtils.GetDayLines(Day);
            int part1 = 0, part2 = 0;

            foreach (var line in input)
            {
                var split = line.Split(' ');
                var pwd = split[2];
                var ch = split[1][0];
                var rules = split[0].Split('-');
                var rulemin = int.Parse(rules[0]);
                var rulemax = int.Parse(rules[1]);

                // Part 1
                var chcount = pwd.Count(c => c == ch);
                if (chcount >= rulemin && chcount <= rulemax)
                {
                    part1++;
                }

                // Part 2
                var c1 = pwd[rulemin - 1];
                var c2 = pwd[rulemax - 1]; // Should really do a length check first. Assume we don't need to.
                if (c1 != c2 && (c1 == ch || c2 == ch))
                {
                    part2++;
                }
            }
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");

            await Task.CompletedTask;
        }
    }
}
