using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;

namespace GlobalWarmingCellularAutomata.Automata.Services
{
    internal static class ReportsCreator
    {
        public static void CreateReport(List<double> data, string title, string path)
        {
            var cartesianChart = new SKCartesianChart
            {
                Width = 900,
                Height = 600,
                Series = new ISeries[]
                {
                    new LineSeries<double> { Values = data },
                },
                Title = new LabelVisual
                {
                    Text = title,
                    TextSize = 30,
                    Padding = new Padding(15),
                    Paint = new SolidColorPaint(0xff303030)
                },
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                Background = SKColors.White
            };

            cartesianChart.SaveImage(path);
        }

        public static void CreateReport(List<double> data1,List<double> data2,List<int> data3 ,List<int> data4,List<int> data5, string title, string path)
        {
            var cartesianChart = new SKCartesianChart
            {
                Width = 900,
                Height = 600,
                Series = new ISeries[]
                {
                    new LineSeries<double> { Values = data1, Tag = "test"},
                    new LineSeries<double> { Values = data2 },
                    new LineSeries<int> { Values = data3 },
                    new LineSeries<int> { Values = data4 },
                    new LineSeries<int> { Values = data5 },
                },
                Title = new LabelVisual
                {
                    Text = title,
                    TextSize = 30,
                    Padding = new Padding(15),
                    Paint = new SolidColorPaint(0xff303030)
                },
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                Background = SKColors.White
            };

            cartesianChart.SaveImage(path);
        }

    }
}
