using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 11
    /// </summary>
    public class Day11
    {
        // https://www.tiger-algebra.com/en/solution/least-common-multiple/lcm(2,3,5,7,11,13,17,19)/
        private const long Part2Relief = 9_699_690;

        public long Part1(string[] input) => Run(input, 20, Part.One);

        public long Part2(string[] input) => Run(input, 10_000, Part.Two);

        private static long Run(string[] input, int rounds, Part part)
        {
            IDictionary<int, Monkey> monkeys = Parse(input);

            for (int round = 0; round < rounds; round++)
            {
                for (int i = 0; i < monkeys.Count; i++)
                {
                    var monkey = monkeys[i];
                    monkey.Inspections += monkey.Items.Count;

                    while (monkey.Items.TryDequeue(out long item))
                    {
                        item = monkey.Operation switch
                        {
                            Operation.Square => item * item,
                            Operation.Add => item + monkey.Operand,
                            Operation.Multiply => item * monkey.Operand,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        item = part switch
                        {
                            Part.One => item / 3,
                            Part.Two => item % Part2Relief,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        int target = item % monkey.Divisor == 0 ? monkey.TrueTarget : monkey.FalseTarget;
                        monkeys[target].Items.Enqueue(item);
                    }
                }
            }

            return monkeys.Values
                          .Select(m => m.Inspections)
                          .OrderByDescending(x => x)
                          .Take(2)
                          .Aggregate(1L, (x, y) => x * y);
        }

        private static IDictionary<int, Monkey> Parse(string[] input)
        {
            var monkeys = new Dictionary<int, Monkey>();
            string[] buffer = new string[6];
            int i = 0;

            foreach (string line in input.Append(string.Empty))
            {
                if (string.IsNullOrEmpty(line))
                {
                    var monkey = Monkey.Parse(buffer);
                    monkeys[monkey.Id] = monkey;
                    i = 0;
                }
                else
                {
                    buffer[i++] = line;
                }
            }

            return monkeys;
        }

        private enum Operation
        {
            Add,
            Multiply,
            Square
        }

        private class Monkey
        {
            public int Id { get; init; }
            public Queue<long> Items { get; init; } = new();
            public Operation Operation { get; init; }
            public long Operand { get; init; }
            public int Divisor { get; init; }
            public int TrueTarget { get; init; }
            public int FalseTarget { get; init; }
            public long Inspections { get; set; }

            public static Monkey Parse(string[] lines)
            {
                int id = lines[0][7] - '0';

                long[] items = lines[1].Numbers<long>();

                var opLine = lines[2].Trim().Split(' ');
                long.TryParse(opLine[5], out long opValue);

                Operation operation = (opLine[4], opLine[5]) switch
                {
                    ("*", "old") => Operation.Square,
                    ("*", _) => Operation.Multiply,
                    ("+", _) => Operation.Add,
                    _ => throw new ArgumentOutOfRangeException()
                };

                int divisor = int.Parse(lines[3].Trim()[19..]);
                int trueTarget = lines[4].Last() - '0';
                int falseTarget = lines[5].Last() - '0';

                return new Monkey
                {
                    Id = id,
                    Items = new Queue<long>(items),
                    Operation = operation,
                    Operand = opValue,
                    Divisor = divisor,
                    TrueTarget = trueTarget,
                    FalseTarget = falseTarget,
                    Inspections = 0
                };
            }
        }
    }
}
