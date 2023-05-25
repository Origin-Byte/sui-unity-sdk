using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class MoveTypeConverter : JsonConverter<MoveType>
    {
        public override MoveType ReadJson(JsonReader reader, Type objectType, MoveType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var json = reader.Value as string;
            return new MoveType(json);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, MoveType value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
