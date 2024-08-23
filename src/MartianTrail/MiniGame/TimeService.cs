namespace MartianTrail.MiniGame;

internal class TimeService
{
    public sealed record DurationResult<T>(T Result, double TimeTaken);

    public virtual DateTime Now() => DateTime.Now;

    public DurationResult<T> MeasureDuration<T>(Func<T> operation)
    {
        var timeStart = Now();
        var result = operation();
        var timeEnd = Now();

        return new DurationResult<T>(result, (timeEnd - timeStart).TotalSeconds);
    }
}
