namespace PostBindOrchestrator.Core;

public static class ValidatorTimeSpan
{
    public static string? Validate(string propertyName, string? propertyValue)
    {
        return ValidatorString.DetermineNullEmptyOrWhiteSpaces(propertyValue) switch
        {
            StringState.Null => $"The property: \"{propertyName}\" must be a valid TimeSpan: `00:00:00` or `00:00:00.5`, and can not be null",
            StringState.Empty => $"The property: \"{propertyName}\" must be a valid TimeSpan: `00:00:00` or `00:00:00.5`, and can not be Empty",
            StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid TimeSpan: `00:00:00` or `00:00:00.5`, and can not be Whitespaces",
            _ => ValidateStringIsTimeSpan(propertyName, propertyValue)
        };
    }

    private static string? ValidateStringIsTimeSpan(string propertyName, string? propertyValue)
    {
        return !TimeSpan.TryParse(propertyValue, Thread.CurrentThread.CurrentCulture, out var _)
            ? $"The property: \"{propertyName}\" must be a valid TimeSpan in the form: `00:00:00` or `00:00:00.5` etc. The current Values: `{propertyValue}`, is not a valid value for a TimeSpan."
            : null;
    }
}
