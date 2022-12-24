using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 24
    /// </summary>
    public class Day24
    {
        private const char Open = '.';
        private const char Wall = '#';

        private const char Left = '<';
        private const char Right = '>';
        private const char Up = '^';
        private const char Down = 'v';

        private static readonly Point2D[] Deltas = { (0, -1), (0, 1), (-1, 0), (1, 0), (0, 0) }; // final delta is staying still

        public int Part1(string[] input)
        {
            Point2D start = (input.First().IndexOf('.'), 0);
            Point2D target = (input.Last().IndexOf('.'), input.Length - 1);

            Obstacles obstacles = FindObstacles(input);

            return ShortestPath(start, target, obstacles, 1);
        }
        public int Part2(string[] input)
        {
            Point2D start = (input.First().IndexOf('.'), 0);
            Point2D target = (input.Last().IndexOf('.'), input.Length - 1);

            Obstacles obstacles = FindObstacles(input);

            return ShortestPath(start, target, obstacles, 3);
        }

        /// <summary>
        /// Parse the obstacles on the course from the input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Course obstacles</returns>
        private static Obstacles FindObstacles(string[] input)
        {
            HashSet<Point2D> walls = new HashSet<Point2D>();

            ISet<Point2D>[] horizontalBlizzards = new ISet<Point2D>[input[0].Length - 2];
            for (int i = 0; i < horizontalBlizzards.Length; i++)
            {
                horizontalBlizzards[i] = new HashSet<Point2D>();
            }

            ISet<Point2D>[] verticalBlizzards = new ISet<Point2D>[input.Length - 2];
            for (int i = 0; i < verticalBlizzards.Length; i++)
            {
                verticalBlizzards[i] = new HashSet<Point2D>();
            }

            // pre-calculate location of each blizzard at each time index
            for (int y = 0; y < input.Length; y++)
            {
                string line = input[y];

                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == Open)
                    {
                        continue;
                    }

                    if (line[x] == Wall)
                    {
                        walls.Add((x, y));
                        continue;
                    }

                    Point2D blizzard = (x, y);

                    // I've done this the long-hand way because I'm too stupid to work out the up/left calculations with mod
                    switch (line[x])
                    {
                        case Up:
                            verticalBlizzards[0].Add(blizzard);

                            for (int t = 1; t < verticalBlizzards.Length; t++)
                            {
                                blizzard += (0, -1);

                                if (blizzard.Y < 1)
                                {
                                    blizzard = (x, input.Length - 2);
                                }

                                verticalBlizzards[t].Add(blizzard);
                            }

                            break;
                        case Down:
                            verticalBlizzards[0].Add(blizzard);

                            for (int t = 1; t < verticalBlizzards.Length; t++)
                            {
                                blizzard += (0, 1);

                                if (blizzard.Y >= input.Length - 1)
                                {
                                    blizzard = (x, 1);
                                }

                                verticalBlizzards[t].Add(blizzard);
                            }

                            break;
                        case Left:
                            horizontalBlizzards[0].Add(blizzard);

                            for (int t = 1; t < horizontalBlizzards.Length; t++)
                            {
                                blizzard += (-1, 0);

                                if (blizzard.X < 1)
                                {
                                    blizzard = (line.Length - 2, y);
                                }

                                horizontalBlizzards[t].Add(blizzard);
                            }

                            break;
                        case Right:
                            horizontalBlizzards[0].Add(blizzard);

                            for (int t = 1; t < horizontalBlizzards.Length; t++)
                            {
                                blizzard += (1, 0);

                                if (blizzard.X >= line.Length - 1)
                                {
                                    blizzard = (1, y);
                                }

                                horizontalBlizzards[t].Add(blizzard);
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return new Obstacles(walls, horizontalBlizzards, verticalBlizzards);
        }

        /// <summary>
        /// Find the shortest path from goal to target
        /// </summary>
        /// <param name="start">Start location</param>
        /// <param name="target">Target location</param>
        /// <param name="obstacles">Obstacles on the course</param>
        /// <param name="trips">Number of trips back and forth between start and target</param>
        /// <returns>Shortest path from source to target taking the given number of trips</returns>
        private static int ShortestPath(Point2D start, Point2D target, Obstacles obstacles, int trips)
        {
            var queue = new PriorityQueue<(Point2D Location, int Time), int>();
            queue.Enqueue((start, 0), start.ManhattanDistance(target));

            var visited = new HashSet<(Point2D, int)>();
            int targetsFound = 0;

            while (queue.TryDequeue(out (Point2D Location, int Time) current, out _))
            {
                if (current.Location == target)
                {
                    targetsFound++;

                    if (targetsFound == trips)
                    {
                        return current.Time;
                    }

                    // switch start and target and go back again
                    (target, start) = (start, target);

                    queue.Clear();
                    visited.Clear();

                    queue.Enqueue(current, start.ManhattanDistance(target));
                }

                if (!visited.Add(current))
                {
                    continue;
                }

                // check where the blizzards would be
                ISet<Point2D> vertical = obstacles.VerticalBlizzards[(current.Time + 1) % obstacles.VerticalBlizzards.Length];
                ISet<Point2D> horizontal = obstacles.HorizontalBlizzards[(current.Time + 1) % obstacles.HorizontalBlizzards.Length];

                // check where I can be given where the blizzards would be
                foreach (Point2D delta in Deltas)
                {
                    Point2D next = current.Location + delta;

                    if (!obstacles.Walls.Contains(next)
                     && !vertical.Contains(next)
                     && !horizontal.Contains(next)
                     && next.Y > -1)
                    {
                        queue.Enqueue((next, current.Time + 1), next.ManhattanDistance(target) + current.Time + 1);
                    }
                }
            }

            throw new InvalidOperationException("No path found");
        }

        /// <summary>
        /// Course obstacles
        /// </summary>
        /// <param name="Walls">Wall locations</param>
        /// <param name="HorizontalBlizzards">Location of horizontally-moving blizzards at each time index</param>
        /// <param name="VerticalBlizzards">Location of vertically-moving blizzards at each time index</param>
        private record Obstacles(ISet<Point2D> Walls,
                                 ISet<Point2D>[] HorizontalBlizzards,
                                 ISet<Point2D>[] VerticalBlizzards);
    }
}
