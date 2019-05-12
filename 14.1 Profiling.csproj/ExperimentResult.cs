namespace Profiling
{
    public class ExperimentResult
    {
        public readonly double ClassResult;
        public readonly int Size;
        public readonly double StructResult;

        public ExperimentResult(int size, double classResult, double structResult)
        {
            Size = size;
            ClassResult = classResult;
            StructResult = structResult;
        }
    }
}