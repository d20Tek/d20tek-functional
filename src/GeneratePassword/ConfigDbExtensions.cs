using D20Tek.LowDb;

namespace GeneratePassword;

internal static class ConfigDbExtensions
{
    public static LowDb<Config> Create() =>
        LowDbFactory.CreateLowDb<Config>(b => b.UseFileDatabase(Constants.ConfigFile));

    public static Config GetConfig(this LowDb<Config> configDb) => configDb.Get();
}
