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

                // Get the address in the same format as the mask
                var addr = Int64.Parse(inst[0].Trim(']').Substring(4));
                var addrstr = Convert.ToString(addr, 2).PadLeft(36, '0');

                var addresses = new List<Int64>{0};
                for (var i = 35; i >= 0; i--) // iterate through the mask/address 'bits'
                {
                    // Apply the mask if necessary
                    var ch = mask[i] == '0' ? addrstr[i] : mask[i];
                    if (ch == '0') // sum remains the same, move on
                        continue;

                    var alen = addresses.Count;
                    for (var ai = 0; ai < alen; ai++)
                    {
                        // An 'X' means we need a sum for both 0 and 1 possibilities
                        if (ch == 'X') // add a new sum for the '0' possibility
                        {
                            addresses.Add(addresses[ai]);
                        }
                        addresses[ai] += 1L << (35 - i);
                    }
                }
                var v = Int64.Parse(inst[1]);
                addresses.ForEach(a => regs[a] = v);
            }
            return regs.Sum(r => r.Value);
        }
    }
}
