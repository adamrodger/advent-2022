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
        public int Part1(string[] input)
        {
            LinkedList<(int index, int value)> values = new (input.Select((x, i) => (i, int.Parse(x))));

            Dictionary<int, LinkedListNode<(int index, int value)>> lookup = new(values.Count);
            LinkedListNode<(int index, int value)> current = values.First;
            LinkedListNode<(int index, int value)> zero = null;

            do
            {
                lookup[current.Value.index] = current;

                if (current.Value.value == 0)
                {
                    zero = current;
                }

                current = current.Next;
            } while (current != null);

            for (int i = 0; i < lookup.Count; i++)
            {
                // remove the current node
                current = lookup[i];
                LinkedListNode<(int index, int value)> sibling = current.Next ?? values.First;

                values.Remove(current);

                // work out where it's going
                int moves = current.Value.value;

                // add it back in the right place
                if (moves > 0)
                {
                    for (int m = 0; m < moves; m++)
                    {
                        sibling = sibling.Next ?? values.First;
                    }

                    values.AddBefore(sibling, current);
                }
                else
                {
                    for (int m = 0; m < Math.Abs(moves); m++)
                    {
                        sibling = sibling.Previous ?? values.Last;
                    }

                    values.AddBefore(sibling, current);
                }
            }

            current = zero;
            int total = 0;

            for (int i = 1; i <= 3001; i++)
            {
                if (i > 1000 && i % 1000 == 1)
                {
                    total += current.Value.value;
                }

                current = current.Next ?? values.First;
            }

            // 7302 -- too low
            return total;
        }

        public long Part2(string[] input)
        {
            LinkedList<(int index, long value)> values = new(input.Select((x, i) => (i, long.Parse(x) * 811_589_153)));

            Dictionary<int, LinkedListNode<(int index, long value)>> lookup = new(values.Count);
            LinkedListNode<(int index, long value)> current = values.First;
            LinkedListNode<(int index, long value)> zero = null;

            do
            {
                lookup[current.Value.index] = current;

                if (current.Value.value == 0)
                {
                    zero = current;
                }

                current = current.Next;
            } while (current != null);

            for (int round = 0; round < 10; round++)
            {
                for (int i = 0; i < lookup.Count; i++)
                {
                    // remove the current node
                    current = lookup[i];
                    LinkedListNode<(int index, long value)> sibling = current.Next ?? values.First;

                    values.Remove(current);

                    // work out where it's going
                    long moves = current.Value.value;

                    // add it back in the right place
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
                        moves = (Math.Abs(moves) % values.Count) * -1;

                        for (int m = 0; m < Math.Abs(moves); m++)
                        {
                            sibling = sibling.Previous ?? values.Last;
                        }

                        values.AddBefore(sibling, current);
                    }
                }
            }

            current = zero;
            long total = 0;

            for (int i = 1; i <= 3001; i++)
            {
                if (i > 1000 && i % 1000 == 1)
                {
                    total += current.Value.value;
                }

                current = current.Next ?? values.First;
            }

            // 7302 -- too low
            return total;
        }
    }
}
