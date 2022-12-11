using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 3
    /// </summary>
    public class Day3
    {
        public int Part1(string[] input)
        {
            int sum = 0;

            foreach (string line in input)
            {
                IEnumerable<char> first = line[..(line.Length/2)];
                IEnumerable<char> second = line[(line.Length / 2)..];

                char common = first.Intersect(second).First();;

                if (common is >= 'a' and <= 'z')
                {
                    sum += common - 'a' + 1;
                }
                else
                {
                    sum += common - 'A' + 27;
                }
            }

            return sum;
        }

        public int Part2(string[] input)
        {
            int sum = 0;

            foreach (string[] group in input.Chunk(3))
            {
                IEnumerable<char> first = group[0];
                IEnumerable<char> second = group[1];
                IEnumerable<char> third = group[2];

                char common = first.Intersect(second).Intersect(third).First();

                if (common is >= 'a' and <= 'z')
                {
                    sum += common - 'a' + 1;
                }
                else
                {
                    sum += common - 'A' + 27;
                }
            }

            return sum;
        }
    }
}
