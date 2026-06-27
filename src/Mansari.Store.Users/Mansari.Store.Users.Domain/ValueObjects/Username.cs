using System.Text.RegularExpressions;
using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class Username : ValueObject
{
    private static readonly Regex Pattern = new("^[a-zA-Z0-9._-]{3,50}$", RegexOptions.Compiled);

    public const int MaxLength = 50;

    public string Value { get; private set; } = default!;

    private Username() { }

    private Username(string value)
    {
        Value = value;
    }

    public static Username Create(string value)
    {
        value = TextNormalizer.ToLowerInvariantTrimmed(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Username cannot be empty.");

        if (value.Length > MaxLength)
            throw new DomainException($"Username cannot exceed {MaxLength} characters.");

        if (!Pattern.IsMatch(value))
            throw new DomainException("Username can contain letters, numbers, dot, underscore and dash only.");

        return new Username(value);
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
