using System.IO;
using AdventOfCode.Utilities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day22Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day22 solver;

        public Day22Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day22();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day22.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 56372;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 22 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 197047;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 22 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }

        private static readonly char[,] Grid = new char[200, 150];

        [Theory]

        [InlineData("top/north -> back/left/east", 92, 0, Bearing.North, 0, 192, Bearing.East)]
        [InlineData("top/south -> front/top/south", 92, 49, Bearing.South, 92, 50, Bearing.South)]
        [InlineData("top/east -> right/left/east", 99, 42, Bearing.East, 100, 42, Bearing.East)]
        [InlineData("top/west -> left/left/east", 50, 42, Bearing.West, 0, 107, Bearing.East)]
        [InlineData("top/west/cornertop -> left/left/east", 50, 0, Bearing.West, 0, 149, Bearing.East)]
        [InlineData("top/west/cornerbottom -> left/left/east", 50, 49, Bearing.West, 0, 100, Bearing.East)]

        [InlineData("right/north -> back/bottom/north", 142, 0, Bearing.North, 42, 199, Bearing.North)]
        [InlineData("right/south -> front/right/west", 142, 49, Bearing.South, 99, 92, Bearing.West)]
        [InlineData("right/east -> bottom/right/west", 149, 42, Bearing.East, 99, 107, Bearing.West)]
        [InlineData("right/east/cornertop -> bottom/right/west", 149, 0, Bearing.East, 99, 149, Bearing.West)]
        [InlineData("right/east/cornerbottom -> bottom/right/west", 149, 49, Bearing.East, 99, 100, Bearing.West)]
        [InlineData("right/west -> top/right/west", 100, 42, Bearing.West, 99, 42, Bearing.West)]
            
        [InlineData("front/north -> top/bottom/north", 92, 50, Bearing.North, 92, 49, Bearing.North)]
        [InlineData("front/south -> bottom/top/south", 92, 99, Bearing.South, 92, 100, Bearing.South)]
        [InlineData("front/east -> right/bottom/north", 99, 92, Bearing.East, 142, 49, Bearing.North)]
        [InlineData("front/west -> left/top/south", 50, 92, Bearing.West, 42, 100, Bearing.South)]

        [InlineData("left/north -> front/left/east", 42, 100, Bearing.North, 50, 92, Bearing.East)]
        [InlineData("left/south -> back/top/south", 42, 149, Bearing.South, 42, 150, Bearing.South)]
        [InlineData("left/east -> bottom/left/east", 49, 142, Bearing.East, 50, 142, Bearing.East)]
        [InlineData("left/west -> top/left/east", 0, 142, Bearing.West, 50, 7, Bearing.East)]
        [InlineData("left/west/cornertop -> top/left/east", 0, 100, Bearing.West, 50, 49, Bearing.East)]
        [InlineData("left/west/cornerbottom -> top/left/east", 0, 149, Bearing.West, 50, 0, Bearing.East)]

        [InlineData("bottom/north -> front/bottom/north", 92, 100, Bearing.North, 92, 99, Bearing.North)]
        [InlineData("bottom/south -> back/right/west", 92, 149, Bearing.South, 49, 192, Bearing.West)] 
        [InlineData("bottom/east -> right/right/west", 99, 142, Bearing.East, 149, 7, Bearing.West)]
        [InlineData("bottom/east/cornertop -> right/right/west", 99, 100, Bearing.East, 149, 49, Bearing.West)]
        [InlineData("bottom/east/cornerbottom -> right/right/west", 99, 149, Bearing.East, 149, 0, Bearing.West)]
        [InlineData("bottom/west -> left/right/west", 50, 142, Bearing.West, 49, 142, Bearing.West)]

        [InlineData("back/north -> left/bottom/north", 42, 150, Bearing.North, 42, 149, Bearing.North)]
        [InlineData("back/south -> right/top/south", 42, 199, Bearing.South, 142, 0, Bearing.South)]
        [InlineData("back/east -> bottom/bottom/north", 49, 192, Bearing.East, 92, 149, Bearing.North)]
        [InlineData("back/west -> top/top/south", 0, 192, Bearing.West, 92, 0, Bearing.South)]

        public void MoveCube_WhenCalled_MovesCorrectly(string comment, int initialX, int initialY, Bearing initialBearing, int expectedX, int expectedY, Bearing expectedBearing)
        {
            Point2D start = (initialX, initialY);

            (Point2D end, Bearing bearing) actual = Day22.MoveCube(start, initialBearing, Grid);

            actual.Should().Be((new Point2D(expectedX, expectedY), expectedBearing), comment);
        }
    }
}
