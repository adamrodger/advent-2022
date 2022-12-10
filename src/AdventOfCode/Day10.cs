using System.Text;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 10
    /// </summary>
    public class Day10
    {
        public int Part1(string[] input)
        {
            int total = 0;
            int register = 1;
            int cycles = 0;

            foreach (string line in input)
            {
                // noop and addx are both at least one cycle
                total += CalculateIncrement(ref cycles, register);

                if (line.StartsWith("addx"))
                {
                    // addx has an extra cycle
                    total += CalculateIncrement(ref cycles, register);

                    string[] parts = line.Split(' ');
                    int value = int.Parse(parts[1]);
                    register += value;
                }
            }

            return total;
        }

        public string Part2(string[] input)
        {
            var output = new StringBuilder();
            int register = 1;
            int cycles = 0;

            foreach (string line in input)
            {
                // noop and addx are both at least one cycle
                AddPixel(ref cycles, register, output);

                if (line.StartsWith("addx"))
                {
                    // addx has an extra cycle
                    AddPixel(ref cycles, register, output);

                    string[] parts = line.Split(' ');
                    int value = int.Parse(parts[1]);
                    register += value;
                }
            }

            return output.ToString();
        }

        private static int CalculateIncrement(ref int cycles, int register)
        {
            cycles++;

            // add to total on cycle 20, 60, 100, 140, 180 and 220
            return (cycles - 20) % 40 == 0 ? cycles * register : 0;
        }

        private static void AddPixel(ref int cycles, int register, StringBuilder output)
        {
            int pixel = cycles % 40;

            bool insideSprite = pixel == register - 1 || pixel == register || pixel == register + 1;
            output.Append(insideSprite ? '■' : ' ');

            if (pixel == 39)
            {
                output.AppendLine();
            }

            cycles++;
        }
    }
}
