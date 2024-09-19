namespace BudgetTracker.Entities;

internal sealed class BudgetCategory
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public decimal BudgetedAmount { get; private set; }

    public BudgetCategory(int id, string name, decimal budgetedAmount)
    {
        Id = id;
        Name = name;
        BudgetedAmount = budgetedAmount;
    }

    internal void SetId(int id) => Id = id;

    internal void UpdateCategory(string name, decimal budgetedAmount)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(budgetedAmount, nameof(budgetedAmount));
        Name = name;
        BudgetedAmount = budgetedAmount;
    }
}
