using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day25Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day25 solver;

        public Day25Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day25();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day25.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "1=-0-2",
                "12111",
                "2=0=",
                "21",
                "2=01",
                "111",
                "20012",
                "112",
                "1=-1=",
                "1-12",
                "12",
                "1=",
                "122"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = "2=-1=0";

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = "2=01-0-2-0=-0==-1=01";

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 25 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
