using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 1
    /// </summary>
    public class Day1
    {
        public int Part1(string[] input)
        {
            int elf = 0;
            int max = int.MinValue;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    max = Math.Max(max, elf);
                    elf = 0;
                }
                else
                {
                    elf += int.Parse(line);
                }
            }

            return max;
        }

        public int Part2(string[] input)
        {
            int elf = 0;
            var elves = new List<int>();

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(elf);
                    elf = 0;
                }
                else
                {
                    elf += int.Parse(line);
                }
            }

            return elves.OrderByDescending(i => i).Take(3).Sum();
        }
    }
}
