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

    private void CreateAccount()
    {
        var result = _repo.Create(new WealthDataEntity(0, _account.Name, [.. _account.Categories]));
        result.Match(
            s => { _nav.NavigateTo("/account"); return string.Empty; },
            e => _errorMessage = e.First().ToString());
    }

    private void CancelHandler() => _nav.NavigateTo("/account");
}
