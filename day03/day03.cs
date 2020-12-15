using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day03 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            var map = ReadMap(input);

            var part1 = Traverse(map, (3,1));

            var routes = new List<(int,int)>
            {
                (1,1),(5,1),(7,1),(1,2)
            };
            Int64 part2 = part1;
            foreach (var route in routes)
            {
                part2 *= Traverse(map, route);
            }

            ShowResult(1, part1);
            ShowResult(2, part2);
        }

        private static int Traverse(Dictionary<(int X,int Y), char> map, (int dX, int dY) route)
        {
            var result = 0;
            (int X, int Y) current = (0,0);
            var maxX = map.Max(m => m.Key.X);
            var maxY = map.Max(m => m.Key.Y);
            while (current.Y <= maxY)
            {
                // Move
                current = ((current.X + route.dX) % (maxX + 1), current.Y + route.dY);
                if (map.ContainsKey(current))
                {
                    result += map[current] == '.' ? 0 : 1;
                }
            }
            return result;
        }

        private static Dictionary<(int X,int Y), char> ReadMap(IList<string> lines)
        {
            var result = new Dictionary<(int,int), char>();
            for (var y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                if (string.IsNullOrWhiteSpace(line))
                    break;

                for (var x = 0; x < line.Length; x++)
                {
                    result[(x,y)] = line[x];
                }
            }
            return result;
        }
    }
}
