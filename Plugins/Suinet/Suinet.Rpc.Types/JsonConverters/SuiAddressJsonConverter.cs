using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{

    public class SuiAddressJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SuiAddress);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            return new SuiAddress(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var address = (SuiAddress)value;
            writer.WriteValue((string)address);
        }
    }
}
