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
            return (int)Simulate(instructions, 2022);
        }

        public long Part2(string[] input)
        {
            IList<char> instructions = input.First().ToCharArray();

            int repeatPoint = instructions.Count * Shapes.Length;
            long height = Simulate(instructions, repeatPoint);

            long secondHeight = Simulate(instructions, repeatPoint * 2);
            long subsequentHeight = height - secondHeight;

            const long requiredRounds = 1_000_000_000_000; // 2022
            long fullRounds = requiredRounds / repeatPoint;

            // there's one partial batch to add on at the end also
            int remainingShapes = (int)(requiredRounds - (fullRounds * repeatPoint));
            long remainingHeight = Simulate(instructions, remainingShapes);

            // this gets close but doesn't work because each batch may tessellate with the one beneath it,
            // so the height wouldn't increase by the full amount. The first batch has a solid floor to
            // fall onto, whereas each subsequent batch doesn't. You need to take off the overlapping amount
            // between each batch to get the right answer
            long fullHeight = (height * fullRounds) + remainingHeight;

            // ???????? Can you use the occupied set to get the top and bottom?
            const long overlapAmount = 0; 
            long overlaps = fullRounds * overlapAmount;

            return fullHeight - overlaps;
        }

        private static int Simulate(IList<char> instructions, int rounds)
        {
            HashSet<Point2D> occupied = new();

            // add the floor
            occupied.UnionWith(Enumerable.Range(LeftEdge, RightEdge + 1).Select(x => new Point2D(x, -1)));

            int moves = 0;
            int shapes = 0;
            int height = 0;

            while (shapes < rounds)
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
            
            return height;
        }

        private record Shape(string Id, ICollection<Point2D> Points);
    }
}
