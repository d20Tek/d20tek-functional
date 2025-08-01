using Microsoft.AspNetCore.Components;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class EditAccount
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<string> Categories { get; set; } = [];

        public string SingleCategory { get; set; } = string.Empty;
    }

    private string _errorMessage = string.Empty;
    private Optional<ViewModel> _account = Optional<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(w => w.Id, Id)
             .HandleResult(
                s => _account = new ViewModel { Id = s.Id, Name = s.Name, Categories = [.. s.Categories] },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _account.MatchAction(
            a => _repo.GetById(w => w.Id, Id)
                      .Bind(entity => _repo.Update(entity.UpdateEntry(a.Name, [.. a.Categories])))
                      .Iter(_ => _repo.SaveChanges())
                      .HandleResult(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Accounts.MissingAccountError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
