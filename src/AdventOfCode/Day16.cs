using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public int Part1(string[] input)
        {
            (IDictionary<string, Valve> valves, IList<Valve> importantValves) = ParseInput(input);
            int[,] distance = CalculateDistanceMatrix(valves);

            // permute important values, as long as the time cost stays below 30, and calculate their score
            int max = int.MinValue;

            foreach (IEnumerable<Valve> permutation in Permutations(distance, valves["AA"], importantValves.ToHashSet(), new HashSet<Valve>(), 30))
            {
                int time = 30;
                int released = 0;
                Valve current = valves["AA"];

                foreach (Valve next in permutation)
                {
                    int cost = distance[current.Index, next.Index] + 1;
                    time -= cost;
                    released += time * next.Rate;
                    current = next;
                }

                max = Math.Max(released, max);
            }

            return max;
        }

        public int Part2(string[] input)
        {
            throw new NotImplementedException();
        }

        private static (IDictionary<string, Valve>, IList<Valve> importantValves) ParseInput(string[] input)
        {
            var valves = new Dictionary<string, Valve>(input.Length);
            var importantValves = new List<Valve>();
            int index = 0;

            foreach (string line in input)
            {
                // Valve GJ has flow rate=14; tunnels lead to valves UV, AO, MM, UD, GM
                string[] parts = line.Split(' ');
                string id = parts[1];
                int rate = int.Parse(parts[4][5..^1]);
                string[] paths = parts[9..].Select(p => p.Replace(",", string.Empty)).ToArray();

                var valve = new Valve(id, rate, paths, index++);
                valves.Add(valve.Id, valve);

                if (rate > 0)
                {
                    importantValves.Add(valve);
                }
            }

            return (valves, importantValves);
        }

        /// <summary>
        /// Calculate the distance matrix between every node using the Floyd-Warshall algorithm
        /// </summary>
        /// <param name="valves">Valves</param>
        /// <returns>Distance matrix</returns>
        private static int[,] CalculateDistanceMatrix(IDictionary<string, Valve> valves)
        {
            // Floyd-Warshall distance calculations
            var distance = new int[valves.Count, valves.Count];

            for (int i = 0; i < valves.Count; i++)
            {
                for (int j = 0; j < valves.Count; j++)
                {
                    distance[i, j] = 100;
                }
            }

            foreach (Valve valve in valves.Values)
            {
                foreach (string dest in valve.Paths)
                {
                    distance[valve.Index, valves[dest].Index] = 1;
                }
            }

            for (int k = 0; k < valves.Count; ++k)
            {
                for (int i = 0; i < valves.Count; ++i)
                {
                    for (int j = 0; j < valves.Count; ++j)
                    {
                        if (distance[i, k] + distance[k, j] < distance[i, j])
                        {
                            distance[i, j] = distance[i, k] + distance[k, j];
                        }
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Get all the possible permutations of paths from the current node to the remaining unvisited nodes
        /// </summary>
        /// <param name="distances">Distance matrix between every node</param>
        /// <param name="current">Current node</param>
        /// <param name="unvisited">Unvisited nodes</param>
        /// <param name="visited">Visited nodes</param>
        /// <param name="time">Remaining time</param>
        /// <returns>All valid permutations</returns>
        private static IEnumerable<IEnumerable<Valve>> Permutations(int[,] distances,
                                                                    Valve current,
                                                                    ISet<Valve> unvisited,
                                                                    ISet<Valve> visited,
                                                                    int time)
        {
            foreach (Valve next in unvisited)
            {
                int cost = distances[current.Index, next.Index] + 1;

                if (cost >= time)
                {
                    continue;
                }

                // TODO: Make these bitmasks instead to prevent iterating this over and over
                ISet<Valve> nextTodo = unvisited.ToHashSet();
                nextTodo.Remove(next);

                ISet<Valve> nextDone = visited.ToHashSet();
                nextDone.Add(next);

                foreach (IEnumerable<Valve> nextPermutation in Permutations(distances, next, nextTodo, nextDone, time - cost))
                {
                    yield return nextPermutation;
                }
            }

            yield return visited;
        }

        private record Valve(string Id, int Rate, ICollection<string> Paths, int Index);
    }
}
