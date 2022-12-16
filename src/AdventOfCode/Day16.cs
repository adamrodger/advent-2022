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
            IDictionary<string, Valve> valves = new Dictionary<string, Valve>(input.Length);

            foreach (string line in input)
            {
                // Valve GJ has flow rate=14; tunnels lead to valves UV, AO, MM, UD, GM
                string[] parts = line.Split(' ');
                string id = parts[1];
                int rate = int.Parse(parts[4][5..^1]);
                string[] paths = parts[9..].Select(p => p.Replace(",", string.Empty)).ToArray();

                valves[id] = new Valve(id, rate, paths);
            }

            Queue<GameState> queue = new Queue<GameState>();
            queue.Enqueue(new GameState("AA", 30, 0, new HashSet<string>()));

            int maxPressure = int.MinValue;

            while (queue.TryDequeue(out GameState state))
            {
                if (state.TimeRemaining < 1)
                {
                    maxPressure = Math.Max(state.PressureReleased, maxPressure);
                    continue;
                }

                Valve valve = valves[state.Current];

                if (!state.OpenValves.Contains(valve.Id))
                {
                    queue.Enqueue(state with
                    {
                        TimeRemaining = state.TimeRemaining - 1,
                        OpenValves = new HashSet<string>(state.OpenValves) { valve.Id } // inefficient copying of set every time we queue
                    });
                }

                // release pressure - inefficient because we're doing this ever step, could multiply up front
                int pressure = state.PressureReleased + state.OpenValves.Select(v => valves[v].Rate).Sum();

                foreach (string other in valve.Paths)
                {
                    queue.Enqueue(state with
                    {
                        Current = other,
                        TimeRemaining = state.TimeRemaining - 1,
                        PressureReleased = pressure
                    });
                }
            }

            return maxPressure;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private record Valve(string Id, int Rate, ICollection<string> Paths);

        private record GameState(string Current, int TimeRemaining, int PressureReleased, ISet<string> OpenValves);
    }
}
