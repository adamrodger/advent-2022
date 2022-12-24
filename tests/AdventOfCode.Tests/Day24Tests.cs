using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day24Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day24 solver;

        public Day24Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day24();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day24.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "#.######",
                "#>>.<^<#",
                "#.<..<<#",
                "#>v.><>#",
                "#<^v^^>#",
                "######.#"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 18;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 308;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 24 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 54;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 908;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 24 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
