using Apps.Repositories;

namespace BudgetTracker.Entities;

internal sealed class Expense : IEntity
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset CommittedDate { get; private set; }

    public decimal Actual {  get; private set; }

    public Expense(int id, string name, DateTimeOffset committedDate, decimal actual)
    {
        Id = id;
        Name = name;
        CommittedDate = committedDate;
        Actual = actual;
    }

    public void SetId(int id) => Id = id;

    internal Expense UpdateExpense(string name, DateTimeOffset committedDate, decimal actual)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(actual, nameof(actual));

        Name = name;
        CommittedDate = committedDate;
        Actual = actual;
        return this;
    }
}
