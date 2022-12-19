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

            // 13167 -- too low
            return optimum;
        }

        private static Blueprint Parse(string line)
        {
            var numbers = line.Numbers<int>();

            // work out which robot costs the most ore to make
            int maxOreCost = Math.Max(numbers[1], numbers[2]);
            maxOreCost = Math.Max(maxOreCost, numbers[3]);
            maxOreCost = Math.Max(maxOreCost, numbers[4]);

            Blueprint blueprint = new(numbers[0], numbers[1], numbers[2], numbers[3], numbers[4], numbers[5], numbers[6], maxOreCost);
            return blueprint;
        }
        
        private static int Optimise(Blueprint blueprint, int remaining)
        {
            return Optimise(blueprint,
                            remaining,
                            new GameState(1, 0, 0, 0, 0, 0, 0),
                            new Dictionary<(int remaining, GameState state), int>());
        }

        private static int Optimise(Blueprint blueprint,
                                    int remaining,
                                    GameState state,
                                    IDictionary<(int remaining, GameState state), int> cache)
        {
            if (remaining == 0)
            {
                // can't make any new geodes no matter what we do if we've run out of time :D
                return 0;
            }

            GameState next = state with
            {
                Ore = Math.Min(state.Ore + state.OreRobots, blueprint.MaxOreCost * remaining),
                Clay = Math.Min(state.Clay + state.ClayRobots, blueprint.ObsidianRobotClayCost * remaining),
                Obsidian = Math.Min(state.Obsidian + state.ObsidianRobots, blueprint.GeodeRobotObsidianCost * remaining)
            };

            (int, GameState) key = (remaining, next);

            if (cache.TryGetValue(key, out int maxGeodes))
            {
                // seen this state before, just return cached max
                return maxGeodes;
            }

            int geodes;
            bool builtRobot = false;

            if (state.Obsidian >= blueprint.GeodeRobotObsidianCost && state.Ore >= blueprint.GeodeRobotOreCost)
            {
                // try to build a geode robot
                geodes = Optimise(blueprint,
                                  remaining - 1,
                                  next with
                                  {
                                      Ore = next.Ore - blueprint.GeodeRobotOreCost,
                                      Obsidian = next.Obsidian - blueprint.GeodeRobotObsidianCost,
                                      GeodeRobots = next.GeodeRobots + 1
                                  },
                                  cache);

                maxGeodes = Math.Max(maxGeodes, geodes + (remaining - 1));
                builtRobot = true;
            }

            int potentialGeodes = (remaining - 1) / 2 * remaining;
            //int potentialGeodes = maxGeodes + (remaining - 1) * state.GeodeRobots;

            if (potentialGeodes < maxGeodes)
            {
                // this branch can't win
                cache[key] = maxGeodes;
                return maxGeodes;
            }

            if (state.Ore >= blueprint.OreRobotCost)
            {
                // try to build an ore robot
                geodes = Optimise(blueprint,
                                  remaining - 1,
                                  next with
                                  {
                                      Ore = next.Ore - blueprint.OreRobotCost,
                                      OreRobots = next.OreRobots + 1
                                  },
                                  cache);

                maxGeodes = Math.Max(maxGeodes, geodes);
                builtRobot = true;
            }

            if (state.Ore >= blueprint.ClayRobotCost)
            {
                // try to build a clay robot
                geodes = Optimise(blueprint,
                                  remaining - 1,
                                  next with
                                  {
                                      Ore = next.Ore - blueprint.ClayRobotCost,
                                      ClayRobots = next.ClayRobots + 1
                                  },
                                  cache);

                maxGeodes = Math.Max(maxGeodes, geodes);
                builtRobot = true;
            }

            if (state.Clay >= blueprint.ObsidianRobotClayCost && state.Ore >= blueprint.ObsidianRobotOreCost)
            {
                // try to build an obsidian robot
                geodes = Optimise(blueprint,
                                  remaining - 1,
                                  next with
                                  {
                                      Ore = next.Ore - blueprint.ObsidianRobotOreCost,
                                      Clay = next.Clay - blueprint.ObsidianRobotClayCost,
                                      ObsidianRobots = next.ObsidianRobots + 1
                                  },
                                  cache);

                maxGeodes = Math.Max(maxGeodes, geodes);
                builtRobot = true;
            }

            // if we didn't build anything this round, just continue without building
            //if (!builtRobot)
            //{
                geodes = Optimise(blueprint, remaining - 1, next, cache);
                maxGeodes = Math.Max(maxGeodes, geodes);
            //}

            cache[key] = maxGeodes;
            return maxGeodes;
        }

        // Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 14 clay. Each geode robot costs 3 ore and 16 obsidian.
        private readonly record struct Blueprint(int Id,
                                                 int OreRobotCost,
                                                 int ClayRobotCost,
                                                 int ObsidianRobotOreCost,
                                                 int ObsidianRobotClayCost,
                                                 int GeodeRobotOreCost,
                                                 int GeodeRobotObsidianCost,
                                                 int MaxOreCost);

        private readonly record struct GameState(int OreRobots,
                                                 int Ore,
                                                 int ClayRobots,
                                                 int Clay,
                                                 int ObsidianRobots,
                                                 int Obsidian,
                                                 int GeodeRobots);
        /*
        [Flags]
        private enum AvailableRobots
        {
            None = 0,
            Ore = 1,
            Clay = 2,
            Obsidian = 4,
            Geode = 8,

            All = Ore | Clay | Obsidian | Geode
        }*/
    }
}
