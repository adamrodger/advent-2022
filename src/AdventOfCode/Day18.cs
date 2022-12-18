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
            HashSet<Point3D> points = new();
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue, minZ = int.MaxValue, maxZ = int.MinValue;

            foreach (string line in input)
            {
                int[] numbers = line.Numbers<int>();
                Point3D point = new Point3D(numbers[0], numbers[1], numbers[2]);
                points.Add(point);

                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
                minZ = Math.Min(minZ, point.Z);
                maxZ = Math.Max(maxZ, point.Z);
            }

            // need room to go around the extremes
            minX--;
            maxX++;
            minY--;
            maxY++;
            minZ--;
            maxZ++;

            // start on the outside of the ball and expand everywhere to get external points
            Point3D current = new(minX, minY, minZ);

            HashSet<Point3D> external = new() { current };
            Queue<Point3D> queue = new();
            queue.Enqueue(current);

            while (queue.TryDequeue(out current))
            {
                foreach (Point3D adjacent in current.Adjacent6())
                {
                    // stay in bounds
                    if (adjacent.X < minX || adjacent.X > maxX || adjacent.Y < minY || adjacent.Y > maxY || adjacent.Z < minZ || adjacent.Z > maxZ)
                    {
                        continue;
                    }

                    if (!external.Contains(adjacent) && !points.Contains(adjacent))
                    {
                        queue.Enqueue(adjacent);
                        external.Add(adjacent);
                    }
                }
            }

            int total = 0;

            foreach (Point3D point in points)
            {
                foreach (Point3D adjacent in point.Adjacent6())
                {
                    if (external.Contains(adjacent))
                    {
                        total++;
                    }
                }
            }
            
            return total;
        }
    }
}
