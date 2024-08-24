using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal sealed class DisplayMartianWeather : IGamePhase
{
    public sealed record NasaMarsData(IEnumerable<NasaSolData> soles);

    public sealed record NasaSolData(
        string id,
        string sol,
        string max_temp,
        string min_temp,
        string local_uv_irradiance_index);

    private readonly int _firstSol;
    private readonly Func<int, SolData?> _solLookup;

    public DisplayMartianWeather(WebApiClient webApiClient) =>
        (_firstSol, _solLookup) = FetchSolData(webApiClient)
            .Map(data => data is Something<IEnumerable<SolData>> s ? s.Value.ToArray() : [])
            .Map(sol => (
                sol.LastOrDefault()?.Sol ?? 0,
                sol.ToDictionary(x => x.Sol, x => x).ToLookupWithDefault()));

    private static Maybe<IEnumerable<SolData>> FetchSolData(WebApiClient webApiClient) =>
        webApiClient.Fetch<NasaMarsData>(Constants.MartianWeather.NasaMarsUrl).Result
                .Bind(x => x.soles.OrderByDescending(x => x.id).Take(Constants.MartianWeather.SolDataLimit))
                .Bind(x => x.Select(y => new SolData(
                    Sol: int.TryParse(y.sol, out var i) ? i : Constants.MartianWeather.InvalidValue,
                    MaxTemp: decimal.TryParse(y.max_temp, out var mt) ? mt : Constants.MartianWeather.InvalidValue,
                    MinTemp: decimal.TryParse(y.min_temp, out var mt2) ? mt2 : Constants.MartianWeather.InvalidValue,
                    UltraViolet: y.local_uv_irradiance_index))
                        .Where(x => x.Sol != Constants.MartianWeather.InvalidValue &&
                                    x.MaxTemp != Constants.MartianWeather.InvalidValue &&
                                    x.MinTemp != Constants.MartianWeather.InvalidValue));

    public GameState DoPhase(GameState oldState) =>
        GetCurrentSol(oldState).Map(sol => oldState with
        {
            CurrentSol = sol,
            LatestMoves = [ Constants.MartianWeather.FormatSolData(GetCurrentSolData(sol)) ]
        });

    private int GetCurrentSol(GameState state) => state.CurrentSol == 0 ? _firstSol : state.CurrentSol + 1;

    private SolData? GetCurrentSolData(int sol) => _solLookup(sol);
}
