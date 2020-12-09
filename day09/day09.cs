using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day09 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputInt64(useTestData);
            var part1 = 0L;
            var len = useTestData ? 5 : 25;

            for (var i = len; i < input.Count; i++)
            {
                if (!CheckSum(input, i - len, len, input[i]))
                {
                    part1 = input[i];
                    break;
                }
            }
            ShowResult(1, part1);

            var part2 = FindContiguous(input, part1);
            ShowResult(2, part2);
        }

        private bool CheckSum(IList<Int64> source, int startIndex, int length, Int64 target)
        {
            for (var i = startIndex; i < startIndex + length; i++)
            {
                if (source[i] >= target)
                    continue;

                for (var j = startIndex; j < startIndex + length; j++)
                {
                    if (i == j)
                        continue;
                    if (source[i] + source[j] == target)
                        return true;
                }
            }
            return false;
        }

        private Int64 FindContiguous(IList<Int64> source, Int64 target)
        {
            int lb, ub = 0;
            for (lb = 0; lb < source.Count; lb++)
            {
                Int64 sum = 0, min = source[lb], max = source[lb];
                ub = lb;
                while (sum < target && ub < source.Count)
                {
                    if (source[ub] < min) min = source[ub];
                    else if (source[ub] > max) max = source[ub];

                    sum += source[ub++];
                }
                if (sum == target)
                    return min + max;
            }
            return 0;
        }
    }
}
