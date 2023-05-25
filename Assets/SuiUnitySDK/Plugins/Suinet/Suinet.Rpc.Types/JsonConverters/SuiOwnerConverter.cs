using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class SuiOwnerConverter : JsonConverter<Owner>
    {
        public override Owner ReadJson(JsonReader reader, Type objectType, Owner existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value.ToString();
                if (stringValue == SuiOwnerType.Shared.ToString())
                {
                    return new Owner(SuiOwnerType.Shared);
                }
                else if (stringValue == SuiOwnerType.Immutable.ToString())
                {
                    return new Owner(SuiOwnerType.Immutable);
                }
            }
            else
            {
                var jObject = serializer.Deserialize<JObject>(reader);
                if (jObject.ContainsKey(SuiOwnerType.AddressOwner.ToString()))
                {
                    return new Owner(SuiOwnerType.AddressOwner, jObject[SuiOwnerType.AddressOwner.ToString()].Value<string>());
                }
                else if (jObject.ContainsKey(SuiOwnerType.ObjectOwner.ToString()))
                {
                    return new Owner(SuiOwnerType.ObjectOwner, jObject[SuiOwnerType.ObjectOwner.ToString()].Value<string>());
                }
            }

            return new Owner(SuiOwnerType.None);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, Owner value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
