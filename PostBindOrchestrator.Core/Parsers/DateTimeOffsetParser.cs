namespace PostBindOrchestrator.Core;

public static class DateTimeOffsetParser
{
    public static DateTimeOffset ParseElseException(string dateTimeOffsetAsString)
    {
        return DateTimeOffset.TryParse(dateTimeOffsetAsString, out var dateTimeOffset)
            ? dateTimeOffset
            : throw new DateTimeOffsetParseException($"Failed to parse DateTimeOffset from string with value: {dateTimeOffsetAsString}");
    }

    public static DateTimeOffset? ParseElseNull(string dateTimeOffsetAsString)
    {
        return DateTimeOffset.TryParse(dateTimeOffsetAsString, out var dateTimeOffset) ? dateTimeOffset : null;
    }
}
