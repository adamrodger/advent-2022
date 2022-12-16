using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

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
            Valve start = valves["AA"];
            int[,] distances = CalculateDistanceMatrix(valves);

            // permute important values, as long as the time cost stays below 30, and calculate their score
            var permutations = Permutations(distances, start, importantValves.ToHashSet(), Array.Empty<Valve>(), 30);
            return permutations.Select(p => CalculateScore(p, distances, start, 30)).Max();
        }

        public int Part2(string[] input)
        {
            (IDictionary<string, Valve> valves, IList<Valve> importantValves) = ParseInput(input);
            Valve start = valves["AA"];
            int[,] distances = CalculateDistanceMatrix(valves);

            // permute important values, as long as the time cost stays below 30, and calculate their score
            var myPermutations = Permutations(distances, start, importantValves.ToHashSet(), Array.Empty<Valve>(), 26).ToArray();
            var myScores = myPermutations.Select(p => (CalculateScore(p, distances, start, 26), p)).OrderByDescending(p => p.Item1).ToArray();

            foreach ((int i, (int score, IList<Valve> opened)) in myScores.Enumerate())
            {
                foreach ((int score2, IList<Valve> opened2) in myScores[(i+1)..])
                {
                    if (opened.Intersect(opened2).Any())
                    {
                        // our opened valves and elephant opened valves must be disjoint
                        continue;
                    }

                    int combinedScore = score + score2;
                    return combinedScore;
                }
            }

            throw new InvalidOperationException("Couldn't find a combined score");
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
        private static IEnumerable<IList<Valve>> Permutations(int[,] distances,
                                                              Valve current,
                                                              ISet<Valve> unvisited,
                                                              IList<Valve> visited,
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

                IList<Valve> nextDone = visited.Append(next).ToList();

                foreach (IList<Valve> nextPermutation in Permutations(distances, next, nextTodo, nextDone, time - cost))
                {
                    yield return nextPermutation;
                }
            }

            yield return visited;
        }

        /// <summary>
        /// Calculate the score for the given path from the start node
        /// </summary>
        /// <param name="path">Path to take</param>
        /// <param name="distances">Distance matrix</param>
        /// <param name="start">Start node</param>
        /// <param name="time">Total amount of time available</param>
        /// <returns>Score for the path</returns>
        private static int CalculateScore(IList<Valve> path, int[,] distances, Valve start, int time)
        {
            int released = 0;
            Valve current = start;

            foreach (Valve next in path)
            {
                int cost = distances[current.Index, next.Index] + 1;
                time -= cost;
                released += time * next.Rate;
                current = next;
            }

            return released;
        }

        private record Valve(string Id, int Rate, ICollection<string> Paths, int Index);
    }
}
