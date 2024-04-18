namespace PostBindOrchestrator.Core;

public static class ValidatorBoolean
{
    public static string? Validate(string propertyName, string? propertyValue)
    {
        return ValidatorString.DetermineNullEmptyOrWhiteSpaces(propertyValue) switch
        {
            StringState.Null => $"The property: \"{propertyName}\" must be a valid boolean: `{bool.TrueString}` or `{bool.FalseString}`, and can not be null",
            StringState.Empty => $"The property: \"{propertyName}\" must be a valid boolean: `{bool.TrueString}` or `{bool.FalseString}`, and can not be Empty",
            StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid boolean: `{bool.TrueString}` or `{bool.FalseString}`, and can not be Whitespaces",
            _ => ValidateStringIsBoolean(propertyName, propertyValue)
        };
    }

    private static string? ValidateStringIsBoolean(string propertyName, string? propertyValue)
    {
        return !bool.TryParse(propertyValue, out var _)
            ? $"The property: \"{propertyName}\" must be a valid boolean and can only have the values: `{bool.TrueString}` and `{bool.FalseString}`"
            : null;
    }
}
