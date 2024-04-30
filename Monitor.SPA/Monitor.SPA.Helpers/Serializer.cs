using System.Text.Json;

namespace Monitor.SPA.Helpers
{
    public static class Serializer
    {
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json);

        public static string Serialize(object value) => JsonSerializer.Serialize(value);
    }
}
