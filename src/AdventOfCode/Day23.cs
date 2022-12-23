using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AdventOfCode.Utilities;
using Optional;
using Optional.Unsafe;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 23
    /// </summary>
    public class Day23
    {
        public int Part1(string[] input)
        {
            // get the initial starting positions
            ISet<Point2D> positions = new HashSet<Point2D>();

            for (int y = 0; y < input.Length; y++)
            {
                string line = input[y];

                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        positions.Add(new Point2D(x, y));
                    }
                }
            }

            Debug.WriteLine("--- Start ---");
            Print(positions);

            // simulate the elves moving around
            for (int i = 0; i < 10; i++)
            {
                positions = Tick(i, positions);

                Debug.WriteLine($"--- After Round {i} ---");
                Print(positions);
            }

            // find the minimum bounding box
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (Point2D elf in positions)
            {
                minX = Math.Min(minX, elf.X);
                maxX = Math.Max(maxX, elf.X);
                minY = Math.Min(minY, elf.Y);
                maxY = Math.Max(maxY, elf.Y);
            }

            // find the empty spaces in the box
            int emptySpaces = 0;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!positions.Contains(new Point2D(x, y)))
                    {
                        emptySpaces++;
                    }
                }
            }

            // 4550 -- too high
            // 3448 -- too low
            return emptySpaces;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private static ISet<Point2D> Tick(int round, ISet<Point2D> current)
        {
            var proposals = new Dictionary<Point2D, Point2D>();
            var proposalCounts = new Dictionary<Point2D, int>();

            // first part - each elf proposes a move
            foreach (Point2D elf in current)
            {
                (Point2D Point, CompassFlags Bearing)[] occupied = elf.Adjacent8()
                                                                      .Zip(AllDirections)
                                                                      .Where(e => current.Contains(e.First))
                                                                      .ToArray();

                if (!occupied.Any())
                {
                    // don't move
                    proposals[elf] = elf;
                    proposalCounts[elf] = proposalCounts.GetOrCreate(elf) + 1;
                    continue;
                }

                for (int i = 0; i < 4; i++)
                {
                    Option<Point2D> tryMove = NextMove(elf, occupied, round + i);

                    if (tryMove.HasValue)
                    {
                        Point2D proposedMove = tryMove.ValueOrFailure();
                        proposals[elf] = proposedMove;
                        proposalCounts[proposedMove] = proposalCounts.GetOrCreate(proposedMove) + 1;
                        break;
                    }
                }

                if (!proposals.ContainsKey(elf))
                {
                    // elf wanted to move but couldn't, so it proposes to stay still
                    proposals[elf] = elf;
                    proposalCounts[elf] = proposalCounts.GetOrCreate(elf) + 1;
                }
            }

            // second part - elves which proposed unique positions will move there
            var next = new HashSet<Point2D>(current.Count);

            foreach (Point2D elf in current)
            {
                var proposedMove = proposals[elf];

                if (proposalCounts[proposedMove] == 1)
                {
                    next.Add(proposedMove);
                }
                else
                {
                    // don't move - other elves proposed this position also
                    next.Add(elf);
                }
            }

            return next;
        }

        private static Option<Point2D> NextMove(Point2D elf, IList<(Point2D Location, CompassFlags Bearing)> occupied, int round)
        {
            switch (round % 4)
            {
                case 0:
                    if (!occupied.Any(o => o.Bearing.HasFlag(CompassFlags.North)))
                    {
                        return (elf with { Y = elf.Y - 1 }).Some();
                    }

                    break;
                case 1:
                    if (!occupied.Any(o => o.Bearing.HasFlag(CompassFlags.South)))
                    {
                        return (elf with { Y = elf.Y + 1 }).Some();
                    }

                    break;
                case 2:
                    if (!occupied.Any(o => o.Bearing.HasFlag(CompassFlags.West)))
                    {
                        return (elf with { X = elf.X - 1 }).Some();
                    }

                    break;
                case 3:
                    if (!occupied.Any(o => o.Bearing.HasFlag(CompassFlags.East)))
                    {
                        return (elf with { X = elf.X + 1 }).Some();
                    }
                    break;
            }

            return Option.None<Point2D>();
        }

        private static void Print(ISet<Point2D> positions)
        {
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (Point2D elf in positions)
            {
                minX = Math.Min(minX, elf.X);
                maxX = Math.Max(maxX, elf.X);
                minY = Math.Min(minY, elf.Y);
                maxY = Math.Max(maxY, elf.Y);
            }

            StringBuilder builder = new();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    builder.Append(positions.Contains(new Point2D(x, y)) ? '#' : '.');
                }

                builder.AppendLine();
            }

            Debug.WriteLine(builder.ToString());
        }

        private static readonly CompassFlags[] AllDirections =
        {
            CompassFlags.NorthWest, CompassFlags.North, CompassFlags.NorthEast,
            CompassFlags.West,                          CompassFlags.East,
            CompassFlags.SouthWest, CompassFlags.South, CompassFlags.SouthEast
        };

        [Flags]
        private enum CompassFlags
        {
            North = 1 << 1,
            South = 1 << 2,
            East =  1 << 3,
            West =  1 << 4,

            NorthWest = North | West,
            NorthEast = North | East,
            SouthEast = South | East,
            SouthWest = South | West
        }
    }
}
