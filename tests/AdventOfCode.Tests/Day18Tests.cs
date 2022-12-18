using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day18Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day18 solver;

        public Day18Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day18();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day18.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 4474;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 18 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 2518;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 18 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
