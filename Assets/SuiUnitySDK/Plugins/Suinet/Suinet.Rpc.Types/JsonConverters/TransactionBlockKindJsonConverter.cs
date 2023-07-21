using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class TransactionBlockKindJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TransactionBlockKind).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            switch (jo["kind"].Value<string>())
            {
                case "ChangeEpoch":
                    return jo.ToObject<ChangeEpochTransactionBlockKind>(serializer);
                case "Genesis":
                    return jo.ToObject<GenesisTransactionBlockKind>(serializer);
                case "ConsensusCommitPrologue":
                    return jo.ToObject<ConsensusCommitPrologueTransactionBlockKind>(serializer);
                case "ProgrammableTransaction":
                    return jo.ToObject<ProgrammableTransactionBlockKind>(serializer);
                default:
                    throw new JsonSerializationException("Type of TransactionBlockKind not recognized");
            }
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
   
        }
    }

}
