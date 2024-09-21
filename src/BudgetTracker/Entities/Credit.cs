using Apps.Repositories;

namespace BudgetTracker.Entities;

internal sealed class Credit : IEntity
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset DepositDate { get; private set; }

    public decimal Amount {  get; private set; }

    public Credit(int id, string name, DateTimeOffset depositDate, decimal amount)
    {
        Id = id;
        Name = name;
        DepositDate = depositDate;
        Amount = amount;
    }

    public void SetId(int id) => Id = id;

    internal Credit UpdateCredit(string name,  DateTimeOffset depositDate, decimal amount)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));

        Name = name;
        DepositDate = depositDate;
        Amount = amount;
        return this;
    }
}
