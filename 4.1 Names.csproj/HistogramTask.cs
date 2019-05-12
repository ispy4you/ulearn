using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var years = new string[31];
            for (var y = 0; y < 31; y++)
                years[y] = (y + 1).ToString();
            var birthsCounts = new double[31];
            foreach (var simpleName in names)
            {
                if (simpleName.Name == name)
                    if (simpleName.BirthDate.Day != 1)
                        birthsCounts[simpleName.BirthDate.Day - 1]++;
            }
            return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name), years, birthsCounts);
        }
    }
}