using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] original, double[,] pixel)
        {
            var zeroLength = original.GetLength(0);
            var oneLength = original.GetLength(1);
            var coreLimit = pixel.GetLength(0) / 2;
            var output = new double[zeroLength, oneLength];
            for (var a = coreLimit; a < zeroLength - coreLimit; a++)
                for (var b = coreLimit; b < oneLength - coreLimit; b++)
                    output[a, b] = ComputeSobel(original, pixel, a - coreLimit, b - coreLimit);
            return output;
        }

        public static double ComputeSobel(double[,] pixel, double[,] original, int firstRow, int firstColumn)
        {
            var coreSize = original.GetLength(0);
            double gx = 0;
            double gy = 0;
            for (int x = 0; x < coreSize; x++)
                for (int y = 0; y < coreSize; y++)
                {
                    gx += pixel[firstRow + x, firstColumn + y] * original[x, y];
                    gy += pixel[firstRow + x, firstColumn + y] * original[y, x];
                }
            return Math.Sqrt(gx * gx + gy * gy);
        }
    }
}