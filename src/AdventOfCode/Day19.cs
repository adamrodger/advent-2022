using System;
using System.Collections.Generic;
using AdventOfCode.Utilities;
using Optional;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        private static readonly RobotType[] RobotTypes = { RobotType.Geode, RobotType.Obsidian, RobotType.Clay, RobotType.Ore };

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
            Blueprint second = Parse(input[1]);
            Blueprint third = Parse(input[2]);

            long optimum = Optimise(first, 32);
            optimum *= Optimise(second, 32);
            optimum *= Optimise(third, 32);
            
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

        /// <summary>
        /// Get the optimum number of geodes that can be made within the given time limit with the given cost blueprint
        /// </summary>
        /// <param name="blueprint">Blueprint of costs</param>
        /// <param name="remaining">Remaining time</param>
        /// <returns>Maximum number of geodes that would be constructed</returns>
        private static int Optimise(Blueprint blueprint, int remaining)
        {
            int max = 0;
            var state = new GameState(remaining, 1, 0, 0, 0, 0, 0, 0, 0);

            var queue = new Queue<GameState>();
            queue.Enqueue(state);

            while (queue.TryDequeue(out state))
            {
                int potential = state.Geodes                                 // how many geodes we already have
                              + (state.GeodeRobots * state.Remaining)        // how many we'll definitely mine
                              + (state.Remaining - 1) * state.Remaining / 2; // how many we could mine if we built a geode robot every time

                if (potential <= max)
                {
                    // this branch could never win, give up
                    continue;
                }

                bool builtRobot = false;

                if (state.Remaining > 1)
                {
                    foreach (RobotType type in RobotTypes)
                    {
                        TryBuild(state, blueprint, type).MatchSome(next =>
                        {
                            queue.Enqueue(next);
                            builtRobot = true;
                        });
                    }
                }

                if (!builtRobot)
                {
                    // we can no longer build any robots in this state, so fast-forward to the end to see what the result is
                    potential = state.Geodes + (state.GeodeRobots * state.Remaining);
                    max = Math.Max(max, potential);
                }
            }

            return max;
        }

        /// <summary>
        /// From the given starting point and cost blueprint, try to build the given robot type
        /// </summary>
        /// <param name="state">Starting state</param>
        /// <param name="blueprint">Blueprint of costs</param>
        /// <param name="type">Robot type to try to build</param>
        /// <returns>The next state at which the robot is built, or None if the robot can't/shouldn't be built</returns>
        private static Option<GameState> TryBuild(GameState state, Blueprint blueprint, RobotType type)
        {
            // should we build this robot or have we already got enough to satisfy demand every turn?
            bool shouldBuild = type switch
            {
                RobotType.Ore => state.OreRobots < blueprint.MaxOreCost,
                RobotType.Clay => state.ClayRobots < blueprint.ObsidianRobotClayCost,
                RobotType.Obsidian => state.ObsidianRobots < blueprint.GeodeRobotObsidianCost,
                RobotType.Geode => true,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (!shouldBuild)
            {
                return Option.None<GameState>();
            }

            // can we build this robot or is there not enough time left to gather the required resources?
            int potentialOre = state.Ore + (state.OreRobots * state.Remaining);
            int potentialClay = state.Clay + (state.ClayRobots * state.Remaining);
            int potentialObsidian = state.Obsidian + (state.ObsidianRobots * state.Remaining);

            bool canBuild = type switch
            {
                RobotType.Ore => potentialOre >= blueprint.OreRobotCost,
                RobotType.Clay => potentialOre >= blueprint.ClayRobotCost,
                RobotType.Obsidian => potentialOre >= blueprint.ObsidianRobotOreCost && potentialClay >= blueprint.ObsidianRobotClayCost,
                RobotType.Geode => potentialOre >= blueprint.GeodeRobotOreCost && potentialObsidian >= blueprint.GeodeRobotObsidianCost,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (!canBuild)
            {
                return Option.None<GameState>();
            }

            // we should and could build this robot, so work out the earliest point at which we could build it
            (int Ore, int Clay, int Obsidian) costs = Costs(blueprint, type);

            int oreMinutesRequired = (state.Ore >= costs.Ore) ? 0 : (costs.Ore - state.Ore + state.OreRobots - 1) / state.OreRobots;
            int clayMinutesRequired = (state.Clay >= costs.Clay) ? 0 : (costs.Clay - state.Clay + state.ClayRobots - 1) / state.ClayRobots;
            int obsidianMinutesRequired = (state.Obsidian >= costs.Obsidian) ? 0 : (costs.Obsidian - state.Obsidian + state.ObsidianRobots - 1) / state.ObsidianRobots;

            // +1 because the robot takes 1 minute to build after starting
            int minutesRequired = Math.Max(Math.Max(oreMinutesRequired, clayMinutesRequired), obsidianMinutesRequired) + 1;

            // fast forward by the appropriate amount of time to start building
            int nextOre = state.Ore + (minutesRequired * state.OreRobots) - costs.Ore;
            int nextClay = state.Clay + (minutesRequired * state.ClayRobots) - costs.Clay;
            int nextObsidian = state.Obsidian + (minutesRequired * state.ObsidianRobots) - costs.Obsidian;
            int nextGeodes = state.Geodes + (minutesRequired * state.GeodeRobots);

            int nextOreRobots = type == RobotType.Ore ? state.OreRobots + 1 : state.OreRobots;
            int nextClayRobots = type == RobotType.Clay ? state.ClayRobots + 1 : state.ClayRobots;
            int nextObsidianRobots = type == RobotType.Obsidian ? state.ObsidianRobots + 1 : state.ObsidianRobots;
            int nextGeodeRobots = type == RobotType.Geode ? state.GeodeRobots + 1 : state.GeodeRobots;

            GameState next = new(state.Remaining - minutesRequired,
                                 nextOreRobots,
                                 nextOre,
                                 nextClayRobots,
                                 nextClay,
                                 nextObsidianRobots,
                                 nextObsidian,
                                 nextGeodeRobots,
                                 nextGeodes);

            return next.Some();
        }

        /// <summary>
        /// Get the costs of the given robot type
        /// </summary>
        /// <param name="blueprint">Blueprint of costs</param>
        /// <param name="type">Robot type to build</param>
        /// <returns>Costs to build the robot</returns>
        private static (int Ore, int Clay, int Obsidian) Costs(Blueprint blueprint, RobotType type) => type switch
        {
            RobotType.Ore => (blueprint.OreRobotCost, 0, 0),
            RobotType.Clay => (blueprint.ClayRobotCost, 0, 0),
            RobotType.Obsidian => (blueprint.ObsidianRobotOreCost, blueprint.ObsidianRobotClayCost, 0),
            RobotType.Geode => (blueprint.GeodeRobotOreCost, 0, blueprint.GeodeRobotObsidianCost),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        // Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 14 clay. Each geode robot costs 3 ore and 16 obsidian.
        private readonly record struct Blueprint(int Id,
                                                 int OreRobotCost,
                                                 int ClayRobotCost,
                                                 int ObsidianRobotOreCost,
                                                 int ObsidianRobotClayCost,
                                                 int GeodeRobotOreCost,
                                                 int GeodeRobotObsidianCost,
                                                 int MaxOreCost);

        private readonly record struct GameState(int Remaining,
                                                 int OreRobots,
                                                 int Ore,
                                                 int ClayRobots,
                                                 int Clay,
                                                 int ObsidianRobots,
                                                 int Obsidian,
                                                 int GeodeRobots,
                                                 int Geodes);

        private enum RobotType
        {
            Ore,
            Clay,
            Obsidian,
            Geode
        }
    }
}
