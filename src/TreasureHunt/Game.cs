using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;

namespace TreasureHunt;

internal static class Game
{
    public static void Play(IAnsiConsole console, Func<int, int> rnd) =>
        InitializeGame(console, rnd)
            .Map(initialState =>
                initialState.IterateUntil(
                    x => x.NextCommand(rnd, console),
                    x => x.IsGameComplete())
            .Apply(x => console.WriteMessage(x.GetEndGameMessage())));

    private static GameState InitializeGame(IAnsiConsole console, Func<int, int> rnd) =>
        GameState.Initialize(
            treasureLocations: GameData.GetTreasureLocations(),
            room: rnd(Constants.TotalRooms))
                .Apply(s => console.Write(Presenters.GameHeader(Constants.GameTitle)))
                .Apply(x => DisplayInstructions(console));

    private static GameState NextCommand(this GameState state, Func<int, int> rnd, IAnsiConsole console) =>
        state.Apply(s => s.DisplayCurrentLocation(console))
             .Map(s => ProcessCommand(s, GetCommand(console)))
             .Apply(s => console.WriteMessage(s.LatestMove));

    private static bool IsGameComplete(this GameState state) =>
        state.TreasureLocations.First()
            .Map(first => state.TreasureLocations.All(x => x == first) || 
                          state.Moves > Constants.MovesAllowed);

    private static string GetEndGameMessage(this GameState endState) =>
        (endState.Moves > Constants.MovesAllowed)
            ? Constants.EndGameLostMessage
            : Constants.EndGameWonMessage;

    private static void DisplayInstructions(IAnsiConsole console) =>
        console.Confirm(Constants.ShowInstructionsLabel)
            .Apply(x => console.WriteMessageConditional(x, Constants.Instructions))
            .Apply(x => x.IfTrueOrElse(() => console.PromptAnyKey(Constants.StartGameLabel)));

    private static void DisplayCurrentLocation(this GameState state, IAnsiConsole console) =>
        state.Apply(s => console.WriteMessage(Constants.RoomDescription(
                          s.CurrentRoom,
                          GameData.GetRoomById(s.CurrentRoom).Description)))
             .Map(s => s.GetAllTreasureInRoom(s.CurrentRoom))
             .Apply(l => console.WriteMessage(Constants.TreasureDescriptions(l)))
             .Apply(l => console.WriteMessage());

    private static string GetCommand(IAnsiConsole console) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.CommandInputLabel)
                                    .AddChoices(Constants.CommandOptions)
                                    .HideChoices());

    private static GameState ProcessCommand(GameState state, string command) =>
        Commands.FindCommand(command)(state);
}
