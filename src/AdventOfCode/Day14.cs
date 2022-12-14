using System;
using System.Linq;
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
            char[,] grid = CreateGrid(input, Part.One);
            return FillGrid(grid);
        }

        public int Part2(string[] input)
        {
            char[,] grid = CreateGrid(input, Part.Two);
            return FillGrid2(grid);
        }

        private static char[,] CreateGrid(string[] input, Part part)
        {
            int height = input.SelectMany(line => line.Split(" -> ").Select(l => l.Split(',')[1]).Select(int.Parse)).Max() + 1;

            if (part == Part.Two)
            {
                height += 2;
            }

            char[,] grid = new char[height, 1000];
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

            if (part == Part.Two)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[height - 1, x] = Wall;
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

        private static int FillGrid2(char[,] grid)
        {
            int count = 0;

            while (true)
            {
                Point2D grain = (500, 0);

                while (true)
                {
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

                    if (grain == (500, 0))
                    {
                        return count;
                    }

                    break;
                }
            }
        }
    }
}
