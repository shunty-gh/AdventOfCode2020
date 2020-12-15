using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day15 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            // Test input
            //var input = new List<int> {0,3,6}; // P1 = 436; P2 = 175594
            //var input = new List<int> {1,3,2}; // P1 = 1
            //var input = new List<int> {2,1,3}; // P1 = 10; P2 = 3544142
            //var input = new List<int> {1,2,3}; // P1 = 27
            //var input = new List<int> {3,1,2}; // P1 = 1836; P2 = 362
            // Real input
            var input = new List<int> {15,5,1,4,7,0};
            var previous = new int[30_000_000]; // Array is much quicker than the Dictionary<> approach
            // var previous = new Dictionary<int,int>();
            for (var i = 0; i < input.Count - 1; i++) // don't add the last item
            {
                previous[input[i]] = i + 1;
            }
            int part1 = 0, lastturn = input.Count, lastspoken = input.Last(), prevspoken = 0;

            while (lastturn < 30_000_000)
            {
                //var alreadyspoken = previous.TryGetValue(lastspoken, out prevspoken);
                prevspoken = previous[lastspoken];
                previous[lastspoken] = lastturn;
                lastspoken = prevspoken > 0 ? lastturn - prevspoken : 0;

                lastturn++;
                if (lastturn == 2020)
                {
                    part1 = lastspoken;
                }
            }

            ShowResult(1, part1);
            ShowResult(2, lastspoken);
            await Task.CompletedTask;
        }
    }
}
