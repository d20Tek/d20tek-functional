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
        public static string[] CannotHuntLabel =
        [
            "",
            "[red]Laser charges error!!![/]",
            "You have no laser charges remaining, so you cannot hunt."
        ];
        public static string[] HuntingFoodLabel =
        [
            "",
            "You've decided to hunt Vrolids for food.",
            "For that you'll have to play the mini-game..."
        ];
        public static string[] HuntingFursLabel =
        [
            "",
            "You've decided to hunt Lophroll for fur.",
            "For that you'll have to play the mini-game..."
        ];
        public static string[] TradingPostLabel =
        [
            "",
            "Welcome to the settlement trading post.",
            "We trade furs for fuel, laser charges, and medipacks..."
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
                    "",
                    "[green]Great shot![/] You brought down a whole load of the things!",
                    "Vrolid burgers are on you today!"
                ],
                0 =>
                [
                    "[orange]You missed.[/] Were you taking a nap?"
                ],
                _ =>
                [
                    "Not a bad shot",
                    "You brought down at least a couple",
                    "Don't go too crazy eating tonight."
                ]
            };

        public static string[] FursAccuracyMessage(decimal accuracy) =>
            accuracy switch
            {
                >= 0.9M =>
                [
                    "[green]Great shot![/] You brought down a whole load of the things!",
                    "Lophroll furs are the fashion of the day!"
                ],
                0 =>
                [
                    "[orange]You missed.[/] Were you afraid of the Lophrolls?"
                ],
                _ =>
                [
                    "Not a bad shot",
                    "You brought down at least a couple",
                    "At least a couple of furs for the trader."
                ]
            };
    }

    public static class RandomEvent
    {
        public const string EventHeader = "*** Event ***";
        public static string[] EventMessages(RandomEventDetails details, int x = 0) =>
            [EventHeader, details.Title, details.SuccessMessage(x), string.Empty];
        public static string[] EventErrorMessages(RandomEventDetails details, int x = 0) =>
            [EventHeader, details.Title, details.FailureMessage, string.Empty];

        public static readonly RandomEventDetails[] Events =
        [
            new(
            "You found a crashed speeder and recovered a stash of credits.",
            (x) => $"Congrats! You recovered {x} credits.",
            string.Empty,
            RandomEventPhase.RecoverCredits),
        new(
            "Sand storm! Atmosphere suits are needed to survive.",
            (x) => $"You were able to use your atmosphere suits to survive.",
            "Unfortunately you don't have any atmosphere suits to use.",
            RandomEventPhase.AtmosphericEvent),
        new(
            "A stampede of Vrolids occurs. You gain extra food today!",
            (x) => $"Congrats! You found {x} extra food.",
            string.Empty,
            RandomEventPhase.ExtraFoodFound),
        new(
            "Dangerous predators attack during the night.",
            (x) => $"You were able to fight back the enemies with your lasers.",
            "Unfortunately you don't have any laser charges remaining to defend yourself.",
            RandomEventPhase.EnemiesAttack),
        new(
            "You encounter settlers having a yard sale. They're selling off old batteries and atmosphere suits extremely cheaply.",
            (x) => "",
            string.Empty,
            RandomEventPhase.YardSaleFound),
        new(
            "You are attacked by a group of bandits on the trail.",
            (x) => $"You were able to fight back the enemies with your lasers.",
            "Unfortunately you don't have any laser charges remaining to defend yourself.",
            RandomEventPhase.EnemiesAttack),
        new(
            "Friendly Martians appear and guide you to a food source.",
            (x) => $"You were able to use your medipacks to recover from the illness.",
            string.Empty,
            RandomEventPhase.ExtraFoodFound),
        new(
            "Your group falls ill from the dreaded Martian flu.",
            (x) => "You use your medipacks to recover from the illness.",
            "Unfortunately you don't have any medipacks to combat the illness.",
            RandomEventPhase.ContractedIllness)
        ];
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
            $"You have " + state.Inventory.Furs + " furs remaining",
            $"You have " + state.Inventory.LaserCharges + " laser charges remaining",
            $"===== End of Sol {state.CurrentSol} =====",
            ""
        ];
    }
}
