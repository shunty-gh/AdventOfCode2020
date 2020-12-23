using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day23 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = (await GetInputLines(useTestData))
                .First()
                .Select(c => (int)Char.GetNumericValue(c))
                .ToArray();

            Func<int[], int, int> IndexOfDest = (a, d) => {
                for (var i = 0; i < a.Length; i++)
                {
                    if (a[i] == d)
                        return i;
                }
                return -1; // Should never happen
            };
            var a1 = input.ToArray();
            var a2 = new int[a1.Length * 2];
            var next3 = new int[3];
            int ci = 0, di = 0, curr = 0, dest = 0;
            for (var move = 1; move <= 100; move++)
            {
                curr = a1[ci];
                for (var n = 0; n < 3; n++)
                {
                    var ni = (ci + 1 + n) % 9;
                    next3[n] = a1[ni];
                    a1[ni] = 0;
                }
                dest = curr > 1 ? curr - 1 : 9;
                while (next3.Contains(dest))
                {
                    dest--;
                    if (dest <= 0)
                        dest = 9;
                }
                di = IndexOfDest(a1, dest);
                // Rewrite the array
                a2[0] = dest;
                a2[1] = next3[0];
                a2[2] = next3[1];
                a2[3] = next3[2];
                var ai = (di + 1) % 9;
                for (var i = 4; i < a1.Length; i++)
                {
                    while (a1[ai] == 0)
                        ai = (ai + 1) % 9;
                    a2[i] = a1[ai];
                    ai = (ai + 1) % 9;
                }
                // Swap them over
                Array.Copy(a2, a1, a1.Length);
                // Find the new current cup
                ci = (IndexOfDest(a1, curr) + 1) % 9;
                //Console.WriteLine(string.Join(", ", a1) + $"; Curr: {a1[ci]}");
            }

            var i1 = IndexOfDest(a1, 1);
            var part1 = "";
            for (var i = 1; i < 9; i++)
            {
                part1 += a1[(i1 + i) % 9];
            }
            var part2 = 0;

            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
