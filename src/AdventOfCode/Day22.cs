using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        private const char Open = '.';
        private const char Wall = '#';
        private const char Blank = ' ';

        public int Part1(string[] input)
        {
            char[,] grid = input[..^2].ToGrid();
            grid.Print();

            IList<Instruction> instructions = Parse(input[^1]);

            Point2D location = (input[0].IndexOf(Open), 0);
            Bearing bearing = Bearing.East;

            foreach (Instruction instruction in instructions)
            {
                if (instruction is Turn turn)
                {
                    bearing = bearing.Turn(turn.Direction);
                    continue;
                }

                var steps = instruction as Step ?? throw new ArgumentOutOfRangeException();

                for (int i = 0; i < steps.Count; i++)
                {
                    Point2D nextLocation = Move(location, bearing, grid);
                    char nextSquare = grid[nextLocation.Y, nextLocation.X];

                    if (nextSquare == Open)
                    {
                        location = nextLocation;
                    }
                    else if (nextSquare == Wall)
                    {
                        break;
                    }
                    else
                    {
                        // teleport round to the other side
                        while (nextSquare == Blank)
                        {
                            nextLocation = Move(nextLocation, bearing, grid);
                            nextSquare = grid[nextLocation.Y, nextLocation.X];
                        }

                        if (nextSquare == Wall)
                        {
                            break;
                        }

                        location = nextLocation;
                    }
                }
            }

            int bearingValue = bearing switch
            {
                Bearing.North => 3,
                Bearing.South => 1,
                Bearing.East => 0,
                Bearing.West => 2,
                _ => throw new ArgumentOutOfRangeException()
            };

            // 45400 -- too low
            return (location.Y + 1) * 1000 + (location.X + 1) * 4 + bearingValue;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private IList<Instruction> Parse(string input)
        {
            var instructions = new List<Instruction>();

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsAsciiDigit(input[i]))
                {
                    TurnDirection direction = input[i] switch
                    {
                        'L' => TurnDirection.Left,
                        'R' => TurnDirection.Right,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    instructions.Add(new Turn(direction));
                }
                else
                {
                    int length = 0;

                    while (i + length < input.Length && char.IsAsciiDigit(input[i + length]))
                    {
                        length++;
                    }

                    int steps = int.Parse(input.Substring(i, length));
                    instructions.Add(new Step(steps));

                    i += length - 1;
                }
            }

            return instructions;
        }

        private static Point2D Move(Point2D current, Bearing bearing, char[,] grid) => bearing switch
        {
            Bearing.North => (current.X, current.Y > 0 ? (current.Y - 1) % grid.GetLength(0) : grid.GetLength(0) - 1),
            Bearing.South => (current.X, (current.Y + 1) % grid.GetLength(0)),
            Bearing.East => ((current.X + 1) % grid.GetLength(1), current.Y),
            Bearing.West => (current.X > 0 ? (current.X - 1) % grid.GetLength(1) : grid.GetLength(1) - 1, current.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        private abstract record Instruction;

        private record Step(int Count) : Instruction;

        private record Turn(TurnDirection Direction) : Instruction;
    }
}
