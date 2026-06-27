using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; private set; } = default!;

    private LastName() { }

    private LastName(string value)
    {
        Value = value;
    }

    public static LastName Create(string value)
    {
        value = TextNormalizer.NormalizeWhitespace(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Last name cannot be empty.");

        if (value.Length > MaxLength)
            throw new DomainException($"Last name cannot exceed {MaxLength} characters.");

        return new LastName(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString()
    {
        return Value;
    }
}
