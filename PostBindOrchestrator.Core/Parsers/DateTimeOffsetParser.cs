namespace PostBindOrchestrator.Core;

public static class DateTimeOffsetParser
{
    public static DateTimeOffset ParseElseException(string dateTimeOffsetAsString)
    {
        if (DateTimeOffset.TryParse(dateTimeOffsetAsString, out var dateTimeOffset))
        {
            return dateTimeOffset;
        }

        throw new DateTimeOffsetParseException($"Failed to parse DateTimeOffset from string with value: {dateTimeOffsetAsString}");
    }

    public static DateTimeOffset? ParseElseNull(string dateTimeOffsetAsString)
    {
        if (DateTimeOffset.TryParse(dateTimeOffsetAsString, out var dateTimeOffset))
        {
            return dateTimeOffset;
        }

        return null;
    }
}
