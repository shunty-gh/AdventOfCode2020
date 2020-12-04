using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;

namespace Shunty.AdventOfCode2020
{
    public class Day04 : IAoCRunner
    {
        public int Day => 4;

        public async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            await AoCUtils.GetDayInput(config, Day, "");

            var input = useTestData
                ? AoCUtils.GetTestLines(Day)
                : AoCUtils.GetDayLines(Day);
            if (!input.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error Day [blue]{Day}[/]; Part [blue]1[/]: No {(useTestData ? "test " : "")}input available[/]");
            }

            int part1 = 0, part2 = 0;
            string byr = "", iyr = "", eyr = "",hgt = "",hcl = "",ecl = "",pid = "";
            bool hasbyr = false, hasiyr = false, haseyr = false, hashgt = false, hashcl = false, hasecl = false, haspid = false;
            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                {

                    if (hasbyr && hasiyr && haseyr && hashgt && hashcl && hasecl && haspid)
                    {
                        part1++;
                        part2 += AreAllValid(byr, iyr, eyr, hgt, hcl, ecl, pid) ? 1 : 0;
                    }
                    // Reset
                    hasbyr = false;
                    hasiyr = false;
                    haseyr = false;
                    hashgt = false;
                    hashcl = false;
                    hasecl = false;
                    haspid = false;
                    continue;
                }

                if (line.Contains("byr"))
                {
                    hasbyr = true;
                    byr = GetFieldValue(line, "byr");
                }
                if (line.Contains("iyr"))
                {
                    hasiyr = true;
                    iyr = GetFieldValue(line, "iyr");
                }
                if (line.Contains("eyr"))
                {
                    haseyr = true;
                    eyr = GetFieldValue(line, "eyr");
                }
                if (line.Contains("hgt"))
                {
                    hashgt = true;
                    hgt = GetFieldValue(line, "hgt");
                }
                if (line.Contains("hcl"))
                {
                    hashcl = true;
                    hcl = GetFieldValue(line, "hcl");
                }
                if (line.Contains("ecl"))
                {
                    hasecl = true;
                    ecl = GetFieldValue(line, "ecl");
                }
                if (line.Contains("pid"))
                {
                    haspid = true;
                    pid = GetFieldValue(line, "pid");
                }
                // Ignore cid field
            }
            // Check the last line too
            if (hasbyr && hasiyr && haseyr && hashgt && hashcl && hasecl && haspid)
            {
                part1++;
                part2 += AreAllValid(byr, iyr, eyr, hgt, hcl, ecl, pid) ? 1 : 0;
            }

            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]1[/]:[/] [yellow]{part1}[/]");
            AnsiConsole.MarkupLine($"[white]Day [blue]{Day}[/]; Part [blue]2[/]:[/] [yellow]{part2}[/]");

            await Task.CompletedTask;
        }

        private string GetFieldValue(string line, string fieldId)
        {
            if (string.IsNullOrWhiteSpace(line))
                return "";

            var start = line.IndexOf(fieldId);
            if (start < 0)
                return "";
            start += (fieldId.Length + 1); // Skip the field and the trailing ':'

            // Return up to the next ' ' or to the end of the line
            var nextsp = line.IndexOf(" ", start);
            if (nextsp < 0)
                return line.Substring(start);
            return line.Substring(start, nextsp - start);
        }

        private bool AreAllValid(string byr, string iyr, string eyr, string hgt, string hcl, string ecl, string pid)
        {
            // Check rules for each element
            bool isvalid = true;
            isvalid &= (int.Parse(byr) >= 1920 && int.Parse(byr) <= 2002);
            isvalid &= (int.Parse(iyr) >= 2010 && int.Parse(iyr) <= 2020);
            isvalid &= (int.Parse(eyr) >= 2020 && int.Parse(eyr) <= 2030);
            if (hgt.EndsWith("cm") || hgt.EndsWith("in"))
            {
                var units = hgt.Substring(hgt.Length - 2);
                var value = int.Parse(hgt.Substring(0, hgt.Length - 2));
                isvalid &= units == "cm"
                    ? (value >= 150 && value <= 193)
                    : (value >= 59 && value <= 76);
            }
            else
            {
                isvalid = false;
            }
            isvalid &= (hcl.StartsWith("#") && (hcl.Length == 7) && hcl.Substring(1).All(c => Char.IsDigit(c) || (c >= 'a' && c <= 'f')));
            isvalid &= ((new string[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}).Contains(ecl));
            isvalid &= ((pid.Length == 9) && pid.All(c => Char.IsDigit(c)));

            return isvalid;
        }
    }
}
