using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day07 : IAoCRunner
    {
        public int Day => 7;

        public async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await (useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day));
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }
            var map = BuildMap(input);
            var shinygold = map["shiny gold"];
            var containers = FindOuterContainers(shinygold);
            var part1 = containers.Count();
            var part2 = CountContainedBags(shinygold);
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");
        }

        private Dictionary<string, Node> BuildMap(IEnumerable<string> input)
        {
            var result = new Dictionary<string, Node>();
            foreach (var line in input)
            {
                var splits = line.Split("bags contain");
                var key = splits[0].Trim();
                if (!result.ContainsKey(key))
                {
                    result.Add(key, new Node(key));
                }
                var bag = result[key];

                if (splits[1].Trim().StartsWith("no other bags"))
                    continue;

                var contains = splits[1].Split(',')
                    .Select(c => c.Replace("bags", "")
                        .Replace("bag", "")
                        .Trim('.')
                        .Trim());

                foreach (var child in contains)
                {
                    var firstspace = child.IndexOf(' ');
                    var count = int.Parse(child.Substring(0, firstspace));
                    var ckey = child.Substring(firstspace).Trim();
                    if (!result.ContainsKey(ckey))
                    {
                        result.Add(ckey, new Node(ckey));
                    }
                    bag.AddChild(result[ckey], count);
                }
            }
            return result;
        }

        private IEnumerable<Node> FindOuterContainers(Node node)
        {
            var result = new Dictionary<string, Node>();
            foreach (var parent in node.Parents)
            {
                result.TryAdd(parent.Key, parent);
                var containers = FindOuterContainers(parent);
                foreach (var container in containers)
                {
                    result.TryAdd(container.Key, container);
                }
            }
            return result.Values.AsEnumerable();
        }

        private int CountContainedBags(Node node)
        {
            var result = 0;
            foreach (var child in node.Children)
            {
                result += child.Item1 + (child.Item1 * CountContainedBags(child.Item2));
            }
            return result;
        }

        private class Node
        {
            public Node(string key)
            {
                Key = key;
            }
            public string Key { get; }
            public IList<Node> Parents { get; } = new List<Node>();
            public IList<(int, Node)> Children { get; } = new List<(int, Node)>();

            public void AddChild(Node child, int count)
            {
                Children.Add((count, child));
                child.Parents.Add(this);
            }
        }
    }
}
