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
        _repo.Create(new WealthDataEntity(0, _account.Name, [.. _account.Categories]))
             .HandleResult(s => _nav.NavigateTo("/account"), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo("/account");
}
