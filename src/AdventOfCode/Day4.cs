using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 4
    /// </summary>
    public class Day4
    {
        public int Part1(string[] input)
        {
            int count = 0;

            foreach (string line in input)
            {
                (var elf1, var elf2) = ParseLine(line);

                //   2 3
                // 1 2 3 4
                if (elf1.Min >= elf2.Min && elf1.Max <= elf2.Max)
                {
                    count++;
                }
                // 1 2 3 4
                //   2 3
                else if (elf1.Min <= elf2.Min && elf1.Max >= elf2.Max)
                {
                    count++;
                }
            }

            return count;
        }

        public int Part2(string[] input)
        {
            int count = 0;

            foreach (string line in input)
            {
                (var elf1, var elf2) = ParseLine(line);

                //   2 3
                // 1 2 3 4
                if (elf1.Min >= elf2.Min && elf1.Max <= elf2.Max)
                {
                    count++;
                }
                // 1 2 3 4
                //   2 3
                else if (elf1.Min <= elf2.Min && elf1.Max >= elf2.Max)
                {
                    count++;
                }
                // 1 2 3
                //   2 3 4
                else if (elf1.Min <= elf2.Min && elf1.Max <= elf2.Max && elf1.Max >= elf2.Min)
                {
                    count++;
                }
                //   2 3 4
                // 1 2 3
                else if (elf2.Min <= elf1.Min && elf2.Max <= elf1.Max && elf2.Max >= elf1.Min)
                {
                    count++;
                }
            }

            return count;
        }

        private static ((int Min, int Max) Elf1, (int Min, int Max) Elf2) ParseLine(string line)
        {
            string[] elves = line.Split(',');
            string[] elf1 = elves[0].Split('-');
            string[] elf2 = elves[1].Split('-');
            int elf1Min = int.Parse(elf1[0]);
            int elf1Max = int.Parse(elf1[1]);
            int elf2Min = int.Parse(elf2[0]);
            int elf2Max = int.Parse(elf2[1]);

            return ((elf1Min, elf1Max), (elf2Min, elf2Max));
        }
    }
}
