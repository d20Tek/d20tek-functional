using Apps.Repositories;

namespace BudgetTracker.Entities;

internal sealed class Income(int id, string name, DateTimeOffset depositDate, decimal amount) : IEntity
{
    public int Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public DateTimeOffset DepositDate { get; private set; } = depositDate;

    public decimal Amount { get; private set; } = amount;

    public void SetId(int id) => Id = id;

    internal Income UpdateIncome(string name,  DateTimeOffset depositDate, decimal amount)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

        Name = name;
        DepositDate = depositDate;
        Amount = amount;
        return this;
    }
}
