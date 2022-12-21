using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        public long Part1(string[] input)
        {
            Dictionary<string, long> numbers = new();
            Dictionary<string, (string Left, string Right, Operation Operation)> operations = new();

            foreach (string line in input)
            {
                // gjfs: wjcw + gdfb
                // sbwg: 3

                string[] parts = line.Split(' ');

                string id = parts[0][..^1];

                if (parts.Length == 2)
                {
                    numbers[id] = long.Parse(parts[1]);
                }
                else
                {
                    operations[id] = (parts[1], parts[3], parts[2] switch
                    {
                        "+" => Operation.Add,
                        "-" => Operation.Subtract,
                        "*" => Operation.Multiply,
                        "/" => Operation.Divide,
                        _ => throw new ArgumentOutOfRangeException()
                    });
                }
            }

            return Search("root", numbers, operations);
        }

        private long Search(string id, IDictionary<string, long> numbers, IDictionary<string, (string Left, string Right, Operation Operation)> operations)
        {
            if (numbers.TryGetValue(id, out long value))
            {
                return value;
            }

            (string leftId, string rightId, Operation operation) = operations[id];
            
            long left = Search(leftId, numbers, operations);
            long right = Search(rightId, numbers, operations);

            long result = operation switch
            {
                Operation.Add => left + right,
                Operation.Subtract => left - right,
                Operation.Divide => left / right,
                Operation.Multiply => left * right,
                _ => throw new ArgumentOutOfRangeException()
            };

            numbers[id] = result;
            return result;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private enum Operation
        {
            Add,
            Subtract,
            Divide,
            Multiply
        }
    }
}
