using System;
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

        public int Part1(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                Move them = line[0] switch
                {
                    'A' => Move.Rock,
                    'B' => Move.Paper,
                    'C' => Move.Scissors,
                    _ => throw new ArgumentOutOfRangeException()
                };

                Move us = line[2] switch
                {
                    'X' => Move.Rock,
                    'Y' => Move.Paper,
                    'Z' => Move.Scissors,
                    _ => throw new ArgumentOutOfRangeException()
                };

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
                Move them = line[0] switch
                {
                    'A' => Move.Rock,
                    'B' => Move.Paper,
                    'C' => Move.Scissors,
                    _ => throw new ArgumentOutOfRangeException()
                };

                Result result = line[2] switch
                {
                    'X' => Result.Lose,
                    'Y' => Result.Draw,
                    'Z' => Result.Win,
                    _ => throw new ArgumentOutOfRangeException()
                };

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
