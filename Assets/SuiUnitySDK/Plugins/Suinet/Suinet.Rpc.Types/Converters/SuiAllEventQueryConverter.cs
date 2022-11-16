﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suinet.Rpc.Types.Converters
{
    public class SuiAllEventQueryConverter : JsonConverter<SuiAllEventQuery>
    {
        public override SuiAllEventQuery ReadJson(JsonReader reader, Type objectType, SuiAllEventQuery existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, SuiAllEventQuery value, JsonSerializer serializer)
        {
            writer.WriteValue(SuiEventQueryType.All.ToString());
        }
    }
}
