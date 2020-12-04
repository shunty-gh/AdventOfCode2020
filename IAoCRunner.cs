using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public interface IAoCRunner
    {
        int Day { get; }
        Task Execute(IConfiguration config, ILogger log, bool useTestData);
    }
}
