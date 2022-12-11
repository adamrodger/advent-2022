using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 5
    /// </summary>
    public class Day5
    {
        private const int StackCount = 9;
        private const int MaxHeight = 8;

        public string Part1(string[] input)
        {
            var stacks = ParseStacks(input);

            foreach (string line in input.Skip(MaxHeight + 2))
            {
                (int moves, int source, int dest) = ParseLine(line);

                for (int i = 0; i < moves; i++)
                {
                    char temp = stacks[source].Pop();
                    stacks[dest].Push(temp);
                }
            }

            return new string(stacks.Select(s => s.Peek()).ToArray());
        }

        public string Part2(string[] input)
        {
            var temp = new List<char>();
            var stacks = ParseStacks(input);

            foreach (string line in input.Skip(MaxHeight + 2))
            {
                (int moves, int source, int dest) = ParseLine(line);

                for (int i = 0; i < moves; i++)
                {
                    char crate = stacks[source].Pop();
                    temp.Add(crate);
                }

                foreach (char c in Enumerable.Reverse(temp))
                {
                    stacks[dest].Push(c);
                }

                temp.Clear();
            }

            return new string(stacks.Select(s => s.Peek()).ToArray());
        }

        private static Stack<char>[] ParseStacks(string[] input)
        {
            var stacks = new Stack<char>[StackCount];

            for (int i = 0; i < StackCount; i++)
            {
                stacks[i] = new Stack<char>();

                foreach (string line in input.Take(MaxHeight).Reverse())
                {
                    char crate = line[i * 4 + 1];

                    if (!char.IsAsciiLetterUpper(crate))
                    {
                        // top of the stack reached
                        break;
                    }

                    stacks[i].Push(crate);
                }
            }

            return stacks;
        }

        private static (int Moves, int Source, int Dest) ParseLine(string line)
        {
            // move 1 from 3 to 5
            string[] split = line.Split(' ');
            int moves = int.Parse(split[1]);
            int source = split[3][0] - '1';
            int dest = split[5][0] - '1';

            return (moves, source, dest);
        }
    }
}
