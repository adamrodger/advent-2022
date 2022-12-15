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

            /*
             * From part 2 we know that actually there's only 1 range at this point because y=2_000_000
             * will always be one contiguous region. In the general case that's not true because the
             * answer for part 2 could theoretically be on y=2_000_000, so we'll stick with this general
             * solution instead of that minor optimisation
             */
            return ranges.Select(r => r.max - r.min).Sum();
        }

        public long Part2(string[] input)
        {
            ICollection<Sensor> sensors = ParseSensors(input);

            var leftCoefficients = new List<int>(sensors.Count * 2);
            var rightCoefficients = new List<int>(sensors.Count * 2);

            // get the 4 gradients created by the diamond around each sensor
            foreach (Sensor sensor in sensors)
            {
                leftCoefficients.Add(sensor.Location.Y - sensor.Location.X + sensor.Radius + 1);
                leftCoefficients.Add(sensor.Location.Y - sensor.Location.X- sensor.Radius - 1);
                rightCoefficients.Add(sensor.Location.Y + sensor.Location.X + sensor.Radius + 1);
                rightCoefficients.Add(sensor.Location.Y + sensor.Location.X - sensor.Radius - 1);
            }

            // intersect each left-traveling gradient line with each right-traveling one
            foreach (int left in leftCoefficients)
            {
                foreach (int right in rightCoefficients)
                {
                    // find where the 2 sensor lines intersect
                    Point2D intersect = ((right - left) / 2, (left + right) / 2);

                    if (intersect.X is <= 0 or >= 4_000_000 || intersect.Y is <= 0 or >= 4_000_000)
                    {
                        // stay inside the bounded box
                        continue;
                    }

                    if (sensors.All(sensor => sensor.Location.ManhattanDistance(intersect) > sensor.Radius))
                    {
                        // this point is out of range of every scanner
                        return 4_000_000L * intersect.X + intersect.Y;
                    }
                }
            }

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
                // Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                string[] split = line.Split(' ');

                (int sensorX, int sensorY, int beaconX, int beaconY) = (int.Parse(split[2][2..^1]),
                                                                        int.Parse(split[3][2..^1]),
                                                                        int.Parse(split[8][2..^1]),
                                                                        int.Parse(split[9][2..]));

                Point2D location = (sensorX, sensorY);
                Point2D beacon = (beaconX, beaconY);

                int radius = location.ManhattanDistance(beacon);

                var sensor = new Sensor(location, radius);
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
                int minY = sensor.Location.Y - sensor.Radius;
                int maxY = sensor.Location.Y + sensor.Radius;

                if (minY > y || maxY < y)
                {
                    continue;
                }

                int steps = Math.Abs(sensor.Location.Y - y);
                int minX = sensor.Location.X - sensor.Radius + steps;
                int maxX = sensor.Location.X + sensor.Radius - steps;

                ranges.Add((minX, maxX));
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

        private record Sensor(Point2D Location, int Radius);
    }
}
