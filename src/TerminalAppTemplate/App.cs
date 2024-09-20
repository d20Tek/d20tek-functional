﻿using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;
using TerminalAppTemplate.Commands;

namespace TerminalAppTemplate;

internal static class App
{
    public static void Run(IAnsiConsole console) =>
        AppState.Initialize(console)
            .Apply(x => console.DisplayAppHeader(Constants.AppTitle))
            .IterateUntil(
                x => NextCommand(x),
                x => x.CanContinue is false);

    private static AppState NextCommand(AppState prevState) =>
        prevState.Console.GetUserCommandInput()
            .Map(inputCommand =>
                Configuration.GetCommandTypes()
                    .Map(x => x.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand)))
                    .Map(x => x ?? Configuration.ErrorTypeHandler(inputCommand))
                    .Map(x => x.TypeHandler(prevState, x))
            );

    private static string GetUserCommandInput(this IAnsiConsole console) =>
        console.Apply(c => c.WriteLine())
               .Map(c => c.Prompt<string>(new TextPrompt<string>(Constants.AskCommandLabel)
                            .AddChoices(Configuration.GetCommands())
                            .ShowChoices(false)));
}
