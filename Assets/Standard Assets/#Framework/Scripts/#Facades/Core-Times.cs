using System;

public static partial class Core {

    /// <summary>
    ///     生成时间戳(javascript 的时间戳是毫秒)
    /// </summary>
    /// <returns>当前时间减去 1970-01-01 00.00.00 得到的毫秒数</returns>
    public static long GetTimeStampJS(this DateTime nowTime)
    {
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
        nowTime = nowTime == default ? DateTime.Now : nowTime;

        return (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);

        //return unixTime.ToString();
    }

    public static DateTime ConvertTimestampJS(this long timestamp)
    {
        var converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var newDateTime = converted.AddMilliseconds(timestamp);

        // 添加兼容, 如果大于当前时间用毫秒
        return /*newDateTime > DateTime.Now ?converted.AddSeconds(timestamp).ToLocalTime() : */
            newDateTime.ToLocalTime();
    }

    public static TimeSpan GetDateDiff(this DateTime timeStart, DateTime timeEnd) =>
        new TimeSpan(timeEnd.Ticks).Subtract(new TimeSpan(timeStart.Ticks));

    /// <summary>
    ///     Unix时间戳(秒)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double ToTimestampUnix(this DateTime value)
    {
        var span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

        return span.TotalSeconds;
    }

    /// <summary>
    ///     Unix 时间戳, 秒
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ConvertTimestampUnix(this double timestamp)
    {
        var converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var newDateTime = converted.AddSeconds(timestamp);

        return newDateTime.ToLocalTime();
    }

    public static string RelativeFriendlyTime(this long time) =>
        RelativeFriendlyTime(GetDateDiff(DateTime.Now, ConvertTimestampJS(time)));

    public static string RelativeFriendlyTime(this TimeSpan time)
    {
        var output = string.Empty;

        if (time.Days > 0) {
            output += time.Days + " days";
        }

        if ((time.Days == 0 || time.Days == 1) && time.Hours > 0) {
            output += time.Hours + " hr";
        }

        if (time.Days == 0 && time.Minutes > 0) {
            output += time.Minutes + " min";
        }

        if (output.Length == 0) {
            output += time.Seconds + " sec";
        }

        return output.Trim();
    }
}