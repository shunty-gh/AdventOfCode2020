using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day12 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            ShowResult(1, Part1(input));
            ShowResult(2, Part2(input));
        }

        private int Part1(IList<string> input)
        {
            int x = 0, y = 0, facing = 90;
            foreach (var dir in input)
            {
                if (string.IsNullOrWhiteSpace(dir))
                    continue;

                var d = dir[0];
                var i = int.Parse(dir.Substring(1));
                switch (d)
                {
                    case 'N':
                        y += i;
                        break;
                    case 'S':
                        y -= i;
                        break;
                    case 'W':
                        x -= i;
                        break;
                    case 'E':
                        x += i;
                        break;
                    case 'L':
                        facing = (facing + 360 - i) % 360;
                        break;
                    case 'R':
                        facing = (facing + i) % 360;
                        break;
                    case 'F':
                        switch (facing)
                        {
                            case 0:
                                y += i;
                                break;
                            case 90:
                                x += i;
                                break;
                            case 180:
                                y -= i;
                                break;
                            case 270:
                                x -= i;
                                break;
                            default:
                                throw new Exception($"Don't know how to handle angle of {facing} degrees");
                        }
                        break;
                    default:
                        throw new Exception($"Unknown direction command {d}");
                }
            }
            return Math.Abs(x) + Math.Abs(y);
        }

        private Int64 Part2(IList<string> input)
        {
            Int64 x = 0, y = 0, wayX = 10, wayY = 1;
            foreach (var dir in input)
            {
                if (string.IsNullOrWhiteSpace(dir))
                    continue;

                var d = dir[0];
                var i = int.Parse(dir.Substring(1));
                switch (d)
                {
                    case 'N':
                        wayY += i;
                        break;
                    case 'S':
                        wayY -= i;
                        break;
                    case 'W':
                        wayX -= i;
                        break;
                    case 'E':
                        wayX += i;
                        break;
                    case 'L':
                        (wayX, wayY) = Rotate(wayX, wayY, x, y, i);
                        break;
                    case 'R':
                        (wayX, wayY) = Rotate(wayX, wayY, x, y, 360 - i);
                        break;
                    case 'F':
                        // Move 'i' times in the direction of the waypoint
                        Int64 dx = wayX - x, dy = wayY - y;
                        x += (dx * i);
                        y += (dy * i);
                        // Move the waypoint to maintain its distance from the ship
                        wayX = x + dx;
                        wayY = y + dy;
                        break;
                    default:
                        throw new Exception($"Unknown direction command {d}");
                }
            }
            return Math.Abs(x) + Math.Abs(y);
        }

        // Rotate the waypoint about the ship
        private (Int64,Int64) Rotate(Int64 wX, Int64 wY, Int64 sX, Int64 sY, int angle)
        {
            // Get waypoint co-ords relative to the ship
            Int64 x = wX - sX, y = wY - sY;
            // Only deal with 0, 90, 180,270
            (Int64 X, Int64 Y) result;
            switch (angle)
            {
                case 0:
                case 360:
                    result = (x,y);
                    break;
                case 90:
                    result = (-y,x);
                    break;
                case 180:
                    result = (-x,-y);
                    break;
                case 270:
                    result = (y,-x);
                    break;
                default:
                    throw new Exception($"Unhandled rotation for angle {angle}");
            }
            // Add the ship co-ords back in
            return (result.X + sX, result.Y + sY);
        }
    }
}
