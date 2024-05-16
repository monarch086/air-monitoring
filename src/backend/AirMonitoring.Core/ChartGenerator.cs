﻿using ImageChartsLib;
using AirMonitoring.Core.Extensions;
using System.Text;

namespace AirMonitoring.Core
{
    public static class ChartGenerator
    {
        private const string BLUE_LINE = "0077CC";
        private const string BLUE_FILL = "B,C1ECF4,0,0,0";
        private const string LINE_WIDTH = "2";
        private const string LINE_CHART_TYPE = "lc";

        public static byte[] Generate(string[] data, TimeSpan range)
        {
            return generateLineChart(data, range)
                .toBuffer();
        }

        private static ImageCharts generateLineChart(string[] data, TimeSpan range)
        {
            var highestTempRange = (int) Math.Round(decimal.Parse(data.Max())) + 2;
            var lowestTempRange = (int) Math.Round(decimal.Parse(data.Min())) - 2;
            lowestTempRange = lowestTempRange < 0 ? 0 : lowestTempRange;

            return new ImageCharts()
                .cht(LINE_CHART_TYPE)
                .chs("500x300")
                .chd($"a:{string.Join(",", data)}") //data
                .chdl("t,°C")
                .chl($"{string.Join("|", shrinkLabels(data))}") //labels on data
                .chlps("align,top")
                .chls(LINE_WIDTH)
                .chma("10,30,30,10")
                .chm(BLUE_FILL)
                .chco(BLUE_LINE)
                .chxt("x,y") //which axis should have labels
                .chxr($"1,{lowestTempRange},{highestTempRange}") //ranges
                .chxl($"{getXLabels(range)}{getYLabels(lowestTempRange, highestTempRange)}"); //axis labels
        }

        private static string[] shrinkLabels(string[] data)
        {
            var maxLabelCount = 5;
            var skip = data.Length / maxLabelCount;
            var zero = "0";

            for (int i = 0; i < data.Length; i++)
            {
                if (i == data.Length - 1) continue;

                if (i == 0 || i % skip != 0 || data[i] == zero)
                {
                    data[i] = string.Empty;
                }
            }

            return data;
        }

        private static string getYLabels(int lowest, int highest)
        {
            var rangeCount = highest - lowest + 1;
            var values = Enumerable.Range(lowest, rangeCount);
            var itemsToShow = 5;
            if (values.Count() > itemsToShow)
            {
                var skipFactor = values.Count() / itemsToShow;
                values = values.Where(v => (v % skipFactor == 0) || v == lowest || v == highest);
            }

            return $"1:|{string.Join('|', values)}|";
        }

        private static string getXLabels(TimeSpan range)
        {
            var sb = new StringBuilder("0:|");
            var now = DateTime.UtcNow.ToKyivTime();

            var counter = range.Days > 0 ? range.Days : range.Hours > 0 ? range.Hours : range.Minutes > 0 ? range.Minutes : 0;

            for (int i = 0; i <= counter; i++)
            {
                var hour = now.AddHours(i - counter).Hour;
                sb.Append($"{hour}|");
            }

            return sb.ToString();
        }
    }
}