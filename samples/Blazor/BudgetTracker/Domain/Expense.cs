using BudgetTracker.Persistence;

namespace BudgetTracker.Domain;

internal sealed class Expense(int id, string name, int categoryId, DateTimeOffset committedDate, decimal actual) :
    IEntity
{
    public int Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public int CategoryId { get; private set; } = categoryId;

    public DateTimeOffset CommittedDate { get; private set; } = committedDate;

    public decimal Actual { get; private set; } = actual;

    public void SetId(int id) => Id = id;

    internal Expense UpdateExpense(string name, int categoryId, DateTimeOffset committedDate, decimal actual)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(categoryId, nameof(categoryId));
        ArgumentOutOfRangeException.ThrowIfNegative(actual, nameof(actual));

        Name = name;
        CategoryId = categoryId;
        CommittedDate = committedDate;
        Actual = actual;
        return this;
    }
}
