using BudgetTracker.Common;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.BudgetCategories;

public partial class EditCategory
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal BudgetedAmount { get; set; }
    }

    private Option<string> _errorMessage = Option<string>.None();
    private Option<ViewModel> _vm = Option<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(c => c.Id, Id)
             .HandleResult(
                s => _vm = new ViewModel { Id = s.Id, Name = s.Name, BudgetedAmount = s.BudgetedAmount },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _vm.MatchAction(
            a => _repo.GetById(c => c.Id, Id)
                      .Map(prev => prev.UpdateCategory(a.Name, a.BudgetedAmount))
                      .Map(updated => _repo.Update(updated))
                      .Iter(_ => _repo.SaveChanges())
                      .HandleResult(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Categories.MissingCategoryError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
