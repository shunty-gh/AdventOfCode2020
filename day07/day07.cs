using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day07 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            // Parse the input, get a collection of bags and the bags that they contain
            var bags = input
                .Select(line => line.Trim('.').Split(" bags contain "))
                .Select(splits => (splits[0], splits[1].Replace(" bags", "").Replace(" bag", "")))
                .ToDictionary(m =>
                    m.Item1,
                    m => m.Item2.StartsWith("no other")
                        ? new (int, string)[] {}
                        : m.Item2.Split(", ").Select(v => (int.Parse(v.Split(' ')[0]), v.Substring(v.IndexOf(' ')).Trim())));

            // Create a secondary "index" collection to link bags with their containing (parent) bag(s).
            // This is not strictly necessary but it makes part 1 much quicker and the recursive code a little neater.
            var parents = new Dictionary<string, IList<string>>();
            foreach (var bag in bags)
            {
                parents.TryAdd(bag.Key, new List<string>());
                foreach (var ch in bag.Value)
                {
                    parents.TryAdd(ch.Item2, new List<string>());
                    parents[ch.Item2].Add(bag.Key);
                }
            }

            ShowResult(1, ContainersFor(parents, "shiny gold").Count());
            ShowResult(2, ContainedCount(bags, "shiny gold"));
        }

        private IEnumerable<string> ContainersFor(Dictionary<string, IList<string>> map, string key) =>
            map[key].Union(map[key].SelectMany(p => ContainersFor(map, p)));

        private int ContainedCount(Dictionary<string, IEnumerable<(int Count, string Key)>> map, string key) =>
            map[key].Sum(c => c.Count + (c.Count * ContainedCount(map, c.Key)));
    }
}
