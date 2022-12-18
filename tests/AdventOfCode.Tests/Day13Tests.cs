using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day13Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day13 solver;

        public Day13Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day13();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day13.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "[1,1,3,1,1]",
                "[1,1,5,1,1]",
                "",
                "[[1],[2,3,4]]",
                "[[1],4]",
                "",
                "[9]",
                "[[8,7,6]]",
                "",
                "[[4,4],4,4]",
                "[[4,4],4,4,4]",
                "",
                "[7,7,7,7]",
                "[7,7,7]",
                "",
                "[]",
                "[3]",
                "",
                "[[[]]]",
                "[[]]",
                "",
                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                "[1,[2,[3,[4,[5,6,0]]]],8,9]"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 13;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 6395;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 13 - Part 1 - {result}");

            // 6636 is too high
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 140;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 24921;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 13 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
