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
            for (var nd = nodes[1].Next; nd.Value != 1; nd = nd.Next)
            {
                part1 += nd.Value;
            }
            ShowResult(1, part1);

            // *** Part 2
            (nodes, head) = BuildNodes(input, 1_000_000);
            DoMoves(nodes, head, 10_000_000, 1_000_000);

            var part2 = (long)nodes[1].Next.Value * (long)nodes[1].Next.Next.Value;
            ShowResult(2, part2);
        }

        private (Node[], Node) BuildNodes(int[] input, int maxValue)
        {
            var nodes = new Node[maxValue + 1];
            var head = new Node { Value = input[0] };
            nodes[head.Value] = head;
            var prev = head;

            for (var i = 1; i < input.Length; i++)
            {
                var nd = new Node { Value = input[i] };
                nodes[input[i]] = nd;
                prev.Next = nd;
                prev = nd;
            }

            // For part 2 we need to add many extra nodes
            for (var i = input.Length + 1; i <= maxValue; i++)
            {
                var nd = new Node { Value = i };
                nodes[i] = nd;
                prev.Next = nd;
                prev = nd;
            }
            // Link the head and tail together
            prev.Next = head;

            return (nodes, head);
        }

        private void DoMoves(Node[] nodes, Node head, int moves, int maxValue)
        {
            var curr = head;
            for (var move = 1; move <= moves; move++)
            {
                // Cut out next three
                var cut = curr.Next;
                curr.Next = cut.Next.Next.Next;
                // Find suitable destination value
                var dest = curr.Value > 1 ? curr.Value - 1 : maxValue;
                while (dest == cut.Value || dest == cut.Next.Value || dest == cut.Next.Next.Value)
                {
                    // Positions and elements are 1 based
                    dest = ((dest + maxValue - 1 - 1) % maxValue) + 1; // Add maxValue 'cos C# treats (-1 % N) as -1
                }
                // Get the node containing destination
                var n = nodes[dest];
                // Put the cut nodes back in
                cut.Next.Next.Next = n.Next;
                n.Next = cut;
                // Move along
                curr = curr.Next;
                //Console.WriteLine(string.Join(", ", a1) + $"; Curr: {a1[ci]}");
            }
        }
    }
}
