using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class CategoryExtensions
{
    public static int GetExistingCategoryId(this IAnsiConsole console, string label, ICategoryRepository catRepo) =>
        console.Prompt(CreateTextPrompt(label, catRepo));

    public static int GetExistingCategoryId(
        this IAnsiConsole console,
        string label,
        ICategoryRepository catRepo,
        int prevId) =>
        console.Prompt(CreateTextPrompt(label, catRepo).DefaultValue(prevId));

    private static TextPrompt<int> CreateTextPrompt(string label, ICategoryRepository catRepo) =>
        new TextPrompt<int>(label)
            .Validate(
                x => catRepo.GetEntityById(x) is Something<BudgetCategory>,
                Constants.InvalidCatIdError);
}
