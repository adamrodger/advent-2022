using System;
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

            string instructions = input[^1];
            Point2D location = (input[0].IndexOf(Open), 0);
            Bearing bearing = Bearing.East;
            int start = 0;

            while (start < instructions.Length)
            {
                if (char.IsAsciiDigit(instructions[start]))
                {
                    int count = 0;

                    while (start + count < instructions.Length && char.IsAsciiDigit(instructions[start + count]))
                    {
                        count++;
                    }
                    
                    int steps = int.Parse(instructions.Substring(start, count));

                    start += count;

                    for (int i = 0; i < steps; i++)
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

                    continue;
                }

                TurnDirection direction = instructions[start++] switch
                {
                    'L' => TurnDirection.Left,
                    'R' => TurnDirection.Right,
                    _ => throw new ArgumentOutOfRangeException()
                };

                bearing = bearing.Turn(direction);
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

        private static Point2D Move(Point2D current, Bearing bearing, char[,] grid) => bearing switch
        {
            Bearing.North => (current.X, current.Y > 0 ? (current.Y - 1) % grid.GetLength(0) : grid.GetLength(0) - 1),
            Bearing.South => (current.X, (current.Y + 1) % grid.GetLength(0)),
            Bearing.East => ((current.X + 1) % grid.GetLength(1), current.Y),
            Bearing.West => (current.X > 0 ? (current.X - 1) % grid.GetLength(1) : grid.GetLength(1) - 1, current.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
