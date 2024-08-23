using MartianTrail.GamePhases;

namespace MartianTrail;

internal static partial class Constants
{
    public static class MartianWeather
    {
        public const string NasaMarsUrl = "https://mars.nasa.gov/rss/api/?feed=weather&category=msl&feedtype=json";
        public const int InvalidValue = -1;
        public const int SolDataLimit = 300;

        public static string FormatSolData(SolData? sol) =>
            sol is null
                ? string.Empty
                : $"Mars Sol {sol.Sol}{Environment.NewLine}" +
                  $"\tMin Temp: {sol.MinTemp} {Environment.NewLine}" +
                  $"\tMax Temp: {sol.MaxTemp}{Environment.NewLine}" +
                  $"\tUV Irradiance Index: {sol.UltraViolet}{Environment.NewLine}";
    }
}
