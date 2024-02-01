using System.Text.Json.Serialization;

namespace Library.Core.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationEnum
    {
        Buy,
        Sell
    }
}
