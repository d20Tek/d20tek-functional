using Microsoft.AspNetCore.Components;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class DeleteAccount
{
    private string _errorMessage = string.Empty;
    private Option<WealthDataEntity> _account = Option<WealthDataEntity>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(s => _account = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.Delete(Id)
             .HandleResult(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
