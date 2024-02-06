namespace GnomeStack.Functional;

public abstract class Page<T>
{
    public IReadOnlyList<T> Values { get; } = Array.Empty<T>();

    public string? ContinuationToken { get; }
}