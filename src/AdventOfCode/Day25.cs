using System;
using System.Collections.Generic;

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

                for (int i = line.Length - 1; i >= 0; i--)
                {
                    int d = line[line.Length - 1 - i] switch
                    {
                        '2' => 2,
                        '1' => 1,
                        '0' => 0,
                        '-' => -1,
                        '=' => -2,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    n += d * (long)Math.Pow(5, i);
                }

                total += n;
            }

            // reverse the process
            List<char> result = new();

            while (total > 0)
            {
                (total, long remainder) = Math.DivRem(total + 2, 5);

                result.Add(remainder switch
                {
                    0 => '=',
                    1 => '-',
                    2 => '0',
                    3 => '1',
                    4 => '2',
                    _ => throw new ArgumentOutOfRangeException()
                });
            }

            result.Reverse();

            return new string(result.ToArray());
        }
    }
}
