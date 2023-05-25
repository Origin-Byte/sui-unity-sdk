using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class RawDataJsonConverter : JsonConverter<RawData>
    {
        public override RawData ReadJson(JsonReader reader, Type objectType, RawData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var dataType = jsonObject["dataType"].Value<string>();

            switch (dataType)
            {
                case "moveObject":
                    return jsonObject.ToObject<MoveObjectRawData>(serializer);
                case "package":
                    return jsonObject.ToObject<PackageRawData>(serializer);
                default:
                    throw new JsonSerializationException($"Unknown dataType: {dataType}");
            }
        }

        public override void WriteJson(JsonWriter writer, RawData value, JsonSerializer serializer)
        {
            if (value is MoveObjectRawData moveObjectRawData)
            {
                serializer.Serialize(writer, moveObjectRawData);
            }
            else if (value is PackageRawData packageRawData)
            {
                serializer.Serialize(writer, packageRawData);
            }
            else
            {
                throw new JsonSerializationException($"Unsupported type: {value.GetType().FullName}");
            }
        }
    }

}
