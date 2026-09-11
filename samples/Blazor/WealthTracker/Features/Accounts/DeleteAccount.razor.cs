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

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(w => w.Id, Id).HandleResultAsync(s => _account = s, e => _errorMessage = e);

    private async Task DeleteHandler() =>
        await _repo.GetByIdAsync(w => w.Id, Id)
             .BindAsync(entity => _repo.RemoveAsync(entity))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
