using GlobalWarmingCellularAutomata.Automata;
using GlobalWarmingCellularAutomata.Automata.Entities;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Enums;

internal class Program
{
    private static void Main(string[] args)
    {
        const int daysInYear = 365;
        const int delayPerIteration = 1000;


        var GlobalWarmingCellularAutomataSettings = new AutomataSettings()
        {
            CitiesAirPollutionRateInitValue = AirPollutionRate.High,
            GlaciersWindForceScaleInitValue = WindForceScale.CasualWind,
            SeasTemprateureRateInitValue = TemperatureRate.warm,
            LandsCloudsRateInitValue = CloudsRate.Cloudy,
         };
       
        var GlobalWarmingCellularAutomata = new GlobalWarmingCellularAutomataFacade(GlobalWarmingCellularAutomataSettings);

        for(int i = 0; i<= daysInYear; i++)
        {
            GlobalWarmingCellularAutomata.Tick();
            PrintGrid(GlobalWarmingCellularAutomata.CellsGrid);

            // question c.1
            GlobalWarmingCellularAutomata.PrintDataAverageRangeAndStandardDeviation();

            Thread.Sleep(delayPerIteration);
        }

        // question c.2
        GlobalWarmingCellularAutomata.CreateReports();
    }

    private static void PrintGrid(Cell[,] CellsGrid)
    {
        Console.CursorLeft = 0;
        Console.CursorTop = 0;

        for (int i = 0; i < CellsGrid.GetLength(0); i++)
        {
            for (int j = 0; j < CellsGrid.GetLength(1); j++)
            {
                Console.WriteLine($"[{i},{j}] - {CellsGrid[i,j]}");
            }
        }
    }
}