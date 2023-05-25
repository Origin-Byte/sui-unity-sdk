using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class SuiObjectReadConverter : JsonConverter<SuiObjectRead>
    {
        public override SuiObjectRead ReadJson(JsonReader reader, Type objectType, SuiObjectRead existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            if (jObject.TryGetValue("status", out var statusValue))
            {
                var statusString = statusValue.Value<string>();
                if (statusString == SuiObjectReadStatus.NotExists.ToString())
                {
                    return new SuiObjectRead(SuiObjectReadStatus.NotExists, null, null, jObject["details"].Value<string>());
                }
                else if (statusString == SuiObjectReadStatus.Deleted.ToString())
                {
                    return new SuiObjectRead(SuiObjectReadStatus.Deleted, null, jObject["details"].ToObject<SuiObjectRef>(), null);
                }
                else if (statusString == SuiObjectReadStatus.Exists.ToString())
                {
                    return new SuiObjectRead(SuiObjectReadStatus.Exists, jObject["details"].ToObject<SuiObject>(), null, null);
                }
            }            

            return new SuiObjectRead(SuiObjectReadStatus.None);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, SuiObjectRead value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
