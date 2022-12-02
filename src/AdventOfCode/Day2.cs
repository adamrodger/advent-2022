using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 2
    /// </summary>
    public class Day2
    {
        private enum Move
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        };

        private enum Result
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        };

        private static readonly IDictionary<string, Move> MoveCodes = new Dictionary<string, Move>
        {
            ["A"] = Move.Rock,
            ["B"] = Move.Paper,
            ["C"] = Move.Scissors,
            ["X"] = Move.Rock,
            ["Y"] = Move.Paper,
            ["Z"] = Move.Scissors
        };

        private static readonly IDictionary<string, Result> ResultCodes = new Dictionary<string, Result>
        {
            ["X"] = Result.Lose,
            ["Y"] = Result.Draw,
            ["Z"] = Result.Win
        };

        public int Part1(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                var parts = line.Split(' ').Take(2).ToArray();
                Move them = MoveCodes[parts[0]];
                Move us = MoveCodes[parts[1]];

                Result result = (them, us) switch
                {
                    (Move.Rock, Move.Rock) => Result.Draw,
                    (Move.Rock, Move.Paper) => Result.Win,
                    (Move.Rock, Move.Scissors) => Result.Lose,
                    (Move.Paper, Move.Rock) => Result.Lose,
                    (Move.Paper, Move.Paper) => Result.Draw,
                    (Move.Paper, Move.Scissors) => Result.Win,
                    (Move.Scissors, Move.Rock) => Result.Win,
                    (Move.Scissors, Move.Paper) => Result.Lose,
                    (Move.Scissors, Move.Scissors) => Result.Draw,
                    _ => throw new ArgumentOutOfRangeException()
                };

                total += (int)us + (int)result;
            }

            return total;
        }

        public int Part2(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                var parts = line.Split(' ').Take(2).ToArray();
                Move them = MoveCodes[parts[0]];
                Result result = ResultCodes[parts[1]];

                Move us = (them, result) switch
                {
                    (Move.Rock, Result.Lose) => Move.Scissors,
                    (Move.Rock, Result.Draw) => Move.Rock,
                    (Move.Rock, Result.Win) => Move.Paper,
                    (Move.Paper, Result.Lose) => Move.Rock,
                    (Move.Paper, Result.Draw) => Move.Paper,
                    (Move.Paper, Result.Win) => Move.Scissors,
                    (Move.Scissors, Result.Lose) => Move.Paper,
                    (Move.Scissors, Result.Draw) => Move.Scissors,
                    (Move.Scissors, Result.Win) => Move.Rock,
                    _ => throw new ArgumentOutOfRangeException()
                };

                total += (int)us + (int)result;
            }

            return total;
        }
    }
}
