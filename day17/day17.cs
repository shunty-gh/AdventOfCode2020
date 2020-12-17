using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day17 : AoCRunnerBase
    {
        private int MinX = 0;
        private int MaxX = 0;
        private int MinY = 0;
        private int MaxY = 0;
        private int MinZ = 0;
        private int MaxZ = 0;

        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var world = new Dictionary<(int,int,int), (int,int)>();
            for (var y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (var x = 0; x < input[0].Length; x++)
                {
                    world[(x,y,0)] = (line[x] == '#' ? 1 : 0, 0);

                    world[(x,y,1)] = (0,0);
                    world[(x,y,-1)] = (0,0);
                }
            }
            MinZ = -1;
            MaxZ = 1;
            MaxY = input.Count - 1;
            MaxX = input[0].Length - 1;

            Print(world, 0);
            for (var round = 0; round < 6; round++)
            {
                Prepare(world);
                DoRound(world);
                Print(world, round + 1);
            }
            var part1 = world.Count(w => w.Value.Item1 == 1);
            var part2 = 0;

            ShowResult(1, part1);
            ShowResult(2, part2);
        }

        private void Prepare(Dictionary<(int X,int Y, int Z), (int,int)> world)
        {
            MinX = world.Min(w => w.Key.X);
            MaxX = world.Max(w => w.Key.X);
            MinY = world.Min(w => w.Key.Y);
            MaxY = world.Max(w => w.Key.Y);
            MinZ = world.Min(w => w.Key.Z);
            MaxZ = world.Max(w => w.Key.Z);
            for (var z = MinZ - 1; z <= MaxZ + 1; z++)
            {
                for (var y = MinY - 1; y <= MaxY + 1; y++)
                {
                    for (var x = MinX - 1; x <= MaxX + 1; x++)
                    {
                        var key = (x,y,z);
                        var nc = ActiveNeighbourCount(world, key, 3);
                        var exists = world.ContainsKey(key);
                        var (curr, _) = exists ? world[key] : (0,0);
                        if (curr == 1)
                        {
                            world[key] = (1, (nc == 2 || nc == 3) ? 1 : 0);
                        }
                        else if (nc == 3)
                        {
                            world[key] = (0, 1);
                        }
                        else if (exists)
                        {
                            world[key] = (curr, curr);
                        }
                    }
                }
            }
        }

        private void DoRound(Dictionary<(int, int, int), (int,int)> world)
        {
            // Move future setting into current setting for each seat
            foreach (var (k, (curr, next)) in world)
            {
                if (curr != next)
                    world[k] = (next, next);
            }
        }

        private int ActiveNeighbourCount(Dictionary<(int X,int Y,int Z), (int Current, int _)> world, (int X,int Y,int Z) loc, int lim)
        {
            var result = 0;
            for (var dz = -1; dz <= 1; dz++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0)
                            continue;
                        var pt = (loc.X + dx, loc.Y + dy, loc.Z + dz);
                        if (world.ContainsKey(pt) && world[pt].Current == 1)
                        {
                            result++;
                            if (result > lim)
                                return result;
                        }
                    }
                }
            }
            return result;
        }

        private void Print(Dictionary<(int X,int Y,int Z), (int Current, int _)> world, int round)
        {
            MinX = world.Min(w => w.Key.X);
            MaxX = world.Max(w => w.Key.X);
            MinY = world.Min(w => w.Key.Y);
            MaxY = world.Max(w => w.Key.Y);
            MinZ = world.Min(w => w.Key.Z);
            MaxZ = world.Max(w => w.Key.Z);
            Console.WriteLine($"Round {round} [X={MinX}..{MaxX}; Y={MinY}..{MaxY}; Z={MinZ}..{MaxZ}]");
            for (var z = MinZ; z <= MaxZ; z++)
            {
                Console.WriteLine($"z={z}");
                for (var y = MinY; y <= MaxY; y++)
                {
                    for (var x = MinX; x <= MaxX; x++)
                    {
                        var key = (x,y,z);
                        if (!world.ContainsKey(key))
                        {
                            Console.Write('.');
                            continue;
                        }

                        var (curr, _) = world[key];
                        Console.Write(curr == 1 ? '#' : '.');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("---");
                Console.WriteLine();
            }
        }
    }
}
