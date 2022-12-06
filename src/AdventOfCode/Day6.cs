using System.Collections.Generic;
using System.Linq;
using MoreLinq;

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
            int i = size;

            foreach (IList<char> chars in input.Window(size))
            {
                if (chars.Distinct().Count() == size)
                {
                    break;
                }

                i++;
            }

            return i;
        }
    }
}
