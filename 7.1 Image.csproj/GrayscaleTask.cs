namespace Recognizer
{
    public static class GrayscaleTask
    {
        public static double[,] ToGrayscale(Pixel[,] original)
        {
            var zeroLength = original.GetLength(0);
            var oneLength = original.GetLength(1);
            var grayscale = new double[zeroLength, oneLength];
            for (var x = 0; x < zeroLength; x++)
                for (var y = 0; y < oneLength; y++)
                {
                    var currPixel = original[x, y];
                    grayscale[x, y] = ((0.299 * currPixel.R) +
                                        0.587 * currPixel.G +
                                        0.114 * currPixel.B) / 255;
                }
            return grayscale;
        }
    }
}