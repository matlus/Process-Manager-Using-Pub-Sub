using System.Text;

namespace Testing.Core;

public static class StringBuilderExtension
{
    public static void AppendLineIfNotNull(this StringBuilder stringBuilder, string? value)
    {
        if (value is not null)
        {
            stringBuilder.AppendLine(value);
        }
    }
}

