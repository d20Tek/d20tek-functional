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

        public List<DateTimeOffset> EntriesRemoved { get; } = [];

        public void RemoveRecordedEntry(DateTimeOffset date) =>
            RecordedValues.Remove(date).Pipe(_ => EntriesRemoved.Add(date));
    }

    private string _errorMessage = string.Empty;
    private Optional<ViewModel> _optionalVM = Optional<ViewModel>.None();
    private Optional<WealthDataEntity> _account = Optional<WealthDataEntity>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(w => w.Id, Id)
             .HandleResultAsync(s =>
                {
                    _optionalVM = new ViewModel { Id = s.Id, Name = s.Name, RecordedValues = new(s.DailyValues) };
                    _account = s;
                },
                e => _errorMessage = e);

    private async Task UpdateHandler() =>
        await _optionalVM.MatchActionAsync(
            vm =>  ChangeDailyValues(vm)
                      .BindAsync(updated => _repo.UpdateAsync(updated))
                      .IterAsync(_ => _repo.SaveChangesAsync())
                      .HandleResultAsync(s => _nav.NavigateTo(Constants.Reports.CurrentUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Accounts.MissingAccountError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Reports.CurrentUrl);

    private Task<Result<WealthDataEntity>> ChangeDailyValues(ViewModel vm) =>
        _repo.GetByIdAsync(w => w.Id, Id).MapAsync(prev => Task.FromResult(prev.RemoveDailyValues(vm.EntriesRemoved)));
}
