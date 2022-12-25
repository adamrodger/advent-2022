using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 25
    /// </summary>
    public class Day25
    {
        public string Part1(string[] input)
        {
            long total = 0;

            foreach (string line in input)
            {
                long n = 0;
                long power = 1;

                for (int i = line.Length - 1; i >= 0; i--)
                {
                    int d = line[i] switch
                    {
                        '2' => 2,
                        '1' => 1,
                        '0' => 0,
                        '-' => -1,
                        '=' => -2,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    n += d * power;
                    power *= 5;
                }

                total += n;
            }

            // reverse the process
            Stack<char> result = new();

            while (total > 0)
            {
                (total, long remainder) = Math.DivRem(total + 2, 5);

                result.Push(remainder switch
                {
                    0 => '=',
                    1 => '-',
                    2 => '0',
                    3 => '1',
                    4 => '2',
                    _ => throw new ArgumentOutOfRangeException()
                });
            }

            return new string(result.ToArray());
        }
    }
}
