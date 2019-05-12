using System.Collections.Generic;
using System.Windows.Forms;

namespace Profiling
{
    public interface IChartBuilder
    {
        Control Build(List<ExperimentResult> result);
    }
}