namespace PostBindOrchestrator.Core;

public static class ValidatorInt
{
    public static string? Validate(string propertyName, string? propertyValue)
    {
        return ValidatorString.DetermineNullEmptyOrWhiteSpaces(propertyValue) switch
        {
            StringState.Null => $"The property: \"{propertyName}\" must be a valid Integer, and can not be null",
            StringState.Empty => $"The property: \"{propertyName}\" must be a valid Integer, and can not be Empty",
            StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid Integer, and can not be Whitespaces",
            _ => ValidateStringIsInt(propertyName, propertyValue)
        };
    }

    private static string? ValidateStringIsInt(string propertyName, string? propertyValue)
    {
        return !int.TryParse(propertyValue, out var _)
            ? $"The property: \"{propertyName}\" must be a valid Integer"
            : null;
    }
}
