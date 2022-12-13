using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

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

            foreach (string[] lines in input.Append(string.Empty).Chunk(3))
            {
                if (Compare(lines[0], lines[1]) == Outcome.Valid)
                {
                    total += i;
                }

                i++;
            }

            return total;
        }

        public int Part2(string[] input)
        {
            var packets = input.Where(l => !string.IsNullOrEmpty(l)).Concat(new[] { "[[2]]", "[[6]]" }).ToList();

            packets.Sort(new PacketComparer());

            return (packets.IndexOf("[[2]]") + 1) * (packets.IndexOf("[[6]]") + 1);
        }

        public static Outcome Compare(string left, string right)
        {
            JToken leftObj = JArray.Parse(left);
            JToken rightObj = JArray.Parse(right);

            return Compare(leftObj, rightObj);
        }

        private static Outcome Compare(JToken left, JToken right)
        {
            switch (left.Type, right.Type)
            {
                case (JTokenType.Integer, JTokenType.Integer):
                    long leftValue = (long)((JValue)left).Value;
                    long rightValue = (long)((JValue)right).Value;

                    return leftValue == rightValue
                               ? Outcome.Undecided
                               : leftValue < rightValue
                                   ? Outcome.Valid
                                   : Outcome.Invalid;

                case (JTokenType.Array, JTokenType.Array):
                    List<JToken> leftParts = left.Children().ToList();
                    List<JToken> rightParts = right.Children().ToList();

                    for (int i = 0; i < leftParts.Count; i++)
                    {
                        if (rightParts.Count < i + 1)
                        {
                            // right ran out
                            return Outcome.Invalid;
                        }

                        Outcome comparison = Compare(leftParts[i], rightParts[i]);

                        if (comparison != Outcome.Undecided)
                        {
                            return comparison;
                        }
                    }

                    if (leftParts.Count == rightParts.Count)
                    {
                        return Outcome.Undecided;
                    }

                    return Outcome.Valid; // left was shorter, so valid

                case (JTokenType.Array, JTokenType.Integer):
                    JArray tempRight = new JArray(new[] { right });
                    return Compare(left, tempRight);

                case (JTokenType.Integer, JTokenType.Array):
                    JArray tempLeft = new JArray(new[] { left });
                    return Compare(tempLeft, right);

                default:
                    throw new NotSupportedException();
            }

        }

        public enum Outcome
        {
            Valid,
            Invalid,
            Undecided
        }

        public class PacketComparer : IComparer<string>
        {
            /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
            /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="x" /> is less than <paramref name="y" />.</description></item><item><term> Zero</term><description><paramref name="x" /> equals <paramref name="y" />.</description></item><item><term> Greater than zero</term><description><paramref name="x" /> is greater than <paramref name="y" />.</description></item></list></returns>
            public int Compare(string x, string y) => Day13.Compare(x, y) switch
            {
                Outcome.Valid => -1,
                Outcome.Invalid => 1,
                _ => 0
            };
        }
    }
}
