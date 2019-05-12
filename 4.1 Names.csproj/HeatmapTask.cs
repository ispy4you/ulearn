using System;
namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var lenghtDays = 30;
            var minValueDays = 2;
            var lenghtMonths = 12;
            var minMonth = 1;
            var days = new string[lenghtDays];
            for (int i = 0; i < lenghtDays; i++)
                days[i] = (i + minValueDays).ToString();
            var months = new string[lenghtMonths];
            for (int i = 0; i < lenghtMonths; i++)
                months[i] = (i + minMonth).ToString();
            var heatMap = new double[30, 12];
            foreach (var calendar in names)
                if (calendar.BirthDate.Day != 1)
                    heatMap[calendar.BirthDate.Day - 2, calendar.BirthDate.Month - 1]++;
            return new HeatmapData("Пример карты интенсивностей", heatMap, days, months);
        }
    }
}