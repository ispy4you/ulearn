using System;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static double Maximum = double.MaxValue;

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var bestPath = new int[checkpoints.Length];
            Maximum = double.MaxValue;
            ChoosingBestPath(bestPath, new int[checkpoints.Length], 1, checkpoints);
            return bestPath;
        }

        static void ChoosingBestPath(int[] bestPath, int[] order, int position, Point[] checkpoints)
        {
            int iteration = 0;
            double orderLength = 0;
            if (iteration > 0)
                orderLength += checkpoints[order[iteration - 1]].DistanceTo(checkpoints[order[iteration]]);
            if (orderLength > Maximum)
                return;

            if (position == order.Length)
            {
                double currentLength = 0;
                for (var i = 0; i < position - 1; i++)
                    currentLength += PointExtensions.DistanceTo(checkpoints[order[i]], checkpoints[order[i + 1]]);
                if (currentLength < Maximum)
                {
                    Maximum = currentLength;
                    for (var i = 0; i < order.Length; i++)
                        bestPath[i] = order[i];
                }
                return;
            }

            double length = 0;
            for (var i = 0; i < position - 1; i++)
                length += PointExtensions.DistanceTo(checkpoints[order[i]], checkpoints[order[i + 1]]);
            if (length > Maximum)
                return;

            for (var i = 1; i < order.Length; i++)
            {
                var index = Array.IndexOf(order, i, 1, position - 1);
                if (index != -1)
                    continue;
                order[position] = i;
                ChoosingBestPath(bestPath, order, position + 1, checkpoints);
            }
        }
    }
}