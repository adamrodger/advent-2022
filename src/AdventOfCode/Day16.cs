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

            PriorityQueue<GameState, int> queue = new();
            queue.Enqueue(new GameState("AA", 30, 0, string.Empty), 0);

            HashSet<GameState> visited = new();

            int maxPressure = int.MinValue;

            while (queue.TryDequeue(out GameState state, out int priority))
            {
                if (state.TimeRemaining < 1)
                {
                    //return state.PressureReleased;
                    maxPressure = Math.Max(state.PressureReleased, maxPressure);
                    continue;
                }

                if (visited.Contains(state))
                {
                    continue;
                }

                visited.Add(state);

                Valve valve = valves[state.Current];

                if (!state.OpenValves.Contains(valve.Id) && valve.Rate > 0)
                {
                    // open the valve
                    GameState nextState = state with
                    {
                        TimeRemaining = state.TimeRemaining - 1,
                        OpenValves = string.Join(",", state.OpenValves.Split(',').Append(valve.Id).OrderBy(v => v)),
                        PressureReleased = state.PressureReleased + ((state.TimeRemaining - 1) * valve.Rate)
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }

                // move to all the other valves
                foreach (string other in valve.Paths)
                {
                    GameState nextState = state with
                    {
                        Current = other,
                        TimeRemaining = state.TimeRemaining - 1
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }
            }

            return maxPressure;
            throw new InvalidOperationException("Never got to the end somehow");
        }

        public int Part2(string[] input)
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

            PriorityQueue<GameState2, int> queue = new();
            queue.Enqueue(new GameState2("AA", 26, 0, string.Empty, false), 0);

            HashSet<GameState2> visited = new();

            int maxPressure = int.MinValue;

            //ICollection<GameState2> myEndStates = new List<GameState2>();
            GameState2 bestState = new GameState2(null, -1, -1, null, false);

            while (queue.TryDequeue(out GameState2 state, out int priority))
            {
                if (state.TimeRemaining < 1)
                {
                    //myEndStates.Add(state);

                    if (bestState.PressureReleased < state.PressureReleased)
                    {
                        bestState = state;
                    }

                    continue;
                }

                if (visited.Contains(state))
                {
                    continue;
                }

                visited.Add(state);

                Valve valve = valves[state.Current];

                if (!state.OpenValves.Contains(valve.Id) && valve.Rate > 0)
                {
                    // open the valve
                    GameState2 nextState = state with
                    {
                        TimeRemaining = state.TimeRemaining - 1,
                        OpenValves = string.Join(",", state.OpenValves.Split(',').Append(valve.Id).OrderBy(v => v)),
                        PressureReleased = state.PressureReleased + ((state.TimeRemaining - 1) * valve.Rate)
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }

                // move to all the other valves
                foreach (string other in valve.Paths)
                {
                    GameState2 nextState = state with
                    {
                        Current = other,
                        TimeRemaining = state.TimeRemaining - 1
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }
            }


            queue.Enqueue(new GameState2("AA", 26, bestState.PressureReleased, bestState.OpenValves, true), bestState.PressureReleased * -1);

            while (queue.TryDequeue(out GameState2 state, out int priority))
            {
                if (state.TimeRemaining < 1)
                {
                    maxPressure = Math.Max(maxPressure, state.PressureReleased);
                    continue;
                }

                if (visited.Contains(state))
                {
                    continue;
                }

                visited.Add(state);

                Valve valve = valves[state.Current];

                if (!state.OpenValves.Contains(valve.Id) && valve.Rate > 0)
                {
                    // open the valve
                    GameState2 nextState = state with
                    {
                        TimeRemaining = state.TimeRemaining - 1,
                        OpenValves = string.Join(",", state.OpenValves.Split(',').Append(valve.Id).OrderBy(v => v)),
                        PressureReleased = state.PressureReleased + ((state.TimeRemaining - 1) * valve.Rate)
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }

                // move to all the other valves
                foreach (string other in valve.Paths)
                {
                    GameState2 nextState = state with
                    {
                        Current = other,
                        TimeRemaining = state.TimeRemaining - 1
                    };

                    queue.Enqueue(nextState, nextState.PressureReleased * -1);
                }
            }

            return maxPressure;
        }

        private record Valve(string Id, int Rate, ICollection<string> Paths);

        private record GameState(string Current, int TimeRemaining, int PressureReleased, string OpenValves);
        private record GameState2(string Current, int TimeRemaining, int PressureReleased, string OpenValves, bool ElephantTurn);
    }
}
