using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Suinet.Rpc.Types.Converters
{
    public class SuiOwnerConverter : JsonConverter<SuiOwner>
    {
        public override SuiOwner ReadJson(JsonReader reader, Type objectType, SuiOwner existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value.ToString();
                if (stringValue == SuiOwnerType.Shared.ToString())
                {
                    return new SuiOwner(SuiOwnerType.Shared);
                }
                else if (stringValue == SuiOwnerType.Immutable.ToString())
                {
                    return new SuiOwner(SuiOwnerType.Immutable);
                }
            }
            else
            {
                var jObject = serializer.Deserialize<JObject>(reader);
                if (jObject.ContainsKey(SuiOwnerType.AddressOwner.ToString()))
                {
                    return new SuiOwner(SuiOwnerType.AddressOwner, jObject[SuiOwnerType.AddressOwner.ToString()].Value<string>());
                }
                else if (jObject.ContainsKey(SuiOwnerType.ObjectOwner.ToString()))
                {
                    return new SuiOwner(SuiOwnerType.ObjectOwner, jObject[SuiOwnerType.ObjectOwner.ToString()].Value<string>());
                }
            }

            return new SuiOwner(SuiOwnerType.None);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, SuiOwner value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
