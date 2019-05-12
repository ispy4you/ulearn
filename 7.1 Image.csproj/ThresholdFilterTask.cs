namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			return new double[original.GetLength(0), original.GetLength(1)];
		}
	}
}