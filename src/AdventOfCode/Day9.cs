using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 9
    /// </summary>
    public class Day9
    {
        public int Part1(string[] input)
        {
            Point2D head = new(0, 0);
            Point2D tail = new(0, 0);

            var visited = new HashSet<(int, int)> { tail };

            foreach (string line in input)
            {
                (Bearing direction, int steps) = Parse(line);

                for (int i = 0; i < steps; i++)
                {
                    head = head.Move(direction);
                    tail = Follow(head, tail);

                    visited.Add(tail);
                }
            }

            return visited.Count;
        }

        public int Part2(string[] input)
        {
            Point2D[] ropes = Enumerable.Range(0, 10).Select(r => new Point2D(0, 0)).ToArray();
            var visited = new HashSet<(int, int)> { ropes[9] };

            foreach (string line in input)
            {
                (Bearing direction, int steps) = Parse(line);

                for (int i = 0; i < steps; i++)
                {
                    // move the head
                    ropes[0] = ropes[0].Move(direction);

                    // move all the tails to follow the one ahead of them
                    for (int r = 1; r < ropes.Length; r++)
                    {
                        ropes[r] = Follow(ropes[r - 1], ropes[r]);
                    }

                    visited.Add(ropes.Last());
                }
            }

            return visited.Count;
        }

        private static (Bearing direction, int steps) Parse(string line)
        {
            Bearing direction = line[0] switch
            {
                'U' => Bearing.North,
                'D' => Bearing.South,
                'L' => Bearing.West,
                'R' => Bearing.East,
                _ => throw new ArgumentOutOfRangeException()
            };
            int steps = int.Parse(line[2..]);

            return (direction, steps);
        }

        private static Point2D Follow(Point2D head, Point2D tail)
        {
            if (head == tail || Math.Abs(head.X - tail.X) < 2 && Math.Abs(head.Y - tail.Y) < 2)
            {
                // already touching, don't move tail
                return tail;
            }

            // check if head to the right, and if so move the tail to the right
            if (head.X > tail.X)
            {
                tail = tail.Move(Bearing.East);
            }
            else if (head.X < tail.X)
            {
                tail = tail.Move(Bearing.West);
            }

            if (head.Y < tail.Y)
            {
                tail = tail.Move(Bearing.South);
            }
            else if (head.Y > tail.Y)
            {
                tail = tail.Move(Bearing.North);
            }

            return tail;
        }
    }
}
