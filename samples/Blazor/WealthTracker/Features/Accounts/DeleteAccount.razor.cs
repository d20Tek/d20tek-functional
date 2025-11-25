using Microsoft.AspNetCore.Components;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class DeleteAccount
{
    private string _errorMessage = string.Empty;
    private Optional<WealthDataEntity> _account = Optional<WealthDataEntity>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(w => w.Id, Id).HandleResult(s => _account = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.GetById(w => w.Id, Id)
             .Bind(entity => _repo.Remove(entity))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
