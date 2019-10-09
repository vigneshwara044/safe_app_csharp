using Newtonsoft.Json;

namespace SafeApp.Core
{
    public static class Serialization
    {
        public static T Deserialize<T>(string json)
            => JsonConvert.DeserializeObject<T>(json);
    }
}
