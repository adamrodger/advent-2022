using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

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
            var stacks = Parse(input);

            foreach (string line in input.Skip(MaxHeight + 2))
            {
                var numbers = line.Numbers<int>();
                int moves = numbers[0];
                int source = numbers[1] - 1;
                int dest = numbers[2] - 1;

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
            var stacks = Parse(input);

            foreach (string line in input.Skip(MaxHeight + 2))
            {
                var numbers = line.Numbers<int>();
                int moves = numbers[0];
                int source = numbers[1] - 1;
                int dest = numbers[2] - 1;

                var temp = new List<char>(moves);

                for (int i = 0; i < moves; i++)
                {
                    char crate = stacks[source].Pop();
                    temp.Add(crate);
                }

                temp.Reverse();

                foreach (char c in temp)
                {
                    stacks[dest].Push(c);
                }
                
            }

            return new string(stacks.Select(s => s.Peek()).ToArray());
        }

        private static Stack<char>[] Parse(string[] input)
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
    }
}
