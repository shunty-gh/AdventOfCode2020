using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day16 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

#region Parse input
            var i = 0;
            var fields = new Dictionary<string, int[]>();
            while (true)
            {
                var line = input[i++];
                if (string.IsNullOrWhiteSpace(line))
                    break;
                var splits = line.Split(": ");
                var nums = splits[1].Split(" or ")
                    .SelectMany(s => s.Split('-'))
                    .Select(n => int.Parse(n));
                fields[splits[0]] = nums.ToArray();
            }

            int[] myticket;
            while (true)
            {
                var line = input[i++];
                if (string.IsNullOrWhiteSpace(line)) // Skip blank lines, if any
                    continue;
                if (line.StartsWith("your ticket"))
                {
                    myticket = input[i++].Split(',').Select(x => int.Parse(x)).ToArray();
                    break;
                }
            }

            var nearbytickets = new List<int[]>();
            while (i < input.Count)
            {
                var line = input[i++];
                if (string.IsNullOrWhiteSpace(line)) // Skip blank lines, if any
                    continue;
                if (line.StartsWith("nearby"))
                    continue;
                var ticket = line.Split(',').Select(x => int.Parse(x)).ToArray();
                nearbytickets.Add(ticket);
            }
#endregion

            var invalid = new List<int>();
            var validtickets = new List<int[]>();
            // Create an array to hold all allowed field values
            // Makes it much quicker to check if a value is acceptable or not
            var maxvalue = fields.Max(f => f.Value.Max());
            var allowablevalues = new bool[maxvalue + 1];
            foreach (var fv in fields.Values)
            {
                for (var j = fv[0]; j <= fv[3]; j++)
                {
                    if (j <= fv[1] || j >= fv[2])
                        allowablevalues[j] = true;
                }
            }

            var part1 = 0;
            foreach (var ticket in nearbytickets)
            {
                var bad = false;
                foreach (var tv in ticket)
                {
                    if (tv > maxvalue || !allowablevalues[tv])
                    {
                        bad = true;
                        part1 += tv;
                    }
                }
                if (!bad)
                {
                    validtickets.Add(ticket);
                }
            }
            ShowResult(1, part1);
            ShowResult(2, Part2(fields, validtickets, myticket));
        }

        private Int64 Part2(Dictionary<string, int[]> fields, List<int[]> validTickets, int[] myTicket)
        {
            // Assume all data is valid - ie each field value has 4
            // elements and each ticket has the same number of values as there are fields
            var fcount = fields.Count;

            // From observation most tickets appear to have one value that can't fit one
            // field. All other values in each ticket seem to be able to fit any field.
            // Need to get a list of the positions for which a field cannot be in

            // Initialise the "cantbe" store to save having to check each time round
            var cantbe = fields.Select(kvp => kvp.Key)
                .ToDictionary(s => s, _ => new List<int>());

            // ...for each field value within each ticket... check if value is allowed in each field...
            for (var ti = 0; ti < validTickets.Count; ti++)
            {
                var ticket = validTickets[ti];
                for (var fieldindex = 0; fieldindex < fcount; fieldindex++)
                {
                    var tfv = ticket[fieldindex];
                    foreach (var (fieldname,v) in fields)
                    {
                        if ((tfv >= v[0] && tfv <= v[1]) || (tfv >= v[2] && tfv <= v[3]))
                        {
                            continue;
                        }
                        cantbe[fieldname].Add(fieldindex);
                    }
                }
            }

            // Now sort the "can't be" fields by magnitude and do a simple
            // process of elimination to find out what each field *can* be
            var cantbe2 = cantbe.Select(kvp => (kvp.Key, FieldIndexes: kvp.Value))
                .OrderByDescending(x => x.FieldIndexes.Count());
            var mustbe = new List<(string Field, int FieldIndex)>();

            foreach (var cb in cantbe2)
            {
                for (var i = 0; i < fcount; i++)
                {
                    if (!cb.FieldIndexes.Contains(i)) // find missing field indexes
                    {
                        if (!mustbe.Any(m => m.FieldIndex == i)) // have we already allocated it
                        {
                            mustbe.Add((cb.Key, i));
                        }
                    }
                }
            }

            // Result is the product of the values of fields beginning with "departure" in
            // my ticket
            var result = mustbe.Where(m => m.Field.StartsWith("departure"))
                .Select(m => (Int64)m.FieldIndex)
                .Aggregate(1L, (acc, fi) => acc * myTicket[fi]);
            return result;
        }
    }
}
