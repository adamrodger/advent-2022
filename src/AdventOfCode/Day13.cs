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
                if (Compare(lines[0], lines[1]))
                {
                    total += i;
                }

                i++;
            }

            return total;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        public static bool Compare(string left, string right)
        {
            JToken leftObj = JArray.Parse(left);
            JToken rightObj = JArray.Parse(right);

            return Compare(leftObj, rightObj) == Outcome.Valid;
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

        private enum Outcome
        {
            Valid,
            Invalid,
            Undecided
        }
    }
}
