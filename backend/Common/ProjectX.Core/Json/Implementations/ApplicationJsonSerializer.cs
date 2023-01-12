using ProjectX.Core.Json.Interfaces;
using System.Text.Json;

namespace ProjectX.Core.Json.Implementations;

public sealed class ApplicationJsonSerializer : IApplicationJsonSerializer
{
    private readonly JsonSerializerOptions _serializationOptions;

    public ApplicationJsonSerializer()
    {
        _serializationOptions = SerializationOptionsBuilder
            .DefaultOptions()
            .AddNumberConverters()
            .AddJsonNonStringKeyDictionaryConverterFactory();
    }

    public string Serialize<TIn>(TIn item)
    {
        return JsonSerializer.Serialize(item, item.GetType(), _serializationOptions);
    }

    public string Serialize(object item, Type type)
    {
        return JsonSerializer.Serialize(item, type, _serializationOptions);

    }

    public byte[] SerializeToBytes<TIn>(TIn item)
    {
        return JsonSerializer.SerializeToUtf8Bytes(item, item.GetType(), _serializationOptions);
    }

    public TOut Deserialize<TOut>(byte[] json)
    {
        return JsonSerializer.Deserialize<TOut>(json, _serializationOptions);
    }

    public TOut Deserialize<TOut>(string json)
    {
        return JsonSerializer.Deserialize<TOut>(json, _serializationOptions);
    }

    public object Deserialize(string item, Type type)
    {
        return JsonSerializer.Deserialize(item, type, _serializationOptions);
    }
}