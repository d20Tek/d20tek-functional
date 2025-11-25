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

    protected override void OnInitialized() =>
        _repo.GetById(w => w.Id, Id)
             .HandleResult(s =>
                {
                    _optionalVm = new ViewModel { Id = s.Id, Name = s.Name };
                    _account = s;
                },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _optionalVm.MatchAction(
            vm => Validate(vm)
                    .Bind(x => ChangeDailyValues(vm))
                    .Bind(updated => _repo.Update(updated))
                    .Iter(_ => _repo.SaveChanges())
                    .HandleResult(s => _nav.NavigateTo(Constants.Reports.CurrentUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Accounts.MissingAccountError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Reports.CurrentUrl);

    private static Result<ViewModel> Validate(ViewModel vm) =>
        (vm.Date > DateTimeOffset.Now) ? Constants.Accounts.FutureDateError<ViewModel>() : vm;

    private Result<WealthDataEntity> ChangeDailyValues(ViewModel vm) =>
        _repo.GetById(w => w.Id, Id).Map(prev => prev.AddDailyValue(vm.Date, vm.Amount));
}
