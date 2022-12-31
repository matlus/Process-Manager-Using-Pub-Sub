namespace PostBindOrchestrator.Core;

public enum StringState
{
    Null,
    Empty,
    WhiteSpaces,
    Valid
}

public static class ValidatorString
{
    public static string? Validate(string propertyName, string? propertyValue)
    {
        return DetermineNullEmptyOrWhiteSpaces(propertyValue) switch
        {
            StringState.Null => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be null",
            StringState.Empty => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Empty",
            StringState.WhiteSpaces => $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Whitespaces",
            _ => null,
        };
    }

    public static string? GetValueOrNull(string? value)
    {
        return DetermineNullEmptyOrWhiteSpaces(value) switch
        {
            StringState.Null or StringState.Empty or StringState.WhiteSpaces => null,
            _ => value,
        };
    }

    public static StringState DetermineNullEmptyOrWhiteSpaces(string? data)
    {
        if (data == null)
        {
            return StringState.Null;
        }
        else if (data.Length == 0)
        {
            return StringState.Empty;
        }

        foreach (var chr in data)
        {
            if (!char.IsWhiteSpace(chr))
            {
                return StringState.Valid;
            }
        }

        return StringState.WhiteSpaces;
    }
}
