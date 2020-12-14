using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day14 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            var part1 = 0L;
            var part2 = 0;

            var s = input.Select(l => l.Split(" = ")).ToList();
            Int64 maskAnd = 0, maskOr = 0;
            var regs = new Dictionary<int, Int64>();
            foreach (var inst in s)
            {
                if (inst[0].StartsWith("mask"))
                {
                    maskAnd = Convert.ToInt64(inst[1].Replace('X', '1'), 2);
                    maskOr = Convert.ToInt64(inst[1].Replace('X', '0'), 2);
                }
                else
                {
                    var reg = int.Parse(inst[0].Trim(']').Substring(4));
                    var v = Int64.Parse(inst[1]);
                    v |= maskOr;
                    v &= maskAnd;
                    regs[reg] = v;
                }
            }
            part1 = regs.Sum(r => r.Value);
            ShowResult(1, part1);
            ShowResult(2, part2);
        }
    }
}
