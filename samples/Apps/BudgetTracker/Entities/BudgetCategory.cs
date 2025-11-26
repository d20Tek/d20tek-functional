using Apps.Repositories;

namespace BudgetTracker.Entities;

internal sealed class BudgetCategory(int id, string name, decimal budgetedAmount) : IEntity
{
    public int Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public decimal BudgetedAmount { get; private set; } = budgetedAmount;

    public void SetId(int id) => Id = id;

    internal BudgetCategory UpdateCategory(string name, decimal budgetedAmount)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(budgetedAmount, nameof(budgetedAmount));

        Name = name;
        BudgetedAmount = budgetedAmount;
        return this;
    }
}
