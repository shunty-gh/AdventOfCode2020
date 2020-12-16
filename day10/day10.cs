using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day10 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = (await GetInputInts(useTestData))
                .Append(0).OrderBy(i => i).ToList();

            ShowResult(1, GetCounts(input));
            ShowResult(2, FindArrangements(input));
        }

        private int GetCounts(IList<int> items)
        {
            // Assume items is sorted
            int ones = 0, threes = 1; // Add a 3 for the final charger-to-device connection
            for (var i = 1; i < items.Count; i++)
            {
                ones += items[i] == items[i - 1] + 1 ? 1 : 0;
                threes += items[i] == items[i - 1] + 3 ? 1 : 0;
            }
            return ones * threes;
        }

        private Int64 FindArrangements(IList<int> items)
        {
            // Assume items is sorted
            var possiblePaths = new Int64[items.Count];
            possiblePaths[0] = 1;
            var icount = items.Count;

            for (var i = 0; i < icount; i++)
            {
                var pcount = possiblePaths[i];
                // Find paths from here to next possible candidates
                for (var j = 1; j <= 3; j++)
                {
                    if (i+j >= icount || items[i+j] > items[i] + 3)
                        break;
                    possiblePaths[i+j] += pcount;
                }
            }
            return possiblePaths[icount - 1];
        }
    }
}
