using GlobalWarmingCellularAutomata.Automata.Entities;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Services;
using GlobalWarmingCellularAutomata.Automata.Enums;

namespace GlobalWarmingCellularAutomata.Automata
{
    public class GlobalWarmingCellularAutomataFacade
    {
        public Cell[,] CellsGrid { get; }

        private readonly AutomataSettings automataSettings;
        private readonly GlobalWarmingStatistics globalWarmingStatistics;
        private readonly GlobalWarmimgRuleSet globalWarmimgRuleSet;
        private readonly Random rnd;
        
        public GlobalWarmingCellularAutomataFacade() : this(new AutomataSettings())
        {
        }

        public GlobalWarmingCellularAutomataFacade(AutomataSettings automataSettings) 
        {
            CellsGrid = new Cell[automataSettings.GridSize, automataSettings.GridSize];
            this.automataSettings = automataSettings;
            globalWarmingStatistics = new GlobalWarmingStatistics();
            globalWarmimgRuleSet = new GlobalWarmimgRuleSet(CellsGrid);
            rnd = new Random();
            InitializeCellGrid();
        }

        // Automata iteration
        public void Tick()
        {
            globalWarmimgRuleSet.Tick();
            globalWarmingStatistics.SaveStatistics(CellsGrid);
        }

        // question c.1
        public void PrintDataAverageRangeAndStandardDeviation()
        {
            globalWarmingStatistics.PrintDataAverageRangeAndStandardDeviation();
        }

        // Creates the Reports for question c.2
        public void CreateReports()
        {
            globalWarmingStatistics.CreateReports();
        }

        private void InitializeCellGrid()
        {
            int cellsInGris = CellsGrid.Length;

            // cell type mapped to the expected call amount in grid
            var cellsTypeToDistributeDict = new Dictionary<CellType, int>()
            {
                {CellType.Land, GetRoundedPercentage(cellsInGris, automataSettings.EarthLandsPercentage)},
                {CellType.City, GetRoundedPercentage(cellsInGris, automataSettings.EarthCitiesPercentage)},
                {CellType.Forest, GetRoundedPercentage(cellsInGris, automataSettings.EarthForestsPercentage)},
                {CellType.Glaciers, GetRoundedPercentage(cellsInGris, automataSettings.EarthGlaciersPercentage)},
                {CellType.Sea, GetRoundedPercentage(cellsInGris, automataSettings.EarthSeasPercentage)},
            };

            // Lists of all posible cell metadatas
            var cloudsRatesRateList = new List<CloudsRate> { CloudsRate.None, CloudsRate.Cloudy, CloudsRate.RainClouds };
            var temprateureRatesList = new List<TemperatureRate> { TemperatureRate.Freezing,TemperatureRate.warm, TemperatureRate.Hot, TemperatureRate.ExtreamHot};
            var WindDirectionsList = new List<WindDirection> { WindDirection.North, WindDirection.East, WindDirection.South, WindDirection.West};
            var WindForceScalesList = new List<WindForceScale> { WindForceScale.ExtreamWind, WindForceScale.None, WindForceScale.StrongWind, WindForceScale.CasualWind};

            for (int i = 0; i < CellsGrid.GetLength(0); i++)
            {
                for (int j = 0; j < CellsGrid.GetLength(1); j++)
                {
                    CellType cellType = GetRandomCellTypeFromDistributeDict(cellsTypeToDistributeDict);
                    CloudsRate clouds = GetRandomValueFromList(cloudsRatesRateList);
                    TemperatureRate temprateure = GetRandomValueFromList(temprateureRatesList);
                    WindDirection windDirection = GetRandomValueFromList(WindDirectionsList);
                    WindForceScale windForceScale = GetRandomValueFromList(WindForceScalesList);
                    AirPollutionRate airPollution = AirPollutionRate.None;

                    // apply specific settings
                    switch (cellType)
                    {
                        case CellType.City:
                            airPollution = automataSettings.CitiesAirPollutionRateInitValue;
                            break;
                        case CellType.Sea:
                            temprateure = automataSettings.SeasTemprateureRateInitValue; 
                            break;
                        case CellType.Land:
                            clouds = automataSettings.LandsCloudsRateInitValue;
                            break;
                        case CellType.Glaciers:
                            windForceScale = automataSettings.GlaciersWindForceScaleInitValue;
                            break;
                    }


                    CellsGrid[i,j] = new Cell(cellType, airPollution, clouds, temprateure, new Wind(windDirection, windForceScale));
                }
            }
        }

        private CellType GetRandomCellTypeFromDistributeDict(Dictionary<CellType, int> cellsTypeToDistributeDict)
        {
            CellType cellType;

            if (cellsTypeToDistributeDict.Count == 0)
            {
                cellType = CellType.Sea;

            }
            else
            {
                var randomCellTypeIndex = rnd.Next(0, cellsTypeToDistributeDict.Count);
                cellType = cellsTypeToDistributeDict.ElementAt(randomCellTypeIndex).Key;
                cellsTypeToDistributeDict[cellType] = cellsTypeToDistributeDict[cellType] - 1;

                if (cellsTypeToDistributeDict[cellType] == 0)
                {
                    cellsTypeToDistributeDict.Remove(cellType);
                }
            }

            return cellType;
        }

        private T GetRandomValueFromList<T>(List<T> list)
        {
            return list[rnd.Next(0, list.Count)];
        }

        private static int GetRoundedPercentage(int num, int percentage)
        {
            return Convert.ToInt32(Math.Round(((double)num * percentage) / 100, 0));
        }
    }
}
