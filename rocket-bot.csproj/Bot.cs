using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace rocket_bot
{
    public class Bot
    {
        private readonly Level level;
        private readonly Channel<Rocket> channel;
        private readonly int movesCount;
        private readonly int iterationsCount;
        private readonly Random random;

        public Bot(Level level, Channel<Rocket> channel, int movesCount, int iterationsCount, Random random)
        {
            this.level = level;
            this.channel = channel;
            this.movesCount = movesCount;
            this.iterationsCount = iterationsCount;
            this.random = random;
        }

        public void Execute()
        {
            while (true)
            {
                if (level.IsCompleted)
                {
                    // Чтобы не тратить 100% одного ядра на ничего не делающий цикл.
                    Thread.Sleep(100);
                    continue;
                }
                var lastRocket = channel.LastItem();
                if (lastRocket == null) return;
                level.Rocket = lastRocket;
                var bestMove = GetScoredBestMove(level);
                level.Move(bestMove.Item1);
                if (!level.IsCompleted)
                    channel.AppendIfLastItemIsUnchanged(level.Rocket, lastRocket);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private Tuple<Turn, double> GetScoredBestMove(Level currentLevel)
        {
            var initialRocketTime = currentLevel.Rocket.Time;
            List<Turn> bestStrategy = null;
            double bestScore = 0;
            var i = 0;
            do
            {
                var moves = GetRandomTwoPhaseStrategy(movesCount, random).ToList();
                //var moves = GetRandomStrategy(movesCount, random).ToList();
                var score = EstimateMoves(moves, currentLevel);
                if (bestStrategy == null || score > bestScore)
                {
                    //Trace.WriteLine($"Iteration #{i}. Found better strategy. Score: {score} (+{score - bestScore})");
                    bestScore = score;
                    bestStrategy = moves;
                }

                i++;
            } while (!ChannelStateIsChanged(initialRocketTime) && i < iterationsCount);
            //Trace.WriteLine($"{i} iterations used");

            return Tuple.Create(bestStrategy.First(), bestScore);
        }

        private bool ChannelStateIsChanged(int initialRocketTime)
        {
            return channel.LastItem().Time != initialRocketTime;
        }

        private double EstimateMoves(IEnumerable<Turn> moves, Level currentLevel)
        {
            var rocket = currentLevel.Rocket;
            try
            {
                foreach (var turn in moves)
                    currentLevel.Move(turn);
                return currentLevel.Rocket.TakenCheckpointsCount + 1 / (GetDistanceToNextCheckpoint(currentLevel) + 1);
            }
            finally
            {
                currentLevel.Rocket = rocket;
            }
        }

        private double GetDistanceToNextCheckpoint(Level currentLevel)
        {
            return (currentLevel.Rocket.Location - currentLevel.GetNextRocketCheckpoint()).Length;
        }

        private IEnumerable<Turn> GetRandomTwoPhaseStrategy(int steps, Random rnd)
        {
            var move1Duration = rnd.Next(steps);
            var move2Duration = steps - move1Duration;
            var move1 = (Turn)(rnd.Next(3) - 1);
            var move2 = (Turn)(rnd.Next(3) - 1);
            return Enumerable.Repeat(move1, move1Duration).Concat(
                Enumerable.Repeat(move2, move2Duration));
        }

        private IEnumerable<Turn> GetRandomStrategy(int steps, Random rnd)
        {
            return Enumerable.Range(0, steps).Select(i => (Turn) (rnd.Next(3) - 1));
        }
    }
}
