using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shunty.AdventOfCode2020
{
    public class Day18 : AoCRunnerBase
    {
        public override async Task Execute(IConfiguration config, ILogger logger, bool useTestData)
        {
            var input = await GetInputLines(useTestData);

            ShowResult(1, Part1(input));
            ShowResult(2, Part2(input));
        }

        private Int64 ApplyOp(string op, Int64 n1, Int64 n2)
        {
            if (op == "+") return n1 + n2;
            else if (op == "-") return n1 - n2;
            else if (op == "*") return n1 * n2;
            //else if (op == "/") return n1 / n2;
            return 0;
        }

        private (int OpenParen, int CloseParen, int Value) ParseToken(string token)
        {
            // 0 or more '('; number; 0 or more ')'
            var op = token.Count(c => c == '(');
            var cp = token.Count(c => c == ')');
            var n = int.Parse(token.Substring(op, token.Length - op - cp));
            return (op, cp, n);
        }

        private Int64 Part1(IList<string> input)
        {
            var result = 0L;
            foreach (var line in input)
            {
                var splits = line.Split(' ');
                var si = 0;
                var stack = new Stack<(Int64, string)>();
                var op = "+";
                var sub = 0L;

                while (si < splits.Length)
                {
                    var (oc, cc, n) = ParseToken(splits[si++]);
                    for (var i = 0; i < oc; i++)
                    {
                        stack.Push((sub, op));
                        sub = 0;
                        op = "+";
                    }
                    sub = ApplyOp(op, sub, n);

                    for (var i = 0; i < cc; i++)
                    {
                        var (sub1, op1) = stack.Pop();
                        sub = ApplyOp(op1, sub1, sub);
                    }
                    // Next operator, if available
                    if (si < splits.Length)
                        op = splits[si++];
                }
                result += sub;
            }
            return result;
        }

        private Int64 Part2(IList<string> input)
        {
            var result = 0L;
            foreach (var line in input)
            {
                result += CalculateExpression(line);
            }
            return result;
        }

        private Int64 CalculateExpression(string expression)
        {
            var sub = 0L;
            int i = 0, space = 0;
            var op = "+";
            var stack = new List<Int64> { 0 };
            while (i < expression.Length)
            {
                // Read token - get number
                if (expression[i] == ' ')
                {
                    i++;
                    continue;
                }

                if (expression[i] == '(')
                {
                    var match = FindMatchingParen(expression, i);
                    var expr = expression.Substring(i + 1, match - i - 1);
                    sub = CalculateExpression(expr);
                    i = match + 1;
                }
                else
                {
                    space = expression.IndexOf(' ', i);
                    if (space >= 0)
                    {
                        sub = int.Parse(expression[i..space].Trim());
                        i = space + 1;
                    }
                    else
                    {
                        sub = int.Parse(expression[i..].Trim());
                        i = expression.Length;
                    }
                }

                if (op == "+")
                {
                    stack[stack.Count - 1] += sub;
                }
                else
                {
                    stack.Add(sub);
                }

                // Get next operator
                while (i < expression.Length && expression[i] == ' ')
                    i++;
                if (i < expression.Length)
                {
                    op = expression[i..(i+1)].Trim();
                    i++;
                }
            }
            var result = 1L;
            foreach (var v in stack)
            {
                result *= v;
            }
            return result;
        }

        private int FindMatchingParen(string expr, int openingParenPos)
        {
            var pc = 1;
            for (var i = openingParenPos + 1; i < expr.Length; i++)
            {
                if (expr[i] == '(') pc++;
                else if (expr[i] == ')') pc--;

                if (pc == 0)
                    return i;
            }
            return -1;
        }

    }
}
