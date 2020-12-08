using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day08 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);
            int part1 = 0, part2 = 0, faultindex = 0;
            IList<string> patched;
            (part1, _) = RunProgram(input);
            var fault = true;
            while (fault)
            {
                (patched, faultindex) = PatchInput(input, faultindex);
                (part2, fault) = RunProgram(patched);
            }
            ShowResult(1, part1);
            ShowResult(2, part2);
        }

        private (IList<string>, int) PatchInput(IList<string> source, int startindex)
        {
            var result = new List<string>();
            var faultindex = 0;
            for (var i = 0; i < source.Count; i++)
            {
                var line = source[i];
                if (i > startindex && faultindex == 0)
                {
                    switch (line.Substring(0, 3))
                    {
                        case "nop":
                            line = line.Replace("nop", "jmp");
                            faultindex = i;
                            break;
                        case "jmp":
                            line = line.Replace("jmp", "nop");
                            faultindex = i;
                            break;
                        default:
                            break;
                    }
                }
                result.Add(line);
            }
            return (result, faultindex);
        }
        private (int acc, bool fault) RunProgram(IList<string> input)
        {
            int ip = 0, acc = 0;
            var iplog = new Dictionary<int,int>();
            while (true)
            {
                var instruction = input[ip].Split(' ')[0];
                var count = int.Parse(input[ip].Split(' ')[1]);
                if (iplog.ContainsKey(ip))
                {
                    return (acc, true);
                }
                iplog[ip] = 1;
                switch (instruction)
                {
                    case "nop":
                        ip++;
                        break;
                    case "acc":
                        acc += count;
                        ip++;
                        break;
                    case "jmp":
                        ip += count;
                        break;
                }
                if (ip >=input.Count)
                {
                    return (acc, false);
                }
            }
        }
    }
}
