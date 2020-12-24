using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day24 : AoCRunnerBase
    {
        /// Hex grid. Treat as an offset regular grid with co-ords scaled by 10
        /// ----------------------------------------------------------------
        /// |       | -15,10| -5,10 |  5,10 | 15,10 | 25,10 | 35,10 |
        /// ----------------------------------------------------------------
        ///     |       | -10,0 |  0,0  |  10,0 |  20,0 | 30,0  |       |
        /// ----------------------------------------------------------------
        /// |       |-15,-10| -5,-10| 5,-10 | 15,-10| 25,-10|       |
        /// ----------------------------------------------------------------
        ///
        /// To go:
        ///   nw => (x,y) => (x-5, y+10) ; ne => (x,y) => (x+5, y+10)
        ///   sw => (x,y) => (x-5, y-10) ; se => (x,y) => (x+5, y-10)
        ///    w => (x,y) => (x-10, y)   ;  e => (x,y) => (x+10, y)

        /// Implementing this with a ConcurrentDictionary allows us to use
        /// Parallel.For(...) in the Prepare() method which speeds the whole
        /// thing up by quite a bit (~20%). Not exactly necessary though. But
        /// why not.

        private class LobbyFloor : ConcurrentDictionary<(int X, int Y), (bool Current, bool Next)>
        {
            public int BlackTiles => this.Count(t => t.Value.Current);

            public void FlipTileState((int,int) key)
            {
                bool v = false, n = false;
                if (this.ContainsKey(key))
                    (v, n) = this[key];
                this[key] = (!v, n);
            }

            public void FlipTiles(int count = 1)
            {
                for (var i = 0; i < count; i++)
                {
                    PrepareNextState();
                    UpdateState();
                }
            }

            private static readonly (int,int)[] AdjacentOffsets = new (int,int)[] {(-5,10),(5,10),(-10,0),(10,0),(-5,-10),(5,-10)};
            private int GetAdjacentBlackTiles((int X, int Y) tile)
            {
                var bb = 0;
                var (x, y) = tile;
                foreach (var (dx,dy) in AdjacentOffsets)
                {
                    var key = (x + dx, y + dy);
                    if (this.ContainsKey(key) && this[key].Current)
                    {
                        bb++;
                        if (bb > 2)
                            return bb; // We're not interested in the exact number, just if it's > 2
                    }
                }
                return bb;
            }

            private void PrepareNextState()
            {
                var minX = this.Min(w => w.Key.X);
                var maxX = this.Max(w => w.Key.X);
                var minY = this.Min(w => w.Key.Y);
                var maxY = this.Max(w => w.Key.Y);

                // Not every cell in the grid is populated so make sure we
                // allow for a wider boundary when chacking for adjacent tiles
                // ie add 10 to x and y bounds
                //for (var y = minY - 10; y <= maxY + 10; y += 10)
                //{
                Parallel.For(minY - 10, maxY + 10 + 1, y =>
                {
                    //for (var x = minX - 10; x <= maxX + 10; x += 5)
                    //{
                    if (y % 10 != 0)
                        return;

                    Parallel.For(minX - 10, maxX + 10 + 1, x =>
                    {
                        if (x % 5 != 0)
                            return;
                        var key = (x,y);
                        var bb = this.GetAdjacentBlackTiles(key);
                        var exists = this.ContainsKey(key);
                        var (curr, _) = exists ? this[key] : (false,false);
                        if (curr) // Is it black - flip if 0 or > 2 neighbouring tiles are black
                        {
                            this[key] = (true, (bb == 0 || bb > 2 ? false : true));
                        }
                        else if (bb == 2) // white - flip if exactly 2 neighbouring tiles are black
                        {
                            this[key] = (false, true);
                        }
                        else if (exists)
                        {
                            this[key] = (curr, curr);
                        }
                    });
                    //}
                });
                //}
            }

            private void UpdateState()
            {
                // Copy 'next' state into 'current'
                foreach (var (k, (_, next)) in this)
                {
                    this[k] = (next,false);
                }
                // The overhead of .AsParallel appears to make this a bit slower
                //this.AsParallel().ForAll(kvp => this[kvp.Key] = (kvp.Value.Next, false));
            }
        }

        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var allsteps = new List<int[]>();
            // Convert input lines into lists of x,y increments
            foreach (var line in input)
            {
                // The multiple .Replace calls don't cause any noticeable slow down
                // in this case.
                allsteps.Add(line.Replace("nw", "-5,10,")
                    .Replace("ne", "5,10,")
                    .Replace("sw", "-5,-10,")
                    .Replace("se", "5,-10,")
                    .Replace("w", "-10,0,")
                    .Replace("e", "10,0,")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))
                    .ToArray());
            }

            var floor = new LobbyFloor();
            foreach (var steps in allsteps)
            {
                int x = 0, y = 0;
                for (var m = 0; m < steps.Length; m += 2)
                {
                    x += steps[m];
                    y += steps[m+1];
                }

                floor.FlipTileState((x,y));
            }
            ShowResult(1, floor.BlackTiles);

            floor.FlipTiles(100);
            ShowResult(2, floor.BlackTiles);
        }
    }
}
