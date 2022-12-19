using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        public int Part1(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                var blueprint = Parse(line);
                int optimum = Optimise(blueprint, 24);

                total += blueprint.Id * optimum;
            }

            return total;
        }

        public long Part2(string[] input)
        {
            Blueprint first = Parse(input[0]);
            long optimum = Optimise(first, 32);

            Blueprint second = Parse(input[1]);
            optimum *= Optimise(second, 32);

            Blueprint third = Parse(input[2]);
            optimum *= Optimise(third, 32);

            return optimum;
        }

        private static Blueprint Parse(string line)
        {
            var numbers = line.Numbers<int>();
            Blueprint blueprint = new(numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5], numbers[6]);
            return blueprint;
        }

        private static int Optimise(Blueprint blueprint, int minutes)
        {
            List<GameState> states = new() { new GameState(1, 0, 0, 0, 0, 0, 0, 0) };

            int optimum = 0;

            for (int t = 0; t < minutes; t++)
            {
                HashSet<GameState> nextStates = new();

                foreach (GameState state in states)
                {
                    int nextOre = state.Ore + state.OreRobots;
                    int nextClay = state.Clay + state.ClayRobots;
                    int nextObsidian = state.Obsidian + state.ObsidianRobots;
                    int nextGeodes = state.Geodes + state.GeodeRobots;

                    optimum = Math.Max(optimum, nextGeodes);

                    if (state.Ore >= blueprint.OreRobotCost)
                    {
                        nextStates.Add(state with
                        {
                            Ore = nextOre - blueprint.OreRobotCost,
                            Clay = nextClay,
                            Obsidian = nextObsidian,
                            Geodes = nextGeodes,
                            OreRobots = state.OreRobots + 1
                        });
                    }

                    if (state.Ore >= blueprint.ClayRobotCost)
                    {
                        nextStates.Add(state with
                        {
                            Ore = nextOre - blueprint.ClayRobotCost,
                            Clay = nextClay,
                            Obsidian = nextObsidian,
                            Geodes = nextGeodes,
                            ClayRobots = state.ClayRobots + 1
                        });
                    }

                    if (state.Clay >= blueprint.ObsidianRobotClayCost && state.Ore >= blueprint.ObsidianRobotOreCost)
                    {
                        nextStates.Add(state with
                        {
                            Ore = nextOre - blueprint.ObsidianRobotOreCost,
                            Clay = nextClay - blueprint.ObsidianRobotClayCost,
                            Obsidian = nextObsidian,
                            Geodes = nextGeodes,
                            ObsidianRobots = state.ObsidianRobots + 1
                        });
                    }

                    if (state.Obsidian >= blueprint.GeodeRobotObsidianCost && state.Ore >= blueprint.GeodeRobotOreCost)
                    {
                        nextStates.Add(state with
                        {
                            Ore = nextOre - blueprint.GeodeRobotOreCost,
                            Clay = nextClay,
                            Obsidian = nextObsidian - blueprint.GeodeRobotObsidianCost,
                            Geodes = nextGeodes,
                            GeodeRobots = state.GeodeRobots + 1
                        });
                    }

                    nextStates.Add(state with
                    {
                        Ore = nextOre,
                        Clay = nextClay,
                        Obsidian = nextObsidian,
                        Geodes = nextGeodes
                    });
                }

                states.Clear();

                if (t == minutes - 1)
                {
                    // finished
                    break;
                }

                int remainingTime = minutes - 1 - t;

                foreach (GameState state in nextStates)
                {
                    int upperBound = state.Geodes + (remainingTime * state.GeodeRobots);

                    // don't explore paths that couldn't possibly win
                    if (upperBound >= optimum)
                    {
                        states.Add(state);
                    }
                }
            }

            return optimum;
        }

        // Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 14 clay. Each geode robot costs 3 ore and 16 obsidian.
        private readonly record struct Blueprint(int Id,
                                                 int OreRobotCost,
                                                 int ClayRobotCost,
                                                 int ObsidianRobotOreCost,
                                                 int ObsidianRobotClayCost,
                                                 int GeodeRobotOreCost,
                                                 int GeodeRobotObsidianCost);

        private readonly record struct GameState(int OreRobots,
                                                 int Ore,
                                                 int ClayRobots,
                                                 int Clay,
                                                 int ObsidianRobots,
                                                 int Obsidian,
                                                 int GeodeRobots,
                                                 int Geodes);
    }
}
