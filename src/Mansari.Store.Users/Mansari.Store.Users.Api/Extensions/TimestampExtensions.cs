using Google.Protobuf.WellKnownTypes;

namespace Mansari.Store.Users.Api.Extensions;

internal static class TimestampExtensions
{
    public static Timestamp ToTimestamp(this DateOnly date)
    {
        var dateTime = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        return Timestamp.FromDateTime(dateTime);
    }

    public static Timestamp ToTimestamp(this DateTime dateTime)
    {
        var utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return Timestamp.FromDateTime(utcDateTime);
    }

    public static DateOnly ToDateOnly(this Timestamp timestamp)
    {
        return DateOnly.FromDateTime(timestamp.ToDateTime());
    }
}
