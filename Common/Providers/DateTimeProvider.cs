using Common.Providers.Interfaces;

namespace Common.Providers;

internal class DateTimeProvider : IDateTime
{
    public DateTimeOffset DateTimeOffsetUtcNow => DateTimeOffset.UtcNow;
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
