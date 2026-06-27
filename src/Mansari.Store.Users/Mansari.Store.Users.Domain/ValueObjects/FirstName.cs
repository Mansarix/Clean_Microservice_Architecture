using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 100;

    public string Value { get; private set; } = default!;

    private FirstName() { }

    private FirstName(string value)
    {
        Value = value;
    }

    public static FirstName Create(string value)
    {
        value = TextNormalizer.NormalizeWhitespace(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("First name cannot be empty.");

        if (value.Length > MaxLength)
            throw new DomainException($"First name cannot exceed {MaxLength} characters.");

        return new FirstName(value);
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
