using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Bookings.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeskType
    {
        [EnumMember(Value = "Any Desk")]
        AnyDesk,
        [EnumMember(Value = "Group Desk")]
        GroupDesk,
        [EnumMember(Value = "Single Room")]
        SingleRoom,
        [EnumMember(Value = "Doubled Room")]
        DoubledRoom
    }
}