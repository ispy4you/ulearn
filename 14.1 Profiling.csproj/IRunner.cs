namespace Profiling
{
    public interface IRunner
    {
        void Call(bool isClass, int size, int count);
    }
}