using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day01 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = (await GetInputLines(useTestData))
                .Select(i => Int64.Parse(i));

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

            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
