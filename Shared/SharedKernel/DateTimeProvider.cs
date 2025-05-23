namespace ElementLogiq.SharedKernel;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get { return DateTime.UtcNow; } }
}
