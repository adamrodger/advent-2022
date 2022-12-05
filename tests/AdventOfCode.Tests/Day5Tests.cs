using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day5Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day5 solver;

        public Day5Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day5();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day5.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = "MQTPGLLDN";

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 5 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = "LVZPSTTCZ";

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 5 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
