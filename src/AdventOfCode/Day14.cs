using System;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 14
    /// </summary>
    public class Day14
    {
        private const char Wall = '■';
        private const char Sand = 'o';
        private const char Air = ' ';

        public int Part1(string[] input)
        {
            char[,] grid = CreateGrid(input);
            return FillGrid(grid);
        }

        private static char[,] CreateGrid(string[] input)
        {
            char[,] grid = new char[300, 1000];
            grid.ForEach(((x, y, _) => grid[y, x] = Air));

            foreach (string line in input)
            {
                string[] corners = line.Split(" -> ");

                for (int i = 0; i < corners.Length - 1; i++)
                {
                    string[] startPoints = corners[i].Split(',');
                    Point2D start = (int.Parse(startPoints[0]), int.Parse(startPoints[1]));

                    string[] endPoints = corners[i + 1].Split(',');
                    Point2D end = (int.Parse(endPoints[0]), int.Parse(endPoints[1]));

                    for (int y = Math.Min(start.Y, end.Y); y <= Math.Max(start.Y, end.Y); y++)
                    {
                        for (int x = Math.Min(start.X, end.X); x <= Math.Max(start.X, end.X); x++)
                        {
                            grid[y, x] = Wall;
                        }
                    }
                }
            }

            return grid;
        }

        private static int FillGrid(char[,] grid)
        {
            int count = 0;

            while (true)
            {
                Point2D grain = (500, 0);

                while (true)
                {
                    if (grain.Y >= grid.GetLength(0) - 1)
                    {
                        // fell off the end
                        return count;
                    }

                    if (grid[grain.Y + 1, grain.X] == Air)
                    {
                        grain += (0, 1);
                        continue;
                    }

                    if (grid[grain.Y + 1, grain.X - 1] == Air)
                    {
                        grain += (-1, 1);
                        continue;
                    }

                    if (grid[grain.Y + 1, grain.X + 1] == Air)
                    {
                        grain += (1, 1);
                        continue;
                    }

                    // grain has settled
                    grid[grain.Y, grain.X] = Sand;
                    count++;
                    break;
                }
            }
        }

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
