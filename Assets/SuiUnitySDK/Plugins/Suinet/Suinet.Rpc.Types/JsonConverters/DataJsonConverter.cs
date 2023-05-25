using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class DataJsonConverter : JsonConverter<Data>
    {
        public override Data ReadJson(JsonReader reader, Type objectType, Data existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var dataType = jsonObject["dataType"].Value<string>();

            switch (dataType)
            {
                case "moveObject":
                    return jsonObject.ToObject<MoveObjectData>(serializer);
                case "package":
                    return jsonObject.ToObject<PackageData>(serializer);
                default:
                    throw new JsonSerializationException($"Unknown dataType: {dataType}");
            }
        }

        public override void WriteJson(JsonWriter writer, Data value, JsonSerializer serializer)
        {
            if (value is MoveObjectData moveObjectData)
            {
                serializer.Serialize(writer, moveObjectData);
            }
            else if (value is PackageData packageData)
            {
                serializer.Serialize(writer, packageData);
            }
            else
            {
                throw new JsonSerializationException($"Unsupported type: {value.GetType().FullName}");
            }
        }
    }

}
