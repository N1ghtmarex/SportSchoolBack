namespace Core.Utils;

public static class DateTimeUtils
{
    /// <summary>
    /// Creates a row of dates from the start date to the end date.
    /// </summary>
    public static IEnumerable<DateTime> EnumerateDates(
        DateTimeOffset from, DateTimeOffset to)
    {
        return Enumerable.Range(0, 1 + to.Date.Subtract(from.Date).Days)
                        .Select(offset => from.Date.AddDays(offset));
    }
}
