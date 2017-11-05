using System;

namespace lpubsppop01.ContentUpdateNotifier
{
    public static class UnixTimestamp
    {
        static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int ToUnixTimestamp(this DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }
    }
}
