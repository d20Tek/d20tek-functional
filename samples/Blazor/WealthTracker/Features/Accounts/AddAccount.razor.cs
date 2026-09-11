using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class AddAccount
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public List<string> Categories { get; set; } = [];

        public string SingleCategory { get; set; } = string.Empty;
    }

    private string _errorMessage = string.Empty;
    private readonly ViewModel _account = new();

    private async Task CreateAccount() =>
        await _repo.AddAsync(new WealthDataEntity(Guid.NewGuid().GetHashCode(), _account.Name, [.. _account.Categories]))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
