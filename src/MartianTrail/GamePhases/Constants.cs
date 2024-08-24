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
                : $"===== Mars Sol {sol.Sol} ====={Environment.NewLine}" +
                  $"Local Weather{Environment.NewLine}" +
                  $"    Min Temp: {sol.MinTemp} {Environment.NewLine}" +
                  $"    Max Temp: {sol.MaxTemp}{Environment.NewLine}" +
                  $"    UV Irradiance Index: {sol.UltraViolet}{Environment.NewLine}";
    }

    public static class SelectAction
    {
        public const string PlayerChoiceLabel = "What would you like to do?";
        public static string[] HuntingFoodLabel =
        [
            "You've decided to hunt Vrolids for food.",
            "For that you'll have to play the mini-game..."
        ];

        public static string PlayerActionsMessage(
            PlayerActionOptions[] options,
            bool isWilderness) =>
            string.Join(
                Environment.NewLine,
                new[]
                {
                    "The area you are passing through is " + (isWilderness? "wilderness" : "a small settlement"),
                    "Here are your options for what you can do:"
                }.Concat(
                    options.Select(x => "    " + x.ChoiceNumber + " - " + x.Action switch
                    {
                        PlayerActions.TradeAtOutpost => "Trade at the nearby outpost",
                        PlayerActions.HuntForFood => "Hunt for food",
                        PlayerActions.HuntForSkins => "Hunt for furs to sell later",
                        PlayerActions.PushOn => "Just push on to travel faster",
                        _ => "No further actions are supported this turn"
                    })
                ));

        public static string[] FoodAccuracyMessage(decimal accuracy) =>
            accuracy switch
            {
                >= 0.9M =>
                [
                       "Great shot! You brought down a whole load of the things!",
                       "Vrolid burgers are on you today!"
                ],
                0 =>
                [
                       "You missed.  Were you taking a nap?"
                ],
                _ =>
                [
                       "Not a bad shot",
                       "You brought down at least a couple",
                       "Don't go too crazy eating tonight"
                ]
            };
    }

public static class UpdateProgress
    {
        public const int CompletionDistance = 16000;

        public static string[] Display(GameState state) =>
        [
            $"You have traveled {state.DistanceThisTurn} this Sol.",
            $"That's a total distance of {state.DistanceTraveled}.",
            $"You have " + state.Inventory.Batteries + " batteries remaining",
            $"You have " + state.Inventory.Food + " food remaining",
            $"===== End of Sol {state.CurrentSol} =====",
            ""
        ];
    }
}
