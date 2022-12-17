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
            return Simulate(instructions, 1_000_000_000_000);
        }

        private static long Simulate(IList<char> instructions, long rounds)
        {
            // can't use Point2D because we need y to be long instead of int
            HashSet<(int X, long Y)> occupied = new();
            Dictionary<(int move, long time), (long time, long height)> seenStates = new();

            // add the floor
            occupied.UnionWith(Enumerable.Range(LeftEdge, RightEdge + 1).Select(x => (x, 0L)));

            int moves = 0;
            long shapes = 0;
            long height = 0;
            long cycleHeight = 0;
            
            while (shapes < rounds)
            {
                Shape shape = Shapes[shapes % Shapes.Length];

                // move to start location
                (int X, long Y)[] points = shape.Points.Select(p => (p.X + 2, p.Y + height + 4)).ToArray();

                // repeat L/R shift then down until stopping
                while (true)
                {
                    char move = instructions[moves];
                    moves = (moves + 1) % instructions.Count;

                    // try a L/R shift
                    if (move == LeftMove && points.All(p => p.X > LeftEdge))
                    {
                        (int X, long Y)[] shiftPoints = points.Select(p => (p.X - 1, y: p.Y)).ToArray();

                        if (!shiftPoints.Any(occupied.Contains))
                        {
                            points = shiftPoints;
                        }
                    }
                    else if (move == RightMove && points.All(p => p.X < RightEdge))
                    {
                        (int X, long Y)[] shiftPoints = points.Select(p => (p.X + 1, y: p.Y)).ToArray();

                        if (!shiftPoints.Any(occupied.Contains))
                        {
                            points = shiftPoints;
                        }
                    }

                    (int X, long Y)[] downPoints = points.Select(p => (x: p.X, p.Y - 1)).ToArray();

                    // check if we would collide with floor or a settled shape
                    if (downPoints.Any(occupied.Contains))
                    {
                        height = Math.Max(height, points.Max(p => p.Y));
                        occupied.UnionWith(points);
                        
                        (int move, long time) state = (moves, shapes % 5);

                        if (seenStates.ContainsKey(state) && shapes >= 2022)
                        {
                            // we can simulate one huge jump as far as possible before completing the simulation
                            (long previousMove, long previousHeight) = seenStates[state];

                            long deltaHeight = height - previousHeight;
                            long deltaMove = shapes - previousMove;
                            long cycles = (rounds - shapes) / deltaMove;

                            cycleHeight += cycles * deltaHeight;
                            shapes += cycles * deltaMove;
                        }

                        seenStates[state] = (shapes, height);

                        break;
                    }

                    // apply the downward move
                    points = downPoints;
                }

                shapes++;
            }

            return height + cycleHeight;
        }

        private record Shape(string Id, ICollection<Point2D> Points);
    }
}
