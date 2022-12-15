using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

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

            int total = 0;

            foreach (Sensor sensor in sensors)
            {
                if (sensor.MinY > 2_000_000 || sensor.MaxY < 2_000_000)
                {
                    continue;
                }

                int steps = Math.Abs(sensor.Location.Y - 2_000_000);
                int minRow = sensor.MinX + steps;
                int maxRow = sensor.MaxX - steps;

                // this is wrong, what about overlaps?
                total += maxRow - minRow;
            }

            return total;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private record Sensor(Point2D Location, Point2D Beacon, int MinX, int MaxX, int MinY, int MaxY);
    }
}
