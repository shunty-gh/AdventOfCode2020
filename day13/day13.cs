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

            ShowResult(1, Part1(buses, est));
            ShowResult(2, Part2(buses));
        }

        private int Part1(IList<int> buses, int estimate)
        {
            var busid = 0;
            var earliest = int.MaxValue;
            foreach (var bus in buses)
            {
                if (bus == 0)
                    continue;

                var x = bus - (estimate % bus);
                if (x < earliest)
                {
                    busid = bus;
                    earliest = x;
                }
            }
            return busid * earliest;
        }

        private Int64 Part2(IList<int> buses)
        {
            var pairs = buses.Select((b, i) => (i,b)).Where(x => x.b != 0).ToList();

            Int64 t = buses[0], prev = buses[0], mul = 0, incr = buses[0];
            foreach (var (i, next) in pairs.Skip(1))
            {
                // Find match for this bus and previous result
                // Increment t by ((prev increment amount) * next bus id)
                // 'cos bus Ids are prime
                mul = 0;
                while (true)
                {
                    mul++;
                    var test = t + (mul * incr);
                    if ((test + i) % next == 0)
                    {
                        t = test;
                        incr = incr * next;
                        prev = next;
                        break;
                    }
                }
            }

            return t;
        }
    }
}
