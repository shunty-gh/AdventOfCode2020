using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day11 : AoCRunnerBase
    {
        private int MaxX = 0;
        private int MaxY = 0;

        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            ShowResult(1, DoPart(1, input));
            ShowResult(2, DoPart(2, input));
        }

        private int DoPart(int part, IList<string> input)
        {
            var grid = GetInitialGrid(input);
            MaxX = grid.Max(g => g.Key.Item1);
            MaxY = grid.Max(g => g.Key.Item2);

            while (true)
            {
                var nextroundisstable = Prepare(grid, part);
                if (nextroundisstable)
                {
                    return grid.Count(g => g.Value.Item1 == 2);
                }
                DoRound(grid);
            }
        }

        private Dictionary<(int,int), (int,int)> GetInitialGrid(IList<string> input)
        {
            // Grid element first item = current setting => 0 = floor; 1 = empty seat; 2 = occupied seat
            // Grid element second item = future setting (after next round)
            var grid = new Dictionary<(int, int), (int,int)>();
            for (var y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (var x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            grid[(x,y)] = (0,0);
                            break;
                        case 'L':
                            grid[(x,y)] = (1,0);
                            break;
                    }
                }
            }
            return grid;
        }

        private static readonly (int, int)[] AllDirections = new (int,int)[]
        {
             (1,0), (1,-1), (0,-1), (-1,-1), (-1,0), (-1,1), (0,1), (1,1)
        };

        private int NeighbourCount1(Dictionary<(int,int), (int,int)> grid, int x, int y)
        {
            var result = 0;
            foreach (var (dx,dy) in AllDirections)
            {
                (int X, int Y) next = (x+dx,y+dy);
                if (grid.ContainsKey(next))
                    result += grid[next].Item1 == 2 ? 1 : 0;
            }
            return result;
        }

        private int NeighbourCount2(Dictionary<(int,int), (int,int)> grid, int x, int y)
        {
            var result = 0;
            foreach (var (dx,dy) in AllDirections)
            {
                (int X, int Y) next = (x+dx,y+dy);
                while (true)
                {
                    // Need to keep looking in this direction until we find a seat
                    // or reach the edge of the seating plan
                    if (!grid.ContainsKey(next) || grid[next].Item1 == 1)
                        break;

                    if (grid[next].Item1 == 2)
                    {
                        result++;
                        break;
                    }
                    next = (next.X + dx, next.Y + dy);
                }
            }
            return result;
        }

        private bool Prepare(Dictionary<(int,int), (int,int)> grid, int part)
        {
            var result = true;
            for (var y = 0; y <= MaxY; y++)
            {
                for (var x = 0; x <= MaxX; x++)
                {
                    var key = (x,y);
                    var (current, _) = grid[key];
                    if (current == 0) // Skip empty floor space
                        continue;

                    var n = part == 1
                        ? NeighbourCount1(grid, x, y)
                        : NeighbourCount2(grid, x, y);

                    // Seat with no occupied neighbours becomes occupied
                    if (n == 0 && current == 1)
                    {
                        result = false;
                        grid[key] = (current, 2);
                    }
                    // Occupied seat with too many neighbours becomes empty
                    else if (current == 2 && ((part == 1 && n >= 4) || (part == 2 && n >= 5)))
                    {
                        result = false;
                        grid[key] = (current, 1);
                    }
                    // Otherwise no change
                    else
                    {
                        grid[key] = (current, current);
                    }
                }
            }
            return result;
        }
        private void DoRound(Dictionary<(int,int), (int,int)> grid)
        {
            // Move future setting into current setting for each seat
            for (var y = 0; y <= MaxY; y++)
            {
                for (var x = 0; x <= MaxX; x++)
                {
                    var (current, next) = grid[(x,y)];
                    if (current != 0)
                        grid[(x,y)] = (next, next);
                }
            }
        }

    }
}
