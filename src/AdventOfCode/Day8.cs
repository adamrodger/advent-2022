using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 8
    /// </summary>
    public class Day8
    {
        public int Part1(string[] input)
        {
            int visible = 0;
            int[][] grid = input.Select(row => row.Select(i => i - '0').ToArray()).ToArray();

            for (int y = 1; y < grid.Length - 1; y++)
            {
                int[] row = grid[y];

                for (int x = 1; x < row.Length - 1; x++)
                {
                    int current = row[x];

                    // look left
                    IEnumerable<int> left = row[..x];

                    if (left.All(x => x < current))
                    {
                        visible++;
                        continue;
                    }

                    // look right
                    IEnumerable<int> right = row[(x + 1)..];

                    if (right.All(x => x < current))
                    {
                        visible++;
                        continue;
                    }

                    // look up
                    IEnumerable<int> above = grid.Select(i => i[x]).Take(y);

                    if (above.All(x => x < current))
                    {
                        visible++;
                        continue;
                    }

                    // look down
                    IEnumerable<int> below = grid.Select(i => i[x]).TakeLast(grid.Length - y - 1);

                    if (below.All(x => x < current))
                    {
                        visible++;
                        continue;
                    }
                }
            }

            return visible + (grid.Length * 2) + (grid.Length * 2) - 4;
        }

        public int Part2(string[] input)
        {
            int max = int.MinValue;
            int[][] grid = input.Select(row => row.Select(i => i - '0').ToArray()).ToArray();

            for (int y = 1; y < grid.Length - 1; y++)
            {
                int[] row = grid[y];

                for (int x = 1; x < row.Length - 1; x++)
                {
                    int current = row[x];
                    int upVisible = 0, downVisible = 0, leftVisible = 0, rightVisible = 0;

                    // look left
                    IEnumerable<int> left = row[..x];

                    foreach (int i in left.Reverse())
                    {
                        leftVisible++;

                        if (i >= current)
                        {
                            break;
                        }
                    }

                    // look right
                    IEnumerable<int> right = row[(x + 1)..];

                    foreach (int i in right)
                    {
                        rightVisible++;

                        if (i >= current)
                        {
                            break;
                        }
                    }

                    // look up
                    IEnumerable<int> above = grid.Select(i => i[x]).Take(y);

                    foreach (int i in above.Reverse())
                    {
                        upVisible++;

                        if (i >= current)
                        {
                            break;
                        }
                    }

                    // look down
                    IEnumerable<int> below = grid.Select(i => i[x]).Skip(y + 1);

                    foreach (int i in below)
                    {
                        downVisible++;

                        if (i >= current)
                        {
                            break;
                        }
                    }

                    // check total score
                    int total = leftVisible * rightVisible * upVisible * downVisible;
                    max = Math.Max(total, max);
                }
            }

            return max;
        }
    }
}
