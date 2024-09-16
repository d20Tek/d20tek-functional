using GeneratePassword;
using Spectre.Console;

return App.Run(args, AnsiConsole.Console, new Random().Next);
