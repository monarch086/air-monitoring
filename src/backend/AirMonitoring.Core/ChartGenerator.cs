using ImageChartsLib;
using AirMonitoring.Core.Extensions;
using System.Text;
using Newtonsoft.Json.Linq;

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
            var needRotation = range.Days > 360;


            return new ImageCharts()
                .cht(LINE_CHART_TYPE)
                .chs("500x300")
                .chd($"a:{string.Join(",", data)}")
                .chdl(unitLabel)
                .chg("40,100,5,5,CECECE")
                .chlps("align,top")
                .chls(LINE_WIDTH)
                .chma("10,30,30,10")
                .chm(BLUE_FILL)
                .chco(BLUE_LINE)
                .chxt("x,y") //which axis should have labels
                .chxs(needRotation ? "0,s,min40" : "0,s")
                .chxr($"1,{yMinLimit},{yMaxLimit}") //ranges
                .chxl($"{getXLabels(range)}{getYLabels(yMinLimit, yMaxLimit)}"); //axis labels
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
            var isDay = range.TotalHours < 25;
            var isYear = range.TotalDays > 360;

            return isDay ? getDayXLabels(range)
                : isYear ? getYearXLabels(range)
                    : getMonthXLabels(range);
        }

        private static string getDayXLabels(TimeSpan range)
        {
            var sb = new StringBuilder("0:|");
            var now = DateTime.UtcNow.ToKyivTime();
            var counter = range.TotalHours;

            var itemsToShow = 12;
            var skipFactor = counter / itemsToShow;

            for (int i = 0; i <= counter; i++)
            {
                if (i % skipFactor != 0) continue;

                var item = now.AddHours(i - counter).Hour.ToString();
                sb.Append($"{item}|");
            }

            Console.WriteLine(sb.ToString());

            return sb.ToString();
        }

        private static string getMonthXLabels(TimeSpan range)
        {
            var sb = new StringBuilder("0:|");
            var now = DateTime.UtcNow.ToKyivTime();
            var counter = range.Days;

            for (int i = 0; i <= counter; i++)
            {
                var item = now.AddDays(i - counter).Day.ToString();
                sb.Append($"{item}|");
            }

            return sb.ToString();
        }

        private static string getYearXLabels(TimeSpan range)
        {
            var sb = new StringBuilder("0:|");
            var now = DateTime.UtcNow.ToKyivTime();
            var counter = range.Days;

            for (int i = 0; i <= counter; i++)
            {
                var item = now.AddDays(i - counter).ToString("MMM");
                sb.Append($"{item}|");
            }

            return sb.ToString();
        }
    }
}
