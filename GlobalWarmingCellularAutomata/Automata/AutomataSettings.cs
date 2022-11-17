using GlobalWarmingCellularAutomata.Automata.Enums;
using GlobalWarmingCellularAutomata.Automata.Entities.Data;

namespace GlobalWarmingCellularAutomata.Automata
{
    public class AutomataSettings
    {
        public int GridSize { get; set; } = 100;
        public int EarthLandsPercentage { get; } = 10;
        public int EarthCitiesPercentage { get; } = 10;
        public int EarthForestsPercentage { get; } = 10;
        public int EarthGlaciersPercentage { get; } = 30;
        public int EarthSeasPercentage { get; } = 40;
        public AirPollutionRate CitiesAirPollutionRateInitValue { get; set; } = AirPollutionRate.Medium;
        public TemprateureRate SeasTemprateureRateInitValue { get; set; } = TemprateureRate.Cold;
        public CloudsRate LandsCloudsRateInitValue { get; set; } = CloudsRate.None;
        public WindForceScale glaciersWindForceScaleInitValue { get; set; } = WindForceScale.CasualWind;
    }
}
