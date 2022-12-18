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
                Point3D left = point with { X = point.X - 1 };
                Point3D right = point with { X = point.X + 1 };
                Point3D bottom = point with { Y = point.Y - 1 };
                Point3D top = point with { Y = point.Y + 1 };
                Point3D back = point with { Z = point.Z - 1 };
                Point3D front = point with { Z = point.Z + 1 };

                if (points.Contains(left) && !points.Contains(right))
                {
                    total++;
                }

                if (points.Contains(right) && !points.Contains(left))
                {
                    total++;
                }

                if (points.Contains(bottom) && !points.Contains(top))
                {
                    total++;
                }

                if (points.Contains(top) && !points.Contains(bottom))
                {
                    total++;
                }

                if (points.Contains(back) && !points.Contains(front))
                {
                    total++;
                }

                if (points.Contains(front) && !points.Contains(back))
                {
                    total++;
                }

                // hmmmm, could be a 'void' left and right but still be internal
            }

            // 3280 too high
            return total;
        }
    }
}
