using Newtonsoft.Json;

namespace SafeApp.Core
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Serialization
    {
        public static T Deserialize<T>(string json)
            => JsonConvert.DeserializeObject<T>(json);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
