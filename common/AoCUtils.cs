using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

    public static class StringHelpers
    {
        /// <summary>
        /// Replace multiple strings with specific replacements. Might be faster than repeated
        /// use of .Replace() depending on number of replacements and context etc.
        /// WARNING: Will only work for simple match strings
        /// </summary>
        /// <param name="text">The source string</param>
        /// <param name="replacements">Dictionary of match:replacment pairs</param>
        /// <returns>A new string with the matched texts replaced</returns>
        public static string MultiReplace(this string text, Dictionary<string,string> replacements) {
            return Regex.Replace(text,
                "(" + string.Join("|", replacements.Keys.ToArray()) + ")",
                m => replacements[m.Value]
            );
        }

        public static string ReverseString(this string src)
        {
            var arr = src.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Rotate a list of strings clockwise by a number of 90 degree increments
        /// </summary>
        /// <param name="rows">The source list of strings</param>
        /// <param name="rotateBy">How many increments of 90 degrees clockwise to rotate</param>
        /// <returns>A new list of strings</returns>
        public static List<string> Rotate(this List<string> rows, int rotateBy = 1)
        {
            // Assume each row has the same length
            var ylen = rows.Count;
            var xlen = rows[0].Length;
            var result = new List<string>(rows);

            var rot = rotateBy % 4;
            if (rotateBy < 0)
            {
                // C# treats (-3 % 4) as -3, not +1
                rot = (rot + 4) % 4;
            }
            if (rot > 0)
            {
                var sb = new StringBuilder();
                while (rot > 0)
                {
                    var next = new List<string>();
                    for (var x = 0; x < xlen; x++)
                    {
                        sb.Clear();
                        for (var y = ylen - 1; y >= 0; y--)
                        {
                            sb.Append(result[y][x]);
                        }
                        next.Add(sb.ToString());
                    }
                    result = next;
                    rot--;
                }
            }
            return result;
        }

        public static List<string> FlipAlongHorizontal(this List<string> rows)
        {
            var result = new List<string>(rows);
            result.Reverse();
            return result;
        }

        public static List<string> FlipAlongVertical(this List<string> rows)
        {
            var result = new List<string>();
            for (var i = 0; i < rows.Count; i++)
            {
                result.Add(rows[i].ReverseString());
            }
            return result;
        }

        /// <summary>
        /// Get a string representing an edge of a rectangular "grid" of strings. The edge returned is
        /// taken in a clockwise direction so that, eg when rotated the edge value will stay the same.
        /// ie Top edge is read L to R, bottom edge is R to L. L edge is bottom to top whereas R edge is
        /// top to bottom.
        /// </summary>
        /// <param name="rows">The source list of strings</param>
        /// <param name="edgeNo">The edge number we require. 0 = top edge, 1 = right, 2 = bottom, 3 = left</param>
        /// <returns></returns>
        public static string GetEdge(this List<string> rows, int edgeNo)
        {
            if (rows.Count == 0)
                return "";

            // Assume all rows are the same length
            var rlen = rows[0].Length;
            switch (edgeNo)
            {
                case 0:
                    return rows[0];
                case 1:
                    return new string(rows.Select(r => r[rlen - 1]).ToArray());
                case 2:
                    return rows.Last().ReverseString();
                case 3:
                    return new string(rows.Select(r => r[0]).Reverse().ToArray());
                default:
                    return "";
            }
        }

        public static (int, bool) FindEdge(this List<string> rows, string edgeToFind)
        {
            for (var i = 0; i < 4; i++)
            {
                var e = rows.GetEdge(i);
                if (e == edgeToFind)
                {
                    return (i, false);
                }
                else if (e.ReverseString() == edgeToFind)
                {
                    return (i, true);
                }
            }
            return (-1, false);
        }
    }
}