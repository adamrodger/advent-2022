using System;
using System.Collections.Generic;
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
        private const char Air = '\0';
        private const int OriginColumn = 500;

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
            int rowMax = int.MinValue;
            var rocks = new List<List<Point2D>>(input.Length);

            // parse the rock instructions
            foreach (string line in input)
            {
                string[] corners = line.Split(" -> ");
                var points = new List<Point2D>(corners.Length);

                foreach (string corner in corners)
                {
                    string[] coords = corner.Split(',');
                    Point2D point = (int.Parse(coords[0]), int.Parse(coords[1]));
                    
                    rowMax = Math.Max(rowMax, point.Y);

                    points.Add(point);
                }

                rocks.Add(points);
            }

            // indices are inclusive
            rowMax += 1;

            if (part == Part.Two)
            {
                rowMax += 2;
            }

            // create the grid with rock walls added
            char[,] grid = new char[rowMax, OriginColumn + rowMax * 2];

            foreach (List<Point2D> coords in rocks)
            {
                for (int i = 0; i < coords.Count - 1; i++)
                {
                    Point2D start = coords[i];
                    Point2D end = coords[i + 1];

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
                // add an infinite wall to the bottom
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[rowMax - 1, x] = Wall;
                }
            }

            return grid;
        }

        private static int FillGrid(char[,] grid)
        {
            int count = 0;

            while (true)
            {
                Point2D grain = (OriginColumn, 0);

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
            Point2D down = (0, 1);
            Point2D left = (-1, 1);
            Point2D right = (1, 1);
            int count = 0;

            var queue = new Queue<Point2D>();
            queue.Enqueue((OriginColumn, 0));

            while (queue.TryDequeue(out Point2D grain))
            {
                count++;

                Point2D tryDown = grain + down;

                if (grid[tryDown.Y, tryDown.X] == Air)
                {
                    grid[tryDown.Y, tryDown.X] = Sand;
                    queue.Enqueue(tryDown);
                }

                Point2D tryLeft = grain + left;

                if (grid[tryLeft.Y, tryLeft.X] == Air)
                {
                    grid[tryLeft.Y, tryLeft.X] = Sand;
                    queue.Enqueue(tryLeft);
                }

                Point2D tryRight = grain + right;

                if (grid[tryRight.Y, tryRight.X] == Air)
                {
                    grid[tryRight.Y, tryRight.X] = Sand;
                    queue.Enqueue(tryRight);
                }
            }

            return count;
        }
    }
}
