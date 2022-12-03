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
                HashSet<char> first = line[..(line.Length/2)].ToHashSet();
                HashSet<char> second = line[(line.Length / 2)..].ToHashSet();

                first.IntersectWith(second);

                Debug.Assert(first.Count == 1);
                char common = first.First();

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
                HashSet<char> first = group[0].ToHashSet();
                HashSet<char> second = group[1].ToHashSet();
                HashSet<char> third = group[2].ToHashSet();

                first.IntersectWith(second);
                first.IntersectWith(third);

                Debug.Assert(first.Count == 1);
                char common = first.First();

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
