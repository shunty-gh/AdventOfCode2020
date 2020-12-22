using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day22 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            var (q1, q2) = GetCardDecks(input);
            ShowResult(1, Part1(new Queue<int>(q1), new Queue<int>(q2)));
            ShowResult(2, Part2(new Queue<int>(q1), new Queue<int>(q2)));
        }

        private (Queue<int>, Queue<int>) GetCardDecks(IList<string> input)
        {
            var p1 = false;
            var q1 = new Queue<int>();
            var q2 = new Queue<int>();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.StartsWith("P"))
                {
                    p1 = !p1;
                    continue;
                }
                if (p1)
                    q1.Enqueue(int.Parse(line));
                else
                    q2.Enqueue(int.Parse(line));
            }
            return (q1, q2);
        }

        private int Part1(Queue<int> q1, Queue<int> q2)
        {
            while (q1.Count > 0 && q2.Count > 0)
            {
                var c1 = q1.Dequeue();
                var c2 = q2.Dequeue();
                if (c1 > c2)
                {
                    q1.Enqueue(c1);
                    q1.Enqueue(c2);
                }
                else
                {
                    q2.Enqueue(c2);
                    q2.Enqueue(c1);
                }
            }

            return GetScore(q1.Count > 0 ? q1 : q2);
        }

        private int Part2(Queue<int> q1, Queue<int> q2)
        {
            var (_,q) = PlayGame(q1, q2);
            return GetScore(q);
        }

        private (bool,Queue<int>) PlayGame(Queue<int> q1, Queue<int> q2)
        {
            Dictionary<string, bool> states = new Dictionary<string, bool>();

            while (q1.Any() && q2.Any())
            {
                // First check if we have already had this configuration
                var state = GetState(q1, q2);
                if (states.ContainsKey(state))
                {
                    // if so then player 1 is the winner of the game
                    return (true, q1);
                }
                else
                {
                    states[state] = true;
                    PlayRound(q1, q2);
                }
            }
            return q1.Any() ? (true, q1) : (false, q2);
        }

        private string GetState(Queue<int> q1, Queue<int> q2)
        {
            return $"{string.Join(',', q1.Select(q => q))}#{string.Join(',', q2.Select(q => q))}";
        }

        private void PlayRound(Queue<int> q1, Queue<int> q2)
        {
            var c1 = q1.Dequeue();
            var c2 = q2.Dequeue();
            var leftWins = (c1 > c2);
            if (q1.Count >= c1 && q2.Count >= c2)
            {
                (leftWins, _) = PlayGame(new Queue<int>(q1.Take(c1)), new Queue<int>(q2.Take(c2)));
            }

            if (leftWins)
            {
                q1.Enqueue(c1);
                q1.Enqueue(c2);
            }
            else
            {
                q2.Enqueue(c2);
                q2.Enqueue(c1);
            }
        }

        private int GetScore(Queue<int> cards)
        {
            var result = 0;
            var i = cards.Count;
            while (i > 0)
            {
                result += (i-- * cards.Dequeue());
            }
            return result;
        }

    }
}
