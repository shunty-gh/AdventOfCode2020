using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day06 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines();
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
            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
