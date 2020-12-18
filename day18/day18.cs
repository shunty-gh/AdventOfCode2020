using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day18 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var part1 = 0L;
            //var part2 = 0;

            foreach (var line in input)
            {
                var splits = line.Split(' ');
                var si = 0;
                var stack = new List<(Int64,string)>();
                var op = "+";
                var sub = 0L;
                while (si < splits.Length)
                {
                    var sym = splits[si++];
                    // 0 or more '('; number; 0 or more ')'
                    var ob = sym.Count(c => c == '(');
                    var cb = sym.Count(c => c == ')');
                    var n = int.Parse(sym.Substring(ob, sym.Length - ob - cb));
                    for (var i = 0; i < ob; i++)
                    {
                        stack.Add((sub, op));
                        sub = 0;
                        op = "+";
                    }
                    if (op == "+") sub += n;
                    else if (op == "-") sub -= n;
                    else if (op == "*") sub *= n;
                    else if (op == "/") sub /= n;

                    for (var i = 0; i < cb; i++)
                    {
                        var nn = sub;
                        (sub, op) = stack.Last();
                        stack.RemoveAt(stack.Count - 1);
                        if (op == "+") sub += nn;
                        else if (op == "-") sub -= nn;
                        else if (op == "*") sub *= nn;
                        else if (op == "/") sub /= nn;
                    }
                    // Next operator, if available
                    if (si < splits.Length)
                        op = splits[si++];
                }
                part1 += sub;
            }
            ShowResult(1, part1);
            //ShowResult(2, part2);
        }
    }
}
