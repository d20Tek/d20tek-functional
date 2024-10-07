using D20Tek.Functional;

namespace MartianTrail.GamePhases;

internal static class GameCalculations
{
    public sealed record LocationFlags(
        bool IsWilderness,
        bool IsTradingPost,
        bool IsHuntingArea,
        bool HuntingFoodAvailable,
        bool HuntingFursAvaiable);

    public static PlayerActionOptions[] CalculatePlayerOptions(Func<int> rnd, GameCalculations.LocationFlags flags) =>
        new[]
        {
            flags.IsTradingPost ? PlayerActions.TradeAtOutpost : PlayerActions.Unavailable,
            flags.IsHuntingArea && flags.HuntingFoodAvailable ? PlayerActions.HuntForFood : PlayerActions.Unavailable,
            flags.IsHuntingArea && flags.HuntingFursAvaiable ? PlayerActions.HuntForSkins : PlayerActions.Unavailable,
            PlayerActions.PushOn
        }.Where(x => x != PlayerActions.Unavailable)
            .Select((x, i) => new PlayerActionOptions(
                x,
                i + 1))
            .ToArray();

    public static int CalculateChargesUsed(decimal accuracy) =>
        (int)(50 * (1 - accuracy));

    public static int CalculateChargesUsed(Func<int, int> rnd) =>
        (int)(50 * (rnd(100) / 100.0));

    public static int CalculateMediPacksUsed(Func<int, int> rnd) =>
        (int)(10 * (rnd(100)/100.0));

    public static int CalculateAtmosphereSuitsUsed(Func<int, int> rnd) =>
        (int)(20 * (rnd(100) / 100.0));

    public static int CalculateFoodGained(decimal accuracy) =>
        (int)(100 * accuracy);

    public static int CalculateFursGained(decimal accuracy) =>
        (int)(50 * accuracy);

    public static int CalculateCreditsFromFurs(int furCount) =>
        (int)(25 * furCount);

    public static bool PlayerCanHunt(this GameState state) => state.Inventory.LaserCharges > 0;

    public static int DistanceTraveled(GameState state) =>
        state.Inventory.Batteries * (state.UserActionSelectedThisTurn == PlayerActions.PushOn ? 100 : 50);

    public static bool RandomEventOccurred(Func<int> rnd) => rnd() > 90;

    public static int CalculateCreditsGained(Func<int, int> rnd) =>
        (int)(100 * (rnd(100) / 100.0));

    public static int CalculateExtraFoodGained(Func<int, int> rnd) =>
        (int)(100 * (rnd(100) / 100.0));

    public static LocationFlags CalculateLocationFlags(Func<int> rnd) =>
        GameCalculations.IsWilderness(rnd)
            .ToIdentity()
            .Map(isWilderness =>
                new LocationFlags(
                    IsWilderness(rnd),
                    IsTradingPost(rnd, isWilderness),
                    IsHuntingArea(rnd, isWilderness),
                    IsHuntingAvailable(rnd),
                    IsHuntingAvailable(rnd)));

    public static bool IsHuntingArea(Func<int> rnd, bool isWilderness) =>
        rnd() > (isWilderness ? 10 : 20);

    public static bool IsHuntingAvailable(Func<int> rnd) =>
        rnd() > 33;

    public static bool IsTradingPost(Func<int> rnd, bool isWilderness) =>
        rnd() > (isWilderness ? 80 : 20);

    public static bool IsWilderness(Func<int> rnd) =>
        rnd() > 33;
}