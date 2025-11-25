using D20Tek.Functional;

namespace TreasureHunt.Commands;

internal static class Commands
{
    public delegate GameState CommandFunc(GameState state);

    private static Dictionary<string, CommandFunc> _commands = new()
    {
        { "N", MoveCommands.North },
        { "E", MoveCommands.East },
        { "S", MoveCommands.South },
        { "W", MoveCommands.West },
        { "HELP", Help },
        { "LOCATE", LocateCommand.Locate },
        { "GRAB", InventoryCommands.Grab },
        { "PUT", InventoryCommands.Put }
    };

    public static GameState ProcessCommand(GameState state, string command) =>
        command.ToUpper().ToIdentity()
               .Map(c => _commands.TryGetValue(c, out CommandFunc? value) ? value : None)
               .Map(f => f(state))
               .Map(s => s with { Moves = s.Moves + 1 });

    private static GameState None(GameState state) => state with { LatestMove = Constants.Commands.CommandError };

    private static GameState Help(GameState state) => state with { LatestMove = Constants.Instructions };
}
