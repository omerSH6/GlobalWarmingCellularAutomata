using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Enums;

namespace GlobalWarmingCellularAutomata.Automata.Entities
{
    public class Cell
    {
        public CellType cellType { get; set; }
        public AirPollutionRate airPollution { get; set; }
        public CloudsRate clouds { get; set; }
        public TemprateureRate temperature { get; set; }
        public Wind wind { get; set; }
        public int HotDaysCounter { get; set; }
        public int ColdDaysCounter { get; set; }
        public int RainDaysCounter { get; set; }


        public Cell(CellType cellType, AirPollutionRate airPollution, CloudsRate clouds, TemprateureRate temperature, Wind wind)
        {
            this.cellType = cellType;
            this.airPollution = airPollution;
            this.clouds = clouds;
            this.temperature = temperature;
            this.wind = wind;
        }
    }

    public enum CellType
    {
        City,
        Forest,
        Glaciers,
        Land,
        Sea
    }
}
