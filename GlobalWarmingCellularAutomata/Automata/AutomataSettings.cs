using GlobalWarmingCellularAutomata.Automata.Enums;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;

namespace GlobalWarmingCellularAutomata.Automata
{
    public class AutomataSettings
    {
        public int GridSize { get; set; } = 10;
        public int EarthLandsPercentage { get; } = 10;
        public int EarthCitiesPercentage { get; } = 20;
        public int EarthForestsPercentage { get; } = 20;
        public int EarthGlaciersPercentage { get; } = 10;
        public int EarthSeasPercentage { get; } = 40;
        public AirPollutionRate CitiesAirPollutionRateInitValue { get; set; } = AirPollutionRate.Medium;
        public TemperatureRate SeasTemprateureRateInitValue { get; set; } = TemperatureRate.Cold;
        public CloudsRate LandsCloudsRateInitValue { get; set; } = CloudsRate.None;
        public WindForceScale GlaciersWindForceScaleInitValue { get; set; } = WindForceScale.CasualWind;
    }
}
