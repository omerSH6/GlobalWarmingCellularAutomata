using System.Xml.Linq;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;
using GlobalWarmingCellularAutomata.Automata.Enums;

namespace GlobalWarmingCellularAutomata.Automata.Entities
{
    public class Cell
    {
        public CellType cellType { get; set; }
        public AirPollutionRate airPollution { get; set; }
        public CloudsRate clouds { get; set; }
        public TemperatureRate temperature { get; set; }
        public Wind wind { get; set; }
        public int HotDaysCounter { get; set; }
        public int ColdDaysCounter { get; set; }
        public int RainDaysCounter { get; set; }


        public Cell(CellType cellType, AirPollutionRate airPollution, CloudsRate clouds, TemperatureRate temperature, Wind wind, int hotDaysCounter = 0, int coldDaysCounter = 0, int rainDaysCounter = 0)
        {
            this.cellType = cellType;
            this.airPollution = airPollution;
            this.clouds = clouds;
            this.temperature = temperature;
            this.wind = wind;
            HotDaysCounter = hotDaysCounter;
            ColdDaysCounter = coldDaysCounter;
            RainDaysCounter = rainDaysCounter;
        }

        public override string ToString()
        {
            return $"CellType:{cellType}, Pollution:{airPollution}, Clouds:{clouds}, Temprateure:{temperature}, Wind:{wind}";
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
