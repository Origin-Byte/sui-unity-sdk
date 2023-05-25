using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class SuiArgumentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(SuiArgument));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["GasCoin"] != null)
            {
                return new GasCoinArgument();
            }
            else if (jo["Input"] != null)
            {
                return new InputArgument
                {
                    Input = jo["Input"].Value<ushort>()
                };
            }
            else if (jo["Result"] != null)
            {
                return new ResultArgument
                {
                    Result = jo["Result"].Value<ushort>()
                };
            }
            else if (jo["NestedResult"] != null)
            {
                return new NestedResultArgument
                {
                    NestedResult = jo["NestedResult"].ToObject<ushort[]>()
                };
            }
            else
            {
                throw new JsonSerializationException();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
