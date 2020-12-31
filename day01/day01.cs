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
                .Select(i => int.Parse(i))
                .OrderBy(i => i)
                .ToList();

            Int64 part1 = 0, part2 = 0;
            int icount = input.Count, i = 0, j = 0, k = 0, ij = 0, ijk = 0;
            for (var ii = 0; ii < icount - 1 && (part1 == 0 || part2 == 0); ii++)
            {
                i = input[ii];
                for (var jj = ii + 1; jj < icount && (part1 == 0 || part2 == 0); jj++)
                {
                    j = input[jj];
                    ij = i + j;

                    if (ij > 2020)
                        break;

                    if (part1 == 0 && ij == 2020)
                        part1 = i * j;

                    if (part2 == 0)
                    {
                        for (var kk = jj + 1; kk < icount; kk++)
                        {
                            k = input[kk];
                            ijk = ij + k;
                            if (ijk > 2020)
                                break;
                            if (ijk == 2020)
                            {
                                part2 = i * j * k;
                                break;
                            }
                        }
                    }
                }
            }

            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
