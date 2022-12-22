using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PostBindOrchestrator.Core;

public static class ApplicationSerializer
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new ()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static byte[] Serialize<T>(T value)
    {
        string jsonString = JsonSerializer.Serialize(value, jsonSerializerOptions);
        return Encoding.UTF8.GetBytes(jsonString);
    }

    public static void Serialize(Stream stream, object value)
    {
        using var utf8JsonWriter = new Utf8JsonWriter(stream);
        JsonSerializer.Serialize(utf8JsonWriter, value, jsonSerializerOptions);
        utf8JsonWriter.Flush();
    }

    public static string SerializeToString<T>(T value)
    {
        return JsonSerializer.Serialize(value, jsonSerializerOptions);
    }

    public static T Deserialize<T>(byte[] bytes)
    {
        var bytesAsString = Encoding.UTF8.GetString(bytes);

        try
        {
            return (JsonSerializer.Deserialize<T>(bytesAsString, jsonSerializerOptions))!;
        }
        catch (JsonException e)
        {
            var message = $"{nameof(Deserialize)}: Exception occurred while attempting to deserialize JSON string to target of Type: {typeof(T).Name}. " +
                $"JSON received: {bytesAsString}. The Original Exception type is: {e.GetType().Name}. Original Exception message: {e.Message}";

            throw new MessageDeserializationFailedException(message, e);
        }
    }

    public static T Deserialize<T>(string jsonString)
    {
        try
        {
            return (JsonSerializer.Deserialize<T>(jsonString, jsonSerializerOptions))!;
        }
        catch (JsonException e)
        {
            var message = $"{nameof(Deserialize)}: Exception occurred while attempting to deserialize JSON Message body to target of Type: {typeof(T).Name}. " +
                $"JSON received: {jsonString}. The Original Exception type is: {e.GetType().Name}. Original Exception message: {e.Message}";

            throw new MessageDeserializationFailedException(message, e);
        }
    }
}
