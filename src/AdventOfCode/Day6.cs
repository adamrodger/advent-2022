using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 6
    /// </summary>
    public class Day6
    {
        public int Part1(string[] input) => IndexOfDistinctRegion(input.First(), 4);

        public int Part2(string[] input) => IndexOfDistinctRegion(input.First(), 14);

        private static int IndexOfDistinctRegion(string input, int size)
        {
            ISet<char> seen = new HashSet<char>();
            int i = size;
            ReadOnlySpan<char> span = input.AsSpan();

            while (true)
            {
                ReadOnlySpan<char> slice = span[(i - size)..i];

                foreach (char c in slice)
                {
                    if (!seen.Add(c))
                    {
                        break;
                    }
                }

                if (seen.Count == size)
                {
                    return i;
                }

                i++;
                seen.Clear();
            }
        }
    }
}
