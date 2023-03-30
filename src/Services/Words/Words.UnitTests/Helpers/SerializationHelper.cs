using System.Text;
using Newtonsoft.Json;

namespace Words.UnitTests.Helpers;

public static class SerializationHelper
{
    public static byte[] SerializeObject(object value) =>Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, GetJsonSerializerSettings()));
    
    private static JsonSerializerSettings GetJsonSerializerSettings()
    {
        return new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}