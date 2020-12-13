using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day13 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var est = int.Parse(input[0]);
            var buses = input[1].Split(',')
                .Select(s => s == "x" ? 0 : int.Parse(s))
                .ToList();

            var busid = 0;
            var earliest = int.MaxValue;
            foreach (var bus in buses)
            {
                if (bus == 0)
                    continue;

                var x = bus - (est % bus);
                if (x < earliest)
                {
                    busid = bus;
                    earliest = x;
                }
            }
            var part1 = busid * earliest;
            var part2 = 0;

            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
