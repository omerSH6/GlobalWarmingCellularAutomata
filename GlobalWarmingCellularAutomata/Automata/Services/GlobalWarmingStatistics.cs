using GlobalWarmingCellularAutomata.Automata.Entities;

namespace GlobalWarmingCellularAutomata.Automata.Services
{
    internal class GlobalWarmingStatistics
    {
        // all the lists contains statistics that will be saves every day.
        private List<double> DailyTemperatureAverage = new List<double>();
        private List<Tuple<int, int>> DailyTemperatureMinMax = new List<Tuple<int, int>>();
        private List<double> DailyTemperatureStandardDeviation = new List<double>();

        private List<double> DailyPollutionAverage = new List<double>();
        private List<Tuple<int, int>> DailyPollutionMinMax = new List<Tuple<int, int>>();
        private List<double> DailyPollutionStandardDeviation = new List<double>();

        private List<int> GlaciersCellsAmountPerDay = new List<int>();
        private List<int> SeaCellsAmountPerDay = new List<int>();
        private List<int> LandCellsAmountPerDay = new List<int>();

        public void SaveStatistics(Cell[,] gridCell)
        {
            var cellsList = new List<Cell>();

            foreach (var cell in gridCell) 
            {
                cellsList.Add(cell);
            }

            var temperatureList = cellsList.Select(cell => (int)cell.temperature);
            var temperatureAverage = temperatureList.Average(cell => cell);
            var temperatureStandardDeviation = Math.Sqrt(temperatureList.Average(temperature => Math.Pow((int)temperature - temperatureAverage, 2)));

            var pollutionList = cellsList.Select(cell => (int)cell.airPollution);
            var pollutionAverage = pollutionList.Average(cell => cell);
            var pollutionStandardDeviation = Math.Sqrt(pollutionList.Average(pollution => Math.Pow((int)pollution - temperatureAverage, 2)));

            GlaciersCellsAmountPerDay.Add(cellsList.Where(cell => cell.cellType == CellType.Glaciers).ToList().Count());
            SeaCellsAmountPerDay.Add(cellsList.Where(cell => cell.cellType == CellType.Sea).ToList().Count());
            LandCellsAmountPerDay.Add(cellsList.Where(cell => cell.cellType == CellType.Land).ToList().Count());
            DailyTemperatureAverage.Add(temperatureAverage);
            DailyTemperatureStandardDeviation.Add(temperatureStandardDeviation);
            DailyTemperatureMinMax.Add(Tuple.Create(temperatureList.Min(), temperatureList.Max()));
            DailyPollutionAverage.Add(pollutionAverage);
            DailyPollutionStandardDeviation.Add(pollutionStandardDeviation);
            DailyPollutionMinMax.Add(Tuple.Create(pollutionList.Min(), pollutionList.Max()));
        }

        public void PrintDataAverageRangeAndStandardDeviation()
        {
            Console.WriteLine("");
            Console.WriteLine("____________________________________________");
            Console.WriteLine("____________________________________________");
            Console.WriteLine($"Daily Temperature Average:{DailyTemperatureAverage.Last()}, Min:{DailyTemperatureMinMax.Last().Item1}, Max:{DailyTemperatureMinMax.Last().Item2}, Standard Deviation:{DailyTemperatureStandardDeviation.Last()}");
            Console.WriteLine($"Daily Pollution Average:{DailyPollutionAverage.Last()}, Min:{DailyPollutionMinMax.Last().Item1}, Max:{DailyPollutionMinMax.Last().Item2},Standard Deviation:{DailyPollutionStandardDeviation.Last()}");
            Console.WriteLine("____________________________________________");
            Console.WriteLine("____________________________________________");
        }

        public void CreateReports()
        {
            var temperatureNormalizedData = NormalizeData(DailyTemperatureAverage, DailyTemperatureStandardDeviation);
            var pollutionNormalizedData = NormalizeData(DailyPollutionAverage, DailyPollutionStandardDeviation);

            ReportsCreator.CreateReport(temperatureNormalizedData, "Temperature Normalized Data", "TemperatureNormalizedData.png");
            ReportsCreator.CreateReport(pollutionNormalizedData, "Pollution Normalized Data", "PollutionNormalizedData.png");
            ReportsCreator.CreateReport(DailyPollutionAverage, DailyTemperatureAverage, GlaciersCellsAmountPerDay, SeaCellsAmountPerDay, LandCellsAmountPerDay, "Pollution - Temperature - Glaciers - Sea - Land Correlation", "PollutionTemperatureGlaciersSeaLandCorrelation.png");
        }

        private List<double> NormalizeData(List<double> data,List<double> standardDeviation)
        {
            var result = new List<double>();
            var totalAvarage = data.Average();

            for (int i = 0; i< data.Count; i++)
            {
                result.Add((data[i] - totalAvarage) / standardDeviation[i]);
            }

            return result;
        }
    }
}
