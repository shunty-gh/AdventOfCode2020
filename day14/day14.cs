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

            var s = input.Select(l => l.Split(" = ")).ToList();
            ShowResult(1, Part1(s));
            ShowResult(2, Part2(s));
        }

        private Int64 Part1(IList<string[]> instructions)
        {
            Int64 maskAnd = 0, maskOr = 0;
            var regs = new Dictionary<int, Int64>();
            foreach (var inst in instructions)
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
            return regs.Sum(r => r.Value);
        }

        private Int64 Part2(IList<string[]> instructions)
        {
            var mask = "";
            var regs = new Dictionary<Int64, Int64>();
            foreach (var inst in instructions)
            {
                if (inst[0].StartsWith("mask"))
                {
                    mask = inst[1];
                    continue;
                }

                // Apply mask (including Xs) to address
                var reg = Int64.Parse(inst[0].Trim(']').Substring(4));
                var regstr = Convert.ToString(reg, 2).PadLeft(36, '0');
                // Apply mask to reg
                var maskedreg = new char[36];
                for (var i = 0; i < 36; i++)
                {
                    maskedreg[i] = mask[i] == '0' ? regstr[i] : mask[i];
                }
                // Make a note of the index positions of the X chars
                var xpos = mask.Select((c,i) => (c, i))
                    .Where(cc => cc.c == 'X')
                    .Select(cc => cc.i);

                // Get all combinations
                var xcount = xpos.Count();
                var combinations = 1 << xcount;
                for (var comb = 0; comb < combinations; comb++)
                {
                    var maskoverlay = Convert.ToString(comb, 2).PadLeft(xcount, '0');
                    var mindex = 0;
                    var chs = maskedreg.ToArray();
                    foreach (var i in xpos)
                    {
                        chs[i] = maskoverlay[mindex];
                        mindex++;
                    }

                    var newreg = Convert.ToInt64(new string(chs), 2);
                    regs[newreg] = Int64.Parse(inst[1]);
                }
            }
            return regs.Sum(r => r.Value);
        }
    }
}
