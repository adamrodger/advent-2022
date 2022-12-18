using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // start in the centre of the ball and expand outwards to get the internal points
            Point3D current = new(maxX - minX, maxY - minY, maxZ - minZ);
            Debug.Assert(!points.Contains(current));

            HashSet<Point3D> seen = new() { current };
            Queue<Point3D> queue = new();
            queue.Enqueue(current);

            while (queue.TryDequeue(out current))
            {
                foreach (Point3D adjacent in current.Adjacent6())
                {
                    if (adjacent.X < minX || adjacent.X > maxX || adjacent.Y < minY || adjacent.Y > maxY || adjacent.Z < minZ || adjacent.Z > maxZ)
                    {
                        continue;
                    }

                    if (!seen.Contains(adjacent) && !points.Contains(adjacent))
                    {
                        queue.Enqueue(adjacent);
                        seen.Add(adjacent);
                    }
                }
            }

            // get the bounding box of internal stuff
            /*int internalMinX = int.MaxValue,
                internalMaxX = int.MinValue,
                internalMinY = int.MaxValue,
                internalMaxY = int.MinValue,
                internalMinZ = int.MaxValue,
                internalMaxZ = int.MinValue;

            foreach (Point3D point in seen)
            {
                internalMinX = Math.Min(internalMinX, point.X);
                internalMaxX = Math.Max(internalMaxX, point.X);
                internalMinY = Math.Min(internalMinY, point.Y);
                internalMaxY = Math.Max(internalMaxY, point.Y);
                internalMinZ = Math.Min(internalMinZ, point.Z);
                internalMaxZ = Math.Max(internalMaxZ, point.Z);
            }*/

            int total = 0;

            foreach (Point3D point in points)
            {
                /*
                Point3D left = point with { X = point.X - 1 };
                Point3D right = point with { X = point.X + 1 };
                Point3D bottom = point with { Y = point.Y - 1 };
                Point3D top = point with { Y = point.Y + 1 };
                Point3D back = point with { Z = point.Z - 1 };
                Point3D front = point with { Z = point.Z + 1 };
                */

                foreach (Point3D adjacent in point.Adjacent6())
                {
                    if (!seen.Contains(adjacent) && !points.Contains(adjacent))
                    {
                        total++;
                    }
                }
            }

            // 3280 too high
            // 1998 too low
            return total;
        }
    }
}
