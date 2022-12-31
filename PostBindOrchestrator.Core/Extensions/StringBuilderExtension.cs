using System.Text;

namespace PostBindOrchestrator.Core;

internal static class StringBuilderExtension
{
    public static void AppendLineIfNotNull(this StringBuilder stringBuilder, string? value)
    {
        if (value is not null)
        {
            stringBuilder.AppendLine(value);
        }
    }
}
