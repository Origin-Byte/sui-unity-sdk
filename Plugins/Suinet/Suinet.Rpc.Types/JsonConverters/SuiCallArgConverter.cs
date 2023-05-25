using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Numerics;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class SuiCallArgConverter : JsonConverter<SuiCallArg>
    {
        public override void WriteJson(JsonWriter writer, SuiCallArg value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value, new JsonSerializer
            {
                Converters = { new StringEnumConverter(new CamelCaseNamingStrategy()) }
            });
            writer.WriteToken(jObject.CreateReader());
        }

        public override SuiCallArg ReadJson(JsonReader reader, Type objectType, SuiCallArg existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"].ToObject<SuiCallArgType>();

            SuiCallArg result;

            switch (type)
            {
                case SuiCallArgType.Object:
                    result = new ObjectSuiCallArg
                    {
                        Properties = GetObjectProperties(jObject)
                    };
                    break;

                case SuiCallArgType.Pure:
                    result = new PureSuiCallArg
                    {
                        Value = jObject["value"].ToObject<object>(),
                        ValueType = jObject["valueType"]?.ToObject<string>()
                    };
                    break;

                default:
                    throw new Exception("Invalid type");
            }

            result.Type = type;
            return result;
        }

        private ObjectProperties GetObjectProperties(JObject jObject)
        {
            var objectType = jObject["objectType"].ToObject<SuiObjectType>();

            switch (objectType)
            {
                case SuiObjectType.ImmOrOwnedObject:
                    return new ImmOrOwnedObjectProperties
                    {
                        Digest = jObject["digest"]?.ToObject<string>(),
                        Version = BigInteger.Parse(jObject["version"]?.Value<string>() ?? "0"),
                        ObjectId = jObject["objectId"]?.ToObject<ObjectId>(),
                        ObjectType = objectType
                    };

                case SuiObjectType.SharedObject:
                    return new SharedObjectProperties
                    {
                        InitialSharedVersion = BigInteger.Parse(jObject["initialSharedVersion"]?.Value<string>() ?? "0"),
                        Mutable = jObject["mutable"]?.Value<bool>() ?? false,
                        ObjectId = jObject["objectId"]?.ToObject<ObjectId>(),
                        ObjectType = objectType
                    };

                default:
                    throw new Exception("Invalid object type");
            }
        }
    }

}
