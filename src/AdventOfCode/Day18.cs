using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 18
    /// </summary>
    public class Day18
    {
        public int Part1(string[] input)
        {
            HashSet<Point3D> points = new();

            foreach (string line in input)
            {
                int[] numbers = line.Numbers<int>();
                Point3D point = new Point3D(numbers[0], numbers[1], numbers[2]);
                points.Add(point);
            }

            int total = 0;

            foreach (Point3D point in points)
            {
                foreach (Point3D adjacent in point.Adjacent6())
                {
                    if (!points.Contains(adjacent))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
