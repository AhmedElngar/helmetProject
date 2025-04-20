using System.Text.Json.Serialization;

namespace HelmetApiProject.Models
{
    public enum AlertStatus
    {
        Drunk,
        Damaged,
        NotWorn
    }

    public enum AlertLevel
    {
        High,
        Medium,
        Low
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AlertType
    {
        Alcohol,
        Accident
    }
}