using GlobalWarmingCellularAutomata.Automata.Entities;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Services;
using GlobalWarmingCellularAutomata.Automata.Enums;

namespace GlobalWarmingCellularAutomata.Automata
{
    public class GlobalWarmingCellularAutomata
    {
        private readonly AutomataSettings automataSettings;
        private readonly GlobalWarmingStatistics globalWarmingStatistics;
        private readonly GlobalWarmimgRuleSet globalWarmimgRuleSet;
        private readonly Random rnd;
        private readonly Cell[ , ] cellsGrid;

        public GlobalWarmingCellularAutomata() : this(new AutomataSettings())
        {
        }

        public GlobalWarmingCellularAutomata(AutomataSettings automataSettings) 
        {
            cellsGrid = new Cell[automataSettings.GridSize, automataSettings.GridSize];
            this.automataSettings = automataSettings;
            globalWarmingStatistics = new GlobalWarmingStatistics();
            globalWarmimgRuleSet = new GlobalWarmimgRuleSet(cellsGrid);
            rnd = new Random();
            InitializeCellGrid();
        }

        public void Tick()
        {
            globalWarmimgRuleSet.Tick();
        }

        private void InitializeCellGrid()
        {
            int cellsInGris = cellsGrid.Length;

            var cellsTypeToDistributeDict = new Dictionary<CellType, int>()
            {
                {CellType.Land, GetRoundedPercentage(cellsInGris, automataSettings.EarthLandsPercentage)},
                {CellType.City, GetRoundedPercentage(cellsInGris, automataSettings.EarthCitiesPercentage)},
                {CellType.Forest, GetRoundedPercentage(cellsInGris, automataSettings.EarthForestsPercentage)},
                {CellType.Glaciers, GetRoundedPercentage(cellsInGris, automataSettings.EarthGlaciersPercentage)},
                {CellType.Sea, GetRoundedPercentage(cellsInGris, automataSettings.EarthSeasPercentage)},
            };

            var airPollutionRatesList = new List<AirPollutionRate> { AirPollutionRate.None, AirPollutionRate.Medium, AirPollutionRate.high };
            var cloudsRatesRateList = new List<CloudsRate> { CloudsRate.None, CloudsRate.Cloudy, CloudsRate.RainClouds };
            var temprateureRatesList = new List<TemprateureRate> { TemprateureRate.Freezing,TemprateureRate.warm, TemprateureRate.Hot, TemprateureRate.ExtreamHot};
            var WindDirectionsList = new List<WindDirection> { WindDirection.North, WindDirection.East, WindDirection.South, WindDirection.West};
            var WindForceScalesList = new List<WindForceScale> { WindForceScale.ExtreamWind, WindForceScale.None, WindForceScale.StrongWind, WindForceScale.CasualWind};

            for (int i = 0; i < cellsGrid.GetLength(0); i++)
            {
                for (int j = 0; j < cellsGrid.GetLength(1); j++)
                {
                    CellType cellType = GetRandomCellTypeFromDistributeDict(cellsTypeToDistributeDict);
                    AirPollutionRate airPollution = GetRandomValueFromList(airPollutionRatesList);
                    CloudsRate clouds = GetRandomValueFromList(cloudsRatesRateList);
                    TemprateureRate temprateure = GetRandomValueFromList(temprateureRatesList);
                    WindDirection windDirection = GetRandomValueFromList(WindDirectionsList);
                    WindForceScale windForceScale = GetRandomValueFromList(WindForceScalesList);

                    cellsGrid[i,j] = new Cell(cellType, airPollution, clouds, temprateure, new Wind(windDirection, windForceScale));
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
            return Convert.ToInt32(Math.Round(((double)num / percentage) * 100, 0));
        }
    }
}
