using System.Collections.Generic;

namespace Profiling
{
    public interface IProfiler
    {
        List<ExperimentResult> Measure(IRunner runner, int repetitionsCount);
    }
}