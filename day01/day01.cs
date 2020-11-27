using System;
using System.Threading.Tasks;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day01 : IAoCRunner
    {
        public int Day => 1;

        public async Task Execute(ILogger logger)
        {
            Console.WriteLine("This is day 1");
            await Task.CompletedTask;
        }
    }
}
