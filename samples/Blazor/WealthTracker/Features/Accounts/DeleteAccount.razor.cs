using Microsoft.AspNetCore.Components;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class DeleteAccount
{
    private string _errorMessage = string.Empty;
    private WealthDataEntity? _account;

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized()
    {
        var result = _repo.GetEntityById(Id);
        result.Match(
            s => { _account = s; return string.Empty; },
            e => _errorMessage = e.First().ToString());
    }

    private void DeleteHandler()
    {
        var result = _repo.Delete(Id);
        result.Match(
            s => { _nav.NavigateTo("/account"); return string.Empty; },
            e => _errorMessage = e.First().ToString());
    }

    private void CancelHandler() => _nav.NavigateTo("/account");
}
