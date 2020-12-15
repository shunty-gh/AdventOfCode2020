using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day02 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
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
            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
