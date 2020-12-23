using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day23 : AoCRunnerBase
    {
        public class Node
        {
            public Node Previous { get; set; }
            public Node Next { get; set; }
            public int Value { get; set; }
        }

        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = (await GetInputLines(useTestData))
                .First()
                .Select(c => (int)Char.GetNumericValue(c))
                .ToArray();

            // *** Part 1
            var (nodes, head) = BuildNodes(input, 9);
            DoMoves(nodes, head, 100, 9);

            var part1 = "";
            var nd = nodes[1].Next;
            for (var i = 1; i < 9; i++)
            {
                part1 += nd.Value;
                nd = nd.Next;
            }
            ShowResult(1, part1);

            // *** Part 2
            (nodes, head) = BuildNodes(input, 1_000_000);
            DoMoves(nodes, head, 10_000_000, 1_000_000);

            nd = nodes[1].Next;
            var part2 = (long)nd.Value * (long)nd.Next.Value;
            ShowResult(2, part2);
        }

        private (Node[], Node) BuildNodes(int[] input, int maxValue)
        {
            var nodes = new Node[maxValue + 1];
            var head = new Node { Value = input.First() };
            nodes[head.Value] = head;
            var prev = head;
            for (var i = 1; i < input.Length; i++)
            {
                var nd = new Node { Previous = prev, Value = input[i] };
                nodes[input[i]] = nd;
                prev.Next = nd;
                prev = nd;
            }

            // For part 2 we need to add many extra nodes
            var imax = input.Max();
            if (maxValue > imax)
            {
                for (var i = imax + 1; i <= maxValue; i++)
                {
                    var nd = new Node { Previous = prev, Value = i };
                    nodes[i] = nd;
                    prev.Next = nd;
                    prev = nd;
                }
            }

            // Link the head and tail together
            prev.Next = head;
            head.Previous = prev;

            return (nodes, head);
        }

        private void DoMoves(Node[] nodes, Node head, int moves, int maxValue)
        {
            var next3Nodes = new Node[3];
            var next3Values = new int[3];

            for (var move = 1; move <= moves; move++)
            {
                // Cut out next three
                var n = head.Next;
                for (var i = 0; i < 3; i++)
                {
                    next3Nodes[i] = n;
                    next3Values[i] = n.Value;
                    n = n.Next;
                }
                head.Next = n;
                n.Previous = head;
                // Find suitable destination value
                var dest = head.Value > 1 ? head.Value - 1 : maxValue;
                while (next3Values.Contains(dest))
                {
                    dest--;
                    if (dest <= 0)
                        dest = maxValue;
                }
                // Get the node containing destination
                n = nodes[dest];
                // Put the cut nodes back in
                var tmp = n.Next;
                n.Next = next3Nodes[0];
                next3Nodes[0].Previous = n;
                tmp.Previous = next3Nodes[2];
                next3Nodes[2].Next = tmp;
                // Move the head along
                head = head.Next;
                //Console.WriteLine(string.Join(", ", a1) + $"; Curr: {a1[ci]}");
            }
        }
    }
}
