﻿using Apps.Repositories;
using BudgetTracker.Entities;

namespace BudgetTracker;

internal static class Constants
{
    public const string AppTitle = "Budget Tracker";
    public const string AskCommandLabel = "Enter app command (help or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage =
    [
        string.Empty,
        "[green]--- Command Definitions ---[/]",
        string.Empty,
        "[yellow]Budget Categories:[/]",
        " - list-cat (lc): shows list of the user's budget categories.",
        " - add-cat (ac): allows user to input new budget category.",
        " - edit-cat (ec): allows uer to edit basic category data (name and amount).",
        " - delete-cat (dc): allows user to delete an existing budget category.",
        string.Empty,
        "[yellow]Expenses:[/]",
        " - list-exp (le): shows list of the user's outstanding expenses.",
        " - add-exp (ae): allows user to input new expense item.",
        " - edit-exp (ee): allows uer to edit basic expense data (name, category, date, actual).",
        " - delete-exp (de): allows user to delete an existing expense item.",
        string.Empty,
        "[yellow]Income:[/]",
        " - list-inc (li): shows list of the user's outstanding income.",
        " - add-inc (ai): allows user to input new income item.",
        " - edit-inc (ei): allows uer to edit basic income data (name, date, amount).",
        " - delete-inc (di): allows user to delete an existing income item.",
        string.Empty,
        "[yellow]Common Commands:[/]",
        " - help (h): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) =>
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";

    public const string DeleteIdLabel = "Enter the id of item to delete:";

    public static string[] DeleteSuccessMessage(IEntity entity) =>
    [
        string.Empty,
            $"[green]Success:[/] The item with id={entity.Id} was deleted."
    ];
}
