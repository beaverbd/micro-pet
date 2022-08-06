namespace Common.Providers.Interfaces;

public interface IDateTime
{
    DateTimeOffset DateTimeOffsetUtcNow { get; }
    DateTime DateTimeUtcNow { get; }
}
