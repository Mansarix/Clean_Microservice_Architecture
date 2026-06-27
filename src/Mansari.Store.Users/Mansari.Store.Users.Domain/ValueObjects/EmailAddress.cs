using System.Net.Mail;
using Mansari.Store.Users.Domain.Common;

namespace Mansari.Store.Users.Domain.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public const int MaxLength = 254;

    public string Value { get; private set; } = default!;

    private EmailAddress() { }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress Create(string value)
    {
        value = TextNormalizer.ToLowerInvariantTrimmed(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (value.Length > MaxLength)
            throw new DomainException($"Email cannot exceed {MaxLength} characters.");

        try
        {
            var address = new MailAddress(value);
            value = address.Address.ToLowerInvariant();
        }
        catch
        {
            throw new DomainException("Email is invalid.");
        }

        return new EmailAddress(value);
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
