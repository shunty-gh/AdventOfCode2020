using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day04 : AoCRunnerBase
    {
        static readonly string[] RequiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        static readonly string[] EyeColours = new string[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines();
            int part1 = 0, part2 = 0;
            var current = new Dictionary<string, string>();
            int lineindex = 0, maxindex = input.Count;
            while (lineindex < maxindex)
            {
                var line = input[lineindex++];

                if (!string.IsNullOrWhiteSpace(line))
                {
                    var parts = line.Split(' ');
                    foreach (var part in parts)
                    {
                        var field = part.Split(':');
                        current[field[0]] = field[1];
                    }
                }

                if (string.IsNullOrWhiteSpace(line) || lineindex >= maxindex)
                {
                    if (RequiredFields.All(f => current.ContainsKey(f)))
                    {
                        part1++;
                        part2 += AreAllValid(current) ? 1 : 0;
                    }
                    current.Clear();
                }
            }

            ShowResult(1, part1);
            ShowResult(2, part2);
        }

        private bool AreAllValid(Dictionary<string, string> fields)
        {
            // Check rules for each element. Assume that all required fields are supplied.
            int i;
            var result = true;
            foreach ((var key, var v) in fields)
            {
                switch (key)
                {
                    case "byr":
                        result &= (int.TryParse(v, out i) && i >= 1920 && i <= 2002);
                        break;
                    case "iyr":
                        result &= (int.TryParse(v, out i) && i >= 2010 && i <= 2020);
                        break;
                    case "eyr":
                        result &= (int.TryParse(v, out i) && i >= 2020 && i <= 2030);
                        break;
                    case "hgt":
                        result &= (v.EndsWith("cm") || v.EndsWith("in"));
                        if (result)
                        {
                            var units = v.Substring(v.Length - 2);
                            var value = int.Parse(v.Substring(0, v.Length - 2));
                            result &= units == "cm"
                                ? (value >= 150 && value <= 193)
                                : (value >= 59 && value <= 76);
                        }
                        break;
                    case "hcl":
                        result &= (v.StartsWith("#")
                            && (v.Length == 7)
                            && v.Substring(1)
                                .All(c => Uri.IsHexDigit(c))
                            );
                        break;
                    case "ecl":
                        result &= EyeColours.Contains(v);
                        break;
                    case "pid":
                        result &= ((v.Length == 9) && v.All(c => Char.IsDigit(c)));
                        break;
                }
            }
            return result;
        }
    }
}
