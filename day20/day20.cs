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

            var pn = new List<(int,int)>();
            foreach (var (tileid, tile) in tiles)
            {
                foreach (var edge in tile.Edges)
                {
                    // Look for potential neighbours
                    foreach (var (eid, etile) in tiles)
                    {
                        if (eid == tileid)
                            continue;
                        pn.AddRange(etile.PotentialNeighbours(edge));
                    }
                }
            }
            var pnd = pn.GroupBy(x => x.Item1).OrderBy(x => x.Count());
            // Hopefully(!) the first 4 elements will have only 2 matchable sides each
            // indicating they are the corner blocks
            var part1 = 1L;
            foreach (var g in pnd)
            {
                if (g.Count() == 2)
                    part1 *= g.Key;
            }
            var part2 = 0;

            ShowResult(1, part1);
            ShowResult(2, part2);
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
        public IList<string> Rows { get; } = new List<string>();
        public Tile(int tileId)
        {
            TileId = tileId;
        }

        public string[] Edges = new string[4];
        public string[] ReverseEdges = new string[4];

        public void GetEdges()
        {
            Edges[0] = Rows[0].Trim();
            Edges[1] = new string(Rows.Select(r => r[r.Length - 1]).ToArray());
            Edges[2] = Rows.Last().Trim();
            Edges[3] = new string(Rows.Select(r => r[0]).ToArray());

            for (var i = 0; i < 4; i++)
            {
                var arr = Edges[i].ToCharArray();
                Array.Reverse(arr);
                ReverseEdges[i] = new string(arr);
            }
        }

        public IEnumerable<(int, int)> PotentialNeighbours(string edge)
        {
            var result = new List<(int,int)>();
            for (var e = 0; e < 4; e++)
            {
                if (edge == Edges[e])
                {
                    result.Add((TileId, e));
                }
                if (edge == ReverseEdges[e])
                {
                    result.Add((TileId, -e));
                }
            }
            return result;
        }
    }
}
