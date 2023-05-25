using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System;
using System.Collections.Generic;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class MoveValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MoveValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Check if the token is an object
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jo = JObject.Load(reader);

                if (jo["id"] != null)
                {
                    return new ObjectIDMoveValue { Id = jo["id"].ToObject<ObjectId>(serializer) };
                }
                else if (jo["fields"] != null)
                {
                    return new MoveStructMoveValue { Value = jo.ToObject<MoveStruct>(serializer) };
                }
                else if (jo.Type == JTokenType.Array)
                {
                    return new ArrayMoveValue { Value = jo.ToObject<List<MoveValue>>(serializer) };
                }
                else
                {
                    throw new JsonSerializationException("Type of MoveValue not recognized");
                }
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                return new ArrayMoveValue { Value = serializer.Deserialize<List<MoveValue>>(reader) };
            }
            else
            {
                // If it's not an object, then it should be a simple value
                if (reader.TokenType == JsonToken.Integer)
                {
                    return new IntegerMoveValue { Value = Convert.ToUInt32(reader.Value) };
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    // Check if string can be converted to SuiAddress
                    var str = reader.Value as string;
                    if (ObjectId.IsValid(str))
                    {
                        return new SuiAddressMoveValue { Value = str };
                    }

                    return new StringMoveValue { Value = str };
                }
                else if (reader.TokenType == JsonToken.Boolean)
                {
                    return new BooleanMoveValue { Value = (bool)reader.Value };
                }
                else
                {
                    throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var moveValue = (MoveValue)value;
            switch (moveValue)
            {
                case IntegerMoveValue intMoveValue:
                    serializer.Serialize(writer, intMoveValue.Value);
                    break;
                case BooleanMoveValue boolMoveValue:
                    serializer.Serialize(writer, boolMoveValue.Value);
                    break;
                case StringMoveValue stringMoveValue:
                    serializer.Serialize(writer, stringMoveValue.Value);
                    break;
                case SuiAddressMoveValue suiAddressMoveValue:
                    serializer.Serialize(writer, suiAddressMoveValue.Value);
                    break;
                case ObjectIDMoveValue objectIdMoveValue:
                    serializer.Serialize(writer, new { objectIdMoveValue.Id });
                    break;
                case MoveStructMoveValue moveStructMoveValue:
                    serializer.Serialize(writer, moveStructMoveValue.Value);
                    break;
                case ArrayMoveValue arrayMoveValue:
                    serializer.Serialize(writer, arrayMoveValue.Value);
                    break;
                default:
                    throw new Exception("Unrecognized MoveValue type: " + moveValue.GetType());
            }
        }
    }

}
