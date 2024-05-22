using ImageChartsLib;
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

        public static byte[] Generate(string[] data, TimeSpan range, string unitLabel)
        {
            return generateLineChart(data, range, unitLabel)
                .toBuffer();
        }

        private static ImageCharts generateLineChart(string[] data, TimeSpan range, string unitLabel)
        {
            var yMaxLimit = (int) Math.Round(decimal.Parse(data.Max())) + 2;
            var yMinLimit = (int) Math.Round(decimal.Parse(data.Min())) - 2;
            yMinLimit = yMinLimit < 0 ? 0 : yMinLimit;

            return new ImageCharts()
                .cht(LINE_CHART_TYPE)
                .chs("500x300")
                .chd($"a:{string.Join(",", data)}") //data
                .chdl(unitLabel)
                .chl($"{string.Join("|", shrinkLabels(data))}") //labels on data
                .chlps("align,top")
                .chls(LINE_WIDTH)
                .chma("10,30,30,10")
                .chm(BLUE_FILL)
                .chco(BLUE_LINE)
                .chxt("x,y") //which axis should have labels
                .chxr($"1,{yMinLimit},{yMaxLimit}") //ranges
                .chxl($"{getXLabels(range)}{getYLabels(yMinLimit, yMaxLimit)}"); //axis labels
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
            var counter = range.Days;

            var isYear = counter > 360;

            for (int i = 0; i <= counter; i++)
            {
                if (isYear && (i % 15 != 0)) continue;

                var item = isYear
                    ? now.AddDays(i - counter).ToShortDateString()
                    : now.AddDays(i - counter).Day.ToString();
                sb.Append($"{item}|");
            }

            return sb.ToString();
        }
    }
}
