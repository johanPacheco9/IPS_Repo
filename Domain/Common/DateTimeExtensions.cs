namespace Domain.Common;

public static class DateTimeExtensions
{
    public static DateTime ToUtcKind(this DateTime dt)
    {
        if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };
    }
}
