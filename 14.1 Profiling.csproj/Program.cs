using System;
using System.Drawing;
using System.Windows.Forms;

namespace Profiling
{
    internal class Program
    {
        public static void Run(IRunner caller, IProfiler profiler, int repetitionsCount, IChartBuilder builder)
        {
            var result = profiler.Measure(caller, repetitionsCount);
            var chart = builder.Build(result);
            var form = new Form {ClientSize = new Size(800, 600)};
            chart.Dock = DockStyle.Fill;
            form.Controls.Add(chart);
            Application.Run(form);
        }

        [STAThread]
        public static void Main()
        {
            //Run(new checking.ArrayRunner(), new ProfilerTask(), 100000, new ChartBuilder());
            //Run(new checking.CallRunner(), new ProfilerTask(), 7000000, new ChartBuilder());
        }
    }
}