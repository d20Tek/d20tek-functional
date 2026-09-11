using Microsoft.AspNetCore.Components;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Features.Accounts;

public partial class RecordAccountAmount
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    }

    private string _errorMessage = string.Empty;
    private Optional<ViewModel> _optionalVm = Optional<ViewModel>.None();
    private Optional<WealthDataEntity> _account = Optional<WealthDataEntity>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(w => w.Id, Id)
             .HandleResultAsync(s =>
                {
                    _optionalVm = new ViewModel { Id = s.Id, Name = s.Name };
                    _account = s;
                },
                e => _errorMessage = e);

    private async Task UpdateHandler() =>
        await _optionalVm.MatchActionAsync(
            vm => Validate(vm)
                    .BindAsync(x => ChangeDailyValues(vm))
                    .BindAsync(updated => _repo.UpdateAsync(updated))
                    .IterAsync(_ => _repo.SaveChangesAsync())
                    .HandleResultAsync(s => _nav.NavigateTo(Constants.Reports.CurrentUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Accounts.MissingAccountError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Reports.CurrentUrl);

    private static Result<ViewModel> Validate(ViewModel vm) =>
        (vm.Date > DateTimeOffset.Now) ? Constants.Accounts.FutureDateError<ViewModel>() : vm;

    private Task<Result<WealthDataEntity>> ChangeDailyValues(ViewModel vm) =>
        _repo.GetByIdAsync(w => w.Id, Id).MapAsync(prev => Task.FromResult(prev.AddDailyValue(vm.Date, vm.Amount)));
}
