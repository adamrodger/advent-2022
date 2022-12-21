using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day19Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day19 solver;

        public Day19Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day19();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day19.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
                "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian."
            };
        }

        [Fact(Skip = "Takes too long")]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 33;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact(Skip = "Takes too long")]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 978;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 19 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact(Skip = "Takes too long")]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 15939;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 19 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}