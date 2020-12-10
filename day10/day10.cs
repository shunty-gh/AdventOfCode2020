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
            var possiblePaths = new Dictionary<int, Int64> { { items.Min(), 1 } };
            foreach (var item in items.Skip(1))
            {
                // Find items that could preceed this one and sum their paths
                var possibles = items.Where(i => (item > i) && (item <= i + 3));
                possiblePaths[item] = possibles.Sum(p => possiblePaths[p]);
            }
            return possiblePaths[items.Max()];
        }
    }
}
