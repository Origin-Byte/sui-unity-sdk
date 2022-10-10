using System;

public static class TimestampService
{
    public static long UtcTimestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
