using D20Tek.Functional;
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
    private ViewModel? _account;

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(
                s => _account = new ViewModel { Id = s.Id, Name = s.Name, Categories = [.. s.Categories] },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _account.ToOption()
                .MatchAction(
                    a => _repo.Update(new WealthDataEntity(a.Id, a.Name, [.. a.Categories]))
                              .HandleResult(s => _nav.NavigateTo("/account"), e => _errorMessage = e),
                    () => _errorMessage = $"Error: cannot edit an account that doesn't exist");

    private void CancelHandler() => _nav.NavigateTo("/account");
}
