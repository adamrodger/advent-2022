using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 23
    /// </summary>
    public class Day23
    {
        private const int NorthWest = 0;
        private const int North = 1;
        private const int NorthEast = 2;
        private const int West = 3;
        private const int East = 4;
        private const int SouthWest = 5;
        private const int South = 6;
        private const int SouthEast = 7;

        // go to the shadow realm
        private static readonly Point2D NoMove = new(int.MinValue, int.MinValue);

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

            // simulate the elves moving around
            for (int i = 0; i < 10; i++)
            {
                Tick(i, positions);
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
            int emptySpaces = ((maxX - minX + 1) * (maxY - minY + 1)) - positions.Count;
            return emptySpaces;
        }

        public int Part2(string[] input)
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

            // keep going until no elf moves
            int round = 0;

            while (true)
            {
                bool moved = Tick(round++, positions);

                if (!moved)
                {
                    return round;
                }
            }
        }

        /// <summary>
        /// Perform one full round of the simulation
        /// </summary>
        /// <param name="round">Round number</param>
        /// <param name="current">Current positions</param>
        /// <returns>Whether any elf moved from the start positions</returns>
        private static bool Tick(int round, ISet<Point2D> current)
        {
            bool moved = false;
            var proposals = new Dictionary<Point2D, Point2D>(current.Count);
            var proposalCounts = new Dictionary<Point2D, int>(current.Count);

            // first part - each elf proposes a move
            foreach (Point2D elf in current)
            {
                if (TryMove(elf, current, round, out Point2D proposedMove))
                {
                    // elf wants to move, so proposes the move
                    proposals[elf] = proposedMove;
                    proposalCounts[proposedMove] = proposalCounts.GetOrCreate(proposedMove) + 1;
                    moved = true;
                }
            }

            // second part - elves which proposed unique positions will move there, otherwise they stay still
            foreach ((Point2D elf, Point2D proposedMove) in proposals)
            {
                if (proposalCounts[proposedMove] == 1)
                {
                    current.Remove(elf);
                    current.Add(proposedMove);
                }
            }

            return moved;
        }

        /// <summary>
        /// Try a move, depending on the current round and move index
        /// </summary>
        /// <param name="elf">Elf to move</param>
        /// <param name="positions">Current elf positions</param>
        /// <param name="round">Round + move identifier</param>
        /// <param name="move">Proposed move, if a move is possible</param>
        /// <returns>The next move if that move is valid, otherwise None</returns>
        private static bool TryMove(Point2D elf, ISet<Point2D> positions, int round, out Point2D move)
        {
            bool[] adjacent =
            {
                positions.Contains(new(elf.X - 1, elf.Y - 1)), positions.Contains(new(elf.X, elf.Y - 1)), positions.Contains(new(elf.X + 1, elf.Y - 1)),
                positions.Contains(new(elf.X - 1, elf.Y + 0)),                                            positions.Contains(new(elf.X + 1, elf.Y + 0)),
                positions.Contains(new(elf.X - 1, elf.Y + 1)), positions.Contains(new(elf.X, elf.Y + 1)), positions.Contains(new(elf.X + 1, elf.Y + 1))
            };

            if (!adjacent.Any(x => x))
            {
                // don't move if there are no adjacent elves
                move = NoMove;
                return false;
            }

            for (int i = 0; i < 4; i++)
            {
                switch ((round + i) % 4)
                {
                    case 0:
                        if (!adjacent[NorthWest] && !adjacent[North] && !adjacent[NorthEast])
                        {
                            move = elf with { Y = elf.Y - 1 };
                            return true;
                        }

                        break;
                    case 1:
                        if (!adjacent[SouthWest] && !adjacent[South] && !adjacent[SouthEast])
                        {
                            move = elf with { Y = elf.Y + 1 };
                            return true;
                        }

                        break;
                    case 2:
                        if (!adjacent[NorthWest] && !adjacent[West] && !adjacent[SouthWest])
                        {
                            move = elf with { X = elf.X - 1 };
                            return true;
                        }

                        break;
                    case 3:
                        if (!adjacent[NorthEast] && !adjacent[East] && !adjacent[SouthEast])
                        {
                            move = elf with { X = elf.X + 1 };
                            return true;
                        }

                        break;
                }
            }

            // no valid move
            move = NoMove;
            return false;
        }
    }
}
