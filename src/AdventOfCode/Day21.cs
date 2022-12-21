using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            return Resolve("root", numbers, operations);
        }

        public long Part2(string[] input)
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

            numbers.Remove("humn");

            var root = operations["root"];

            // resolve as many of the numbers as we can, where one will fail because it needs humn and we've removed it
            IDictionary<string, long> partiallySolved = new Dictionary<string, long>(numbers);

            long leftValue = Resolve(root.Left, partiallySolved, operations);
            long rightValue = Resolve(root.Right, partiallySolved, operations);

            return leftValue < 1
                       ? Find(root.Left, rightValue, partiallySolved, operations)
                       : Find(root.Right, leftValue, partiallySolved, operations);
        }

        /// <summary>
        /// Resolve all operations from the given node downwards
        /// </summary>
        /// <param name="id">Current node</param>
        /// <param name="numbers">Already resolved nodes</param>
        /// <param name="operations">Operations lookup</param>
        /// <returns>Value of the given node after resolving</returns>
        private static long Resolve(string id, IDictionary<string, long> numbers, IDictionary<string, (string Left, string Right, Operation Operation)> operations)
        {
            if (numbers.TryGetValue(id, out long value))
            {
                return value;
            }

            (string leftId, string rightId, Operation operation) = operations[id];

            long left = -1;
            long right = -1;

            try
            {
                left = Resolve(leftId, numbers, operations);
            }
            catch (KeyNotFoundException)
            {
                // for part 2, one path will be unresolvable so we'll get this problem
                Debugger.Break();
            }

            try
            {
                right = Resolve(rightId, numbers, operations);
            }
            catch (KeyNotFoundException)
            {
                Debugger.Break();
            }

            if (left > 0 && right > 0)
            {
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

            return -1;
        }

        /// <summary>
        /// Go down the path of unresolved operations and work out what each step of that path needs to be
        /// </summary>
        /// <param name="id">Current node to solve</param>
        /// <param name="target">Target value to solve for</param>
        /// <param name="resolved">Already resolved nodes</param>
        /// <param name="operations">Operations lookup</param>
        /// <returns>Value for this node</returns>
        private static long Find(string id,
                                 long target,
                                 IDictionary<string, long> resolved,
                                 IDictionary<string, (string Left, string Right, Operation Operation)> operations)
        {
            if (id == "humn")
            {
                return target;
            }

            if (resolved.TryGetValue(id, out long value))
            {
                return value;
            }

            (string leftId, string rightId, Operation operation) = operations[id];

            if (resolved.TryGetValue(leftId, out value))
            {
                // we have left, solve for right
                long newTarget = operation switch
                {
                    Operation.Add => target - value,
                    Operation.Subtract => value - target,
                    Operation.Divide => target * value,
                    Operation.Multiply => target / value,
                    _ => throw new ArgumentOutOfRangeException()
                };

                long right = Find(rightId, newTarget, resolved, operations);
                resolved[rightId] = right;
                return right;
            }
            else
            {
                // we have right, solve for left
                value = resolved[rightId];

                long newTarget = operation switch
                {
                    Operation.Add => target - value,
                    Operation.Subtract => target + value,
                    Operation.Divide => target * value,
                    Operation.Multiply => target / value,
                    _ => throw new ArgumentOutOfRangeException()
                };

                long left = Find(leftId, newTarget, resolved, operations);
                resolved[leftId] = left;
                return left;
            }
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
