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

            IEnumerable<Instruction> instructions = Parse(input[^1]);

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

            IEnumerable<Instruction> instructions = Parse(input[^1]);

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
        private static IEnumerable<Instruction> Parse(string input)
        {
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

                    yield return new Turn(direction);
                }
                else
                {
                    int length = 0;

                    while (i + length < input.Length && char.IsAsciiDigit(input[i + length]))
                    {
                        length++;
                    }

                    int steps = int.Parse(input.Substring(i, length));
                    yield return new Step(steps);

                    i += length - 1;
                }
            }
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
            Bearing.North => (current.X, current.Y > 0 ? current.Y - 1 : grid.GetLength(0) - 1),
            Bearing.South => (current.X, (current.Y + 1) % grid.GetLength(0)),
            Bearing.East => ((current.X + 1) % grid.GetLength(1), current.Y),
            Bearing.West => (current.X > 0 ? current.X - 1 : grid.GetLength(1) - 1, current.Y),
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
            Point2D next = Move(current, bearing, grid);

            if (grid[next.Y, next.X] != Blank)
            {
                // this is a valid move - either we moved within the same face or we moved to an adjoined face on the 2D net
                return (next, bearing);
            }

            // we've fallen off a face, so we need to work out which face we end up on and which direction we're now facing
            Face face = (current.Y / 50, current.X / 50) switch
            {
                (0, 1) => Face.Top,
                (0, 2) => Face.Right,
                (1, 1) => Face.Front,
                (2, 0) => Face.Left,
                (2, 1) => Face.Bottom,
                (3, 0) => Face.Back,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (face, bearing) switch
            {
                (Face.Top, Bearing.North) => ((0, current.X + 100), Bearing.East),      // back, left edge, heading east
                (Face.Top, Bearing.West) => ((0, 149 - current.Y), Bearing.East),       // left, left edge, heading east
                (Face.Right, Bearing.North) => ((current.X - 100, 199), Bearing.North), // back, bottom edge, heading north
                (Face.Right, Bearing.South) => ((99, current.X - 50), Bearing.West),    // front, right edge, heading west
                (Face.Right, Bearing.East) => ((99, 149 - current.Y), Bearing.West),    // bottom, right edge, heading west
                (Face.Front, Bearing.East) => ((current.Y + 50, 49), Bearing.North),    // right, bottom edge, heading north
                (Face.Front, Bearing.West) => ((current.Y - 50, 100), Bearing.South),   // left, top edge, heading south
                (Face.Left, Bearing.North) => ((50, current.X + 50), Bearing.East),     // front, left edge, heading east
                (Face.Left, Bearing.West) => ((50, 149 - current.Y), Bearing.East),     // top, left edge, heading east
                (Face.Bottom, Bearing.South) => ((49, current.X + 100), Bearing.West),  // back, right edge, heading west
                (Face.Bottom, Bearing.East) => ((149, 149 - current.Y), Bearing.West),  // right, right edge, heading west
                (Face.Back, Bearing.South) => ((current.X + 100, 0), bearing),          // right, top edge, heading south
                (Face.Back, Bearing.East) => ((current.Y - 100, 149), Bearing.North),   // bottom, bottom edge, heading north
                (Face.Back, Bearing.West) => ((current.Y - 100, 0), Bearing.South),     // top, top edge, heading south
                _ => throw new ArgumentOutOfRangeException($"Face {face} doesn't need to jump on bearing {bearing}")
            };
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

        private enum Face
        {
            Top,
            Front,
            Bottom,
            Back,
            Left,
            Right
        }
    }
}
