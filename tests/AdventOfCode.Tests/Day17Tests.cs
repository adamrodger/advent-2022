using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day17Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day17 solver;

        public Day17Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day17();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day17.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 3068;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3149;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 17 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 1514285714288;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 17 - Part 2 - {result}");

            // 1554593201864 too high
            Assert.Equal(expected, result);
        }
    }
}
