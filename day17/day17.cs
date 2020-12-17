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
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            var world =  InitWorld(input);
            for (var round = 0; round < 6; round++)
            {
                Prepare(world, 1);
                DoRound(world);
                //Print(world, round + 1, 0);
            }
            ShowResult(1, world.Count(w => w.Value.Current == 1));

            // Reset the world for part 2
            world =  InitWorld(input);
            for (var round = 0; round < 6; round++)
            {
                Prepare(world, 2);
                DoRound(world);
            }
            ShowResult(2, world.Count(w => w.Value.Current == 1));
        }

        private Dictionary<(int, int,int,int), (int Current,int Next)> InitWorld(IList<string> input)
        {
            var world = new Dictionary<(int,int,int,int), (int,int)>();
            for (var y = 0; y < input.Count; y++)
            {
                var line = input[y];
                for (var x = 0; x < input[0].Length; x++)
                {
                    world[(x,y,0,0)] = (line[x] == '#' ? 1 : 0, 0);
                }
            }

            return world;
        }

        // Turns out this isn't really any quicker
        // private (int,int,int,int,int,int) MaxDimensions3D(Dictionary<(int X,int Y, int Z, int W), (int,int)> world)
        // {
        //     int xl = 0, xh = 0, yl = 0, yh = 0, zl = 0, zh = 0;
        //     foreach (var ((x, y, z, _), _) in world)
        //     {
        //         if (x < xl) xl = x;
        //         else if (x > xh) xh = x;
        //         if (y < yl) yl = y;
        //         else if (y > yh) yh = y;
        //         if (z < zl) zl = z;
        //         else if (z > zh) zh = z;
        //     }
        //     return (xl,xh,yl,yh,zl,zh);
        // }

        private void Prepare(Dictionary<(int X,int Y, int Z, int W), (int,int)> world, int part)
        {
            var minX = world.Min(w => w.Key.X);
            var maxX = world.Max(w => w.Key.X);
            var minY = world.Min(w => w.Key.Y);
            var maxY = world.Max(w => w.Key.Y);
            var minZ = world.Min(w => w.Key.Z);
            var maxZ = world.Max(w => w.Key.Z);
            //var (minX, maxX, minY, maxY, minZ, maxZ) = MaxDimensions3D(world);
            var minW = part == 1 ? 1 : world.Min(w => w.Key.W);
            var maxW = part == 1 ? -1 : world.Max(w => w.Key.W);

            for (var w = minW - 1; w <= maxW + 1; w++)
            {
                for (var z = minZ - 1; z <= maxZ + 1; z++)
                {
                    for (var y = minY - 1; y <= maxY + 1; y++)
                    {
                        for (var x = minX - 1; x <= maxX + 1; x++)
                        {
                            var key = (x,y,z,w);
                            var nc = ActiveNeighbourCount(world, key, 3, part);
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
        }

        private void DoRound(Dictionary<(int, int, int, int), (int,int)> world)
        {
            // Move next setting into current setting
            foreach (var (k, (curr, next)) in world)
            {
                if (curr != next)
                    world[k] = (next, next);
            }
        }

        private int ActiveNeighbourCount(Dictionary<(int X,int Y,int Z, int W), (int Current, int _)> world, (int X,int Y,int Z, int W) loc, int lim, int part)
        {
            var result = 0;
            var mindw = part == 1 ? 0 : -1;
            var maxdw = part == 1 ? 0 : 1;
            for (var dw = mindw; dw <= maxdw; dw++)
            {
                for (var dz = -1; dz <= 1; dz++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        for (var dx = -1; dx <= 1; dx++)
                        {
                            if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                                continue;
                            var pt = (loc.X + dx, loc.Y + dy, loc.Z + dz, loc.W + dw);
                            if (world.ContainsKey(pt) && world[pt].Current == 1)
                            {
                                result++;
                                if (result > lim)
                                    return result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        // private void Print(Dictionary<(int X,int Y,int Z, int W), (int Current, int _)> world, int round, int w = 0)
        // {
        //     var minX = world.Min(w => w.Key.X);
        //     var maxX = world.Max(w => w.Key.X);
        //     var minY = world.Min(w => w.Key.Y);
        //     var maxY = world.Max(w => w.Key.Y);
        //     var minZ = world.Min(w => w.Key.Z);
        //     var maxZ = world.Max(w => w.Key.Z);

        //     Console.WriteLine($"Round {round} [X={minX}..{maxX}; Y={minY}..{maxY}; Z={minZ}..{maxZ}; W={w}]");
        //     for (var z = minZ; z <= maxZ; z++)
        //     {
        //         Console.WriteLine($"z={z}");
        //         for (var y = minY; y <= maxY; y++)
        //         {
        //             for (var x = minX; x <= maxX; x++)
        //             {
        //                 var key = (x,y,z,w);
        //                 if (!world.ContainsKey(key))
        //                 {
        //                     Console.Write('.');
        //                     continue;
        //                 }

        //                 var (curr, _) = world[key];
        //                 Console.Write(curr == 1 ? '#' : '.');
        //             }
        //             Console.WriteLine();
        //         }
        //         Console.WriteLine();
        //         Console.WriteLine("---");
        //         Console.WriteLine();
        //     }
        // }
    }
}
