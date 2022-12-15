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
            ICollection<Sensor> sensors = ParseSensors(input);
            ICollection<(int min, int max)> ranges = ClosedLocations(sensors, 2_000_000);
            return ranges.Select(r => r.max - r.min).Sum();
        }

        public long Part2(string[] input)
        {
            ICollection<Sensor> sensors = ParseSensors(input);

            // brute force, ohhhhhhhhhhhh yeahhhhhhhh
            for (int y = 0; y < 4_000_000; y++)
            {
                ICollection<(int min, int max)> ranges = ClosedLocations(sensors, y);

                if (ranges.Count < 2)
                {
                    // every row except the required one must be completely full
                    continue;
                }

                long x = Math.Min(ranges.First().max, ranges.Last().max) + 1;
                return (x * 4_000_000) + y;
            }

            // 1075151795 -- too low... stupid fucking longs :D
            throw new InvalidOperationException("No single point found");
        }

        /// <summary>
        /// Parse the input to a sensors collection
        /// </summary>
        /// <param name="input">Input lines</param>
        /// <returns>Sensors</returns>
        private static ICollection<Sensor> ParseSensors(string[] input)
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

            return sensors;
        }

        /// <summary>
        /// For the given row, find out which ranges of X values are closed
        /// </summary>
        /// <param name="sensors">Sensor definition</param>
        /// <param name="y">Row to check</param>
        /// <returns>All closed ranges</returns>
        private static ICollection<(int min, int max)> ClosedLocations(ICollection<Sensor> sensors, int y)
        {
            var ranges = new List<(int min, int max)>(sensors.Count);

            foreach (Sensor sensor in sensors)
            {
                if (sensor.MinY > y || sensor.MaxY < y)
                {
                    continue;
                }

                int steps = Math.Abs(sensor.Location.Y - y);
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

            return ranges;
        }

        /// <summary>
        /// Find the next overlap in the given set of ranges
        /// </summary>
        /// <param name="ranges">Ranges</param>
        /// <returns>Overlapping pair, or None if there aren't any overlaps</returns>
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
