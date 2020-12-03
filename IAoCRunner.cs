using System;
using System.Threading.Tasks;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public interface IAoCRunner
    {
        int Day { get; }
        Task Execute(ILogger log, bool useTestData);
    }
}
