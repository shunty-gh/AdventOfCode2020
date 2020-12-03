using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shunty.AdventOfCode2020
{
    public static class AoCUtils
    {
        /// <summary>
        /// Find the file containing the text input for the given day
        /// </summary>
        /// <param name="day">The day number</param>
        /// <param name="part">An optional part number eg for the second part of the problem</param>
        /// <returns>A string containing the path name of the file, if found</returns>
        public static string FindInput(int day, int part = 0)
        {
            var parttxt = part > 0 ? $"-part{part}" : "";
            var fname = $"day{day:D2}{parttxt}-input.txt";

            // Look in the current directory first
            var result = Path.Combine(".", fname);
            if (File.Exists(result))
                return result;

            // Look in a day specific directory
            result = Path.Combine(".", $"day{day:D2}", fname);
            if (File.Exists(result))
                return result;

            // Otherwise return blank
            return "";
        }

        /// <summary>
        /// Read and return the input for the given day (and optional part) as a list of strings
        /// </summary>
        /// <param name="day">The day number</param>
        /// <param name="part">An optional part number eg for the second part of the problem</param>
        public static IList<string> GetDayLines(int day, int part = 0)
        {
            var fname = FindInput(day, part);
            if (string.IsNullOrWhiteSpace(fname))
                throw new FileNotFoundException($"Unable to find input file for day {day}{(part > 0 ? $" and part {part}" : "")}");

            return File.ReadAllLines(fname)
                .ToList();
        }

       public static IList<string> GetTestLines(int day, int part = 0)
        {
            var fname = FindInput(day, part);
            // Replace final part of file name
            fname = fname.Replace("-input.txt", "-input-test.txt");
            if (string.IsNullOrWhiteSpace(fname))
                throw new FileNotFoundException($"Unable to find input test file for day {day}{(part > 0 ? $" and part {part}" : "")}");

            return File.ReadAllLines(fname)
                .ToList();
        }

        /// <summary>
        /// Read and return the input for the given day (and optional part) as a single string
        /// </summary>
        /// <param name="day">The day number</param>
        /// <param name="part">An optional part number eg for the second part of the problem</param>
        public static string GetDayText(int day, int part = 0)
        {
            var fname = FindInput(day, part);
            if (string.IsNullOrWhiteSpace(fname))
                throw new FileNotFoundException($"Unable to find input file for day {day}{(part > 0 ? $" and part {part}" : "")}");

            return File.ReadAllText(fname);
        }
    }
}