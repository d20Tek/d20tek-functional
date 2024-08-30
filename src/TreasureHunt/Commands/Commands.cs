using D20Tek.Minimal.Functional;

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

    public static CommandFunc FindCommand(string command) =>
        command.ToUpper()
            .Map(c => _commands.TryGetValue(c, out CommandFunc? value) ? value : None);

    private static GameState None(GameState state) =>
        state with { LatestMove = Constants.Commands.CommandError };

    private static GameState Help(GameState state) =>
        state with { LatestMove = Constants.Instructions };
}
