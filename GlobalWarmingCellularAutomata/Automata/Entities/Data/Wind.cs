namespace GlobalWarmingCellularAutomata.Automata.Entities.Data
{
    public class Wind
    {
        public WindDirection WindDirection { get; set; }
        public WindForceScale WindForceScale { get; set; }

        public Wind(WindDirection windDirection, WindForceScale windForceScale)
        {
            WindDirection = windDirection; 
            WindForceScale = windForceScale;
        }

        public override string ToString()
        {
            return $"Direction:{WindDirection},Force:{WindForceScale}";
        }
    }

    public enum WindDirection
    {
        North,
        South,
        West,
        East
    }

    public enum WindForceScale
    {
        None,
        CasualWind,
        StrongWind,
        ExtreamWind
    }
}