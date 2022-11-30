namespace Server.Utils;

public static class DateTimeExtensions
{
    public static long ToUnixTime(this DateTime dateTime)
    {
        var dto = new DateTimeOffset(dateTime.ToUniversalTime());
        return dto.ToUnixTimeSeconds();
    }

    public static DateTime FromUnixTime(this long unixTimestamp)
    {
        var dt = DateTime.UnixEpoch;
        return dt.AddSeconds(unixTimestamp);
        
    }
}