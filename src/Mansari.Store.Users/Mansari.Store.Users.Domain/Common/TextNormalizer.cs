using System.Globalization;
using System.Text;

namespace Mansari.Store.Users.Domain.Common;

internal static class TextNormalizer
{
    public static string NormalizeWhitespace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var parts = value
            .Trim()
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(" ", parts);
    }

    public static string NormalizeDigits(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var builder = new StringBuilder(value.Length);

        foreach (var ch in value.Trim())
        {
            builder.Append(ch switch
            {
                '۰' => '0',
                '۱' => '1',
                '۲' => '2',
                '۳' => '3',
                '۴' => '4',
                '۵' => '5',
                '۶' => '6',
                '۷' => '7',
                '۸' => '8',
                '۹' => '9',
                '٠' => '0',
                '١' => '1',
                '٢' => '2',
                '٣' => '3',
                '٤' => '4',
                '٥' => '5',
                '٦' => '6',
                '٧' => '7',
                '٨' => '8',
                '٩' => '9',
                _ => ch
            });
        }

        return builder.ToString();
    }

    public static string ToLowerInvariantTrimmed(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}
