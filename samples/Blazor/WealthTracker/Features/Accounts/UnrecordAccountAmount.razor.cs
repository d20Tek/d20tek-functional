using D20Tek.Functional;
using Microsoft.AspNetCore.Components;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class UnrecordAccountAmount
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public SortedDictionary<DateTimeOffset, decimal> RecordedValues { get; set; } = [];
    }

    private string _errorMessage = string.Empty;
    private ViewModel? _vm;
    private WealthDataEntity? _account;

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(
                s =>
                {
                    _vm = new ViewModel { Id = s.Id, Name = s.Name, RecordedValues = s.DailyValues };
                    _account = s;
                },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _vm.ToOption()
           .MatchAction(
               vm =>  _repo.Update(_account!)
                           .HandleResult(s => _nav.NavigateTo(Constants.Reports.CurrentUrl), e => _errorMessage = e),
               () => _errorMessage = Constants.Accounts.MissingAccountError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Reports.CurrentUrl);
}
