namespace ProjectX.Core.Json.Interfaces;

public interface IApplicationJsonSerializer
{
    TOut Deserialize<TOut>(string json);
    string Serialize<TIn>(TIn item);
    string Serialize(object item, Type type);
    object Deserialize(string item, Type type);
    TOut Deserialize<TOut>(byte[] json);
    byte[] SerializeToBytes<TIn>(TIn item);
}