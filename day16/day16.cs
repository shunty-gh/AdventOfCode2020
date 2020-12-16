using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day16 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var part1 = 0;
            var part2 = 0;

            var fields = new Dictionary<string, int[]>();
            int[] myticket;
            var nearbytickets = new List<int[]>();
            var getnearby = false;
            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.StartsWith("your ticket"))
                {
                    i++;
                    myticket = input[i].Split(',').Select(x => int.Parse(x)).ToArray();
                    continue;
                }
                if (line.StartsWith("nearby"))
                {
                    getnearby = true;
                    continue;
                }
                if (getnearby)
                {
                    var ticket = input[i].Split(',').Select(x => int.Parse(x)).ToArray();
                    nearbytickets.Add(ticket);
                }
                else
                {
                    var splits = line.Split(": ");
                    var nums = splits[1].Split(" or ").SelectMany(s => s.Split('-'));
                    fields[splits[0]] = nums.Select(n => int.Parse(n)).ToArray();
                }

            }
            var invalid = new List<int>();
            var maxfield = fields.Max(f => f.Value.Max());
            var fv = new int[maxfield + 1];
            foreach (var f in fields.Values)
            {
                for (var j = f[0]; j <= f[1]; j++)
                {
                    fv[j] = 1;
                }
                for (var j = f[2]; j <= f[3]; j++)
                {
                    fv[j] = 1;
                }
            }

            foreach (var t in nearbytickets)
            {
                foreach (var r in t)
                {
                    if (r > maxfield || fv[r] == 0)
                    {
                        invalid.Add(r);
                    }
                }
            }
            ShowResult(1, invalid.Sum(x => x));
            ShowResult(2, part2);
        }
    }
}
