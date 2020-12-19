using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day19 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            var getrules = true;
            var rules = new Dictionary<string,string>();
            var msgs = new List<string>();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    getrules = false;
                    continue;
                }
                if (getrules)
                {
                    var parts = line.Split(':');
                    rules[parts[0]] = parts[1].Trim();
                }
                else
                {
                    msgs.Add(line.Trim());
                }
            }

            ShowResult(1, GetValidMessages(rules, msgs, 1));
            ShowResult(2, GetValidMessages(rules, msgs, 2));
        }

        private int GetValidMessages(Dictionary<string,string> rules, IList<string> messages, int part)
        {
            var exprule = ExpandRule(rules["0"], rules, part);

            var re = new Regex("^" + exprule.Replace(" ", "") + "$");
            var badcount = 0;
            foreach (var msg in messages)
            {
                if (!re.IsMatch(msg))
                {
                    badcount++;
                }
            }
            return messages.Count - badcount;
        }

        private string ExpandRule(string rule, Dictionary<string,string> rules, int part)
        {
            var result = "";
            if (rule.Contains("|"))
            {
                var morerules = rule.Split("|");
                foreach (var r in morerules)
                {
                    if (string.IsNullOrWhiteSpace(result))
                        result += " ( (" + ExpandRule(r, rules, part) + ") ";
                    else
                        result += " | (" + ExpandRule(r, rules, part) + ") ";
                }
                result += " ) ";
            }
            else
            {
                var morerules = rule.Split(' ');
                if (morerules.Count() == 1)
                {
                    var r = morerules[0].Trim();
                    if (int.TryParse(r, out var num))
                    {
				        var next = rules[r];
                        if (part == 2)
                        {
                            // Part 2 changes:
                            //   8: 42    =>  8: 42 | 42 8        => 42 42 8...42 42 42 8 => (42)+ 8
                            //  11: 42 31 => 11: 42 31 | 42 11 31 => 42 42 11 31 31... 42 42 42 11 31 31 31
                            if (r == "8")
                            {
                                var tmp = "(" + ExpandRule(next, rules, part) + ")+ ";
                                return tmp;
                            }
                            else if (r == "11")
                            {
                                //var r42 = ExpandRule("42", rules, part);
                                //var r31 = ExpandRule("31", rules);
                                //return "((" + r42 + r31 + ")"
                                //	+ "|(" + r42 + r42 + r31 + r31 + ")"
                                //	+ "|(" + r42 + r42 + r42 + r31 + r31 + r31 + ")"
                                //	+ "|(" + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + ")"
                                //	+ "|(" + r42 + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + r31 + ")"
                                //	+ "|(" + r42 + r42 + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + r31 + r31 + "))";
                                var replaced = "42 31 "
                                    + "| 42 42 31 31 "
                                    + "| 42 42 42 31 31 31 "
                                    + "| 42 42 42 42 31 31 31 31 "
                                    + "| 42 42 42 42 42 31 31 31 31 31"
                                    + "| 42 42 42 42 42 42 31 31 31 31 31 31"
                                    + "| 42 42 42 42 42 42 42 31 31 31 31 31 31 31";
                                return ExpandRule(replaced, rules, part);
                            }
                        }
                        return ExpandRule(next, rules, part);
                    }
                    // otherwise it's a letter. EOT.
                    return morerules[0].Trim('"');
                }
                else
                {
                    foreach (var r in morerules)
                    {
                        result += ExpandRule(r, rules, part);
                    }
                }
            }
            return result;
        }
    }
}
