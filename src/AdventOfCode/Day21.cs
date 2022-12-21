using System;
using System.Collections.Generic;
using Optional;
using Optional.Unsafe;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        private const string Root = "root";
        private const string Human = "humn";

        public long Part1(string[] input)
        {
            (IDictionary<string, long> resolved, IDictionary<string, Calculation> operations) = ParseInput(input);
            return Calculate(Root, resolved, operations).ValueOrFailure();
        }

        public long Part2(string[] input)
        {
            (IDictionary<string, long> resolved, IDictionary<string, Calculation> operations) = ParseInput(input);

            // we don't know the value of human
            resolved.Remove(Human);

            // resolve as many of the numbers as we can, where one will fail because it needs humn and we've removed it
            Calculation root = operations[Root];
            Calculate(Root, resolved, operations);

            return resolved.ContainsKey(root.Left)
                       ? Find(root.Right, resolved[root.Left], resolved, operations)
                       : Find(root.Left, resolved[root.Right], resolved, operations);
        }

        /// <summary>
        /// Parse the input to a lookup of resolved numbers and calculations to perform
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Parsed input</returns>
        private static (IDictionary<string, long> Resolved, IDictionary<string, Calculation> Operations) ParseInput(string[] input)
        {
            Dictionary<string, long> resolved = new();
            Dictionary<string, Calculation> operations = new();

            foreach (string line in input)
            {
                // gjfs: wjcw + gdfb
                // sbwg: 3
                string[] parts = line.Split(' ');
                string id = parts[0][..^1];

                if (parts.Length == 2)
                {
                    resolved[id] = long.Parse(parts[1]);
                }
                else
                {
                    operations[id] = new Calculation(parts[1], parts[3], parts[2] switch
                    {
                        "+" => Operation.Add,
                        "-" => Operation.Subtract,
                        "*" => Operation.Multiply,
                        "/" => Operation.Divide,
                        _ => throw new ArgumentOutOfRangeException()
                    });
                }
            }

            return (resolved, operations);
        }

        /// <summary>
        /// Calculate all operations from the given node downwards which can be solved
        /// </summary>
        /// <param name="id">Current node</param>
        /// <param name="resolved">Already resolved nodes</param>
        /// <param name="operations">Operations lookup</param>
        /// <returns>Value of the given node after calculating, or none if it can't be calculated currently</returns>
        private static Option<long> Calculate(string id, IDictionary<string, long> resolved, IDictionary<string, Calculation> operations)
        {
            if (resolved.TryGetValue(id, out long value))
            {
                return value.Some();
            }

            if (!operations.ContainsKey(id))
            {
                return Option.None<long>();
            }

            (string leftId, string rightId, Operation operation) = operations[id];

            Option<long> maybeLeft = Calculate(leftId, resolved, operations);
            Option<long> maybeRight = Calculate(rightId, resolved, operations);

            return maybeLeft.FlatMap(left => maybeRight.Map(right =>
            {
                long result = operation switch
                {
                    Operation.Add => left + right,
                    Operation.Subtract => left - right,
                    Operation.Divide => left / right,
                    Operation.Multiply => left * right,
                    _ => throw new ArgumentOutOfRangeException()
                };

                resolved[id] = result;

                return result;
            }));
        }

        /// <summary>
        /// Go down the path of unresolved operations and work out what each step of that path needs to be
        /// </summary>
        /// <param name="id">Current node to solve</param>
        /// <param name="target">Target value to solve for</param>
        /// <param name="resolved">Already resolved nodes</param>
        /// <param name="operations">Operations lookup</param>
        /// <returns>Value for this node</returns>
        private static long Find(string id, long target, IDictionary<string, long> resolved, IDictionary<string, Calculation> operations)
        {
            if (id == Human)
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

        private readonly record struct Calculation(string Left, string Right, Operation Operation);

        private enum Operation
        {
            Add,
            Subtract,
            Divide,
            Multiply
        }
    }
}
