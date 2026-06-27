using System.Text.RegularExpressions;
using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class MobileNumber : ValueObject
{
    private static readonly Regex Pattern = new("^09\d{9}$", RegexOptions.Compiled);

    public string Value { get; private set; } = default!;

    private MobileNumber() { }

    private MobileNumber(string value)
    {
        Value = value;
    }

    public static MobileNumber Create(string value)
    {
        value = TextNormalizer.NormalizeDigits(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Mobile number cannot be empty.");

        if (!Pattern.IsMatch(value))
            throw new DomainException("Mobile number must be a valid Iranian mobile number.");

        return new MobileNumber(value);
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
