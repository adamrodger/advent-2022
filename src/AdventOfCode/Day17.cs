using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 17
    /// </summary>
    public class Day17
    {
        private const int LeftEdge = 0;
        private const int RightEdge = 6;
        private const char LeftMove = '<';
        private const char RightMove = '>';

        private static readonly Shape[] Shapes =
        {
            new("line", new Point2D[]
            {
                new(0,0), new(1,0), new(2,0), new(3,0)
            }),
            new("plus", new Point2D[]
            {
                          new(1,2),
                new(0,1), new(1,1), new(2,1),
                          new(1,0)
            }),
            new("revL", new Point2D[]
            {
                                    new(2,2),
                                    new(2,1),
                new(0,0), new(1,0), new(2,0)
            }),
            new("tall", new Point2D[]
            {
                new(0,3),
                new(0,2),
                new(0,1),
                new(0,0)
            }),
            new("sqre", new Point2D[]
            {
                new(0,1), new(1,1),
                new(0,0), new(1,0)
            })
        };

        public int Part1(string[] input)
        {
            IList<char> instructions = input.First().ToCharArray();
            HashSet<Point2D> occupied = new();

            // add the floor
            occupied.UnionWith(Enumerable.Range(LeftEdge, RightEdge + 1).Select(x => new Point2D(x, -1)));

            int moves = 0;
            int shapes = 0;
            int height = 0;

            while (shapes < 2022)
            {
                Shape shape = Shapes[shapes++ % Shapes.Length];

                // move to start location
                Point2D[] points = shape.Points.Select(p => new Point2D(p.X + 2, p.Y + height + 3)).ToArray();

                // repeat L/R shift then down until stopping
                while (true)
                {
                    char move = instructions[moves++ % instructions.Count];

                    // try a L/R shift
                    if (move == LeftMove && points.All(p => p.X > LeftEdge))
                    {
                        Point2D[] shiftPoints = points.Select(p => new Point2D(p.X - 1, p.Y)).ToArray();

                        if (!shiftPoints.Any(occupied.Contains))
                        {
                            points = shiftPoints;
                        }
                    }
                    else if (move == RightMove && points.All(p => p.X < RightEdge))
                    {
                        Point2D[] shiftPoints = points.Select(p => new Point2D(p.X + 1, p.Y)).ToArray();

                        if (!shiftPoints.Any(occupied.Contains))
                        {
                            points = shiftPoints;
                        }
                    }

                    Point2D[] downPoints = points.Select(p => new Point2D(p.X, p.Y - 1)).ToArray();

                    // check if we would collide with floor or a settled shape
                    if (downPoints.Any(occupied.Contains))
                    {
                        height = Math.Max(height, points.Max(p => p.Y) + 1);
                        occupied.UnionWith(points);
                        break;
                    }

                    // move down
                    points = downPoints;
                }
            }

            // 2820 too low
            // 2990 too low
            return height;
        }

        public long Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private record Shape(string Id, ICollection<Point2D> Points);
    }
}
