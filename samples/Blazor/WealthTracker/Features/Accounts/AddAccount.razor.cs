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

    private void CreateAccount() =>
        _repo.Add(new WealthDataEntity(Guid.NewGuid().GetHashCode(), _account.Name, [.. _account.Categories]))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Accounts.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Accounts.ListUrl);
}
