using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        public int Part1(string[] input)
        {
            (Graph<Point2D> graph, ICollection<Point2D> start, Point2D end) = BuildGraph(input, Part.One);

            List<(Point2D node, int distance)> shortest = graph.GetShortestPath(start.Single(), end);

            return shortest.Count + 2; // for start and end node...?
        }

        public int Part2(string[] input)
        {
            (Graph<Point2D> graph, ICollection<Point2D> start, Point2D end) = BuildGraph(input, Part.Two);

            return start.Select(s => graph.GetShortestPath(s, end))
                        .Where(path => path != null)
                        .Select(path => path.Count + 2)
                        .Min();
        }

        private static (Graph<Point2D>, ICollection<Point2D> start, Point2D end) BuildGraph(string[] input, Part part)
        {
            var grid = input.ToGrid();
            var graph = new Graph<Point2D>();

            ICollection<Point2D> start = new List<Point2D>();
            Point2D end = (0, 0);

            grid.ForEach((x, y, c) =>
            {
                Point2D current = (x, y);

                if (c == 'E')
                {
                    end = (x, y);
                    return;
                }

                if (part == Part.Two && c == 'a')
                {
                    start.Add(current);
                }

                if (c == 'S')
                {
                    c = 'a';
                    start.Add(current);
                }

                foreach (Point2D neighbour in grid.Adjacent4Positions(current))
                {
                    char neighbourValue = grid[neighbour.Y, neighbour.X];

                    if (neighbourValue == 'E' || neighbourValue - c < 2)
                    {
                        graph.AddVertex(current, neighbour);
                    }
                }
            });

            return (graph, start, end);
        }
    }
}
