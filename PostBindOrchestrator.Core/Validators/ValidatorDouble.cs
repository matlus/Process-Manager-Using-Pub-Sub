namespace PostBindOrchestrator.Core;
public static class ValidatorDouble
{
    public static string? Validate(string propertyName, string? propertyValue)
    {
        return ValidatorString.DetermineNullEmptyOrWhiteSpaces(propertyValue) switch
        {
            StringState.Null => $"The property: \"{propertyName}\" must be a valid double, and can not be null",
            StringState.Empty => $"The property: \"{propertyName}\" must be a valid double, and can not be Empty",
            StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid double, and can not be Whitespaces",
            _ => ValidateStringIsDouble(propertyName, propertyValue)
        };
    }

    private static string? ValidateStringIsDouble(string propertyName, string? propertyValue)
    {
        return !double.TryParse(propertyValue, out var _)
            ? $"The property: \"{propertyName}\" must be a valid double"
            : null;
    }
}
