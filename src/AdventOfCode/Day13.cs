using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 13
    /// </summary>
    public class Day13
    {
        public int Part1(string[] input)
        {
            int i = 1;
            int total = 0;
            var comparer = new PacketComparer();

            foreach (string[] lines in input.Append(string.Empty).Chunk(3))
            {
                Packet left = Parse(lines[0]);
                Packet right = Parse(lines[1]);

                if (comparer.Compare(left, right) <= 0)
                {
                    total += i;
                }

                i++;
            }

            return total;
        }

        public int Part2(string[] input)
        {
            Packet extra1 = Parse("[[2]]");
            Packet extra2 = Parse("[[6]]");

            List<Packet> packets = input.Where(l => !string.IsNullOrEmpty(l))
                                        .Select(Parse)
                                        .Append(extra1)
                                        .Append(extra2)
                                        .ToList();

            packets.Sort(new PacketComparer());

            return (packets.IndexOf(extra1) + 1) * (packets.IndexOf(extra2) + 1);
        }

        /// <summary>
        /// Parse a packet from a string
        /// </summary>
        /// <param name="line">Line to parse</param>
        /// <returns>Parsed packet</returns>
        private static Packet Parse(string line)
        {
            int i = 0;
            return Parse(line.AsSpan(), ref i);
        }

        /// <summary>
        /// Parse a packet from the given read index, which will be mutated as parsing progresses
        /// </summary>
        /// <param name="line">Line to parse</param>
        /// <param name="i">Current read index</param>
        /// <returns>Parsed packet</returns>
        /// <exception cref="NotSupportedException">Malformed line</exception>
        private static Packet Parse(ReadOnlySpan<char> line, ref int i) => line[i] switch
        {
            // e.g. [[[5,4],5]]
            '[' => ParseList(line, ref i),
            >= '0' and <= '9' => ParseNumber(line, ref i),
            _ => throw new NotSupportedException($"Unable to parse item for line starting {line[0]}")
        };

        /// <summary>
        /// Parse a list packet from the index onwards
        /// </summary>
        /// <param name="line">Line to parse</param>
        /// /// <param name="i">Current read index</param>
        /// <returns>List packet</returns>
        private static ListPacket ParseList(ReadOnlySpan<char> line, ref int i)
        {
            i++; // skip over opening [

            ListPacket packet = new ListPacket(new List<Packet>());

            while (line[i] != ']')
            {
                if (line[i] == ',')
                {
                    // skip comma
                    i++;
                }

                Packet child = Parse(line, ref i);
                packet.Children.Add(child);
            }

            i++; // skip closing ]

            return packet;
        }

        /// <summary>
        /// Parse a number from the index onwards
        /// </summary>
        /// <param name="line">Line to parse</param>
        /// /// <param name="i">Current read index</param>
        /// <returns>Parsed number</returns>
        private static NumberPacket ParseNumber(ReadOnlySpan<char> line, ref int i)
        {
            int start = i;

            while (char.IsAsciiDigit(line[i]))
            {
                i++;
            }

            int number = int.Parse(line[start..i]);
            return new NumberPacket(number);
        }

        private abstract record Packet;
        private record ListPacket(IList<Packet> Children) : Packet;
        private record NumberPacket(int Value) : Packet;

        private class PacketComparer : IComparer<Packet>
        {
            public int Compare(Packet x, Packet y) => (x, y) switch
            {
                (NumberPacket left, NumberPacket right) => left.Value - right.Value,
                (ListPacket left, ListPacket right) => Compare(left, right),
                (ListPacket left, NumberPacket right) => Compare(left, new ListPacket(new Packet[] { right })),
                (NumberPacket left, ListPacket right) => Compare(new ListPacket(new Packet[] { left }), right),
                _ => throw new ArgumentOutOfRangeException()
            };

            private int Compare(ListPacket left, ListPacket right)
            {
                // compare left and right children one by one and stop if we hit an unordered pair
                for (int i = 0; i < Math.Min(left.Children.Count, right.Children.Count); i++)
                {
                    Packet leftPacket = left.Children[i];
                    Packet rightPacket = right.Children[i];

                    int compare = this.Compare(leftPacket, rightPacket);

                    if (compare != 0)
                    {
                        return compare;
                    }
                }

                // child collections were in order, compare by length instead
                return left.Children.Count - right.Children.Count;
            }
        }
    }
}
