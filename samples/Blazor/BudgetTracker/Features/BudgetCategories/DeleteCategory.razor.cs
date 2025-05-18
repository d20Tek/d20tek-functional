using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.BudgetCategories;

public partial class DeleteCategory
{
    private Option<string> _errorMessage = Option<string>.None();
    private Option<BudgetCategory> _category = Option<BudgetCategory>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(c => c.Id, Id)
             .HandleResult(s => _category = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.GetById(c => c.Id, Id)
             .Map(cat => _repo.Remove(cat))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
