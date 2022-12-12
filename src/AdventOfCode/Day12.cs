using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        private static readonly Point2D[] Deltas =
        {
            (0, -1),
            (-1, 0),
            (1, 0),
            (0, 1)
        };

        public int Part1(string[] input)
        {
            Point2D? start = (0, 0);
            Point2D? end = (0, 0);

            // find top of hill and start point
            for (int y = 0; y < input.Length; y++)
            {
                string row = input[y];

                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == 'S')
                    {
                        start = (x, y);
                        char[] temp = row.ToCharArray();
                        temp[x] = 'a';
                        row = new string(temp);
                        input[y] = row;
                    }
                    else if (row[x] == 'E')
                    {
                        end = (x, y);
                        char[] temp = row.ToCharArray();
                        temp[x] = 'z';
                        row = new string(temp);
                        input[y] = row;
                    }
                }
            }

            // path from top of hill to start
            return FindPath(input, end.Value, new HashSet<Point2D> { start.Value });
        }

        public int Part2(string[] input)
        {
            ISet<Point2D> targets = new HashSet<Point2D>();
            Point2D? start = (0, 0);

            // find top of hill and all 'a' points
            for (int y = 0; y < input.Length; y++)
            {
                string row = input[y];

                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == 'a')
                    {
                        targets.Add((x, y));
                    }
                    else if (row[x] == 'E')
                    {
                        start = (x, y);
                        char[] temp = row.ToCharArray();
                        temp[x] = 'z';
                        row = new string(temp);
                        input[y] = row;
                    }
                }
            }

            // path from top of hill to first 'a' point
            return FindPath(input, start.Value, targets);
        }

        /// <summary>
        /// Find a path from the start to one of the given targets
        /// </summary>
        /// <param name="input">Input grid</param>
        /// <param name="start">Start point</param>
        /// <param name="targets">Targets</param>
        /// <returns>Path cost</returns>
        /// <exception cref="InvalidOperationException">No path found</exception>
        private static int FindPath(string[] input, Point2D start, ISet<Point2D> targets)
        {
            // find path from E to the first 'a' we meet
            Queue<(Point2D Point, int Cost)> queue = new();
            HashSet<Point2D> visited = new() { start };

            queue.Enqueue((start, 0));

            while (queue.TryDequeue(out var current))
            {
                if (targets.Contains(current.Point))
                {
                    return current.Cost;
                }

                char currentValue = input[current.Point.Y][current.Point.X];

                foreach (Point2D delta in Deltas)
                {
                    Point2D next = current.Point + delta;

                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    // stay in bounds
                    if (next.X < 0 || next.Y < 0 || next.X >= input[0].Length || next.Y >= input.Length)
                    {
                        continue;
                    }

                    char nextValue = input[next.Y][next.X];

                    if (nextValue - currentValue >= -1)
                    {
                        visited.Add(next);
                        queue.Enqueue((next, current.Cost + 1));
                    }
                }
            }

            throw new InvalidOperationException("No path found");
        }
    }
}
