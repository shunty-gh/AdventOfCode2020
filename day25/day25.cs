using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day25 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputInts(useTestData);

            var sn = 7;
            var cardPK = input[0];
            var doorPK = input[1];
            var cardLoop = FindLoopCount(sn, cardPK);
            var doorLoop = FindLoopCount(sn, doorPK);

            var ek = Transform(cardPK, doorLoop);
            //var ek2 = Transform(doorPK, cardLoop);
            //System.Diagnostics.Debug.Assert(ek == ek2);

            ShowResult(1, ek);
            //ShowResult(2, part2);
        }

        const int Divisor = 20201227;
        private int FindLoopCount(int sn, int target)
        {
            /*
              - Set the value to itself multiplied by the subject number.
              - Set the value to the remainder after dividing the value by 20201227.
             */

            var curr = 1;
            for (var i = 0; curr != target; i++)
            {
                curr = (sn * curr) % Divisor;
                if (curr == target)
                    return i + 1;
            }
            return -1;
        }

        private Int64 Transform(int sn, int loopCount)
        {
            /*
              - Set the value to itself multiplied by the subject number.
              - Set the value to the remainder after dividing the value by 20201227.
             */

            var curr = 1L;
            for (var i = 0; i < loopCount; i++)
            {
                curr = (sn * curr) % Divisor;
            }
            return curr;
        }
    }
}
