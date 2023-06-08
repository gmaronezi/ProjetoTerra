using System.Text.Json;
using System.Text.Json.Serialization;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Shared.Extensions;

public class JsonSnakeSerializer
{
    public static readonly JsonNamingPolicy SnakeNamingPolicy = new JsonSnakeCaseNamingPolicy();
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = SnakeNamingPolicy,
        WriteIndented = false,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    static JsonSnakeSerializer()
    {
        DefaultSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, DefaultSerializerOptions);
    }
}