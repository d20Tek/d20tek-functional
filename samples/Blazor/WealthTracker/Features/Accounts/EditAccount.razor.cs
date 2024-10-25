using Microsoft.AspNetCore.Components;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class EditAccount
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<string> Categories { get; init; } = [];

        public string SingleCategory { get; set; } = string.Empty;

        public bool HasCategory => !string.IsNullOrEmpty(SingleCategory);

        public string GetCategoriesAsString() => Categories.Count > 0 ? string.Join(", ", Categories) : "none.";
    }

    private string _errorMessage = string.Empty;
    private ViewModel? _account;

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized()
    {
        var result = _repo.GetEntityById(Id);
        result.Match(
            s =>
            {
                _account = new ViewModel { Id = s.Id, Name = s.Name, Categories = [.. s.Categories] };
                return string.Empty;
            },
            e => _errorMessage = e.First().ToString());
    }

    private void AddCategory()
    {
        if (string.IsNullOrEmpty(_account?.SingleCategory) is false)
        {
            _account.Categories.Add(_account.SingleCategory);
            _account.SingleCategory = string.Empty;
        }
    }

    private void ClearCategories() => _account?.Categories.Clear();

    private void UpdateHandler()
    {
        if (_account is null)
        {
            _errorMessage = $"Error: cannot edit an account that doesn't exist";
            return;
        }

        var result = _repo.Update(new WealthDataEntity(_account.Id, _account.Name, [.. _account.Categories]));
        result.Match(
            s => { _nav.NavigateTo("/account"); return string.Empty; },
            e => _errorMessage = e.First().ToString());
    }

    private void CancelHandler() => _nav.NavigateTo("/account");
}
