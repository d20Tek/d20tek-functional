using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class AddAccount
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public List<string> Categories { get; } = [];

        public string SingleCategory { get; set; } = string.Empty;

        public bool HasCategory => !string.IsNullOrEmpty(SingleCategory);
    }

    private string _errorMessage = string.Empty;
    private readonly ViewModel _account = new();

    private void AddCategory()
    {
        if (string.IsNullOrEmpty(_account.SingleCategory) is false)
        {
            _account.Categories.Add(_account.SingleCategory);
            _account.SingleCategory = string.Empty;
        }
    }

    private void CreateAccount()
    {
        var result = _repo.Create(new WealthDataEntity(0, _account.Name, [.. _account.Categories]));
        result.Match(
            s => { _nav.NavigateTo("/account"); return string.Empty; },
            e => _errorMessage = e.First().ToString());
    }

    private void CancelHandler() => _nav.NavigateTo("/account");
}
