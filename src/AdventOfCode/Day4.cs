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
                string[] elves = line.Split(',');
                int[] elf1 = elves[0].Split('-').Select(int.Parse).ToArray();
                int[] elf2 = elves[1].Split('-').Select(int.Parse).ToArray();

                //   2 3
                // 1 2 3 4
                if (elf1[0] >= elf2[0] && elf1[1] <= elf2[1])
                {
                    count++;
                }
                // 1 2 3 4
                //   2 3 
                else if (elf1[0] <= elf2[0] && elf1[1] >= elf2[1])
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
                string[] elves = line.Split(',');
                int[] elf1 = elves[0].Split('-').Select(int.Parse).ToArray();
                int[] elf2 = elves[1].Split('-').Select(int.Parse).ToArray();

                //   2 3
                // 1 2 3 4
                if (elf1[0] >= elf2[0] && elf1[1] <= elf2[1])
                {
                    count++;
                }
                // 1 2 3 4
                //   2 3 
                else if (elf1[0] <= elf2[0] && elf1[1] >= elf2[1])
                {
                    count++;
                }
                // 1 2 3
                //   2 3 4
                else if (elf1[0] <= elf2[0] && elf1[1] <= elf2[1] && elf1[1] >= elf2[0])
                {
                    count++;
                }
                //   2 3 4
                // 1 2 3
                else if (elf2[0] <= elf1[0] && elf2[1] <= elf1[1] && elf2[1] >= elf1[0])
                {
                    count++;
                }
            }

            return count;
        }
    }
}
