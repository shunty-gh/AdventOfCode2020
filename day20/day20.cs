using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day20 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var tiles = ReadTiles(input);

            // Look for neighbours for each edge of each tile
            foreach (var (_, tile) in tiles)
            {
                foreach (var (_, next) in tiles)
                {
                    if (next != tile)
                    {
                        tile.CheckNeighbour(next);
                    }
                }
            }
            // Hopefully(!) there are exactly 4 blocks with only 2 matchable sides each
            // indicating they are the corner blocks. Multiply their Ids together.
            var corners = tiles.Where(t => t.Value.NeighbourCount == 2);
            var part1 = corners.Aggregate(1L, (acc, kvp) => acc * kvp.Key);
            ShowResult(1, part1);

            // Hopefully no tiles have more than 4 potential neighbours
            System.Diagnostics.Debug.Assert(tiles.Count(t => t.Value.NeighbourCount > 4) == 0);

            var todo = new Queue<int>();
            var grid = new Dictionary<(int,int), List<string>>();
            var gridIndex = new Dictionary<int, (int,int)>();
            var pt = (0,0);
            // Pick a corner to start
            var startTileId = corners.First().Key;
            grid[pt] = new List<string>(tiles[startTileId].Rows);
            gridIndex[startTileId] = pt;
            todo.Enqueue(startTileId);
            while (todo.Any())
            {
                var tileId = todo.Dequeue();
                var thisTile = tiles[tileId];
                var (x,y) = gridIndex[tileId];
                var theseRows = grid[(x,y)];

                foreach (var (tid, eid) in thisTile.Neighbours)
                {
                    if (gridIndex.ContainsKey(tid))
                        continue;

                    var nextTile = tiles[tid];
                    var nextRows = new List<string>(nextTile.Rows);
                    // The current tile may have been flipped and rotated so find
                    // the matching edges.
                    var edge = nextRows.GetEdge(eid).ReverseString();
                    // Flip/rotate these rows and attach them to the relevant edge
                    var (e, flip) = theseRows.FindEdge(edge);
                    // This edge "e" matches that edge "eid"
                    (int,int) nextloc = (x,y);
                    switch (e)
                    {
                        case 0:
                            nextloc = (x,y-1);
                            break;
                        case 1:
                            nextloc = (x+1,y);
                            break;
                        case 2:
                            nextloc = (x,y+1);
                            break;
                        case 3:
                            nextloc = (x-1,y);
                            break;
                    }
                    // Need eid to be opposite to e, ie +2 onwards
                    // Need to add 4 because C# treats (-ve % x) as -ve NOT +ve
                    var rotateBy = (e + 2 - eid + 4) % 4;

                    if (flip && (e == 1 || e == 3))
                        grid[nextloc] = nextRows.Rotate(rotateBy).FlipAlongHorizontal();
                    else if (flip && (e == 0 || e == 2))
                        grid[nextloc] = nextRows.Rotate(rotateBy).FlipAlongVertical();
                    else
                        grid[nextloc] = nextRows.Rotate(rotateBy);
                    gridIndex[tid] = nextloc;

                    todo.Enqueue(tid);
                }
            }

            var minx = grid.Min(g => g.Key.Item1);
            var maxx = grid.Max(g => g.Key.Item1);
            var miny = grid.Min(g => g.Key.Item2);
            var maxy = grid.Max(g => g.Key.Item2);
            // Print the grid
            // for (var y = miny; y <= maxy; y++)
            // {
            //     for (var ri = 0; ri < 10; ri++)
            //     {
            //         for (var x = minx; x <= maxx; x++)
            //         {
            //             var rows = grid[(x,y)];
            //             Console.Write(rows[ri] + "  ");
            //         }
            //         Console.WriteLine();
            //     }
            //     Console.WriteLine();
            // }

            // Remove borders
            var image = new List<string>();
            for (var y = miny; y <= maxy; y++)
            {
                for (var ri = 1; ri < 9; ri++)  // Skip top and bottom rows
                {
                    var s = "";
                    for (var x = minx; x <= maxx; x++)
                    {
                        var rows = grid[(x,y)];
                        s += rows[ri].Substring(1, 8);
                    }
                    image.Add(s);
                }
            }

            // Print image
            // foreach (var line in image)
            // {
            //     Console.WriteLine(line);
            // }

            var part2 = GetWaterRoughness(image);
            ShowResult(2, part2);
        }

        private int GetWaterRoughness(List<string> image)
        {
            var hashcount = 0;
            foreach (var line in image)
            {
                hashcount += line.Count(c => c == '#');
            }
            var monsters = 0;
            while (monsters == 0)
            {
                monsters = FindMonsters(image);
                if (monsters == 0)
                {
                    monsters = FindMonsters(image.FlipAlongHorizontal());
                }
                if (monsters == 0)
                {
                    image = image.Rotate();
                }
            }

            return hashcount - (monsters * MonsterChars);
        }

        private int MonsterLength = 20;
        private int MonsterChars = 15;
        /*    Sea monster layout        Char indexes
         *    ---------------------     -----------------------
         *                      #       {18}
         *    #....##....##....###      {0,5,6,11,12,17,18,19}
         *    .#..#..#..#..#..#         {1,4,7,10,13,16}
         */

        private int FindMonsters(List<string> image)
        {
            var result = 0;
            for (var y = 0; y < image.Count - 2; y++)
            {
                for (var x = 0; x < image[0].Length - MonsterLength; x++)
                {
                    var l1 = image[y];
                    var l2 = image[y+1];
                    var l3 = image[y+2];
                    if (l1[x+18] == '#'
                        && l2[x] == '#' && l2[x+5] == '#' && l2[x+6] == '#' && l2[x+11] == '#' && l2[x+12] == '#' && l2[x+17] == '#' && l2[x+18] == '#' && l2[x+19] == '#'
                        && l3[x+1] == '#' && l3[x+4] == '#' && l3[x+7] == '#' && l3[x+10] == '#' && l3[x+13] == '#' && l3[x+16] == '#')
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        private Dictionary<int, Tile> ReadTiles(IList<string> input)
        {
            var result = new Dictionary<int, Tile>();
            var i = 0;
            while (i < input.Count)
            {
                var line = input[i++];
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var tileid = int.Parse(line.Substring(5, line.Length - 6));
                var tile = new Tile(tileid);
                while (i < input.Count)
                {
                    var next = input[i++];
                    if (string.IsNullOrWhiteSpace(next))
                        break;
                    tile.Rows.Add(next);
                }
                tile.GetEdges();
                result.Add(tileid, tile);
            }
            return result;
        }
    }

    public class Tile
    {
        public int TileId { get; }
        public List<string> Rows { get; } = new List<string>();

        public Tile(int tileId)
        {
            TileId = tileId;
        }

        public string[] Edges = new string[4];
        public string[] ReverseEdges = new string[4];

        public void GetEdges()
        {
            for (var i = 0; i < 4; i++)
            {
                Edges[i] = Rows.GetEdge(i);
                ReverseEdges[i] = StringHelpers.ReverseString(Edges[i]);
            }
        }

        private IEnumerable<int> MatchingEdges(string edge)
        {
            var result = new List<int>();
            for (var e = 0; e < 4; e++)
            {
                if (edge == Edges[e] || edge == ReverseEdges[e])
                {
                    result.Add(e);
                }
            }
            return result;
        }

        private List<(int,int)> _neighbours = new List<(int,int)>();
        public void CheckNeighbour(Tile tile)
        {
            if (tile.TileId == this.TileId)
                return;

            for (var ei = 0; ei < 4; ei++)
            {
                var pns = tile.MatchingEdges(Edges[ei]);
                foreach (var eid in pns)
                {
                    _neighbours.Add((tile.TileId, eid));
                }
            }
        }

        public int NeighbourCount => _neighbours.Count;
        public IEnumerable<(int,int)> Neighbours => _neighbours;

    }
}
