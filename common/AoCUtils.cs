using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shunty.AdventOfCode2020
{
    public static class AoCUtils
    {
        /// <summary>
        /// Find the file containing the text input for the given day. Try and download it
        /// from AoC if it can't be found on disk.
        /// </summary>
        /// <param name="day">The day number</param>
        /// <returns>A string containing the path name of the file, if found</returns>
        public static async Task<string> FindInput(int day)
        {
            var fname = $"day{day:D2}-input.txt";

            // Look in the current directory first
            var result = Path.Combine(".", fname);
            if (File.Exists(result))
                return result;

            // Look in a day specific directory
            result = Path.Combine(".", $"day{day:D2}", fname);
            if (File.Exists(result))
                return result;

            // Try and download it
            await DownloadInput(day, result);

            // Otherwise return blank
            return result;
        }

        /// <summary>
        /// Read and return the input for the given day (and optional part) as a list of strings
        /// </summary>
        /// <param name="day">The day number</param>
        /// <param name="part">An optional part number eg for the second part of the problem</param>
        public static async Task<IList<string>> GetDayLines(int day)
        {
            var fname = await FindInput(day);
            if (string.IsNullOrWhiteSpace(fname))
                throw new FileNotFoundException($"Unable to find input file for day {day}");

            return File.ReadAllLines(fname)
                .ToList();
        }

       public static async Task<IList<string>> GetTestLines(int day, int part = 0)
        {
            var fname = Path.Combine(".", $"day{day:D2}", $"day{day:D2}-input-test.txt");
            // If it doesn't exist just return an empty list
            if (!File.Exists(fname))
                return new List<string>();

            var result = await File.ReadAllLinesAsync(fname);
            return result.ToList();
        }

        /// <summary>
        /// Read and return the input for the given day (and optional part) as a single string
        /// </summary>
        /// <param name="day">The day number</param>
        /// <param name="part">An optional part number eg for the second part of the problem</param>
        public static async Task<string> GetDayText(int day)
        {
            var fname = await FindInput(day);
            if (string.IsNullOrWhiteSpace(fname))
                throw new FileNotFoundException($"Unable to find input file for day {day}");

            return await File.ReadAllTextAsync(fname);
        }

        static readonly Uri baseAddress = new Uri("https://adventofcode.com/");
        private static async Task DownloadInput(int day, string filename, int year = 2020)
        {
            // Need to have the session cookie available
            var cookie = Program.Configuration["sessionCookie"];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                throw new KeyNotFoundException("Session cookie not set. Cannot download daily input. Please add a 'sessionCookie' setting to appsettings.json or (better) to appsettings.private.json");
            }
            // Check the day
            var requiredDate = new DateTime(year, 12, day);
            if (DateTime.Today < requiredDate)
            {
                throw new FileNotFoundException($"Input file for day {day} will not be available yet. Have patience!");
            }

            var fragment = $"{year}/day/{day}/input";
            var cookies = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookies })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookies.Add(baseAddress, new Cookie("session", cookie));
                var response = await client.GetAsync(fragment);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException($"Input file for day {day} not found");
                }
                var content = await response.Content.ReadAsStringAsync();
                await File.WriteAllTextAsync(filename, content);
            }
        }
    }
}