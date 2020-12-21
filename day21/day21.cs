using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day21 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var ingredients = new Dictionary<string, int>();
            var allergens = new Dictionary<string, IList<string>>();
            foreach (var line in input)
            {
                var ingr = line.Split('(')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var aller = line.Split('(')[1].Trim(')').Replace("contains", "").Replace(",", "").Split(' ');

                foreach (var i in ingr)
                {
                    if (!ingredients.ContainsKey(i))
                        ingredients[i] = 1;
                    else
                        ingredients[i] += 1;
                }
                foreach (var a in aller)
                {
                    if (string.IsNullOrWhiteSpace(a))
                        continue;
                    if (!allergens.ContainsKey(a))
                    {
                        allergens[a] = new List<string>(ingr);
                    }
                    else
                    {
                        var current = allergens[a];
                        allergens[a] = current.Intersect(ingr).ToList();
                    }
                }
            }

            var matched = new Dictionary<string, string>();
            var match = allergens.Select(x => (x.Key, x.Value)).OrderBy(x => x.Value.Count);
            foreach (var (a, i) in match)
            {
                if (i.Count == 1)
                    matched[i.First()] = a;
                else
                {
                    foreach (var ing in i)
                    {
                        if (matched.ContainsKey(ing))
                            continue;
                        matched[ing] = a;
                    }
                }
            }

            var part1 = 0;
            foreach (var (ingredient, count) in ingredients)
            {
                if (matched.ContainsKey(ingredient))
                    continue;
                part1 += count;
            }

            ShowResult(1, part1);
            var part2 = string.Join(',', matched.OrderBy(x => x.Value).Select(x => x.Key));
            ShowResult(2, part2);
        }
    }
}
