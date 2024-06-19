using Newtonsoft.Json;

namespace Domain.Entities;
public class Messages
{
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}
