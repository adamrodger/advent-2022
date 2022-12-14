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
                string[] numbers = line.Split(',');
                Point3D point = new Point3D(int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
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
            int total = 0;
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue, minZ = int.MaxValue, maxZ = int.MinValue;

            foreach (string line in input)
            {
                string[] numbers = line.Split(',');
                Point3D point = new Point3D(int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
                points.Add(point);

                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
                minZ = Math.Min(minZ, point.Z);
                maxZ = Math.Max(maxZ, point.Z);
            }

            // need room to go around the extremes
            minX--; maxX++;
            minY--; maxY++;
            minZ--; maxZ++;

            // start on the outside of the ball and expand everywhere to find external faces
            Point3D current = new(minX, minY, minZ);

            HashSet<Point3D> seen = new() { current };
            Queue<Point3D> queue = new();
            queue.Enqueue(current);

            while (queue.TryDequeue(out current))
            {
                foreach (Point3D adjacent in current.Adjacent6())
                {
                    if (adjacent.X < minX || adjacent.X > maxX || adjacent.Y < minY || adjacent.Y > maxY || adjacent.Z < minZ || adjacent.Z > maxZ)
                    {
                        // stay in bounds
                        continue;
                    }

                    if (seen.Contains(adjacent))
                    {
                        // don't revisit
                        continue;
                    }

                    if (points.Contains(adjacent))
                    {
                        // found an external face
                        total++;
                    }
                    else
                    {
                        // explore into more open space
                        queue.Enqueue(adjacent);
                        seen.Add(adjacent);
                    }
                }
            }
            
            return total;
        }
    }
}
