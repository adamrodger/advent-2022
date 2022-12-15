using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;
using Optional;
using Optional.Unsafe;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 15
    /// </summary>
    public class Day15
    {
        public int Part1(string[] input)
        {
            var sensors = new List<Sensor>(input.Length);

            foreach (string line in input)
            {
                var numbers = line.Numbers<int>();

                (int sensorX, int sensorY, int beaconX, int beaconY) = (numbers[0], numbers[1], numbers[2], numbers[3]);

                Point2D location = (sensorX, sensorY);
                Point2D beacon = (beaconX, beaconY);

                int distance = Math.Abs(location.X - beacon.X) + Math.Abs(location.Y - beacon.Y);

                int minX = sensorX - distance;
                int maxX = sensorX + distance;
                int minY = sensorY - distance;
                int maxY = sensorY + distance;

                var sensor = new Sensor(location, beacon, minX, maxX, minY, maxY);
                sensors.Add(sensor);
            }

            var ranges = new List<(int min, int max)>();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.MinY > 2_000_000 || sensor.MaxY < 2_000_000)
                {
                    continue;
                }

                int steps = Math.Abs(sensor.Location.Y - 2_000_000);
                int minRow = sensor.MinX + steps;
                int maxRow = sensor.MaxX - steps;

                ranges.Add((minRow, maxRow));
            }

            // merge the ranges together.....
            while (true)
            {
                var overlap = NextOverlap(ranges);

                if (!overlap.HasValue)
                {
                    break;
                }

                ((int min, int max) left, (int min, int max) right) = overlap.ValueOrFailure();

                ranges.Remove(left);
                ranges.Remove(right);

                ranges.Add((Math.Min(left.min, right.min), Math.Max(left.max, right.max)));
            }

            return ranges.Select(r => r.max - r.min).Sum();
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private static Option<((int min, int max) left, (int min, int max) right)> NextOverlap(ICollection<(int min, int max)> ranges)
        {
            foreach ((int min, int max) left in ranges)
            {
                foreach ((int min, int max) right in ranges)
                {
                    if (left == right)
                    {
                        continue;
                    }

                    if (left.min > right.max || left.max < right.min)
                    {
                        // no overlap
                        continue;
                    }

                    return (left, right).Some();
                }
            }

            return Option.None<((int, int), (int, int))>();
        }

        private record Sensor(Point2D Location, Point2D Beacon, int MinX, int MaxX, int MinY, int MaxY);
    }
}
