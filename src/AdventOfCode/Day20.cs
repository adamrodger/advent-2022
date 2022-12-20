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
            // create a circular ring of nodes
            Node[] values = input.Select(x => long.Parse(x) * multiplier)
                                 .Select(n => new Node { Value = n })
                                 .ToArray();

            foreach ((Node first, Node second) in values.Zip(values[1..]))
            {
                first.Next = second;
                second.Previous = first;
            }

            values[0].Previous = values[^1];
            values[^1].Next = values[0];

            // mix the values for the required number of rounds
            for (int round = 0; round < rounds; round++)
            {
                Mix(values);
            }

            // sum the nodes at index 1000, 2000 and 3000 after the node with value 0
            long total = 0;
            Node current = values.First(v => v.Value == 0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    current = current.Next;
                }

                total += current.Value;
            }

            return total;
        }

        private static void Mix(IList<Node> values)
        {
            // mix the values
            foreach (Node current in values)
            {
                // take the node out of the ring
                Node previous = current.Previous;
                Node next = current.Next;

                previous.Next = next;
                next.Previous = previous;

                // move to where it needs to be inserted
                if (current.Value > 0)
                {
                    // go forwards
                    long moves = current.Value % (values.Count - 1);

                    for (int m = 0; m < moves; m++)
                    {
                        previous = previous.Next;
                        next = next.Next;
                    }
                }
                else
                {
                    // go backwards
                    long moves = Math.Abs(current.Value) % (values.Count - 1);

                    for (int m = 0; m < moves; m++)
                    {
                        previous = previous.Previous;
                        next = next.Previous;
                    }
                }

                // add it back in to the ring
                previous.Next = current;
                current.Previous = previous;

                next.Previous = current;
                current.Next = next;
            }
        }

        private class Node
        {
            public long Value { get; init; }
            public Node Next { get; set; }
            public Node Previous { get; set; }
        }
    }
}
