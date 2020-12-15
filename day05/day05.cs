using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day05 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
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
            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
