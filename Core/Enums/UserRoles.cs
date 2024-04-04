using System.Text.Json.Serialization;

namespace Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoles
    {
        User = 0,
        Moderator = 1,
        Admin = 2
    }
}
