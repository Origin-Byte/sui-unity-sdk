using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class ObjectData
    {
        [JsonConverter(typeof(RawDataJsonConverter))]
        public RawData Bcs { get; set; }

        [JsonConverter(typeof(DataJsonConverter))]
        public Data Content { get; set; }

        public string Digest { get; set; }

        public DisplayFieldsResponse Display { get; set; }

        public string ObjectId { get; set; }

        public Owner Owner { get; set; }

        public string PreviousTransaction { get; set; }

        public BigInteger? StorageRebate { get; set; }

        public string Type { get; set; }

        public BigInteger Version { get; set; }
    }

}
