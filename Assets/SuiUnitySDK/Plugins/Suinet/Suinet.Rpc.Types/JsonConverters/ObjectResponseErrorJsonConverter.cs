using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class ObjectResponseErrorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectResponseError).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null || reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var jsonObject = JObject.Load(reader);
            ObjectResponseError error;

            string code = jsonObject["Code"]?.ToString();

            switch (code)
            {
                case "NotExistsError":
                    error = new NotExistsError();
                    break;
                case "DynamicFieldNotFoundError":
                    error = new DynamicFieldNotFoundError();
                    break;
                case "DeletedError":
                    error = new DeletedError();
                    break;
                case "UnknownError":
                    error = new UnknownError();
                    break;
                case "DisplayError":
                    error = new DisplayError();
                    break;
                default:
                    throw new JsonSerializationException($"Unsupported type: {code}");
            }

            serializer.Populate(jsonObject.CreateReader(), error);

            return error;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
