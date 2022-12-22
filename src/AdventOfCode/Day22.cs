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

            return CalculateScore(bearing, location);
        }

        public int Part2(string[] input)
        {
            char[,] grid = input[..^2].ToGrid();

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
                    (Point2D nextLocation, Bearing nextBearing) = MoveCube(location, bearing, grid);
                    char nextSquare = grid[nextLocation.Y, nextLocation.X];

                    if (nextSquare == Open)
                    {
                        location = nextLocation;
                        bearing = nextBearing;
                    }
                    else if (nextSquare == Wall)
                    {
                        break;
                    }
                    else
                    {
                        throw new InvalidOperationException("Tried to step off the cube");
                    }
                }
            }
            
            return CalculateScore(bearing, location);
        }

        /// <summary>
        /// Parse the input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>A list of parsed instructions</returns>
        private static IList<Instruction> Parse(string input)
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

        /// <summary>
        /// Work out where we'd move to when stepping around the grid as a 2D map
        /// </summary>
        /// <param name="current">Current location</param>
        /// <param name="bearing">Current bearing</param>
        /// <param name="grid">Grid</param>
        /// <returns>Potential next location</returns>
        private static Point2D Move(Point2D current, Bearing bearing, char[,] grid) => bearing switch
        {
            Bearing.North => (current.X, current.Y > 0 ? (current.Y - 1) % grid.GetLength(0) : grid.GetLength(0) - 1),
            Bearing.South => (current.X, (current.Y + 1) % grid.GetLength(0)),
            Bearing.East => ((current.X + 1) % grid.GetLength(1), current.Y),
            Bearing.West => (current.X > 0 ? (current.X - 1) % grid.GetLength(1) : grid.GetLength(1) - 1, current.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        /// <summary>
        /// Work out where we'd move to and which direction e'd be facing when stepping around the map as a cube
        ///
        /// This can cause the bearing to change as you move from one cube face to another
        /// </summary>
        /// <param name="current">Current location on the 2D map</param>
        /// <param name="bearing">Current bearing</param>
        /// <param name="grid">2D map</param>
        /// <returns>Next location and next bearing</returns>
        public static (Point2D Next, Bearing Bearing) MoveCube(Point2D current, Bearing bearing, char[,] grid)
        {
            int currentRow = current.Y / 50;
            int currentColumn = current.X / 50;

            Point2D next = Move(current, bearing, grid);
            int nextRow = next.Y / 50;
            int nextColumn = next.X / 50;

            if (currentRow == nextRow && currentColumn == nextColumn)
            {
                // we can do a regular step because we've stayed on the same face
                return (next, bearing);
            }

            // we've fallen off a face, so we need to work out which face we end up on and which direction we're now facing
            switch ((currentRow, currentColumn))
            {
                case (0, 1): // top
                    switch (bearing)
                    {
                        case Bearing.North: // back, left edge, heading east
                            return ((0, current.X + 100), Bearing.East);
                        case Bearing.South: // front, top edge, heading south
                            return (next, bearing);
                        case Bearing.East: // right, left edge, heading east
                            return (next, bearing);
                        case Bearing.West: // left, left edge, heading east
                            return ((0, (50 - current.Y) + 99), Bearing.East);
                    }
                    break;
                case (0, 2): // right
                    switch (bearing)
                    {
                        case Bearing.North: // back, bottom edge, heading north
                            return ((current.X - 100, 199), Bearing.North);
                        case Bearing.South: // front, right edge, heading west
                            return ((99, current.X - 50), Bearing.West);
                        case Bearing.East: // bottom, right edge, heading west
                            return ((99, (50 - current.Y) + 99), Bearing.West);
                        case Bearing.West: // top, right edge, heading west
                            return (next, bearing);
                    }
                    break;
                case (1, 1): // front
                    switch (bearing)
                    {
                        case Bearing.North: // top, bottom edge, heading north
                            return (next, bearing);
                        case Bearing.South: // bottom, top edge, heading south
                            return (next, bearing);
                        case Bearing.East: // right, bottom edge, heading north
                            return ((current.Y + 50, 49), Bearing.North);
                        case Bearing.West: // left, top edge, heading south
                            return ((current.Y - 50, 100), Bearing.South);
                    }
                    break;
                case (2, 0): // left
                    switch (bearing)
                    {
                        case Bearing.North: // front, left edge, heading east
                            return ((50, current.X + 50), Bearing.East);
                        case Bearing.South: // back, top edge, heading south
                            return (next, bearing);
                        case Bearing.East: // bottom, left edge, heading east
                            return (next, bearing);
                        case Bearing.West: // top, left edge, heading east
                            return ((50, 50 - current.Y + 99), Bearing.East);
                    }
                    break;
                case (2, 1): // bottom
                    switch (bearing)
                    {
                        case Bearing.North: // front, bottom edge, heading north
                            return (next, bearing);
                        case Bearing.South: // back, right edge, heading west
                            return ((49, current.X + 100), Bearing.West);
                        case Bearing.East: // right, right edge, heading west
                            return ((149, (50 - current.Y) + 99), Bearing.West);
                        case Bearing.West: // left, right edge, heading west
                            return (next, bearing);
                    }
                    break;
                case (3, 0): // back
                    switch (bearing)
                    {
                        case Bearing.North: // left, bottom edge, heading north
                            return (next, bearing);
                        case Bearing.South: // right, top edge, heading south
                            return ((current.X + 100, 0), bearing);
                        case Bearing.East: // bottom, bottom edge, heading north
                            return ((current.Y - 100, 149), Bearing.North);
                        case Bearing.West: // top, top edge, heading south
                            return ((current.Y - 100, 0), Bearing.South);
                    }
                    break;
                default:
                    throw new InvalidOperationException($"There is no face on row {currentRow} and column {currentColumn}");
            }

            throw new ArgumentOutOfRangeException(nameof(bearing), bearing, "Tried to make a weird move on an existing face");
        }

        /// <summary>
        /// Calculate the final score depending on where you finish and what direction you're facing
        /// </summary>
        /// <param name="bearing">Final bearing</param>
        /// <param name="location">Final location</param>
        /// <returns>Score</returns>
        private static int CalculateScore(Bearing bearing, Point2D location)
        {
            int bearingValue = bearing switch
            {
                Bearing.North => 3,
                Bearing.South => 1,
                Bearing.East => 0,
                Bearing.West => 2,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (location.Y + 1) * 1000 + (location.X + 1) * 4 + bearingValue;
        }

        private abstract record Instruction;

        private record Step(int Count) : Instruction;

        private record Turn(TurnDirection Direction) : Instruction;
    }
}
