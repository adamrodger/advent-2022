using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 20
    /// </summary>
    public class Day20
    {
        public long Part1(string[] input) => Simulate(input, 1, 1);

        public long Part2(string[] input) => Simulate(input, 10, 811_589_153);

        private static long Simulate(string[] input, int rounds, int multiplier)
        {
            LinkedList<long> values = new(input.Select(x => long.Parse(x) * multiplier));

            // populate node lookup
            Dictionary<int, LinkedListNode<long>> lookup = new(values.Count);
            LinkedListNode<long> current = values.First;
            LinkedListNode<long> zero = null;

            for (int i = 0; i < values.Count; i++)
            {
                lookup[i] = current;

                if (current.Value == 0)
                {
                    zero = current;
                }

                current = current.Next;
            }

            // mix the values for the required number of rounds
            for (int round = 0; round < rounds; round++)
            {
                Mix(values, lookup);
            }

            current = zero;
            long total = 0;

            for (int i = 1; i <= 3001; i++)
            {
                if (i > 1000 && i % 1000 == 1)
                {
                    total += current.Value;
                }

                current = current.Next ?? values.First;
            }

            return total;
        }

        private static void Mix(LinkedList<long> values, IDictionary<int, LinkedListNode<long>> lookup)
        {
            // mix the values
            for (int i = 0; i < lookup.Count; i++)
            {
                // remove the current node
                LinkedListNode<long> current = lookup[i];
                LinkedListNode<long> sibling = current.Next ?? values.First;

                values.Remove(current);

                // add it back in the right place
                long moves = current.Value;

                if (moves > 0)
                {
                    moves %= values.Count;

                    for (int m = 0; m < moves; m++)
                    {
                        sibling = sibling.Next ?? values.First;
                    }

                    values.AddBefore(sibling, current);
                }
                else
                {
                    moves = Math.Abs(moves) % values.Count;

                    for (int m = 0; m < moves; m++)
                    {
                        sibling = sibling.Previous ?? values.Last;
                    }

                    values.AddBefore(sibling, current);
                }
            }
        }
    }
}
