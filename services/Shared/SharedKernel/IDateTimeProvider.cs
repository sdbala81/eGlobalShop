namespace ElementLogiq.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
