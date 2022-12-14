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
            int colMin = int.MaxValue;
            int colMax = int.MinValue;
            int rowMax = int.MinValue;
            var rocks = new List<List<Point2D>>(input.Length);

            foreach (string line in input)
            {
                string[] corners = line.Split(" -> ");
                var points = new List<Point2D>(corners.Length);

                foreach (string corner in corners)
                {
                    string[] coords = corner.Split(',');
                    Point2D point = (int.Parse(coords[0]), int.Parse(coords[1]));

                    colMin = Math.Min(colMin, point.X);
                    colMax = Math.Max(colMax, point.X);
                    rowMax = Math.Max(rowMax, point.Y);

                    points.Add(point);
                }

                rocks.Add(points);
            }

            // indices are inclusive
            rowMax += 1;
            colMax += 1;
            colMin += 1;

            if (part == Part.Two)
            {
                rowMax += 2;
            }

            char[,] grid = new char[rowMax, 500 + rowMax * 2];
            //grid.ForEach(((x, y, _) => grid[y, x] = Air));

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
            Point2D down = (0, 1);
            Point2D left = (-1, 1);
            Point2D right = (1, 1);
            int count = 0;

            var queue = new Queue<Point2D>();
            queue.Enqueue((500, 0));

            while (queue.TryDequeue(out Point2D grain))
            {
                count++;

                int dy = grain.Y + 1;

                if (grid[dy, grain.X] == Air)
                {
                    grid[dy, grain.X] = Sand;
                    queue.Enqueue(grain + down);
                }

                if (grid[dy, grain.X - 1] == Air)
                {
                    grid[dy, grain.X - 1] = Sand;
                    queue.Enqueue(grain + left);
                }

                if (grid[dy, grain.X + 1] == Air)
                {
                    grid[dy, grain.X + 1] = Sand;
                    queue.Enqueue(grain + right);
                }
            }

            return count;
        }
    }
}
