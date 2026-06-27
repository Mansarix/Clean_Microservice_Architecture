using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class NationalId : ValueObject
{
    public const int RequiredLength = 10;

    public string Value { get; private set; } = default!;

    private NationalId() { }

    private NationalId(string value)
    {
        Value = value;
    }

    public static NationalId Create(string value)
    {
        value = TextNormalizer.NormalizeDigits(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("National id cannot be empty.");

        if (value.Length != RequiredLength || !value.All(char.IsDigit))
            throw new DomainException("National id must be exactly 10 digits.");

        if (value.Distinct().Count() == 1)
            throw new DomainException("National id is invalid.");

        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += (value[i] - '0') * (10 - i);
        }

        var remainder = sum % 11;
        var checkDigit = value[9] - '0';
        var expectedCheckDigit = remainder < 2 ? remainder : 11 - remainder;

        if (checkDigit != expectedCheckDigit)
            throw new DomainException("National id checksum is invalid.");

        return new NationalId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
