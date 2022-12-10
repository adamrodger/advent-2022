using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day10Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day10 solver;

        public Day10Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day10();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day10.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 15880;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 10 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = string.Join(Environment.NewLine,
                                       "■■■  ■     ■■  ■■■■ ■  ■  ■■  ■■■■  ■■  ",
                                       "■  ■ ■    ■  ■ ■    ■ ■  ■  ■    ■ ■  ■ ",
                                       "■  ■ ■    ■    ■■■  ■■   ■  ■   ■  ■    ",
                                       "■■■  ■    ■ ■■ ■    ■ ■  ■■■■  ■   ■ ■■ ",
                                       "■    ■    ■  ■ ■    ■ ■  ■  ■ ■    ■  ■ ",
                                       "■    ■■■■  ■■■ ■    ■  ■ ■  ■ ■■■■  ■■■ ");

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 10 - Part 2 -\n\n{result}");

            Assert.Equal(expected.Trim(), result.Trim());
        }
    }
}
