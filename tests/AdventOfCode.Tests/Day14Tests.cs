using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day14 solver;

        public Day14Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day14();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day14.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 715;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 14 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 25248;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 14 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
