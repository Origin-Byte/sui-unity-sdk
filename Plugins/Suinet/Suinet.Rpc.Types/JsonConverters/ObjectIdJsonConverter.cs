using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class ObjectIdJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            if (ObjectId.IsValid(value))
            {
                return new ObjectId(value);
            }
            else
            {
                throw new JsonSerializationException("Invalid ObjectId value.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectId = (ObjectId)value;
            writer.WriteValue((string)objectId);
        }
    }
}
